import os
import pathlib
from lib.csv_core import CsvReader, CsvWriter

class CsvFilterProcess:
    """
    A process to remove rows from a CSV file.
    """
    exclude_ids = set()
    """Ids to be excluded from the output."""

    include_ids = set()
    """Ids to be included in the output."""

    def __init__(self, output_directory, key_column):
        """
        Create a new process instance.
        @param output_directory: Path to the output directory.
        @param key_column: Name of the column in the CSV file used to uniquely identify records.
        """
        self.output_directory = output_directory
        self.key_column = key_column

    def __get_output_filename(self, filename):
        return pathlib.Path(filename).name.split(".")[0] + ".filter.csv"

    def __hasFilteredValue(self, filtered_values, row):
        for field in row.keys():
            if field in filtered_values.keys():
                if filtered_values[field] == row[field]:
                    return True

        return False

    def __isFilteredId(self, id):
        if len(self.exclude_ids):
            return id in self.exclude_ids
        elif len(self.include_ids):
            return id not in self.include_ids
        else:
            return False

    def execute(self, input_filename, include_filename = "", exclude_filename = "", filter_values = dict(), output_filename = ""):
        """
        Execute the filtering process on a file.
        @param input_filename: Path to the input CSV file.
        @param include_filename: A file containing the ids of records to add to the generated output; all other ids will be removed.
        @param exclude_filename: A file containing the ids of records to remove from the generated output; all other ids will be added.
        @param filter_values: A dictionary mapping column names to values. Rows with matching cell values will be removed.
        @param output_filename: Optional path to the output CSV file; will be generated if omitted.
        @return: Name of the output file.
        """
        if not os.path.isfile(input_filename):
            raise Exception(f"File not found: {input_filename}")

        if len(output_filename) == 0:
            output_filename = self.__get_output_filename(input_filename)

        if self.output_directory is None:
            self.output_directory = "."

        if not os.path.isdir(self.output_directory):
            raise Exception("Directory not found: " + self.output_directory)

        output_path = os.path.join(self.output_directory, output_filename)

        if os.path.isdir(output_path):
            raise Exception(f"Output file must not be a directory: {output_path}")

        if os.path.isfile(output_path):
            raise Exception(f"Output file already exists: {output_path}")

        if len(exclude_filename) > 0:
            if not os.path.isfile(exclude_filename):
                raise Exception(f"File not found: {exclude_filename}")

            self.exclude_ids = set([id.lower().strip() for id in open(exclude_filename, 'r') if id != '\n'])

        if len(include_filename) > 0:
            if not os.path.isfile(include_filename):
                raise Exception(f"File not found: {include_filename}")

            self.include_ids = set([id.lower().strip() for id in open(include_filename, 'r') if id != '\n'])

        print(f"Filtering {input_filename}")

        input_reader = CsvReader(input_filename, self.key_column)
        output_writer = CsvWriter(output_path, input_reader.fieldnames)

        for r in input_reader:
            id = r[self.key_column].lower().strip()

            if self.__isFilteredId(id):
                print(f" [-] {id}")
            elif self.__hasFilteredValue(filter_values, r):
                print(f" [!] {id}")
            else:
                output_writer.write(r)

        return str(output_path)
        