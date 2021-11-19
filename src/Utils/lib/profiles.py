from . import csv_core
import re

def internalusername(row):
    v = f"{row['first-name']}{row['last-name']}"

    return v.lower().strip()

def displayname(row):
    v = f"{row['first-name']} {row['middle-name']} {row['last-name']}"

    return re.sub('\s+', ' ', v).strip()

def addressstr(row):
    v = ""

    if len(row['street']): v += row['street']
    if len(row['number']): v += " " + row['number']
    if len(row['city']): v += ", " + row['city']
    if len(row['zip-1']): v += ", " + row['zip-1']

    return v.strip()

def facultyrank(row):
    if row['faculty-rank'] == 'Other' and row['specify-faculty-rank'] != '':
        return row['specify-faculty-rank']
    else:
        return row['faculty-rank']

def facultyrankorder(rank):
    mapping = {
        'Full Professor': 0,
        'Assistant Professor': 1,
        'Associate Professor': 2,
        'Junior Professor': 3,
        'Postdoc': 4,
        'Other': 5
    }

    if rank not in mapping.keys():
        mapping[rank] = max(mapping.values())+1

    return mapping[rank]

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
        p.City = row['city']
        p.Zip = row['zip-1']
        p.Building = row['building'] if row['building'] != '' else '-'
        p.Room = row['room']
        p.Floor = row['floor']
        p.Phone = row['phone']
        p.Emailaddr = row['confirm-or-change-email-address']

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
        a.title = ' '.join(eval(row['title-1'])) if len(row['title-1']) else ''
        a.institutionname = row['affiliation'] if row['affiliation'] != 'other' else row['specify-affiliation']
        a.departmentname = row['department']+f', {row["institute"]}' if row["institute"] != '' else row['department']
        a.facultyrank = facultyrank(row)
        a.facultyrankorder = facultyrankorder(a.facultyrankorder)

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