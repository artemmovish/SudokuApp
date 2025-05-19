using Sudoku.Core.ViewModels.SinglTone;
using Sudoku.UI.Controls;
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
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        public MapPage()
        {
            InitializeComponent();
        }

        private void ToBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void VitebskCard_Click(object sender, RoutedEventArgs e)
        {
            Panel.SetZIndex(VitebskCard, 0);
        }

        private bool _isAnimating = false;
        private CancellationTokenSource _cts;

        private async void Level_Click(object sender, RoutedEventArgs e)
        {
            if (_isAnimating) return; // Если анимация уже идёт — игнорируем клик

            _isAnimating = true;
            _cts = new CancellationTokenSource();

            try
            {
                await ChangeSize((LevelControl)sender, _cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Анимация была отменена (например, новым кликом) — ничего не делаем
            }
            finally
            {
                _isAnimating = false;
            }
        }

        private async Task ChangeSize(LevelControl ctr, CancellationToken cancellationToken)
        {
            double initialLeft = Canvas.GetLeft(ctr);
            double initialTop = Canvas.GetTop(ctr);

            double startWith = ctr.Width;
            double startHeight = ctr.Height;

            if (ctr.Width < 200)
            {
                // Анимация увеличения
                while (ctr.Width < 200)
                {
                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    ctr.Width += 5;
                    ctr.Height += 5; // если нужно сохранять пропорции

                    // Корректируем позицию для масштабирования от центра
                    Canvas.SetLeft(ctr, initialLeft - (ctr.Width - startWith) / 2);
                    Canvas.SetTop(ctr, initialTop - (ctr.Height - startHeight) / 2);

                    await Task.Delay(16, cancellationToken); // ~60 FPS
                }
            }
            else
            {
                // Анимация уменьшения
                while (ctr.Width > 90)
                {
                    if (cancellationToken.IsCancellationRequested)
                        cancellationToken.ThrowIfCancellationRequested();

                    ctr.Width -= 5;
                    ctr.Height -= 5; // если нужно сохранять пропорции

                    // Корректируем позицию для масштабирования к центру
                    Canvas.SetLeft(ctr, initialLeft + (startWith - ctr.Width) / 2);
                    Canvas.SetTop(ctr, initialTop + (startHeight - ctr.Height) / 2);

                    await Task.Delay(16, cancellationToken); // ~60 FPS
                }
            }
        }
    }
}
