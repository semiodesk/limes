#!/usr/bin/env python3

from lib.csv_core import CsvReader, fieldnames
from lib.profiles_converter import *
from os import path
from argparse import ArgumentParser

# Provide a flexible command line arguments and user friendly help.
parser = ArgumentParser(description='Transform a CSV file into ProfilesRNS data.')
parser.add_argument('-i', '--input', type=str, help='Input file in CSV format to be processed.')
parser.add_argument('-o', '--output', type=str, help='Output folder for the generated CSV files.')

args = parser.parse_args()

person_file = path.join(args.output, 'Person.csv')
person_writer = PersonWriter(person_file, fieldnames(Person()))

affiliation_file = path.join(args.output, 'PersonAffiliation.csv')
affiliation_writer = PersonAffiliationWriter(affiliation_file, fieldnames(PersonAffiliation()))

filter_file = path.join(args.output, 'PersonFilterFlag.csv')
filter_writer = PersonFilterFlagWriter(filter_file, fieldnames(PersonFilterFlag()))

reader = CsvReader(args.input)

for row in reader:
    person_writer.write(row)
    affiliation_writer.write(row)
    filter_writer.write(row)
