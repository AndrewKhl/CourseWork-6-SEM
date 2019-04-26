using System.Globalization;
using System.Threading;
using System.Windows;

namespace Watcher
{
    public partial class CurrentDataWindow : Window
    {
        private CurrentStateLoader _loader;


        public CurrentDataWindow(CurrentStateLoader loader, ResourceDictionary theme, CultureInfo lang)
        {
            Thread.CurrentThread.CurrentUICulture = lang;
            InitializeComponent();

            _loader = loader;
            SetNewTheme(theme);

            DataContext = _loader;
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _loader.StopLoader();
            (Owner as WorkWindow).OpenCurrDataWnd = false;
        }
    }
}
