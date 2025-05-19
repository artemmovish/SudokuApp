using Sudoku.UI.Windows;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public LevelControl()
        {
            InitializeComponent();
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
                        lockPanel.Children.Remove(clickedLock);
                        LockCount--;
                    }
                };

                lockPanel.Children.Add(lockImage);
            }
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