#!/usr/bin/env python3

from lib.csv_core import CsvReader, CsvWriter
from argparse import ArgumentParser

# Provide a flexible command line arguments and user friendly help.
parser = ArgumentParser(description='Filter CSV files before data transformations.')
parser.add_argument('-k', '--key', type=str, help='Column name to be used to identify records.')
parser.add_argument('-i', '--input', type=str, help='Input file in CSV format to be read.')
parser.add_argument('-o', '--output', type=str, help='Output file in CSV format to be written.')
parser.add_argument('-f', '--filterlist', type=str, default=None, help='Text file contaning one id per line of the rows to be included in the output.')
parser.add_argument('-e', '--exclude', type=bool, default=False, help='Trigger the ids in the filter list to be excluded from the output.')
parser.add_argument('-v', '--value', type=str, nargs='+', action='append', help='Column values to be filtered in the format column:value')

args = parser.parse_args()

# Read the value filters into a dictionary
filtered_values = None

if args.value:
    filtered_values = {}

    for arg in args.value:
        a = arg[0].split(':')

        filtered_values[a[0]] = a[1]

def valuefilter(row):
    if filtered_values:
        for field in row.keys():
            if field in filtered_values.keys():
                if filtered_values[field] == row[field]:
                    return True
    else:
        return False

# Read the filterlist into a set for fast lookup
filtered_ids = None

if args.filterlist:
    filtered_ids = set([id.lower().strip() for id in open(args.filterlist, 'r') if id != '\n'])

def idfilter(id):
    if filtered_ids:
        filtered = id in filtered_ids

        if (not args.exclude and filtered) or (args.exclude and not filtered):
            return False
        else:
            return True
    else:
        return False

# Read the input file and execute the filtering
reader = CsvReader(args.input, args.key)
writer = CsvWriter(args.output, reader.fieldnames)

for r in reader:
    id = r[args.key].lower().strip()

    if idfilter(id):
        print(f" [-] {id}")
    elif valuefilter(r):
        print(f" [!] {id}")
    else:
        writer.write(r)
