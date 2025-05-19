using Sudoku.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

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
            // Get difficulty from singleton (replace with your actual singleton)
            int difficulty = 40; // Default value if singleton not available
                                 // difficulty = GameSettings.Instance.DifficultyLevel; // Uncomment and use your actual singleton

            _currentPuzzle = _sudokuGenerator.GenerateSudoku(difficulty);
            _originalSolution = (int[,])_currentPuzzle.Clone();
            _sudokuGenerator.SolveSudoku(_originalSolution); // Store the complete solution

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
                        Background = GetBoxBackground(row, col)
                    };

                    _cells.Add(cell);
                }
            }

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

        private void RegenerateButton_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewPuzzle();
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            int[,] userSolution = new int[9, 9];

            // Конвертируем ввод пользователя в сетку
            foreach (var cell in _cells)
            {
                int value;
                if (int.TryParse(cell.Value, out value) && value >= 1 && value <= 9)
                {
                    userSolution[cell.Row, cell.Column] = value;
                }
                else if (!cell.IsFixed)
                {
                    // Пустая или неверная ячейка
                    MessageBox.Show("Пожалуйста, заполните все ячейки числами от 1 до 9.");
                    return;
                }
            }

            // Проверяем корректность решения
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
    }

    public class SudokuCell
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public string Value { get; set; }
        public bool IsFixed { get; set; }
        public Brush TextColor { get; set; }
        public Brush Background { get; set; }
    }
}