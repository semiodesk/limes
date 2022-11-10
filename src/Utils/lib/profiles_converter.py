import os
import re
import binascii
from .csv_core import CsvReader, CsvWriter, fieldnames, rowdict

def internalusername(row):
    v = row['confirm-or-change-email-address'].encode()

    # This returns an unsinged 64-bit integer, however..
    h = binascii.crc32(v)

    # Profiles RNS expects a signed 32-bit integer if the internal username should be used as a primary key.
    # See: https://mail.python.org/pipermail/python-3000/2008-March/012615.html
    return h - ((h & 0x80000000) <<1)

def displayname(row):
    v = f"{row['first-name']} {row['middle-name']} {row['last-name']}"

    return re.sub('\s+', ' ', v).strip()

def addressstr(row):
    v = ""

    if len(row['street']): v += row['street']
    if len(row['number']): v += " " + row['number']
    if len(row['zip-1']): v += ", " + row['zip-1']
    if len(row['city']): v += " " + row['city']
    if len(row['country']): v += ", " + row['country']

    return v.strip()

def facultyrank(row):
    r = row['faculty-rank']
    s = row['specify-faculty-rank']

    if r and r.lower() == 'other' and s:
        return row['specify-faculty-rank']
    else:
        return row['faculty-rank']

facultyrankmapping = {
    'full professor': 0,
    'assistant professor': 1,
    'associate professor': 2,
    'junior professor': 3,
    'postdoc': 4,
    'other': 5
}

def facultyrankorder(rank):
    r = rank.lower()

    if r not in facultyrankmapping.keys():
        facultyrankmapping[r] = max(facultyrankmapping.values()) + 1

    return facultyrankmapping[r]

class Person:
    def __init__(self):
        self.Internalusername = ""
        self.Firstname = ""
        self.Middlename = ""
        self.Lastname = ""
        self.Displayname = ""
        self.Suffix = ""
        self.addressline1 = ""
        self.addressline2 = ""
        self.addressline3 = ""
        self.addressline4 = ""
        self.Addressstring = ""
        self.State = ""
        self.City = ""
        self.Zip = ""
        self.Building = ""
        self.Room = 0
        self.Floor = 0
        self.Latitude = ""
        self.Longitude = ""
        self.Phone = ""
        self.Fax = ""
        self.Emailaddr = ""
        self.Isactive = 1
        self.Isvisible = 1

class PersonWriter(CsvWriter):
    def __init__(self, filepath, fieldnames):
        super().__init__(filepath, fieldnames, delimiter=';', quotechar='|')

    def write(self, row):
        p = Person()
        p.Internalusername = internalusername(row)
        p.Firstname = row['first-name']
        p.Middlename = row['middle-name']
        p.Lastname = row['last-name']
        p.Displayname = displayname(row)
        p.Suffix = row['suffix']
        p.Addressstring = addressstr(row)
        p.addressline1 = p.Addressstring
        p.City = row['city']
        p.Zip = row['zip-1']
        p.Building = row['building'] if len(row['building']) > 0 else ' '
        p.Room = row['room'] if len(row['room']) > 0 else ' '
        p.Floor = row['floor']
        p.Phone = row['phone']
        p.Emailaddr = row['confirm-or-change-email-address']

        hide = row['hide-contact-information'].strip().lower()

        if hide == "true":
            p.Addressstring = " "
            p.addressline1 = " "
            p.City = " "
            p.Zip = " "
            p.Building = " "
            p.Room = " "
            p.Floor = " "
            p.Phone = " "

        super().write(rowdict(p))

class PersonAffiliation:
    def __init__(self):
        self.internalusername  =""
        self.title = ""
        self.emailaddr = ""
        self.primaryaffiliation = 1
        self.affiliationorder = 1
        self.institutionname = ""
        self.institutionabbreviation = ""
        self.departmentname = ""
        self.departmentvisible = 1
        self.divisionname = ""
        self.facultyrank = 0
        self.facultyrankorder = 0

class PersonAffiliationWriter(CsvWriter):
    def __init__(self, filepath, fieldnames):
        super().__init__(filepath, fieldnames, delimiter=';', quotechar='|')

    def write(self, row):
        a = PersonAffiliation()
        a.internalusername = internalusername(row)
        a.institutionname = row['affiliation'] if row['affiliation'] != 'other' else row['specify-affiliation']
        a.institutionabbreviation = a.institutionname[:50]
        a.departmentname = row['department']+f', {row["institute"]}' if row["institute"] != '' else row['department']
        a.divisionname = row['ngs-cc-affiliation']
        a.facultyrank = facultyrank(row)
        a.facultyrankorder = facultyrankorder(a.facultyrank)

        titles = [t for t in eval(row['title-1']) if t]
        titles.sort(reverse=True)

        if len(titles) > 0:
            a.title = ' '.join(titles)
        else:
            a.title = '-'

        super().write(rowdict(a))

