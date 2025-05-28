using Sudoku.Core.ViewModels.SinglTone;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartMenuPage.xaml
    /// </summary>
    public partial class StartMenuPage : Page
    {
        public StartMenuPage()
        {
            InitializeComponent();

        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            var page = PageStorage.Instance.MapPage;
            page.LoadSave();
            NavigationService?.Navigate(page);
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            PageStorage.ResetStorage();
            PageStorage.Instance.MapPage = new MapPage();

            var page = PageStorage.Instance.MapPage;
            page.LoadSave();
            NavigationService?.Navigate(page);
        }

        private void ToSettingBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(PageStorage.Instance.SettingsPage);
        }

        private void ToAboutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Не сделано");
        }

        private void ToBackBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow?.Close();
        }
    }
}
