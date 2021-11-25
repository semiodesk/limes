import re

from lib.csv_data_cleaner import CsvDataCleaner
from lib.german_address_formatter import GermanAddressFormatter

class ProfilesDataCleaner(CsvDataCleaner):
    """
    A special data cleaner for Profiles RNS datasets.
    """

    def __init__(self, key_column):
        super().__init__(key_column)

        # Initialize the address formatter here *once* because it reads a large database file.
        self.address_formatter = GermanAddressFormatter()

        # NOTE: The substitutions will be executed in the order they are registered:
        # Remove leading and trailing whitespaces from all values..
        self.substitute("^\s*", "")
        self.substitute("\s*$", "")

        # Replace 'Deutschland' with Germany for postal addresses
        self.substitute("deutschland", "Germany", flags=re.IGNORECASE)

        # Use German city names so that they can be used as a post address.
        self.substitute("cologne", "Köln", flags=re.IGNORECASE)

        # Fix common misspellings..
        self.substitute("str\.?$","straße")
        self.substitute("strasse","straße")
        self.substitute("Str\.?$","Straße")
        self.substitute("Strasse","Straße")
        self.substitute("auewelstr","auwelsstr")
        self.substitute("niversitaet","niversität")
        self.substitute("nstitute of","nstitute for")
        self.substitute("\s?[0-9]*$","",rows=["street"])
        self.substitute("uelpicher","ülpicher",rows=["street"])
        self.substitute("Am botanischen Garten","Am Botanischen Garten",rows=["street"])
        self.substitute("Am [bB]otanischer Garten","Am Botanischen Garten",rows=["street"])
        self.substitute("Carl Troll Straße","Carl-Troll-Straße",rows=["street"])
        self.substitute("Venusberg Campus","Venusberg-Campus",rows=["street"])

        # Replace 'n/a' with an empty value..
        self.substitute("n\/a", "", rows=["title-1"], flags=re.IGNORECASE)
        self.substitute("^\-$", "", rows=["room"], flags=re.IGNORECASE)

        # Remove telephone numbers from suffixes..
        self.substitute("^[0-9]+$", "", rows=["suffix"])

        # Fix wrong email addresses..
        self.substitute("mnothnag@uni-koeln.de", "michael.nothnagel@uni-koeln.de", flags=re.IGNORECASE)
        self.substitute("schwender@math.uni-duesseldorf.de", "holger.schwender@hhu.de", flags=re.IGNORECASE)

    def clean_row_value(self, id, row, column, value):
        v = super().clean_row_value(id, row, column, value)

        if 'email' in column:
            # Make all email addresses lower case.
            return v.lower()
        elif column == 'replyto':
            if len(v.strip()) == 0:
                return row['confirm-or-change-email-address'].lower()
            else:
                return v.lower()
        elif 'phone' in column:
            # Normalize the phone number formats.
            return self.address_formatter.format_phone(v, id=id, zip=row['zip-1'])
        elif column == 'city':
            return self.address_formatter.format_city(v, row['zip-1'], id=id)
        elif column == 'faculty-rank':
            return v.capitalize()
        else:
            return v