import csv
import sys
from io import open

def eprint(*args, **kwargs):
    print(*args, file=sys.stderr, **kwargs)

def fieldnames(obj):
    return [attr for attr, value in obj.__dict__.items()]

def rowdict(obj):
    return {attr: value for attr, value in obj.__dict__.items()}

# Notes:
# - Open the file in UTF-8 encoding to ensure correct handling of German Umlauts.
# - We use the utf-8-sig to remove any BOMs when the input file was edited on Windows.
# - The newline='\n' option is required for correct line endings on Windows.

class CsvWriter:
    linecount = 0

    def __init__(self, filepath, fieldnames, encoding='utf-8-sig', newline='\n', delimiter=',', quotechar='"', writeheader=True):
        self.file = open(filepath, 'w', encoding=encoding, newline=newline)

        if quotechar is not None:
            self.writer = csv.DictWriter(self.file, fieldnames=fieldnames, delimiter=delimiter, quotechar=quotechar)
        else:
            self.writer = csv.DictWriter(self.file, fieldnames=fieldnames, delimiter=delimiter, quoting=csv.QUOTE_NONE)

        if writeheader:
            self.writer.writeheader()

    def __del__(self):
        print(f"Writing {self.file.name}: {self.linecount} rows")

        self.file.close()
        
    def write(self, row):
        self.linecount += 1
        self.writer.writerow(row)


class CsvReader:
    def __init__(self, filepath, keyfield=None, encoding='utf-8-sig', newline='\n', delimiter=',', quotechar='"'):
        self.file = open(filepath, encoding=encoding, newline=newline)
        self.reader = csv.DictReader(self.file, delimiter=delimiter, quotechar=quotechar)
        self.keyfield = keyfield

        if keyfield and not keyfield in self.reader.fieldnames:
            raise Exception(f'Key column {keyfield} not found in file {filepath}')

    def __del__(self):
        self.file.close()

    def __iter__(self):
        return self.reader.__iter__()

    def get_fieldnames(self):
        return self.reader.fieldnames

    fieldnames = property(get_fieldnames)
