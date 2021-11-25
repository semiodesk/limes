import re

class Substitution:
    """
    A value replacement based on regular expressions that will be applied on the values of a data row.
    """

    def __init__(self, pattern, replace, rows=None, flags=None):
        """
        Initialize a new substitution pattern.
        """
        if flags:
            self.regex = re.compile(pattern, flags)
        else:
            self.regex = re.compile(pattern)

        self.replace = replace

        if rows:
            self.columns = set(rows)
        else:
            self.columns = set()

    def apply(self, column, value):
        """
        Invoke the regular expression substitution on a column value and return the modified value.
        """
        if len(self.columns) == 0 or column in self.columns:
            return self.regex.sub(self.replace, value)
        else:
            return value