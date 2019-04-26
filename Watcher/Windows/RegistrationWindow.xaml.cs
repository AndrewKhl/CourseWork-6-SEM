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

namespace Watcher
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        UserManager _manager;
        Window _mainWindows;

        public RegistrationWindow(UserManager manager, Window window, CultureInfo lang)
        {
            Thread.CurrentThread.CurrentUICulture = lang;
            InitializeComponent();

            _manager = manager;
            _mainWindows = window;
        }

        private void BtnRegistr_Click(object sender, RoutedEventArgs e)
        {
            string name = EditBoxLogin.Text.Trim();
            string pass = EditBoxPassword.Password.Trim();
            string confPass = EditBoxConfirmPassword.Password.Trim();
            
            if (name == string.Empty || pass == string.Empty)
            {
                MessageBox.Show("Please, fill the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (pass != confPass)
            {
                MessageBox.Show("Different passwords entered", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_manager.HaveUser(name))
            {
                _manager.AddUser(name, pass);
                MessageBox.Show("User successfully created!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainWindows.Topmost = true;
                Close();
                _mainWindows.Topmost = false;
            }
            else
            {
                MessageBox.Show("User alredy created!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
