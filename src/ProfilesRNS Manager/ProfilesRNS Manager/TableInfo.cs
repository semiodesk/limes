using System;
using System.Collections.Generic;
using System.Linq;

namespace ProfilesRNS_Manager
{
    public class TableInfo
    {
        #region Methods

        public TableIndex Index { get; private set; }

        public string DisplayName { get; set; }

        public string TableName { get; set; }

        public int CreateRowCount { get; set; }

        public int UpdateRowCount { get; set; }

        public int DeleteRowCount { get; set; }

        public int TotalRowCount
        {
            get => Index != null ? Index.Count : 0;
        }

        public bool HasChanges
        {
            get => CreateRowCount > 0 || UpdateRowCount > 0 || DeleteRowCount > 0;
        }

        #endregion

        #region Constructors

        public TableInfo(string tableName, TableIndex rowIndex, string displayName = "")
        {
            DisplayName = displayName;
            TableName = tableName;

            Index = rowIndex.Normalize();
        }

        #endregion

        #region Methods

        public void Clear()
        {
            Index.Clear();
            CreateRowCount = 0;
            UpdateRowCount = 0;
            DeleteRowCount = 0;
        }

        public void ImportData(TableIndex otherIndex, bool mapColumns = true)
        {
            if(otherIndex == null)
            {
                throw new ArgumentNullException("otherIndex");
            }

            // Match the order of the imported column names with the ones from the db.
            Dictionary<int, int> columnMap;

            if(mapColumns)
            {
                columnMap = Index.GetColumnMap(otherIndex);
            }
            else
            {
                columnMap = Index.GetColumnMap();
            }

            // The keys are the primary keys of the dataset mapped to the data in CSV format.
            HashSet<string> commonKeys = new (Index.Keys);
            commonKeys.IntersectWith(otherIndex.Keys);

            foreach(var key in commonKeys)
            {
                // We need to look at the data column by column because there are minor differences in
                // the storage format of the data on the server in comparison to the inupt (i.e. Booleans
                // are True/False in the DB and 1/0 in the dataset)..
                string[] V0 = Index.GetData(Index[key]);
                string[] V1 = Index.GetMappedData(otherIndex[key], columnMap);

                for(int i = 0; i < V0.Length; i++)
                {
                    string v0 = V0[i];
                    string v1 = V1[i];

                    if(!Equals(v0, v1))
                    {
                        Index[key] = string.Join(";", V1).TrimEnd(';');

                        UpdateRowCount += 1;

                        break;
                    }
                }
            }

            string[] deleted = otherIndex.GetDeletedKeys().ToArray();

            HashSet<string> deletedKeys = new (deleted);
            deletedKeys.IntersectWith(Index.Keys);

            HashSet<string> addedKeys = new(otherIndex.Keys);
            addedKeys.ExceptWith(Index.Keys);
            addedKeys.ExceptWith(deleted);

            foreach (var key in addedKeys)
            {
                Index[key] = string.Join(";", Index.GetMappedData(otherIndex[key], columnMap)).TrimEnd(';');
            }

            foreach (var key in deletedKeys)
            {
                Index.Remove(key);
            }

            CreateRowCount += addedKeys.Count;
            DeleteRowCount += deletedKeys.Count;
        }

        private bool Equals(string localData, string otherData)
        {
            string v = otherData.ToLower();

            switch (localData.ToLower())
            {
                case "true":
                    return v.Equals("1") || v.Equals("true");
                case "false":
                    return v.Equals("0") || v.Equals("false");
                default:
                    return localData.Equals(otherData);
            }
        }

        #endregion
    }
}
