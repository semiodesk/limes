#!/usr/bin/env python3

from lib.csv_data_cleaner_factory import CsvDataCleanerFactory
from lib.csv_core import CsvReader, CsvWriter, eprint
from argparse import ArgumentParser

# Provide a flexible command line arguments and user friendly help.
parser = ArgumentParser(description='Clean CSV files before data transformations.')
parser.add_argument('-i', '--input', type=str, help='Input file in CSV format to be read.')
parser.add_argument('-o', '--output', type=str, help='Output file in CSV format to be written.')
parser.add_argument('-k', '--key', type=str, help='Column name to be used to identify records.')
parser.add_argument('-c', '--cleaner', type=str, help='ID of the data cleaner to be used.')

args = parser.parse_args()

reader = CsvReader(args.input, args.key)
writer = CsvWriter(args.output, reader.fieldnames)
cleaner = CsvDataCleanerFactory().get_cleaner(args.cleaner, args.key)

rows = {}
duplicates = {}

# We do *not* write out the ids of the processed rows to increase processing speed.
for row in reader:
    # Invoke the cleansing operations..
    row = cleaner.clean_row(row)

    # Read the primary key value *after* the row was cleaned.
    id = row[args.key]

    # Instead of immediatly writing out the rows, we first process them all
    # to detect duplicates.
    if id in rows.keys():
        if id not in duplicates.keys():
            duplicates[id] = [rows[id]]

        duplicates[id].append(row)

    # Consider the last occurance of a row to be the most recent.
    rows[id] = row

# After we processed all the data we can write it to the output.
for row in rows.values():
    writer.write(row)

# Write the duplicates to stderr so that they can be piped into a file.
if len(duplicates):
    eprint(','.join(reader.fieldnames))

    for id in duplicates.keys():
        for row in duplicates[id]:
            eprint(','.join(row.values()))