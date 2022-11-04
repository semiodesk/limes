import os
import pathlib
from lib.csv_core import CsvReader, CsvWriter
from lib.csv_data_cleaner_factory import CsvDataCleanerFactory

class CsvCleanProcess:
    """
    A process to apply data cleansing functions to rows in a CSV file.
    """

    def __init__(self, output_directory, key_column, cleaner_id = "profiles-rns"):
        """
        Create a new process instance.
        @param output_directory: Path to the output directory.
        @param key_column: Name of the column in the CSV file used to uniquely identify records.
        @param cleaner_id: Identifier of the CSV cleaner type to be used.
        """
        self.output_directory = output_directory
        self.key_column = key_column
        self.cleaner_id = cleaner_id

    def __get_output_filename(self, filename):
        return pathlib.Path(filename).name.split(".")[0] + ".clean.csv"

    def __get_duplicates_filename(self, filename):
        return pathlib.Path(filename).name.split(".")[0] + ".dups.csv"

    def execute(self, input_filename, output_filename = "", duplicates_filename = ""):
        """
        Execute the process on a file.
        @param input_filename: Path to the input CSV file.
        @param output_filename: Path to the output CSV file; will be generated if omitted.
        @param duplicates_filename: Path to the CSV file containing the found duplicate rows; will be generated if omittted.
        @return: Name of the output file.
        """
        if not os.path.isfile(input_filename):
            raise Exception(f"File not found: {input_filename}")

        if len(output_filename) == 0:
            output_filename = self.__get_output_filename(input_filename)

        if len(duplicates_filename) == 0:
            duplicates_filename = self.__get_duplicates_filename(input_filename)

        if self.output_directory is None:
            self.output_directory = "."

        if not os.path.isdir(self.output_directory):
            raise Exception("Directory not found: " + self.output_directory)

        output_path = os.path.join(self.output_directory, output_filename)
        duplicates_path = os.path.join(self.output_directory, duplicates_filename)

        if len(output_path) == 0:
            raise Exception(f"Output file name must not be empty.")

        if os.path.isdir(output_path):
            raise Exception(f"Output file must not be a directory: {output_path}")

        if os.path.isfile(output_path):
            raise Exception(f"Output file already exists: {output_path}")

        if len(duplicates_path) == 0:
            raise Exception(f"Duplicates file name must not be empty.")

        if os.path.isdir(duplicates_path):
            raise Exception(f"Duplicates file must not be a directory: {duplicates_path}")

        if os.path.isfile(duplicates_path):
            raise Exception(f"Duplicates file already exists: {duplicates_path}")

        print(f"Cleaning {input_filename}")

        input_reader = CsvReader(input_filename, self.key_column)
        input_cleaner = CsvDataCleanerFactory().get_cleaner(self.cleaner_id, self.key_column)

        output_writer = CsvWriter(output_path, input_reader.fieldnames)
        duplicates_writer = CsvWriter(duplicates_path, input_reader.fieldnames)

        self.rows = {}
        self.duplicates = {}

        for row in input_reader:
            # Invoke the cleansing operations..
            row = input_cleaner.clean_row(row)

            # Read the primary key value *after* the row was cleaned.
            id = row[self.key_column]

            # Instead of immediatly writing out the rows, we first process them all
            # to detect duplicates.
            if id in self.rows.keys():
                if id not in self.duplicates.keys():
                    self.duplicates[id] = [self.rows[id]]

                self.duplicates[id].append(row)

            # Consider the last occurance of a row to be the most recent.
            self.rows[id] = row

        # After we processed all the data we can write it to the output.
        for row in self.rows.values():
            output_writer.write(row)

        # Write the duplicates to the error output.
        for id in self.duplicates.keys():
            for row in self.duplicates[id]:
                duplicates_writer.write(row)

        return str(output_path)
        