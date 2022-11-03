#! /bin/bash

# Cleanup the working directory and ensure that it exists..
rm -Rf tmp
mkdir tmp

### For each update we need:
######  all WGGC old files (to be matched with the reduced form)
###### only the latest full+reduced ngs-cn forms
update="${1:-20221031}"

# Adjust keyword in input files
sed -i 's/confirm-or-change-your-email-address/confirm-or-change-email-address/g' input/*.csv

# Clean the input data before merging..
python csv-clean.py \
    -i input/wggc2019_round1.csv \
    -o tmp/wggc2019_round1.clean.csv \
    -k confirm-or-change-email-address \
    -c profiles-rns \
    2> tmp/wggc2019_round1.dups.csv

python csv-clean.py \
    -i input/wggc2019_round2.csv \
    -o tmp/wggc2019_round2.clean.csv \
    -k confirm-or-change-email-address \
    -c profiles-rns \
    2> tmp/wggc2019_round2.dups.csv

python csv-clean.py \
    -i input/ngscn2022_full_form_${update}.csv \
    -o tmp/ngscn2022_full_form_${update}.clean.csv \
    -k confirm-or-change-email-address \
    -c profiles-rns \
    2> tmp/ngscn2022_full_form_${update}.dups.csv

python csv-clean.py \
    -i input/ngscn2022_reduced_form_${update}.csv \
    -o tmp/ngscn2022_reduced_form_${update}.clean.csv \
    -k confirm-or-change-email-address \
    2> tmp/ngscn2022_reduced_form_${update}.dups.csv

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
    -ia tmp/ngscn2022_full_form_${update}.clean.csv \
    -ib tmp/ngscn2022_reduced_form_${update}.clean.csv \
    -o tmp/ngscn2022.merge.csv \
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
    -ia tmp/ngscn2022.merge.csv \
    -ib tmp/wggc2019.filter.csv \
    -o tmp/ngscn2022.merge.1.csv \
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

python csv-clean.py \
    -i tmp/ngscn2022.merge.1.csv \
    -o tmp/ngscn2022.clean.1.csv \
    -k confirm-or-change-email-address \
    -c profiles-rns

# Apply filters..
python csv-filter.py \
    -i tmp/ngscn2022.clean.1.csv \
    -o tmp/ngscn2022.csv \
    -k confirm-or-change-email-address \
    -f input/remove_from_webportal.txt \
    -e true \
    -v first-name: \
    -v last-name:

# Convert to ProfilesRNS data..
python csv-convert.py \
    -i tmp/ngscn2022.csv \
    -o output

python csv-keyphrases.py \
    -i tmp/ngscn2022.csv \
    -o output/keyphrases.txt \
    -k affiliation-strings-for-searches-in-pubmed
