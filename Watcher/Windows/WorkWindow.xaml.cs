﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Watcher.Windows;

namespace Watcher
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
	public partial class WorkWindow : Window
    {
        MonitoringModel _monitor;

        public WorkWindow()
        {
            InitializeComponent();
            _monitor = new MonitoringModel();
            DataContext = _monitor;
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
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            ChangeEnabledButton();
            _monitor.StartScanning();
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
