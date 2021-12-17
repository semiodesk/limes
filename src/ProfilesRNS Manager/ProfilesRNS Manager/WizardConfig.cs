using System;

namespace ProfilesRNS_Manager
{
    public class WizardConfig
    {
        public string LastWebConfig { get; set; }

        public DateTime? LastUpdate { get; set; }

        public void Save()
        {
            new WizardConfigLoader().Save(this);
        }
    }
}