class PersonFilterFlag:
    def __init__(self):
        self.Internalusername = ""
        self.Personfilter = ""

class PersonFilterFlagWriter(CsvWriter):
    def __init__(self, filepath, fieldnames):
        super().__init__(filepath, fieldnames, delimiter=';', quotechar='|')

    def write(self, row):
        f = PersonFilterFlag()
        f.Internalusername = internalusername(row)

        super().write(rowdict(f))

class ProfilesConverter:
    """
    Converts a CSV file into a dataset suitable for importing into Profiles RNS.
    """

    def __init__(self, output_directory, key_column):
        """
        Create a new converter instance.
        @param output_directory: Path to the output directory.
        @param key_column: Name of the column in the CSV file used to uniquely identify records.
        """
        self.output_directory = output_directory
        self.key_column = key_column

    def __writedata(self, input_filename):
        person_file = os.path.join(self.output_directory, 'Person.csv')
        person_writer = PersonWriter(person_file, fieldnames(Person()))

        affiliation_file = os.path.join(self.output_directory, 'PersonAffiliation.csv')
        affiliation_writer = PersonAffiliationWriter(affiliation_file, fieldnames(PersonAffiliation()))

        filter_file = os.path.join(self.output_directory, 'PersonFilterFlag.csv')
        filter_writer = PersonFilterFlagWriter(filter_file, fieldnames(PersonFilterFlag()))

        input_reader = CsvReader(input_filename)

        for row in input_reader:
            person_writer.write(row)
            affiliation_writer.write(row)
            filter_writer.write(row)

    def __writekeyphrases(self, input_filename):
        keyphrases_file = os.path.join(self.output_directory, 'keyphrases.txt')
        
        keyphrases_reader = CsvReader(input_filename)
        output_writer = CsvWriter(keyphrases_file, fieldnames=['value'], writeheader=False, quotechar=None)

        keyphrases = set()

        for row in keyphrases_reader:
            value = row[self.key_column]

            for match in re.finditer('%+(?P<phrase>.+?)%+', value):
                phrase = match.group('phrase').strip().lower()

                if len(phrase) > 0:
                    keyphrases.add(phrase)

        person_file = os.path.join(self.output_directory, 'Person.csv')
        person_reader = CsvReader(person_file, delimiter=';')

        for row in person_reader:
            email = row['Emailaddr']

            if len(email) > 0:
                host = email.split('@')[1]
                
                if len(host) > 0:
                    keyphrases.add(host.strip())

        keyphrases = self.__optimize(keyphrases)
        keyphrases.sort()

        for phrase in keyphrases:
            print(phrase)
            output_writer.write({"value": f"%{phrase}%"})

    def __optimize(self, keyphrases:set):
        result = set()

        for phrase in keyphrases:
            # Replace whitespace with wildcard character.
            p = phrase.replace(' ', '_')
            # Remove the delimitar char from any values, if present.
            p = p.replace(',', '')
            p = p.replace('_for_', '%')
            p = p.replace('_of_', '%')
            p = p.replace('_&_', '%')
            p = p.replace('&', '')
            p = p.replace('-', '_')
            # Normalize different spellings.
            p = p.replace('ä', '_')
            p = p.replace('ae', '_')
            p = p.replace('ö', '_')
            p = p.replace('oe', '_')
            p = p.replace('ü', '_')
            p = p.replace('ue', '_')
            # Replace possible suffixes such as 'of' with wildcards.
            p = p.replace('hospital_', 'hospital%')
            p = p.replace('clinic_', 'clinic%')
            p = p.replace('_university_', '_university%')

            result.add(p)

        return list(result)

    def __areequal(self, phraseA, phraseB):
        a = phraseA if len(phraseA) >= len(phraseB) else phraseB
        b = phraseB if len(phraseA) >= len(phraseB) else phraseA

    def execute(self, input_filename):
        """
        Execute the conversion process on a file and write the result to the specified output directory.
        @param input_filename: Path to the input CSV file.
        """
        if not os.path.isfile(input_filename):
            raise Exception(f"File not found: {input_filename}")

        if self.output_directory is None:
            self.output_directory = "."

        if not os.path.isdir(self.output_directory):
            raise Exception(f"Directory not found: {self.output_directory}")

        self.__writedata(input_filename)
        self.__writekeyphrases(input_filename)
