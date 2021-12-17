using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProfilesRNS_Manager
{
    public class FileService
    {
        public async Task<TableIndex> GetFileIndex(string fileName, int keyColumnIndex, bool readHeaderRow = true)
        {
            return await Task.Run(() =>
            {
                TableIndex result = new ();
                result.DataSource = $"file://{fileName}";

                if (File.Exists(fileName))
                {
                    string[] lines = File.ReadLines(fileName).ToArray();

                    if(lines.Length > 0)
                    {
                        if(readHeaderRow)
                        {
                            result.ColumnNames = lines[0].Split(';');
                        }
                        else
                        {
                            result.ColumnNames = new string[0];
                        }

                        foreach (string data in lines.Skip(1))
                        {
                            string key = data.Split(';')[keyColumnIndex];

                            result[key] = data;
                        }
                    }
                }

                return result.Normalize();
            });
        }

        public async Task<bool> Serialize(string fileName, TableIndex tableIndex, bool writeHeaderRow = true)
        {
            try
            {
                using (var file = File.Open(fileName, FileMode.OpenOrCreate))
                {
                    file.SetLength(0);

                    using (var writer = new StreamWriter(file))
                    {
                        if (writeHeaderRow)
                        {
                            await writer.WriteLineAsync(string.Join(";", tableIndex.ColumnNames).TrimEnd(';'));
                        }

                        foreach (var row in tableIndex.Values.Where(r => !string.IsNullOrEmpty(r)))
                        {
                            await writer.WriteLineAsync(row);
                        }

                        await writer.FlushAsync();

                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
