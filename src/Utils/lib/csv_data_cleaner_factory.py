from lib.profiles_data_cleaner import ProfilesDataCleaner
from lib.csv_data_cleaner import CsvDataCleaner

class CsvDataCleanerFactory:
    def get_cleaner(self, id, key_column):
        if id == "profiles-rns":
            return ProfilesDataCleaner(key_column)
        elif len(id) > 0:
            raise Exception(f"Error: Unkown data cleaner id: '{id}'")
        else:
            return CsvDataCleaner(key_column)