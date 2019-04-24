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

namespace Watcher
{
    /// <summary>
    /// Логика взаимодействия для CurrentDataWindow.xaml
    /// </summary>
    public partial class CurrentDataWindow : Window
    {
        public CurrentDataWindow(ResourceDictionary theme)
        {
            InitializeComponent();

            SetNewTheme(theme);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetNewTheme(ResourceDictionary theme)
        {
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme);
        }
    }
}
