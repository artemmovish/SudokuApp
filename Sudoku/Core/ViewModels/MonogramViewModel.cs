using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sudoku.Core.ViewModels.SinglTone;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core.ViewModels
{
    public partial class MonogramViewModel : ObservableObject
    {
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
    new ObservableCollection<bool>() { false, false, false, false, false, false, false, false, false, false, true, true, false, false, false }, // Добавлен недостающий false
    new ObservableCollection<bool>() { false, false, false, false, true, false, false, false, false, false, false, false, false, false, false },
    new ObservableCollection<bool>() { false, false, false, false, true, true, false, false, false, true, true, false, false, false, false },
    new ObservableCollection<bool>() { false, false, false, false, true, false, false, false, false, true, true, false, false, false, false },
    new ObservableCollection<bool>() { false, false, false, true, true, true, true, true, true, true, true, true, true, true, true },
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
        
        public ObservableCollection<ObservableCollection<bool>> CurrentLevel { get; set; }

        public ObservableCollection<ObservableCollection<bool>> CurrentMatrix { get; set; }

        [ObservableProperty]
        string currentImage;

        public MonogramViewModel()
        {

            CurrentMatrix = new ObservableCollection<ObservableCollection<bool>>()
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


            switch (PageStorage.Instance.OpenMonogram)
            {
                case(0):
                    CurrentLevel = Level1;
                    CurrentImage = "E:\\Project\\Учебный процесс\\КПиЯП\\Cursach\\SudokuApp\\Sudoku\\Resource\\Card\\Gomel\\1.jpg";
                    break;
                case (1):
                    CurrentLevel = Level2;
                    CurrentImage = "E:\\Project\\Учебный процесс\\КПиЯП\\Cursach\\SudokuApp\\Sudoku\\Resource\\Card\\Gomel\\2.jpg";
                    break;
                default:
                    break;
            }
        }

        [RelayCommand]
        private void ToggleButton(Tuple<int, int> indices)
        {
            if (indices != null && indices.Item1 >= 0 && indices.Item2 >= 0)
            {
                int row = indices.Item1;
                int col = indices.Item2;

                if (row < CurrentLevel.Count && col < CurrentLevel[row].Count)
                {
                    CurrentLevel[row][col] = !CurrentLevel[row][col];
                }
            }
        }

        public bool CheckMatrix()
        {
            for (int i = 0; i < CurrentMatrix.Count; i++)
            {
                for (int j = 0; j < CurrentMatrix[i].Count; j++)
                {
                    if (Level2[i][j] != CurrentLevel[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}

