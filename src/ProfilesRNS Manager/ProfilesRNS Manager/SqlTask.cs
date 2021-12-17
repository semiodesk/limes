using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ProfilesRNS_Manager
{
    public enum SqlTaskState { NotExecuted, Executing, Success, Error}

    public class SqlTask
    {
        public string DisplayName { get; set; }

        public string CommandText { get; set; }

        public bool Required { get; set; }

        public Exception Error { get; set; }

        public SqlTaskState State { get; set; } = SqlTaskState.NotExecuted;

        public void Bind(string variableName, string value)
        {
            CommandText = CommandText.Replace($"%{variableName}%", value);
        }

        public bool Execute(SqlConnection connection)
        {
            try
            {
                State = SqlTaskState.Executing;

                var commands = Regex.Split(CommandText, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

                foreach (var c in commands)
                {
                    if (!string.IsNullOrEmpty(c.Trim()))
                    {
                        SqlCommand command = new SqlCommand(c, connection);
                        command.CommandTimeout = int.MaxValue;
                        command.ExecuteNonQuery();
                    }
                }

                State = SqlTaskState.Success;

                return true;
            }
            catch (Exception ex)
            {
                State = SqlTaskState.Error;
                Error = ex;

                return false;
            }
        }
    }
}
