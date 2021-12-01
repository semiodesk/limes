from . import csv_core
import re
import binascii

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

class PersonWriter(csv_core.CsvWriter):
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

        super().write(csv_core.rowdict(p))

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

class PersonAffiliationWriter(csv_core.CsvWriter):
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

        super().write(csv_core.rowdict(a))

class PersonFilterFlag:
    def __init__(self):
        self.Internalusername = ""
        self.Personfilter = ""

class PersonFilterFlagWriter(csv_core.CsvWriter):
    def __init__(self, filepath, fieldnames):
        super().__init__(filepath, fieldnames, delimiter=';', quotechar='|')

    def write(self, row):
        f = PersonFilterFlag()
        f.Internalusername = internalusername(row)

        super().write(csv_core.rowdict(f))