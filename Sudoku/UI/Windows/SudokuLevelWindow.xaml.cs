using Sudoku.Core.Services;
using Sudoku.Core.ViewModels.SinglTone;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;

namespace Sudoku.UI.Windows
{
    public partial class SudokuLevelWindow : Window
    {
        private readonly SudokuGenerator _sudokuGenerator = new SudokuGenerator();
        private int[,] _currentPuzzle;
        private int[,] _originalSolution;
        private List<SudokuCell> _cells = new List<SudokuCell>();

        public SudokuLevelWindow()
        {
            InitializeComponent();
            GenerateNewPuzzle();
        }

        private void GenerateNewPuzzle()
        {
            int difficulty = PageStorage.Instance.Difficulty;

            _currentPuzzle = _sudokuGenerator.GenerateSudoku(difficulty);
            _originalSolution = (int[,])_currentPuzzle.Clone();
            _sudokuGenerator.SolveSudoku(_originalSolution);

            DisplayPuzzle();
        }

        private void DisplayPuzzle()
        {
            _cells.Clear();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    bool isFixed = _currentPuzzle[row, col] != 0;
                    var cell = new SudokuCell
                    {
                        Row = row,
                        Column = col,
                        Value = isFixed ? _currentPuzzle[row, col].ToString() : "",
                        IsFixed = isFixed,
                        TextColor = isFixed ? Brushes.White : Brushes.LightBlue,
                        Background = GetBoxBackground(row, col),
                        BorderThickness = GetBorderThickness(row, col)
                    };

                    _cells.Add(cell);
                }
            }
            SudokuBoard.ItemsSource = null;
            SudokuBoard.ItemsSource = _cells;
        }

        private Brush GetBoxBackground(int row, int col)
        {
            int boxRow = row / 3;
            int boxCol = col / 3;

            // Alternate colors for 3x3 boxes
            return (boxRow + boxCol) % 2 == 0
                ? new SolidColorBrush(Color.FromArgb(255, 30, 30, 30))
                : new SolidColorBrush(Color.FromArgb(255, 40, 40, 40));
        }

        private Thickness GetBorderThickness(int row, int col)
        {
            // Thicker borders for 3x3 boxes
            var left = col % 3 == 0 ? 2 : 0.5;
            var top = row % 3 == 0 ? 2 : 0.5;
            var right = col == 8 ? 2 : 0.5;
            var bottom = row == 8 ? 2 : 0.5;

            return new Thickness(left, top, right, bottom);
        }

        private void RegenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPuzzle();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            int[,] userSolution = new int[9, 9];

            // Convert user input to grid
            foreach (var cell in _cells)
            {
                int value;
                if (int.TryParse(cell.Value, out value) && value >= 1 && value <= 9)
                {
                    userSolution[cell.Row, cell.Column] = value;
                }
                else if (!cell.IsFixed)
                {
                    // Empty or invalid cell
                    MessageBox.Show("Пожалуйста, заполните все ячейки числами от 1 до 9.");
                    return;
                }
            }

            // Check solution validity
            if (_sudokuGenerator.IsValidSudoku(userSolution))
            {
                bool isComplete = true;
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (userSolution[i, j] != _originalSolution[i, j])
                        {
                            isComplete = false;
                            break;
                        }
                    }
                    if (!isComplete) break;
                }

                if (isComplete)
                {
                    MessageBox.Show("Поздравляем! Ваше решение верное!");
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Решение соответствует правилам, но не совпадает с оригинальной головоломкой.");
                }
            }
            else
            {
                MessageBox.Show("Решение неверное. Пожалуйста, проверьте числа.");
            }
        }

        // Restrict input to numbers only
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) || e.Text == "0")
            {
                e.Handled = true;
                return;
            }

            // If the textbox already has a value, replace it
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Text.Length > 0)
            {
                textBox.Text = e.Text;
                textBox.CaretIndex = 1;
                e.Handled = true;
            }
        }

        // Prevent pasting non-numeric or multiple characters
        private void TextBox_PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if (!System.Text.RegularExpressions.Regex.IsMatch(text, "^[1-9]$"))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }

    public class SudokuCell : INotifyPropertyChanged
    {
        private string _value;
        public int Row { get; set; }
        public int Column { get; set; }

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public bool IsFixed { get; set; }
        public Brush TextColor { get; set; }
        public Brush Background { get; set; }
        public Thickness BorderThickness { get; set; }

        public ICommand ClearCommand => new RelayCommand(_ => Value = "");

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}