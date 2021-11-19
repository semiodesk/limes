#!/usr/bin/env python3

from lib.csv_core import CsvReader, CsvWriter, eprint
from lib.csv_clean import DataCleaner
from argparse import ArgumentParser

# Provide a flexible command line arguments and user friendly help.
parser = ArgumentParser(description='Clean CSV files before data transformations.')
parser.add_argument('-k', '--key', type=str, help='Column name to be used to identify records.')
parser.add_argument('-i', '--input', type=str, help='Input file in CSV format to be read.')
parser.add_argument('-o', '--output', type=str, help='Output file in CSV format to be written.')

args = parser.parse_args()

reader = CsvReader(args.input, args.key)
writer = CsvWriter(args.output, reader.fieldnames)
cleaner = DataCleaner()

rows = {}
duplicates = {}

# We do *not* write out the ids of the processed rows to increase processing speed.
for row in reader:
    # Normalize the primary key column to lower case.
    id = row[args.key].lower().strip()

    # Write the normalized key back to the file.
    row[args.key] = id

    # Invoke the cleansing functions..
    row = cleaner.clean(row)

    if id in rows.keys():            
        if id not in duplicates.keys():
            duplicates[id] = [rows[id]]

        duplicates[id].append(row)
    else:
        rows[id] = row
        writer.write(row)

# Write the duplicates to stderr so that they can be piped into a file.
if len(duplicates):
    eprint(','.join(reader.fieldnames))

    for id in duplicates.keys():
        for row in duplicates[id]:
            eprint(','.join(row.values()))