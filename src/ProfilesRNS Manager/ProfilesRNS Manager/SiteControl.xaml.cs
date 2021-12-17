using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.XPath;

namespace ProfilesRNS_Manager
{
    /// <summary>
    /// Interaction logic for SiteControl.xaml
    /// </summary>
    public partial class SiteControl : UserControl
    {
        public static readonly DependencyProperty _canChangeSiteProperty =
            DependencyProperty.Register("CanChangeSite", typeof(bool), typeof(SiteControl), new FrameworkPropertyMetadata(true));

        public bool CanChangeSite
        {
            get { return (bool)GetValue(_canChangeSiteProperty); }
            set { SetValue(_canChangeSiteProperty, value); }
        }

        protected WizardData Context { get; set; }

        public SiteControl()
        {
            InitializeComponent();

            DataContextChanged += (sender, e) =>
            {
                if(DataContext is WizardData dataContext)
                {
                    Context = dataContext;

                    SelectSiteButton.IsEnabled = true;

                    if(!string.IsNullOrEmpty(Context.Config.LastWebConfig))
                    {
                        LoadWebConfig(Context.Config.LastWebConfig);
                    }
                }
            };
        }

        void OnSelectSiteClick(object sender, EventArgs e)
        {
            ProgressBar.Height = 3;

            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Config files (web.config)|web.config";
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog() == true)
                {
                    LoadWebConfig(dialog.FileName);
                }
            }
            finally
            {
                ProgressBar.Height = 0;
            }
        }

        void LoadWebConfig(string configFile)
        {
            XPathNavigator config = new XPathDocument(configFile).CreateNavigator();

            var connectionString = config.SelectSingleNode("/configuration/connectionStrings/add/@connectionString")?.Value;

            if (!string.IsNullOrEmpty(connectionString))
            {
                string database = new Regex("Initial\\ Catalog=(?<db>.+?)\\;").Match(connectionString).Groups["db"].Value;

                Context.Db.Connect(database);

                if (Context.Db.Connection.State == ConnectionState.Open)
                {
                    Context.Config.LastWebConfig = configFile;
                    Context.Config.Save();

                    SiteNameBlock.Text = Context.Db.GetSiteUrl();

                    SiteSelected?.Invoke(this, new EventArgs());
                }
            }
        }

        private void OnMenuButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            ContextMenu contextMenu = button.ContextMenu;
            contextMenu.PlacementTarget = button;
            contextMenu.IsOpen = true;

            e.Handled = true;
        }

        public event EventHandler SiteSelected;
    }
}
