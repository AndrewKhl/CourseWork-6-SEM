using System;
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

        ResourceDictionary currentTheme;

        public WorkWindow(UserManager manager, UserModel model)
        {
            InitializeComponent();
            _monitor = new MonitoringModel(manager, model);
            DataContext = _monitor;

            AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
            SetNewTheme("Light");
        }

        private void OpenProcessesWindow(object sender, RoutedEventArgs e)
        {
            var wnd = new ProcessWindow(_monitor.ProcessManager, currentTheme);
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

                currentTheme = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(currentTheme);
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
