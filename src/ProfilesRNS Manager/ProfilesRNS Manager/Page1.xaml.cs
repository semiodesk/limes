using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProfilesRNS_Manager
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : WizardPageFunction
    {
        protected string DataDirectory;

        protected string PersonFileName = "Person.csv";

        protected string PersonFile;

        protected TableInfo PersonTableInfo;

        protected string AffiliationFileName = "PersonAffiliation.csv";

        protected string AffiliationFile;

        protected TableInfo AffiliationTableInfo;

        protected string FilterFileName = "PersonFilterFlag.csv";

        protected string FilterFile;

        protected TableInfo FilterTableInfo;

        protected string KeyphrasesFileName = "Keyphrases.txt";

        protected string KeyphrasesFile;

        protected TableInfo KeyphrasesTableInfo;

        protected bool IsSiteLoaded
        {
            get => !string.IsNullOrEmpty(Context.Config.LastWebConfig);
        }

        public static readonly DependencyProperty _errorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(Page1), new FrameworkPropertyMetadata(""));

        public string ErrorMessage
        {
            get { return (string)GetValue(_errorMessageProperty); }
            set { SetValue(_errorMessageProperty, value); }
        }

        public Page1(WizardData data)
        {
            InitializeComponent();

            Context = data;
        }

        private void SelectFolderClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Supported Files (*.txt,*.csv)|*.txt;*.csv";
            dialog.ValidateNames = true;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.FileName = PersonFileName;

            if (dialog.ShowDialog() == true)
            {
                DataDirectory = Path.GetDirectoryName(dialog.FileName);

                FolderTextBox.Text = DataDirectory;

                bool merge = MergeDataBox.IsChecked == true;

                Task.Run(async () => await LoadData(merge)).ContinueWith((t) => {});
            }
        }

        private void OnMergeDataBoxClick(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(DataDirectory))
            {
                bool merge = MergeDataBox.IsChecked == true;

                Task.Run(async () => await LoadData(merge)).ContinueWith((t) => {});
            }
        }

        private async Task LoadData(bool mergeWithDatabase = true)
        {
            if (IsSiteLoaded)
            {
                Dispatcher.Invoke(() =>
                {
                    ProgressBar.Visibility = System.Windows.Visibility.Visible;
                    ErrorMessage = null;
                });

                try
                {
                    await LoadData(DataDirectory, mergeWithDatabase);
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() =>
                    {
                        ErrorMessage = ex.Message;
                    });
                }
            }

            Dispatcher.Invoke(() =>
            {
                ProgressBar.Visibility = System.Windows.Visibility.Hidden;

                DataGrid.Items.Refresh();

                UpdateControlState();
            });
        }

        private async Task LoadData(string dataDirectory, bool mergeWithDatabase = true)
        {
            Context.Db.TableInfos.Clear();

            PersonTableInfo = await Context.Db.GetTableInfo("[Profile.Import].[Person]");
            PersonTableInfo.DisplayName = PersonFileName;

            AffiliationTableInfo = await Context.Db.GetTableInfo("[Profile.Import].[PersonAffiliation]");
            AffiliationTableInfo.DisplayName = AffiliationFileName;

            FilterTableInfo = await Context.Db.GetTableInfo("[Profile.Import].[PersonFilterFlag]");
            FilterTableInfo.DisplayName = FilterFileName;

            KeyphrasesTableInfo = await Context.Db.GetTableInfo("[Profile.Data].[Publication.PubMed.DisambiguationAffiliation]");
            KeyphrasesTableInfo.DisplayName = KeyphrasesFileName;

            if (Directory.Exists(dataDirectory))
            {
                PersonFile = Path.Join(dataDirectory, PersonFileName);
                AffiliationFile = Path.Join(dataDirectory, AffiliationFileName);
                FilterFile = Path.Join(dataDirectory, FilterFileName);
                KeyphrasesFile = Path.Join(dataDirectory, KeyphrasesFileName);

                if (!mergeWithDatabase)
                {
                    PersonTableInfo.Clear();
                    AffiliationTableInfo.Clear();
                    FilterTableInfo.Clear();
                    KeyphrasesTableInfo.Clear();
                }

                FileService fileService = new FileService();

                TableIndex personIndex = await fileService.GetFileIndex(PersonFile, 0);
                TableIndex affiliationIndex = await fileService.GetFileIndex(AffiliationFile, 0);
                TableIndex filterIndex = await fileService.GetFileIndex(FilterFile, 0);
                TableIndex keyphraseIndex = await fileService.GetFileIndex(KeyphrasesFile, 0, false);

                // Transfer the deleted keys from the persons file to the other files..
                affiliationIndex.DeleteKeys(personIndex.GetDeletedKeys());
                filterIndex.DeleteKeys(personIndex.GetDeletedKeys());

                // This maps the columns of the imported files to the columns in the database..
                PersonTableInfo.ImportData(personIndex);
                AffiliationTableInfo.ImportData(affiliationIndex);
                FilterTableInfo.ImportData(filterIndex);
                KeyphrasesTableInfo.ImportData(keyphraseIndex, false);
            }
            else
            {
                PersonFile = null;
                AffiliationFile = null;
                FilterFile = null;
                KeyphrasesFile = null;
            }

            Context.Db.TableInfos.AddRange(new TableInfo[] { PersonTableInfo, AffiliationTableInfo, FilterTableInfo, KeyphrasesTableInfo });
        }

        private bool CanImport()
        {
            if (PersonTableInfo != null)
            {
                bool result = File.Exists(PersonFile);
                result &= PersonTableInfo.TotalRowCount > 0;
                result &= File.Exists(AffiliationFile);
                result &= PersonTableInfo.TotalRowCount == AffiliationTableInfo.TotalRowCount;
                result &= File.Exists(FilterFile);
                result &= PersonTableInfo.TotalRowCount == FilterTableInfo.TotalRowCount;
                result &= PersonTableInfo.HasChanges;

                return result;
            }
            else
            {
                return false;
            }
        }

        private void UpdateControlState()
        {
            ErrorMessage = null;

            SelectFolderButton.IsEnabled = IsSiteLoaded;

            ImportButton.IsEnabled = CanImport();

            if (string.IsNullOrEmpty(PersonFile))
            {
                FolderImage.Visibility = System.Windows.Visibility.Visible;
                FolderCheckImage.Visibility = System.Windows.Visibility.Collapsed;
                FolderErrorImage.Visibility = System.Windows.Visibility.Collapsed;
                NoChangesMessage.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (CanImport())
            {
                FolderImage.Visibility = System.Windows.Visibility.Collapsed;
                FolderCheckImage.Visibility = System.Windows.Visibility.Visible;
                FolderErrorImage.Visibility = System.Windows.Visibility.Collapsed;
                NoChangesMessage.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                FolderImage.Visibility = System.Windows.Visibility.Collapsed;
                FolderCheckImage.Visibility = System.Windows.Visibility.Collapsed;
                FolderErrorImage.Visibility = System.Windows.Visibility.Visible;
                NoChangesMessage.Visibility = System.Windows.Visibility.Visible;

                if (!File.Exists(PersonFile))
                {
                    ErrorMessage = $"File not found: {PersonFile}";
                }
                else if(!File.Exists(AffiliationFile))
                {
                    ErrorMessage = $"File not found: {AffiliationFile}";
                }
                else if (!File.Exists(FilterFile))
                {
                    ErrorMessage = $"File not found: {FilterFile}";
                }
            }
        }

        private void OnImportButtonClick(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                string dataFolder = "Data";

                if (!Directory.Exists(dataFolder))
                {
                    Directory.CreateDirectory(dataFolder);
                }

                DirectoryInfo dataInfo = new DirectoryInfo(dataFolder);

                Context.PersonFile = Path.Join(dataInfo.FullName, PersonFileName);
                Context.AffiliationFile = Path.Join(dataInfo.FullName, AffiliationFileName);
                Context.FilterFile = Path.Join(dataInfo.FullName, FilterFileName);
                Context.KeyphrasesFile = Path.Join(dataInfo.FullName, KeyphrasesFileName);

                FileService fileService = new();
                await fileService.Serialize(Context.PersonFile, PersonTableInfo.Index);
                await fileService.Serialize(Context.AffiliationFile, AffiliationTableInfo.Index);
                await fileService.Serialize(Context.FilterFile, FilterTableInfo.Index);
                await fileService.Serialize(Context.KeyphrasesFile, KeyphrasesTableInfo.Index, false);

                foreach (SqlTask task in Context.Db.Tasks)
                {
                    task.Bind("database", Context.Db.Connection.Database);
                    task.Bind("personFile", Context.PersonFile);
                    task.Bind("affiliationFile", Context.AffiliationFile);
                    task.Bind("filterFile", Context.FilterFile);
                    task.Bind("keyphrasesFile", Context.KeyphrasesFile);
                }
            }).Wait();

            NavigationService?.Navigate(new Page2(Context));
        }

        private void OnSiteSelected(object sender, EventArgs e)
        {
            Task.Run(async () => await LoadData()).ContinueWith((t) => { });
        }
    }
}
