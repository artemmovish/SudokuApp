using Sudoku.Core.ViewModels;
using Sudoku.Core.ViewModels.SinglTone;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sudoku.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MonogramLevelWindow.xaml
    /// </summary>
    public partial class MonogramLevelWindow : Window
    {
        private ObservableCollection<ObservableCollection<bool>> _currentMatrix;

        private ObservableCollection<ObservableCollection<bool>> _currentLevel;

        private readonly string _basePath = "E:\\Project\\Учебный процесс\\КПиЯП\\Cursach\\SudokuApp\\Sudoku\\Resource\\Card\\";

        public MonogramLevelWindow()
        {
            InitializeComponent();
            InitializeMatrix();
        }

        private void InitializeMatrix()
        {
            _currentMatrix = new ObservableCollection<ObservableCollection<bool>>()
        {
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, false, false, false, false, false, false}
        };
            MatrixItemsControl.ItemsSource = FlattenMatrix(_currentMatrix);

            // картинки перепутаны, сейчас все работает
            switch (PageStorage.Instance.OpenMonogram)
            {
                case 0:
                    _currentLevel = Level1;
                    CurrentImage.Source = new BitmapImage(new Uri(_basePath + "Gomel\\1.jpg"));
                    break;
                case 1:
                    _currentLevel = Level2;
                    CurrentImage.Source = new BitmapImage(new Uri(_basePath + "Gomel\\2.jpg"));
                    break;
                case 2:
                    _currentLevel = Level3;
                    CurrentImage.Source = new BitmapImage(new Uri(_basePath + "Mogilev\\1.jpg"));
                    break;
                case 3:
                    _currentLevel = Level4;
                    CurrentImage.Source = new BitmapImage(new Uri(_basePath + "Mogilev\\2.jpg"));
                    break;
                case 4:
                    _currentLevel = Level5;
                    CurrentImage.Source = new BitmapImage(new Uri(_basePath + "Vitebsk\\1.jpg"));
                    break;
                case 5:
                    _currentLevel = Level6;
                    CurrentImage.Source = new BitmapImage(new Uri(_basePath + "Vitebsk\\2.jpg"));
                    break;
                default:
                    break;
            }
        }

        // "Разворачиваем" матрицу в плоский список для ItemsControl
        private ObservableCollection<bool> FlattenMatrix(ObservableCollection<ObservableCollection<bool>> matrix)
        {
            var flatList = new ObservableCollection<bool>();
            foreach (var row in matrix)
            {
                foreach (var cell in row)
                {
                    flatList.Add(cell);
                }
            }
            return flatList;
        }

        private void CellButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                // Получаем индекс через ItemsControl
                int flatIndex = MatrixItemsControl.ItemContainerGenerator.IndexFromContainer(button);

                // Если не получилось через генератор, ищем вручную
                if (flatIndex == -1)
                {
                    flatIndex = FindButtonIndex(button);
                }

                if (flatIndex >= 0)
                {
                    int row = flatIndex / 15;
                    int col = flatIndex % 15;
                    _currentMatrix[row][col] = !_currentMatrix[row][col];
                    button.Background = _currentMatrix[row][col] ? Brushes.Black : Brushes.Wheat;
                }
            }
        }

        // Метод для ручного поиска индекса кнопки
        private int FindButtonIndex(Button button)
        {
            for (int i = 0; i < MatrixItemsControl.Items.Count; i++)
            {
                var container = MatrixItemsControl.ItemContainerGenerator.ContainerFromIndex(i) as ContentPresenter;
                if (container != null && VisualTreeHelper.GetChildrenCount(container) > 0)
                {
                    var child = VisualTreeHelper.GetChild(container, 0) as Button;
                    if (child == button)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }


        public bool CompareMatricesFast(
    ObservableCollection<ObservableCollection<bool>> matrix1,
    ObservableCollection<ObservableCollection<bool>> matrix2)
        {
            for (int i = 0; i < matrix1.Count; i++)
            {
                for (int j = 0; j < matrix1[i].Count; j++)
                {
                    if (matrix1[i][j] != matrix2[i][j])
                        return false;
                }
            }
            return true;
        }








        public ObservableCollection<ObservableCollection<bool>> Level1 = new ObservableCollection<ObservableCollection<bool>>()
        {
            new ObservableCollection<bool>() {false, false, false, false, false, false, false, true, false, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, true, true, true, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, true, true, true, true, true, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, true, true, true, true, true, true, true, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, true, true, true, true, true, true, true, true, true, false, false, false},
            new ObservableCollection<bool>() {false, false, true, true, true, true, true, true, true, true, true, true, true, false, false},
            new ObservableCollection<bool>() {false, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
            new ObservableCollection<bool>() {false, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
            new ObservableCollection<bool>() {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            new ObservableCollection<bool>() {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            new ObservableCollection<bool>() {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
            new ObservableCollection<bool>() {true, true, true, true, true, true, false, true, false, true, true, true, true, true, true},
            new ObservableCollection<bool>() {false, true, true, true, true, false, false, true, false, false, true, true, true, true, false},
            new ObservableCollection<bool>() {false, false, false, false, false, false, true, true, true, false, false, false, false, false, false},
            new ObservableCollection<bool>() {false, false, false, false, false, true, true, true, true, true, false, false, false, false, false}
        };

        public ObservableCollection<ObservableCollection<bool>> Level2 = new ObservableCollection<ObservableCollection<bool>>()
{
    new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false, false, false, false, false, true },
    new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false, false, true, true, false, false }, // Добавлен недостающий false
    new ObservableCollection<bool>() { false, false, false, false, true, false, false, false, false, false, false, false, false, false, false },
    new ObservableCollection<bool>() { false, false, false, false, true, true, false, false, false, true, true, false, false, false, false },
    new ObservableCollection<bool>() { false, false, false, false, true, false, false, false, false, true, true, false, false, false, false },
    new ObservableCollection<bool>() { false, false, false, true, true, true, true, true, true, true, true, true, false, false, false },
    new ObservableCollection<bool>() { false, false, true, true, true, true, true, false, true, false, true, false, true, false, false },
    new ObservableCollection<bool>() { false, true, true, true, true, true, true, true, false, true, false, true, false, true, false },
    new ObservableCollection<bool>() { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
    new ObservableCollection<bool>() { false, true, false, false, false, false, false, false, false, false, false, false, false, true, false },
    new ObservableCollection<bool>() { false, true, false, true, true, true, false, false, false, false, true, true, false, true, false },
    new ObservableCollection<bool>() { false, true, false, true, true, true, false, true, true, false, true, true, false, true, false },
    new ObservableCollection<bool>() { false, true, false, false, false, false, false, true, true, false, false, false, false, true, false },
    new ObservableCollection<bool>() { false, true, false, false, false, false, false, true, true, false, false, false, false, true, false },
    new ObservableCollection<bool>() { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }
};

        public ObservableCollection<ObservableCollection<bool>> Level3 = new ObservableCollection<ObservableCollection<bool>>()
{
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, true, true, true, false, false, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, true, true, true, true, true, false, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, true, true, true, true, false, true, true, true},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, true, true, true, true, true, true, true, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, true, true, true, true, true, false, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, false, true, true, true, false, false, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, false, true, true, true, true, true, false, false},
    new ObservableCollection<bool>() {true, false, false, false, false, false, true, true, true, true, true, true, true, true, false},
    new ObservableCollection<bool>() {true, true, true, false, false, true, true, true, false, false, false, true, true, true, false},
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, false, true, true, true, false, true, true, false},
    new ObservableCollection<bool>() {false, true, true, true, true, true, false, true, true, true, true, false, true, true, false},
    new ObservableCollection<bool>() {false, true, true, true, true, true, true, true, true, false, false, true, true, false, false},
    new ObservableCollection<bool>() {false, false, true, true, true, true, true, true, true, true, true, true, false, false, false},
    new ObservableCollection<bool>() {false, false, false, false, true, true, false, true, true, true, false, false, false, false, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, true, true, true, true, true, true, false, false, false}
};

        public ObservableCollection<ObservableCollection<bool>> Level4 = new ObservableCollection<ObservableCollection<bool>>()
{
    new ObservableCollection<bool>() {false, false, true, true, true, false, false, false, false, false, true, true, true, false, false},
    new ObservableCollection<bool>() {false, true, true, true, false, false, true, true, true, false, false, true, true, true, false},
    new ObservableCollection<bool>() {true, true, true, false, true, true, true, false, true, true, true, false, true, true, true},
    new ObservableCollection<bool>() {true, true, false, true, true, true, true, false, true, true, true, true, false, true, true},
    new ObservableCollection<bool>() {true, false, true, true, true, true, true, true, true, true, false, true, true, false, true},
    new ObservableCollection<bool>() {false, false, true, true, true, true, true, true, true, false, true, true, true, false, false},
    new ObservableCollection<bool>() {false, true, true, true, true, true, true, true, false, true, true, true, true, true, false},
    new ObservableCollection<bool>() {false, true, false, false, true, false, false, false, true, true, true, false, false, true, false},
    new ObservableCollection<bool>() {false, true, true, true, true, true, true, true, true, true, true, true, true, true, false},
    new ObservableCollection<bool>() {false, false, true, true, true, true, true, true, true, true, true, true, true, false, false},
    new ObservableCollection<bool>() {false, false, true, true, true, true, true, false, true, true, true, true, true, false, false},
    new ObservableCollection<bool>() {false, false, false, true, true, true, true, false, true, true, true, true, false, false, false},
    new ObservableCollection<bool>() {false, false, true, false, false, true, true, true, true, true, false, false, true, false, false},
    new ObservableCollection<bool>() {false, true, false, false, true, false, true, true, true, false, true, false, false, true, false},
    new ObservableCollection<bool>() {false, false, true, true, false, false, false, false, false, false, false, true, true, false, false}
};

        public ObservableCollection<ObservableCollection<bool>> Level5 = new ObservableCollection<ObservableCollection<bool>>()
{
    new ObservableCollection<bool>() {false, false, false, false, false, false, true, true, false, false, false, false, false, false, false},
    new ObservableCollection<bool>() {false, false, false, false, false, false, false, true, true, false, false, false, false, false, false},
    new ObservableCollection<bool>() {false, false, false, false, true, true, true, true, true, true, true, true, true, false, false},
    new ObservableCollection<bool>() {false, false, true, true, true, true, true, true, true, true, true, true, false, true, false},
    new ObservableCollection<bool>() {false, true, true, true, true, true, true, true, true, true, true, true, true, false, true},
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, false, true, true, false, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, false, false, false, false, true, true, false, false, false, true, true, false},
    new ObservableCollection<bool>() {true, true, false, false, false, false, false, false, false, false, false, false, false, true, false},
    new ObservableCollection<bool>() {true, false, false, false, false, false, false, false, false, false, false, false, false, false, false},
    new ObservableCollection<bool>() {true, true, false, false, false, false, false, false, false, false, false, false, false, true, true},
    new ObservableCollection<bool>() {false, true, false, false, false, false, false, true, true, false, true, false, true, false, true},
    new ObservableCollection<bool>() {false, false, false, true, true, false, true, true, false, true, false, true, true, false, false},
    new ObservableCollection<bool>() {false, false, true, false, false, true, true, false, true, true, false, true, true, true, false},
    new ObservableCollection<bool>() {true, true, true, false, true, true, true, false, true, true, true, false, true, true, true}
};

        public ObservableCollection<ObservableCollection<bool>> Level6 = new ObservableCollection<ObservableCollection<bool>>()
{
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, false, true, true, true, true, true, true, true},
    new ObservableCollection<bool>() {true, true, false, true, true, true, true, false, true, true, true, true, false, true, true},
    new ObservableCollection<bool>() {true, true, true, false, true, true, true, true, true, true, true, false, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, true, false, false, false, false, false, true, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, false, false, false, false, false, false, false, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, false, false, true, false, true, false, false, true, true, true, true},
    new ObservableCollection<bool>() {true, false, false, true, false, false, false, false, false, false, false, true, false, false, true},
    new ObservableCollection<bool>() {true, true, true, true, false, false, false, false, false, false, false, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, false, false, true, true, true, false, false, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, true, false, false, false, false, false, true, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, false, true, true, true, true, true, true, true, false, true, true, true},
    new ObservableCollection<bool>() {true, true, false, true, true, true, true, false, true, true, true, true, false, true, true},
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, false, true, true, true, true, true, true, true},
    new ObservableCollection<bool>() {true, true, true, true, true, true, true, true, true, true, true, true, true, true, true}
};

        private void ButtonAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (CompareMatricesFast(_currentMatrix, _currentLevel))
            {
                MessageBox.Show("Поздравляем! Ваше решение верное!");
                PageStorage.Instance.OpenMonogram++;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Неверно");
            }

        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            // Проверяем, что зажат Ctrl (левый или правый)
            bool isCtrlPressed = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control;

            if (sender is Button button && isCtrlPressed)
            {
                // Получаем индекс через ItemsControl
                int flatIndex = MatrixItemsControl.ItemContainerGenerator.IndexFromContainer(button);

                // Если не получилось через генератор, ищем вручную
                if (flatIndex == -1)
                {
                    flatIndex = FindButtonIndex(button);
                }

                if (flatIndex >= 0)
                {
                    int row = flatIndex / 15;
                    int col = flatIndex % 15;

                    _currentMatrix[row][col] = !_currentMatrix[row][col];

                    button.Background = _currentMatrix[row][col] ? Brushes.Black : Brushes.Wheat;
                }
            }
        }
    }

}
