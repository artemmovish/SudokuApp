using Sudoku.Core.Models;
using Sudoku.UI.Pages;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sudoku.Core.ViewModels.SinglTone;

namespace Sudoku;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainFrame.Navigate(new StartMenuPage());
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        string json = JsonSerializer.Serialize(PageStorage.Instance.CompletedLevels, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        string filePath = System.IO.Path.Combine("Saves", "completed_levels.json");

        // Создаём папку, если её нет
        if (!Directory.Exists("Saves"))
        {
            Directory.CreateDirectory("Saves");
        }

        // Теперь записываем файл
        File.WriteAllText(filePath, json);

        filePath = System.IO.Path.Combine("Saves", "completed_monogram.txt");

        File.WriteAllText(filePath, PageStorage.Instance.OpenMonogram.ToString());


        //MessageBox.Show(json);
    }
}