#!/usr/bin/env python3

from lib.csv_core import CsvReader, CsvWriter
from argparse import ArgumentParser

# Provide a flexible command line arguments and user friendly help.
parser = ArgumentParser(description='Merge CSV files before data transformations.')
parser.add_argument('-k', '--key', type=str, help='Column name to be used to identify records.')
parser.add_argument('-ia', '--input-a', type=str, help='Input file in CSV format to be read.')
parser.add_argument('-ib', '--input-b', type=str, help='Input file in CSV format to be read.')
parser.add_argument('-o', '--output', type=str, help='Output file in CSV format to be written.')
parser.add_argument('-d', '--default-value', type=str, nargs='+', action='append', help='Default value to be used if a column is empty in the format column_name:value (i.e. --default-value age:42)')
parser.add_argument('-m', '--merge-column', type=str, nargs='+', action='append', help='Names of two columns to be merged in the format column_a:column_b')

args = parser.parse_args()

try:
    readerA = CsvReader(args.input_a, args.key)
    readerB = CsvReader(args.input_b, args.key)
except:
    exit(-1)

mappings = {}

if args.merge_column:
    for arg in args.merge_column:
        a = arg[0].split(':')

        mappings[a[0]] = a[1]

def getmapping(fieldname):
    return mappings[fieldname] if fieldname in mappings.keys() else fieldname

defaults = {}

if args.default_value:
    for arg in args.default_value:
        a = arg[0].split(':')

        defaults[a[0]] = a[1]

def getdefault(fieldname):
    return defaults[fieldname] if fieldname in defaults.keys() else ''

# Merge the fieldnames of the two files considering the mapping specifications.
fieldnames = []

for field in [getmapping(f) for f in readerA.fieldnames]:
    if not field in fieldnames:
        fieldnames.append(field)

for field in [getmapping(f) for f in readerB.fieldnames]:
    if not field in fieldnames:
        fieldnames.append(field)

writer = CsvWriter(args.output, fieldnames)

# Read the rows from file B into a dictionary for quick access in the following loop.
# Note that this will not scale well to very large files.
rowsB = {}

for rB in readerB:
    id = rB[args.key].lower()
    r = {}

    for field in rB.keys():
        r[getmapping(field)] = rB[field]
    
    rowsB[id] = r

# Read the rows from file A and write values from file B into the rows, if there is any data.
for rA in readerA:
    # Read the normalized primary key of the row.
    id = rA[args.key].lower()

    # Set values for all fields from B that are not in A..
    for field in fieldnames:
        if not field in rA.keys():
            rA[field] = getdefault(field)

    # Merge values for mapped columns.
    for field in [x for x in rA.keys()]:
        if field in mappings.keys():
            v0 = rA[field]
            v1 = rA[getmapping(field)]

            # Only overwrite the existing column if there is a value in the mapped column.
            if len(v0) and len(v1) == 0:
                rA[getmapping(field)] = v0

            del rA[field]

    # If there is an entry in file B, merge it into A and delete the entry from the cache.
    if id in rowsB.keys():
        rB = rowsB[id]

        for field in rB:
            rA[field] = rB[field]

        del rowsB[id]

    writer.write(rA)

# If there are any entries in file B that do not appear in A, append them to the file..
for id in rowsB.keys():
    rA = {}
    rB = rowsB[id]

    for field in fieldnames:
        rA[field] = getdefault(field)

    for field in rB.keys():
        rA[field] = rB[field]

    writer.write(rA)
    