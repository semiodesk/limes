using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfilesRNS_Manager
{
    public class TableIndex : Dictionary<string, string>
    {
        #region Members

        /// <summary>
        /// Get or set the column names of the database or CSV table.
        /// </summary>
        public string[] ColumnNames { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Get the data source URL.
        /// </summary>
        public string DataSource { get; set; }

        /// <summary>
        /// Stores keys which should be deleted.
        /// </summary>
        private readonly HashSet<string> _deletedKeys = new ();

        #endregion

        #region Methods

        public void DeleteKeys(IEnumerable<string> keys)
        {
            foreach (string key in keys)
            {
                _deletedKeys.Add (key);
            }
        }

        public Dictionary<int, int> GetColumnMap(TableIndex otherIndex)
        {
            // Normalize the column names of the local index.
            string[] otherColumns = otherIndex.ColumnNames.Select(k => k.ToLower()).ToArray();

            // Create a map from the local column positions to the position in the imported index.
            Dictionary<int, int> map = new();

            for (int i = 0; i < ColumnNames.Length; i++)
            {
                string c = ColumnNames[i].ToLower();

                map[i] = Array.IndexOf(otherColumns, c);
            }

            return map;
        }

        public Dictionary<int, int> GetColumnMap()
        {
            Dictionary<int, int> map = new();

            for (int i = 0; i < ColumnNames.Length; i++)
            {
                map[i] = i;
            }

            return map;
        }

        public string[] GetData(string data)
        {
            return data.Split(';').ToArray();
        }

        public string[] GetMappedData(string data, Dictionary<int, int> map)
        {
            string[] values = data.Split(';').ToArray();

            string[] result = new string[values.Length];

            // We map the local columns to the imported data because the imported data may have
            // more columns than the local index (i.e. a deleted column).
            for (int i = 0; i < ColumnNames.Length; i++)
            {
                result[i] = values[map[i]];
            }

            return result;
        }

        public IEnumerable<string> GetDeletedKeys()
        {
            int n = Array.IndexOf(ColumnNames.Select(k => k.ToLower()).ToArray(), "delete");

            if(n > 0)
            {
                foreach(string key in Keys)
                {
                    if (!_deletedKeys.Contains(key))
                    {
                        string v = GetData(this[key])[n].Trim().ToLower();

                        if (v == "1" || v == "true")
                        {
                            yield return key;
                        }
                    }
                }
            }

            foreach(string key in _deletedKeys)
            {
                yield return key;
            }
        }

        public TableIndex Normalize()
        {
            foreach (var key in Keys)
            {
                // The data from the SQL server differs from the imported data in the following aspects:
                //
                // - Boolean values are ("True"/"False) whereas the imported values are ("1"/"0")
                // - There might be empty " " strings for empty columns so that ProfilesRNS still displays the non-empty data
                this[key] = string.Join(";", this[key]
                    .Split(';')
                    .Select(v => v.Replace("True", "1").Replace("False", "0"))
                    .Select(v => string.IsNullOrEmpty(v) ? " " : v));
            }

            return this;
        }

        #endregion
    }
}
