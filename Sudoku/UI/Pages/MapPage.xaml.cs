using Sudoku.Core.ViewModels.SinglTone;
using Sudoku.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using System.Xml.Linq;

namespace Sudoku.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для MapPage.xaml
    /// </summary>
    public partial class MapPage : Page
    {
        public LevelControl? LastLevel { get; set; }

        public MapPage()
        {
            InitializeComponent();
            LastLevel = null;
        }

        public void LoadSave()
        {
            var list = PageStorage.Instance.CompletedLevels;

            foreach (var item in list)
            {
                switch (item.Name)
                {
                    case "VitebskLevel_IronGift":
                        VitebskLevel_IronGift.LockCount = item.CountLock;
                        break;
                    case "VitebskLevel_Theatre":
                        VitebskLevel_Theatre.LockCount = item.CountLock;
                        break;
                    case "MogilevLevel_Theatre":
                        MogilevLevel_Theatre.LockCount = item.CountLock;
                        break;
                    case "MogilevLevel_IronGift":
                        MogilevLevel_IronGift.LockCount = item.CountLock;
                        break;
                    case "GomelLevel_Home":
                        GomelLevel_Home.LockCount = item.CountLock;
                        break;
                    case "GomelLevel_Bank":
                        GomelLevel_Bank.LockCount = item.CountLock;
                        break;
                    default:
                        // Можно добавить логирование или обработку неизвестных уровней
                        break;
                }
            }

            StartPeriodicCheck();

            CheckLevelVisible();
        }

        private void ToBackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        private void CardClose_Click(object sender, RoutedEventArgs e)
        {
            var ctr = (UIElement)sender;
            Panel.SetZIndex(ctr, 0);
            ctr.Visibility = Visibility.Hidden;
        }

        private void SettingBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(PageStorage.Instance.SettingsPage);
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
                var ctr = (LevelControl)sender;

                if (LastLevel != null && LastLevel.Width >= 200 
                    && LastLevel != ctr)
                {
                    await ChangeSize(LastLevel, _cts.Token);
                }

                await ChangeSize(ctr, _cts.Token);
                LastLevel = ctr;
            }
            catch (OperationCanceledException)
            {
                // Анимация была отменена (например, новым кликом) — ничего не делаем
            }
            finally
            {
                CheckLevelVisible();
                _isAnimating = false;
            }

            CheckLevelVisible();
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
                ctr.SetVisibleLockPanel(Visibility.Visible);

                if (ctr.IsComplete)
                {
                    string name = ctr.Name;

                    switch (name)
                    {
                        case "VitebskLevel_IronGift":
                            ChangeVisivleCard(VitebskCard_IronGift);
                            break;
                        case "VitebskLevel_Theatre":
                            ChangeVisivleCard(VitebskCard_Theatre);
                            break;
                        case "MogilevLevel_Theatre":
                            ChangeVisivleCard(MogilevCard_Theatre);
                            break;
                        case "MogilevLevel_IronGift": // Обратите внимание на опечатку в имени (IronGif вместо IronGift)
                            ChangeVisivleCard(MogilevCard_IronGift);
                            break;
                        case "GomelLevel_Home":
                            ChangeVisivleCard(GomelCard_Home);
                            break;
                        case "GomelLevel_Bank":
                            ChangeVisivleCard(GomelCard_Bank);
                            break;
                        default:
                            // Можно добавить логирование или обработку неизвестных уровней
                            break;
                    }
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

                    await Task.Delay(32, cancellationToken); // ~30 FPS
                }
                ctr.SetVisibleLockPanel(Visibility.Collapsed);
            }
        }

        private void ChangeVisivleCard(CardControl ctr)
        {
            Panel.SetZIndex(ctr, 3);
            ctr.Visibility = Visibility.Visible;

            CheckLevelVisible();
        }

        private void ToAboutBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Не сделано");
        }

        public void CheckLevelVisible()
        {
            int openLockCount = PageStorage.Instance.OpenLockCount;

            // Всегда сбрасываем видимость (если нужно)
            MogilevLevel_Theatre.Visibility = Visibility.Collapsed;
            MogilevLevel_IronGift.Visibility = Visibility.Collapsed;
            GomelLevel_Home.Visibility = Visibility.Collapsed;
            GomelLevel_Bank.Visibility = Visibility.Collapsed;

            // Устанавливаем видимость в зависимости от OpenLockCount
            if (openLockCount >= 3)
            {
                ChangeImageSourceFromResources("/Resource/Map/level2.png");
                MogilevLevel_Theatre.Visibility = Visibility.Visible;
            }
            if (openLockCount >= 3)
            {
                MogilevLevel_IronGift.Visibility = Visibility.Visible;
            }
            if (openLockCount >= 6)
            {
                ChangeImageSourceFromResources("/Resource/Map/level3.png"); // Если нужно другое изображение, замени путь
                GomelLevel_Home.Visibility = Visibility.Visible;
            }
            if (openLockCount >= 6)
            {
                GomelLevel_Bank.Visibility = Visibility.Visible;
            }
        }

        public void ChangeImageSourceFromResources(string resourcePath)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(resourcePath, UriKind.Relative);
            bitmap.EndInit();

            MapImage.Source = bitmap;
        }

        public void ChangeImageSource(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();

                MapImage.Source = bitmap;
            }
            else
            {
                // Обработка случая, когда файл не найден
                MapImage.Source = null; // или установить изображение по умолчанию
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CheckLevelVisible();
        }


        private DispatcherTimer _timer;

        public void StartPeriodicCheck()
        {
            // Создаем таймер с интервалом 3 секунд
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Этот метод вызывается каждые 5 секунд
            CheckLevelVisible(); // Вызываем твой метод
                                 // Можно добавить другие действия
        }

        public void StopPeriodicCheck()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
                _timer = null;
            }
        }
    }
}
