import re
import csv
import io
import pkgutil

class GermanPhoneFormatter:
    def __init__(self):
        self.prefixes = set()

        data = pkgutil.get_data(__name__, 'german-zip-codes.csv').decode()

        for row in csv.DictReader(io.StringIO(data), delimiter=';'):
            p = row['Vorwahl'][1:]

            self.prefixes.add(p)

    def format(self, value):
        v = value.replace('(0)', '')
        v = v.replace('-', ' ')

        v = re.sub('^49', '', v)
        v = re.sub('^\+49', '', v)
        v = re.sub('^\+49\s?0', '', v)
        v = re.sub('\s', '', v)
        v = v.strip('0')

        # See: https://en.wikipedia.org/wiki/List_of_dialling_codes_in_Germany

        # Format cell phone number prefixes..
        if v.startswith('15') or v.startswith('16') or v.startswith('17'):
            return f"+49 {v[0:3]} {v[3:].strip('0')}"

        # For land lines we start to search for the longest prefix match. The 
        # maximum length for a phone prefix in Germany is 5 digits excluding the leading 0.
        n = 5
        
        while n > 1:
            p = v[0:n]

            if(p in self.prefixes):
                return f"+49 {p} {v[n:].strip('0')}"

            n -= 1

        return f"+49 {v}" if len(v) > 3 else ""

class ReplacementPattern:
    def __init__(self, str):
        self.columns = set()

        s = str.strip('\n')

        if s.find('|') > -1:
            for match in re.findall("^(\|((\w|\-)+))+\|", s):
                self.columns.add(match[1])

            s = s[s.rfind('|') + 1:]

        s = s.strip('/')
        n = len(s)

        # Locate the separator '/', skipping escaped dashes.
        for i in range(len(s) - 1, 0, -1):
            if s[i] == '/' and i > 0 and s[i - 1] != '\\':
                n = i
                break

        self.pattern = s[:n]
        self.replace = s[n + 1:]

        if len(self.pattern) == 0:
            raise Exception(f'Malformed regular expression: \'{str}\'')

    def apply(self, row):
        columns = set(row.keys())

        if len(self.columns):
            columns = self.columns.intersection(columns)

        for column in columns:
            row[column] = re.sub(self.pattern, self.replace, row[column])

class CsvDataCleaner:
    """
    Applies data cleansing functions to rows in a CSV file.
    """

    def __init__(self):
        self.replacement_patterns = []
        self.phone_formatter = GermanPhoneFormatter()

    def read_replacement_patterns(self, filename):
        """
        Read regular expressions from a file - one per line.
        """
        # Preserves the order of the replacement patterns to enable consequent substitutions.
        self.replacement_patterns = []

        with open(filename, 'r') as file:
            # Iterate over all lines in the file..
            for line in file.readlines():
                # Filter comments and empty lines..
                p = line.split('#')[0].strip('\n')
                
                if len(p):
                    self.replacement_patterns.append(ReplacementPattern(line))
            

    def clean(self, row):
        """
        Clean the values of a row. Returns the updated row.
        """
        for r in self.replacement_patterns:
            r.apply(row)

        for k in row.keys():
            v = row[k]
            
            if 'email' in k or k == 'replyto':
                v = v.lower()
            elif 'phone' in k:
                v = self.phone_formatter.format(v)
            elif 'country' in k:
                # Replace 'Deutschland' with 'Germany'.
                v = re.sub('deutschland', 'Germany', v, flags=re.IGNORECASE)
            elif k == 'faculty-rank':
                v = v.capitalize()
            elif k == 'title-1':
                v = re.sub('n/a', '', v, flags=re.IGNORECASE)
            elif k == 'suffix' and re.match('^[0-9]+$', v):
                v = ''
            elif k == 'level':
                # Replace negative levels with empty values.
                continue
            else:
                continue

            row[k] = v

        return row