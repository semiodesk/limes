using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesRNS_Manager
{
    public class SqlService : INotifyPropertyChanged
    {
        #region Properties

        private SqlConnection _connection;

        /// <summary>
        /// Get the SQL connection to be used for queries.
        /// </summary>
        public SqlConnection Connection
        {
            get => _connection;
            private set => SetValue(ref _connection, value);
        }

        private string _username;

        /// <summary>
        /// Get the username used for establishing the SQL connection.
        /// </summary>
        public string Username
        {
            get => _username;
            private set => SetValue(ref _username, value);
        }

        public List<TableInfo> TableInfos { get; private set; } = new List<TableInfo>();

        public List<SqlTask> Tasks { get; private set; } = new List<SqlTask>();

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new instance of the class.
        /// </summary>
        public SqlService()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Try to connect to a database using Windows integrated security.
        /// </summary>
        /// <param name="database">Name of the database.</param>
        /// <returns>A SQL connection object.</returns>
        public SqlConnection Connect(string database)
        {
            Username = WindowsIdentity.GetCurrent().Name;

            Connection = new($"Server=localhost; Database={database}; Integrated Security=True;");
            Connection.Open();

            return Connection;
        }

        protected async Task EnsureConnectionAsync()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                await Connection.OpenAsync();
            }
        }

        /// <summary>
        /// Get the site base URL from the ProfilesRNS database.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        public string GetSiteUrl()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }

            string sql = $"SELECT Value FROM [{Connection.Database}].[Framework.].[Parameter] WHERE ParameterID = 'basePath'";

            SqlCommand command = new (sql, Connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                reader.Read();

                return reader["Value"].ToString();
            }
        }

        /// <summary>
        /// List all user databases on the server. This does not include the system databases 'master', 'model', 'msdb' or 'tempdb'.
        /// </summary>
        /// <returns>An enumerable of all database names.</returns>
        public async IAsyncEnumerable<string> GetUserDatabasesAsync()
        {
            await EnsureConnectionAsync();

            string sql = "SELECT name, database_id FROM sys.databases";

            SqlCommand command = new (sql, Connection);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    string name = reader["name"].ToString();

                    switch (name)
                    {
                        case "master":
                        case "model":
                        case "msdb":
                        case "tempdb":
                            continue;
                    }

                    yield return name;
                }
            }
        }

        /// <summary>
        /// Fetch all rows from a table.
        /// </summary>
        /// <param name="tableName">Name of the table, excluding the database name.</param>
        /// <returns>An enumerable of all database rows.</returns>
        public async IAsyncEnumerable<SqlDataReader> GetAllDataAsync(string tableName)
        {
            await EnsureConnectionAsync();

            string sql = $"SELECT * FROM [{Connection.Database}].{tableName}";

            SqlCommand command = new (sql, Connection);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    yield return reader;
                }
            }
        }

        public async Task<string[]> GetTableSchema(string tableName)
        {
            await EnsureConnectionAsync();

            StringBuilder builder = new ();

            string sql = $"SELECT * FROM [{Connection.Database}].{tableName}";

            SqlCommand command = new (sql, Connection);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                return reader.GetColumnSchema().Select(x => x.ColumnName).ToArray();
            }
        }

        public async Task<TableIndex> GetTableIndex(string tableName, int keyColumnIndex)
        {
            await EnsureConnectionAsync();

            TableIndex result = new ();
            result.DataSource = $"odbc://{Connection.DataSource}/{Connection.Database}/{tableName}";

            string sql = $"SELECT * FROM [{Connection.Database}].{tableName}";

            SqlCommand command = new (sql, Connection);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                result.ColumnNames = reader.GetColumnSchema().Select(c => c.ColumnName).ToArray();

                while (reader.Read())
                {
                    object[] values = new object[reader.FieldCount];

                    reader.GetValues(values);

                    result[values[keyColumnIndex].ToString()] = string.Join(';', values.Select(v => v.ToString()));
                }
            }

            return result;
        }

        public async Task<TableInfo> GetTableInfo(string tableName, int keyColumnIndex = 0)
        {
            var index = await GetTableIndex(tableName, keyColumnIndex);

            return new(tableName, index);
        }

        /// <summary>
        /// Fetch all rows from a table.
        /// </summary>
        /// <param name="tableName">Name of the table, excluding the database name.</param>
        /// <returns>An enumerable of all database rows.</returns>
        public async Task<bool> RenameDatabaseAsync(string oldName, string newName)
        {
            await EnsureConnectionAsync();

            string sql = $"ALTER DATABASE [{oldName}] MODIFY NAME = [{newName}]";

            SqlCommand command = new (sql, Connection);

            try
            {
                await command.ExecuteNonQueryAsync();

                if(oldName == Connection.Database)
                {
                    Connect(newName);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
