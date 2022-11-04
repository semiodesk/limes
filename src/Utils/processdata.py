#!/usr/bin/env python3
from lib import *

# Filter all columns that have empty first or last names.
filter_values = {
    "first-name": "",
    "last-name": ""
}

# Default values for empty columns.
default_values = {
    "ngs-cc-affiliation": "WGGC",
    "hide-contact-information": False
}

# Merge the values of the first column into the second.
merge_columns = {
    "buidling": "building",
    "primary-affiliation": "affiliation",
    "specify-affiliation-1": "specify-affiliation",
    "your-role-s-at-the-wggc": "your-role-s-at-the-ngs-cc",
    "specify-your-role-s-at-the-wggc": "specify-your-role-s",
    "specify-city": "city",
    "you-agree-to-publish-your-data-on-the-ngs-cn-profiles-web-portal-if-affiliated-with-wggc-also-on-the-wggc-profiles-web-portal": "you-agree-to-our-terms-and-policy"
}

# Create empty working directories.
tmp_dir = setupdir("tmp")
output_dir = setupdir("output")

# Column to be used to identify and match records.
key_column = "confirm-or-change-email-address"

# Align primary key column names in input files.
replace("input/*.csv", "confirm-or-change-your-email-address", key_column)

# Execute the data transformations.
clean = CsvCleanProcess(tmp_dir, key_column)
merge = CsvMergeProcess(tmp_dir, key_column)
filter = CsvFilterProcess(tmp_dir, key_column)

round1 = clean.execute("input/WGGC2019_round1.csv")
round2 = clean.execute("input/WGGC2019_round2.csv")
round3 = clean.execute("input/WGGC2019_round3.csv")
rounds = merge.execute(round1, round2)
rounds = merge.execute(rounds, round3)
rounds = filter.execute(rounds, include_filename="input/curated_WGGC_profiles_2019_emails.txt")

form_full = clean.execute("input/ngscn2022_full_forms.csv")
form_redux = clean.execute("input/ngscn2022_reduced_forms.csv")
forms = merge.execute(form_full, form_redux, merge_columns, default_values)

result = merge.execute(forms, rounds, merge_columns, default_values)
result = clean.execute(result)
result = filter.execute(result, exclude_filename="input/remove_from_webportal.txt", filter_values=filter_values)

# Finally, convert the result into a dataset for ProfilesRNS.
converter = ProfilesConverter(output_dir, key_column="affiliation-strings-for-searches-in-pubmed")
converter.execute(result)