#! /bin/bash

# Cleanup the working directory and ensure that it exists..
rm -Rf tmp
mkdir tmp

# Clean the input data before merging..
python csv-clean.py \
    -i input/wggc2019_round1.csv \
    -o tmp/wggc2019_round1.clean.csv \
    -k confirm-or-change-email-address \
    -p input/patches.txt \
    -p input/emails.txt \
    2> tmp/wggc2019_round1.dups.csv

python csv-clean.py \
    -i input/wggc2019_round2.csv \
    -o tmp/wggc2019_round2.clean.csv \
    -k confirm-or-change-email-address \
    -p input/patches.txt \
    -p input/emails.txt \
    2> tmp/wggc2019_round2.dups.csv

python csv-clean.py \
    -i input/ngscn2021_full_form_20211117.csv \
    -o tmp/ngscn2021_full_form_20211117.clean.csv \
    -k confirm-or-change-email-address \
    -p input/patches.txt \
    -p input/emails.txt \
    2> tmp/ngscn2021_full_form_20211117.dups.csv

python csv-clean.py \
    -i input/ngscn2021_reduced_form_20211117.csv \
    -o tmp/ngscn2021_reduced_form_20211117.clean.csv \
    -k confirm-or-change-email-address \
    -p input/patches.txt \
    -p input/emails.txt \
    2> tmp/ngscn2021_reduced_form_20211117.dups.csv

# Merge the input data files..
python csv-merge.py \
    -ia tmp/wggc2019_round1.clean.csv \
    -ib tmp/wggc2019_round2.clean.csv \
    -o tmp/wggc2019.merge.csv \
    -k confirm-or-change-email-address

python csv-filter.py \
    -i tmp/wggc2019.merge.csv \
    -o tmp/wggc2019.filter.csv \
    -f input/wggc2019_curated_profiles_emails.txt \
    -k confirm-or-change-email-address

python csv-merge.py \
    -ia tmp/ngscn2021_full_form_20211117.clean.csv \
    -ib tmp/ngscn2021_reduced_form_20211117.clean.csv \
    -o tmp/ngscn2021.merge.csv \
    -k confirm-or-change-email-address \
    -d ngs-cc-affiliation:'WGGC' \
    -d hide-contact-information:False \
    -m buidling:building \
    -m primary-affiliation:affiliation \
    -m specify-affiliation-1:specify-affiliation \
    -m your-role-s-at-the-wggc:your-role-s-at-the-ngs-cc \
    -m specify-your-role-s-at-the-wggc:specify-your-role-s \
    -m specify-city:city \
    -m you-agree-to-publish-your-data-on-the-ngs-cn-profiles-web-portal-if-affiliated-with-wggc-also-on-the-wggc-profiles-web-portal:you-agree-to-our-terms-and-policy

python csv-merge.py \
    -ia tmp/ngscn2021.merge.csv \
    -ib tmp/wggc2019.filter.csv \
    -o tmp/ngscn2021.merge.1.csv \
    -k confirm-or-change-email-address \
    -d ngs-cc-affiliation:'WGGC' \
    -d hide-contact-information:False \
    -m buidling:building \
    -m primary-affiliation:affiliation \
    -m specify-affiliation-1:specify-affiliation \
    -m your-role-s-at-the-wggc:your-role-s-at-the-ngs-cc \
    -m specify-your-role-s-at-the-wggc:specify-your-role-s \
    -m specify-city:city \
    -m you-agree-to-publish-your-data-on-the-ngs-cn-profiles-web-portal-if-affiliated-with-wggc-also-on-the-wggc-profiles-web-portal:you-agree-to-our-terms-and-policy

# Apply filters..
python csv-filter.py \
    -i tmp/ngscn2021.merge.1.csv \
    -o tmp/ngscn2021.csv \
    -k confirm-or-change-email-address \
    -f input/remove_from_webportal.txt \
    -e true \
    -v first-name: \
    -v last-name:

# Convert to ProfilesRNS data..
python csv-convert.py \
    -i tmp/ngscn2021.csv \
    -o output

python csv-keyphrases.py \
    -i tmp/ngscn2021.csv \
    -o output/keyphrases.txt \
    -k affiliation-strings-for-searches-in-pubmed