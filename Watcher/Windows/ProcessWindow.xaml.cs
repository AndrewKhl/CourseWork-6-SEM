using System.Globalization;
using System.Threading;
using System.Windows;

namespace Watcher.Windows
{
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
                MessageBox.Show(Properties.Resources.Process + $" {removedProc} " + Properties.Resources.SuccessfullyCreated, "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show(Properties.Resources.SelectProcess, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void AddNewProcess(object sender, RoutedEventArgs e)
        {
            var newProcess = ValueNewProcess.Text;

            if (newProcess.Trim() == string.Empty)
            {
                MessageBox.Show(Properties.Resources.NameProcess, "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var result = _manager.AddProcess(newProcess);
            ValueNewProcess.Text = string.Empty;
            if (result)
                MessageBox.Show(Properties.Resources.Process + $" {newProcess} " + Properties.Resources.SuccessfullyCreated, "", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(Properties.Resources.Process + $" {newProcess} " + Properties.Resources.AlredyCreated, "", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _manager.SaveGoodProcessesInFile();
            (Owner as WorkWindow).OpenProcWnd = false;
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
