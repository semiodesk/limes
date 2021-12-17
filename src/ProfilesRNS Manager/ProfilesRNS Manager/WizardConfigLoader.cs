using System.IO;
using System.Text;
using System.Text.Json;

namespace ProfilesRNS_Manager
{
    public class WizardConfigLoader
    {
        private readonly string _configPath = "config.json";

        public WizardConfig Load()
        {
            if (File.Exists(_configPath))
            {
                using(StreamReader reader = new StreamReader(_configPath, Encoding.UTF8))
                {
                    string json = reader.ReadToEnd();

                    return JsonSerializer.Deserialize<WizardConfig>(json);
                }
            }
            else
            {
                return new WizardConfig();
            }
        }

        public void Save(WizardConfig config)
        {
            using (FileStream stream = File.OpenWrite(_configPath))
            {
                stream.SetLength(0);

                JsonSerializer.Serialize(stream, config);

                stream.Flush();
            }
        }
    }
}
