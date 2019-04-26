using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Watcher.Models;
using Watcher.Windows;

namespace Watcher
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
	public partial class WorkWindow : Window
    {
        MonitoringModel _monitor;
        private int _errorCount;
        private bool _runScanning;

        private ResourceDictionary _currentTheme;
        private CultureInfo _currentLang;

        public WorkWindow(UserManager manager, UserModel model, CultureInfo lang)
        {
            Thread.CurrentThread.CurrentUICulture = lang;
            InitializeComponent();

            _monitor = new MonitoringModel(manager, model);
            _currentLang = lang;
            DataContext = _monitor;

            AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
            SetNewTheme(_monitor.CurrentTheme);
        }

        private void OpenProcessesWindow(object sender, RoutedEventArgs e)
        {
            var wnd = new ProcessWindow(_monitor.ProcessManager, _currentTheme, _currentLang);
            wnd.Owner = this;

            wnd.Show();
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            ChangeEnabledButton();
            _monitor.StopSacnning();
            _runScanning = false;
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ChangeEnabledButton();
            _monitor.StartScanning();

            _runScanning = true;
            Visibility = Visibility.Hidden;
        }

        private void ChangeEnabledButton()
        {
            btnStart.IsEnabled = btnStop.IsEnabled;
            btnStop.IsEnabled = !btnStop.IsEnabled;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _monitor.StopSacnning();
        }

        private void OnErrorEvent(object sender, RoutedEventArgs e)
        {
            var validationEventArgs = e as ValidationErrorEventArgs;
            if (validationEventArgs.Action == ValidationErrorEventAction.Added)
                _errorCount++;
            else
            if (validationEventArgs.Action == ValidationErrorEventAction.Removed)
                _errorCount--;

            btnStart.IsEnabled = _errorCount == 0 && !_runScanning;
        }

        private void SetNewTheme(string theme)
        {
            try
            {
                var uri = new Uri($"{theme}.xaml", UriKind.Relative);

                _currentTheme = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(_currentTheme);

                _monitor.CurrentTheme = theme;
            }
            catch
            {
                MessageBox.Show("Theme not found!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SetNewTheme((sender as MenuItem).Header.ToString());
        }

        private void BtnData_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new CurrentDataWindow(_monitor.CreateStateLogger(), _currentTheme, _currentLang);
            wnd.Owner = this;
            wnd.Show();
        }
    }

    public class ShowMessageCommand : ICommand
    {
        public void Execute(object parameter)
        {
            ((Window)parameter).Visibility = Visibility.Visible;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}
