using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProfilesRNS_Manager
{
    public class WizardData : INotifyPropertyChanged
    {
        #region Members

        public WizardConfig Config { get; set; }

        public SqlService Db { get; private set; } = new ();

        private string _personFile;

        public string PersonFile
        {
            get => _personFile;
            set => SetValue(ref _personFile, value);
        }


        private string _affiliationFile;

        public string AffiliationFile
        {
            get => _affiliationFile;
            set => SetValue(ref _affiliationFile, value);
        }


        private string _filterFile;

        public string FilterFile
        {
            get => _filterFile;
            set => SetValue(ref _filterFile, value);
        }


        private string _keyphrasesFile;

        public string KeyphrasesFile
        {
            get => _keyphrasesFile;
            set => SetValue(ref _keyphrasesFile, value);
        }

        private bool _importMergeData = true;

        public bool ImportMergeData
        {
            get => _importMergeData;
            set => SetValue(ref _importMergeData, value);
        }

        #endregion

        #region Constructors

        public WizardData()
        {
            Config = new WizardConfigLoader().Load();

            Db.Tasks.AddRange(new SqlTaskLoader().Load("Sql/ImportData/commands.json"));
        }

        #endregion

        #region Methods

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