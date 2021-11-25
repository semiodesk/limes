import csv
import io
import pkgutil
import re

from lib.csv_core import eprint

class GermanAddressFormatter:
    def __init__(self):
        self.prefixes = set()
        self.zip_to_prefixes = {}
        self.zip_to_city = {}
        self.zip_to_state = {}

        # Import the ZIP code file from the current module folder..
        data = pkgutil.get_data(__name__, 'german-zip-codes.csv').decode()

        for row in csv.DictReader(io.StringIO(data), delimiter=';'):
            p = row['Vorwahl'][1:]

            if len(p):
                self.prefixes.add(p)

                z = row['Plz']

                if not z in self.zip_to_prefixes.keys():
                    self.zip_to_prefixes[z] = set()

                self.zip_to_prefixes[z].add(p)
                self.zip_to_city[z] = row['Ort']
                self.zip_to_state[z] = row['Bundesland']

    def __format_phone_number(self, prefix, number, id=None, zip=None):
        """
        Return a formatted phone number string.
        """
        if len(prefix) == 0:
            print(f" [!] {id}: Undefined phone prefix in number {number}")

        if zip and zip in self.zip_to_prefixes.keys():
            prefixes = self.zip_to_prefixes[zip]

            if not prefix in prefixes:
                msg = f" [!] {id}: Phone prefix '{prefix}' does not match known prefixes for zip code '{zip}':"

                for p in prefixes:
                    msg += f" '{p}'"

                print(msg)

        return f"+49 {prefix} {number.strip('0')}"

    def format_phone(self, value, id=None, zip=None):
        """
        Parse, validate and return a formatted a German phone number.
        """
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
            return self.__format_phone_number(v[0:3], v[3:], id)

        # For land lines we start to search for the longest prefix match. The 
        # maximum length for a phone prefix in Germany is 5 digits excluding the leading 0.
        n = 5
        
        while n > 1:
            p = v[0:n]

            if(p in self.prefixes):
                return self.__format_phone_number(p, v[n:], id, zip)

            n -= 1

        if len(v) > 3:
            return self.__format_phone_number("", p, id, zip)
        else:
            return ""

    def format_city(self, value, zip, id=None):
        """
        Validate and return a formatted a German city name.
        """
        if not zip in self.zip_to_city.keys():
            print(f" [!] {id}: Unkown zip code: '{zip}'")
            return value

        v = value.replace('ae', 'ä')
        v = v.replace('oe', 'ö')
        v = v.replace('ue', 'ü')

        result = self.zip_to_city[zip]

        if len(v) and v.lower() != self.zip_to_city[zip].lower():
            print(f" [!] {id}: City name '{value}' dos not match known name for zip code '{zip}': '{result}'")
            return value
        
        # Return a uniform formatting of the city name.
        return result
            
    def format_state(self, value, zip, id=None):
        """
        Validate and return a formatted a German state name.
        """
        if not zip in self.zip_to_state.keys():
            print(f" [!] {id}: Unkown zip code: '{zip}'")
            return value

        v = value.replace('ae', 'ä')
        v = v.replace('oe', 'ö')
        v = v.replace('ue', 'ü')

        result = self.zip_to_state[zip]

        if len(v) and v.lower() != self.zip_to_state[zip].lower():
            print(f" [!] {id}: State name '{value}' dos not match known name for zip code '{zip}': '{result}'")
            return value
        
        # Return a uniform formatting of the state name.
        return result