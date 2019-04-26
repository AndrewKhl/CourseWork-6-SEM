using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Watcher.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        private ProcessManager _manager;

        public ProcessWindow(ProcessManager manager, ResourceDictionary theme, CultureInfo lang)
        {
            Thread.CurrentThread.CurrentUICulture = lang;
            InitializeComponent();
            _manager = manager;

            DataContext = _manager;
            SetNewTheme(theme);
        }

        private void LoadAllSystemProcess(object sender, RoutedEventArgs e)
        {
            _manager.AddAllSystemProcess();
        }
 
        private void DeleteProcess(object sender, RoutedEventArgs e)
        {
            if (processList.SelectedItem != null)
            {
                var removedProc = processList.SelectedItem.ToString();
                _manager.DeletedSelectProcess(removedProc);
                MessageBox.Show($"Process {removedProc} successfully created!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show($"Please, select a process!", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AddNewProcess(object sender, RoutedEventArgs e)
        {
            var newProcess = ValueNewProcess.Text;

            if (newProcess.Trim() == string.Empty)
            {
                MessageBox.Show($"Please, input name process", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = _manager.AddProcess(newProcess);
            ValueNewProcess.Text = string.Empty;
            if (result)
                MessageBox.Show($"Process {newProcess} successfully created!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show($"Process {newProcess} alredy created!", "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _manager.SaveGoodProcessesInFile();
        }

        private void DeleteAllProcesses(object sender, RoutedEventArgs e)
        {
            _manager.DeleteAllProcesses();
        }

        private void SetNewTheme(ResourceDictionary theme)
        {
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
