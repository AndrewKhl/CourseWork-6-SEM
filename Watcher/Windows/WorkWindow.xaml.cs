using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public WorkWindow()
        {
            InitializeComponent();
            _monitor = new MonitoringModel();
            DataContext = _monitor;

            AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
        }

        private void OpenProcessesWindow(object sender, RoutedEventArgs e)
        {
            var wnd = new ProcessWindow(_monitor.ProcessManager);
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
