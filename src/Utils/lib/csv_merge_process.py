import os
import pathlib
from lib.csv_core import CsvReader, CsvWriter

class CsvMergeProcess:
    """
    A process to merge the rows of two CSV files into a single file.
    """
    def __init__(self, output_directory, key_column):
        """
        Create a new process instance.
        @param output_directory: Path to the output directory.
        @param key_column: Name of the column in the CSV file used to uniquely identify records.
        """
        self.output_directory = output_directory
        self.key_column = key_column

    def __getmapping(self, merge_columns, fieldname):
        return merge_columns[fieldname] if fieldname in merge_columns.keys() else fieldname

    def __getdefault(self, default_values, fieldname):
        return default_values[fieldname] if fieldname in default_values.keys() else ''

    def __get_output_filename(self, input_filenameA, input_filenameB):
        nameA = pathlib.Path(input_filenameA).name.split(".")[0]
        nameB = pathlib.Path(input_filenameB).name.split(".")[0]

        return f"{nameA}_{nameB}.csv"

    def execute(self, input_filenameA, input_filenameB, merge_columns = dict(), default_values = dict(), output_filename = ""):
        """
        Execute the process on input files. If there are conflicting values in both files with the same primary key, then the values in the second file will overwrite the values found in the first file.
        @param input_filenameA: Path to the source input CSV file.
        @param input_filenameB: Path to the target input CSV file.
        @param merge_columns: A dictionary mapping columns in the input file to columns in the output file.
        @param default_values: A dictionary mapping column names to default values.
        @param output_filename: Optional path to the output CSV file; will be generated if omitted.
        @return: Name of the output file.
        """
        if not os.path.isfile(input_filenameA):
            raise Exception(f"File not found: {input_filenameA}")

        if not os.path.isfile(input_filenameB):
            raise Exception(f"File not found: {input_filenameB}")

        if len(output_filename) == 0:
            output_filename = self.__get_output_filename(input_filenameA, input_filenameB)

        if self.output_directory is None:
            self.output_directory = "."

        if not os.path.isdir(self.output_directory):
            raise Exception("Directory not found: " + self.output_directory)
            
        output_path = os.path.join(self.output_directory, output_filename)

        if os.path.isdir(output_path):
            raise Exception(f"Output file must not be a directory: {output_path}")

        if os.path.isfile(output_path):
            raise Exception(f"Output file alread exists: {output_path}")

        if not isinstance(merge_columns, dict):
            raise Exception("Argument merge_columns must be a dictionary.")

        if not isinstance(default_values, dict):
            raise Exception("Argument default_values must be a dictionary.")

        print(f"Merging {input_filenameA} into {input_filenameB}")

        # Merge the fieldnames of the two files considering the mapping specifications.
        fieldnames = []

        input_readerA = CsvReader(input_filenameA, self.key_column)

        for field in [self.__getmapping(merge_columns, f) for f in input_readerA.fieldnames]:
            if not field in fieldnames:
                fieldnames.append(field)

        input_readerB = CsvReader(input_filenameB, self.key_column)

        for field in [self.__getmapping(merge_columns, f) for f in input_readerB.fieldnames]:
            if not field in fieldnames:
                fieldnames.append(field)

        output_writer = CsvWriter(output_path, fieldnames)

        # Read the rows from file B into a dictionary for quick access in the following loop.
        # Note that this will not scale well to very large files.
        rowsB = {}

        for rB in input_readerB:
            id = rB[self.key_column].lower()
            r = {}

            for field in rB.keys():
                r[self.__getmapping(merge_columns, field)] = rB[field]
            
            rowsB[id] = r

        # Read the rows from file A and write values from file B into the rows, if there is any data.
        for rA in input_readerA:
            # Read the normalized primary key of the row.
            id = rA[self.key_column].lower()

            # Set values for all fields from B that are not in A..
            for field in fieldnames:
                if not field in rA.keys():
                    rA[field] = self.__getdefault(default_values, field)

            # Merge values for mapped columns.
            for field in [x for x in rA.keys()]:
                if field in merge_columns.keys():
                    v0 = rA[field]
                    v1 = rA[self.__getmapping(merge_columns, field)]

                    # Only overwrite the existing column if there is a value in the mapped column.
                    if len(v0) and len(v1) == 0:
                        rA[self.__getmapping(merge_columns, field)] = v0

                    del rA[field]

            # If there is an entry in file B, merge it into A and delete the entry from the cache.
            if id in rowsB.keys():
                rB = rowsB[id]

                for field in rB:
                    rA[field] = rB[field]

                del rowsB[id]

            output_writer.write(rA)

        # If there are any entries in file B that do not appear in A, append them to the file..
        for id in rowsB.keys():
            rA = {}
            rB = rowsB[id]

            for field in fieldnames:
                rA[field] = self.__getdefault(default_values, field)

            for field in rB.keys():
                rA[field] = rB[field]

            output_writer.write(rA)

        return output_path
