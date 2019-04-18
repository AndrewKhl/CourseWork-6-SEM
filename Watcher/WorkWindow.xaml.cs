using System;
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

namespace Watcher
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
	public partial class WorkWindow : Window
    {
        Thread backgroundThread;

        public WorkWindow()
        {
            InitializeComponent();
           // DataContext = new ViewModel();
            backgroundThread = null;
            btnStop.IsEnabled = false;
        }

        private void StartApplication(object sender, RoutedEventArgs e)
        {
           // (DataContext as ViewModel).UseGoodProcesses = UseGoodProc.IsChecked;
            if (backgroundThread == null)
            {
               // backgroundThread = new Thread(new ThreadStart((DataContext as ViewModel).StartScanning));
                backgroundThread.Start();
                btnStart.IsEnabled = false;
                btnStop.IsEnabled = true;
                Visibility = Visibility.Hidden;
            }
        }

        private void StopThread(object sender, RoutedEventArgs e)
        {
            if (backgroundThread != null)
            {
                backgroundThread.Abort();
                backgroundThread = null;
                btnStop.IsEnabled = false;
                btnStart.IsEnabled = true;
            }
        }

        private void OpenProcessesWindow(object sender, RoutedEventArgs e)
        {
            ////WindowViewProcesses processWindow = new WindowViewProcesses(DataContext as ViewModel);
            //processWindow.Owner = this;

            //processWindow.Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (backgroundThread != null)
                backgroundThread.Abort();
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
