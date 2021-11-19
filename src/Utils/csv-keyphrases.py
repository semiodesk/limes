from lib.csv_core import CsvReader, CsvWriter, fieldnames
from argparse import ArgumentParser
import re

# Provide a flexible command line arguments and user friendly help.
parser = ArgumentParser(description='Extract keyphrases from a CSV file.')
parser.add_argument('-k', '--key', type=str, help='Column name where to extract the keyphrases.')
parser.add_argument('-i', '--input', type=str, help='Input file in CSV format to be read.')
parser.add_argument('-o', '--output', type=str, help='Output file in CSV format to be written.')

args = parser.parse_args()

reader = CsvReader(args.input)
writer = CsvWriter(args.output, fieldnames=['value'], writeheader=False)

phrases = set()

for row in reader:
    value = row[args.key]

    for match in re.finditer('%+(?P<phrase>.+?)%+', value):
        phrase = match.group('phrase').strip().lower()
        phrases.add(phrase)

phrases = list(phrases)
phrases.sort()

for phrase in phrases:
    writer.write({"value": f"%{phrase}%"})
