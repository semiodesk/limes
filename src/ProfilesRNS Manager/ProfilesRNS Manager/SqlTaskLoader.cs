using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ProfilesRNS_Manager
{
    public class SqlTaskLoader
    {
        public readonly List<SqlTask> Tasks = new List<SqlTask>();

        public IEnumerable<SqlTask> Load(string configFile)
        {
            using (var file = File.OpenRead(configFile))
            {
                var basePath = Path.GetDirectoryName(configFile);

                using (var reader = new StreamReader(file))
                {
                    var json = reader.ReadToEnd();

                    dynamic config = JsonConvert.DeserializeObject<dynamic>(json);

                    foreach(var t in config.executeTasks)
                    {
                        var task = new SqlTask();
                        task.DisplayName = t.displayName;
                        task.Required = t.required;

                        var commandFile = t.commandFile?.ToString();

                        if(!string.IsNullOrEmpty(commandFile))
                        {
                            task.CommandText = ReadFile(Path.Join(basePath, commandFile));
                        }
                        
                        var commandText = t.commandText?.ToString();

                        if(!string.IsNullOrEmpty(commandText))
                        {
                            task.CommandText = commandText;
                        }

                        task.CommandText = task.CommandText.Replace("\r\n", "\n");

                        Tasks.Add(task);
                    }

                    return Tasks;
                }
            }
        }

        private string ReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                using (var file = File.OpenRead(fileName))
                {
                    using (var reader = new StreamReader(file))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            else
            {
                throw new FileNotFoundException(fileName);
            }
        }
    }
}
