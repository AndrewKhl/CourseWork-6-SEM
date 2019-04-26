﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Watcher.Models;

namespace Watcher
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager _manager;

        public MainWindow()
        {
            InitializeComponent();

            _manager = new UserManager();
            DataContext = _manager;

            EditBoxLogin.Text = "Admin";
            EditBoxPassword.Password = "123456";
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            string pass = EditBoxPassword.Password;

            var user = _manager.GetUser(EditBoxLogin.Text.Trim(), pass.Trim());

            if (user != null)
            {
                CultureInfo lang;

                switch (SelectLang.SelectedIndex)
                {
                    case 0:
                        lang = CultureInfo.GetCultureInfo("ru-RU");
                        break;
                    case 1:
                        lang = CultureInfo.GetCultureInfo("eu-US");
                        break;
                    default:
                        return;
                }

                var wnd = new WorkWindow(_manager, user, lang);
                wnd.Show();
                Close();
            }
            else
                MessageBox.Show("Invalid login or username", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnRegistr_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new RegistrationWindow(_manager, this);
            wnd.Owner = this;
            wnd.Show();
        }
    }
}
