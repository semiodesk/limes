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

class DataCleaner:
    def __init__(self):
        self.phone_formatter = GermanPhoneFormatter()

    """
    Clean the values of a row.
    """
    def clean(self, row):
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