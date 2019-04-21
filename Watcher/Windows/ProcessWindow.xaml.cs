using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public ProcessWindow(ProcessManager manager)
        {
            InitializeComponent();
            _manager = manager;

            DataContext = _manager;
        }

        private void LoadAllSystemProcess(object sender, RoutedEventArgs e)
        {
            _manager.AddAllSystemProcess();
        }
 
        private void DeleteProcess(object sender, RoutedEventArgs e)
        {
            _manager.DeletedSelectProcess(processList.SelectedItem.ToString());
        }

        private void AddNewProcess(object sender, RoutedEventArgs e)
        {
            _manager.AddProcess(ValueNewProcess.Text);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _manager.SaveGoodProcessesInFile();
        }

        private void DeleteAllProcesses(object sender, RoutedEventArgs e)
        {
            _manager.DeleteAllProcesses();
        }
    }
}
