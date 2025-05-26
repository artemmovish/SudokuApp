using Sudoku.UI.Windows;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sudoku.UI.Controls
{
    public partial class LevelControl : UserControl
    {
        public static readonly RoutedEvent ClickEvent =
    EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
        typeof(RoutedEventHandler), typeof(LevelControl));

        public static readonly DependencyProperty LockCountProperty =
    DependencyProperty.Register("LockCount", typeof(int), typeof(LevelControl),
    new PropertyMetadata(0, OnLockCountChanged));

        public static readonly DependencyProperty TaskTypeProperty =
    DependencyProperty.Register("TaskType", typeof(string), typeof(LevelControl),
    new PropertyMetadata(string.Empty));

        public bool IsComplete { get; set; }

        public LevelControl()
        {
            InitializeComponent();
            IsComplete = false;
        }

        public void SetVisibleLockPanel(Visibility visibility)
        {
            lockPanel.Visibility = visibility;
        }

        // Свойство для количества замков
        public int LockCount
        {
            get { return (int)GetValue(LockCountProperty); }
            set { SetValue(LockCountProperty, value); }
        }

        // Свойство для типа задания (можно заменить на нужный вам enum)
        public string TaskType
        {
            get { return (string)GetValue(TaskTypeProperty); }
            set { SetValue(TaskTypeProperty, value); }
        }

        private static void OnLockCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as LevelControl;
            control?.UpdateLocks();
        }

        private void UpdateLocks()
        {
            lockPanel.Children.Clear();

            for (int i = 0; i < LockCount; i++)
            {
                var lockImage = new Image
                {
                    Source = new BitmapImage(new Uri("/Resource/Items/lock.png", UriKind.Relative)),
                    Width = 500,
                    Cursor = Cursors.Hand
                };

                lockImage.MouseDown += async (sender, e) =>
                {
                    var clickedLock = (Image)sender;
                    var window = new SudokuLevelWindow();

                    if (window.ShowDialog() == true)
                    {
                        await AnimateLockFall(clickedLock);
                        lockPanel.Children.Remove(clickedLock);
                        LockCount--;
                        if (LockCount < 1)
                        {
                            Chain.Visibility = Visibility.Collapsed;
                            IsComplete = true;
                        }
                    }
                };

                lockPanel.Children.Add(lockImage);
            }
        }

        private async Task AnimateLockFall(Image lockImage)
        {
            // Создаем анимацию падения
            var fallAnimation = new DoubleAnimation
            {
                From = 0,
                To = 500, // расстояние падения
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // Создаем анимацию прозрачности
            var fadeAnimation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
                BeginTime = TimeSpan.FromSeconds(0.5) // начинаем исчезать после половины падения
            };

            // Создаем анимацию вращения
            var rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 90, // угол вращения
                Duration = TimeSpan.FromSeconds(1)
            };

            // Применяем все анимации
            var transformGroup = new TransformGroup();
            var translateTransform = new TranslateTransform();
            var rotateTransform = new RotateTransform();
            transformGroup.Children.Add(translateTransform);
            transformGroup.Children.Add(rotateTransform);
            lockImage.RenderTransform = transformGroup;
            lockImage.RenderTransformOrigin = new Point(0.5, 0.5); // вращение вокруг центра

            // Запускаем анимации
            translateTransform.BeginAnimation(TranslateTransform.YProperty, fallAnimation);
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            lockImage.BeginAnimation(UIElement.OpacityProperty, fadeAnimation);

            // Ждем завершения анимации
            await Task.Delay(1000);
        }

        public event RoutedEventHandler Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                RaiseEvent(new RoutedEventArgs(ClickEvent, this));
            }
        }

        public bool CheckLock()
        {
            if (lockPanel.Children.Count == 0) return true;
            return false;
        }
    }
}