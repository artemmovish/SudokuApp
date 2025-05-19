using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core.Services
{
    using System;
    using System.Collections.Generic;

    public class SudokuGenerator
    {
        private readonly Random _random = new Random();
        private const int Size = 9;
        private const int BoxSize = 3;
        private const int EmptyCell = 0;

        // Генерация новой судоку
        public int[,] GenerateSudoku(int difficultyLevel = 40)
        {
            // Создаем решенную судоку
            int[,] grid = new int[Size, Size];
            SolveSudoku(grid, true);

            // Удаляем некоторые числа в зависимости от уровня сложности
            RemoveNumbers(grid, difficultyLevel);

            return grid;
        }

        // Проверка валидности судоку
        public bool IsValidSudoku(int[,] grid)
        {
            // Проверяем строки
            for (int row = 0; row < Size; row++)
            {
                var rowSet = new HashSet<int>();
                for (int col = 0; col < Size; col++)
                {
                    if (grid[row, col] != EmptyCell && !rowSet.Add(grid[row, col]))
                        return false;
                }
            }

            // Проверяем столбцы
            for (int col = 0; col < Size; col++)
            {
                var colSet = new HashSet<int>();
                for (int row = 0; row < Size; row++)
                {
                    if (grid[row, col] != EmptyCell && !colSet.Add(grid[row, col]))
                        return false;
                }
            }

            // Проверяем квадраты 3x3
            for (int boxRow = 0; boxRow < BoxSize; boxRow++)
            {
                for (int boxCol = 0; boxCol < BoxSize; boxCol++)
                {
                    var boxSet = new HashSet<int>();
                    for (int row = 0; row < BoxSize; row++)
                    {
                        for (int col = 0; col < BoxSize; col++)
                        {
                            int actualRow = boxRow * BoxSize + row;
                            int actualCol = boxCol * BoxSize + col;
                            if (grid[actualRow, actualCol] != EmptyCell && !boxSet.Add(grid[actualRow, actualCol]))
                                return false;
                        }
                    }
                }
            }

            return true;
        }

        // Проверка, можно ли поставить число в конкретную клетку
        public bool IsSafe(int[,] grid, int row, int col, int num)
        {
            // Проверяем строку и столбец
            for (int x = 0; x < Size; x++)
            {
                if (grid[row, x] == num || grid[x, col] == num)
                    return false;
            }

            // Проверяем квадрат 3x3
            int boxStartRow = row - row % BoxSize;
            int boxStartCol = col - col % BoxSize;

            for (int r = 0; r < BoxSize; r++)
            {
                for (int c = 0; c < BoxSize; c++)
                {
                    if (grid[boxStartRow + r, boxStartCol + c] == num)
                        return false;
                }
            }

            return true;
        }

        // Решение судоку
        public bool SolveSudoku(int[,] grid, bool generate = false)
        {
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (grid[row, col] == EmptyCell)
                    {
                        // При генерации пробуем числа в случайном порядке
                        var numbers = generate ? GetRandomNumbers() : Enumerable.Range(1, Size).ToList();

                        foreach (int num in numbers)
                        {
                            if (IsSafe(grid, row, col, num))
                            {
                                grid[row, col] = num;

                                if (SolveSudoku(grid, generate))
                                    return true;

                                grid[row, col] = EmptyCell;
                            }
                        }
                        return false;
                    }
                }
            }
            return true;
        }

        // Удаление чисел из решенной судоку для создания головоломки
        private void RemoveNumbers(int[,] grid, int count)
        {
            while (count > 0)
            {
                int row = _random.Next(0, Size);
                int col = _random.Next(0, Size);

                if (grid[row, col] != EmptyCell)
                {
                    int backup = grid[row, col];
                    grid[row, col] = EmptyCell;

                    // Проверяем, что судоку имеет единственное решение
                    int[,] tempGrid = (int[,])grid.Clone();
                    if (!HasUniqueSolution(tempGrid))
                    {
                        grid[row, col] = backup;
                    }
                    else
                    {
                        count--;
                    }
                }
            }
        }

        // Проверка на единственное решение
        private bool HasUniqueSolution(int[,] grid)
        {
            int[,] tempGrid1 = (int[,])grid.Clone();
            int[,] tempGrid2 = (int[,])grid.Clone();

            // Пробуем решить двумя разными способами
            SolveSudoku(tempGrid1);
            SolveSudoku(tempGrid2, true); // с рандомизацией

            // Сравниваем решения
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (tempGrid1[row, col] != tempGrid2[row, col])
                        return false;
                }
            }

            return true;
        }

        // Генерация списка чисел в случайном порядке
        private List<int> GetRandomNumbers()
        {
            List<int> numbers = Enumerable.Range(1, Size).ToList();
            for (int i = numbers.Count - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                (numbers[i], numbers[j]) = (numbers[j], numbers[i]);
            }
            return numbers;
        }

        // Печать судоку (для отладки)
        public void PrintSudoku(int[,] grid)
        {
            for (int row = 0; row < Size; row++)
            {
                if (row % BoxSize == 0 && row != 0)
                    Console.WriteLine(new string('-', Size * 3 + BoxSize - 1));

                for (int col = 0; col < Size; col++)
                {
                    if (col % BoxSize == 0 && col != 0)
                        Console.Write("|");

                    Console.Write(grid[row, col] == EmptyCell ? " . " : $" {grid[row, col]} ");
                }
                Console.WriteLine();
            }
        }
    }
}
