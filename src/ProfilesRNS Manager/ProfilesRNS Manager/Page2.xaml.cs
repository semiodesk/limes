using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProfilesRNS_Manager
{
    /// <summary>
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : WizardPageFunction
    {
        private SqlTask _currentTask;

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private TimeSpan _elapsedTime = new TimeSpan();

        public static readonly DependencyProperty _elapsedTimeProperty =
            DependencyProperty.Register("ElapsedTime", typeof(string), typeof(Page2), new FrameworkPropertyMetadata(""));

        public string ElapsedTime
        {
            get { return (string)GetValue(_elapsedTimeProperty); }
            set { SetValue(_elapsedTimeProperty, value); }
        }

        public static readonly DependencyProperty _errorMessageProperty =
            DependencyProperty.Register("ErrorMessage", typeof(string), typeof(Page2), new FrameworkPropertyMetadata(null));

        public string ErrorMessage
        {
            get { return (string)GetValue(_errorMessageProperty); }
            set { SetValue(_errorMessageProperty, value); }
        }

        public Page2(WizardData data)
        {
            InitializeComponent();

            Context = data;

            RunTasksAsync().ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    OkButton.IsEnabled = true;

                    ErrorMessage = null;

                    if (!t.IsFaulted)
                    {
                        StatusTextBlock.Text = "Success";

                        CheckImage.Visibility = Visibility.Visible;
                        ErrorImage.Visibility = Visibility.Collapsed;
                        HourglassImage.Visibility = Visibility.Collapsed;

                        Context.Config.LastUpdate = DateTime.Now;
                        Context.Config.Save();
                    }
                    else
                    {
                        StatusTextBlock.Text = "Failed";

                        CheckImage.Visibility = Visibility.Collapsed;
                        ErrorImage.Visibility = Visibility.Visible;
                        HourglassImage.Visibility = Visibility.Collapsed;

                        ErrorMessage = _currentTask.Error.Message;
                    }
                });
            });
        }

        private async Task RunTasksAsync()
        {
            _elapsedTime = new TimeSpan();

            ElapsedTime = _elapsedTime.ToString();

            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += OnTimerTick;
            _timer.Start();

            try
            {
                foreach (SqlTask task in Context.Db.Tasks)
                {
                    _currentTask = task;

                    bool success = await Task.Run(() => task.Execute(Context.Db.Connection));

                    Dispatcher.Invoke(() =>
                    {
                        DataGrid.Items.Refresh();
                    });

                    if (task.Required && !success)
                    {
                        throw new Exception(task.Error.Message);
                    }
                }
            }
            finally
            {
                _timer.Stop();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            _elapsedTime += TimeSpan.FromSeconds(1);

            ElapsedTime = _elapsedTime.ToString();
        }

        public void OnOkButtonClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Page1(new WizardData()));
        }
    }
}
