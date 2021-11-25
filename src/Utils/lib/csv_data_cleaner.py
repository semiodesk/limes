from lib.substitution import Substitution

class CsvDataCleaner:
    """
    Applies data cleansing functions to rows in a CSV file.
    """

    def __init__(self, key_column=None):
        self.key_column = key_column
        self.substitutions = []

    def substitute(self, pattern, replace, rows=None, flags=None):
        self.substitutions.append(Substitution(pattern, replace, rows, flags))

    def clean_row(self, row):
        """
        Clean the values of a row and return the updated row.
        """
        id = None
        
        if self.key_column:
            id = row[self.key_column]

        for k in row.keys():
            v = row[k]

            for s in self.substitutions:
                v = s.apply(k, v)

            row[k] = self.clean_row_value(id, row, k, v)

        return row

    def clean_row_value(self, id, row, column, value):
        """
        Clean the value for a given column and return the new value.
        """
        return value