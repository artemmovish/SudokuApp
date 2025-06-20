﻿using Sudoku.Core.Services;
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
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        MusicPlayer MusicPlayer = new();
        public SettingsPage()
        {
            InitializeComponent();
            
        }

        private void ToBackBtn_Click(object sender, RoutedEventArgs e)
        {
            var dif = (int)DifficultySlider.Value;
            PageStorage.Instance.Difficulty = (int) (50.0 / 100 * dif);
            NavigationService?.GoBack();
        }

        private void SoundToggle_Checked(object sender, RoutedEventArgs e)
        {
            MusicPlayer.Play("");
        }

        private void SoundToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            MusicPlayer.Stop();
        }
    }
}
