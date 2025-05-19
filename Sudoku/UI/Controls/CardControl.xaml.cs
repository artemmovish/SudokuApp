using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku.UI.Controls
{
    public partial class CardControl : UserControl
    {
        // Текущий режим отображения
        private bool _showVideo = false;

        // Dependency Properties
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(CardControl));

        public static readonly DependencyProperty VideoSourceProperty =
            DependencyProperty.Register("VideoSource", typeof(string), typeof(CardControl));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(CardControl));

        public static readonly DependencyProperty InformationProperty =
            DependencyProperty.Register("Information", typeof(string), typeof(CardControl));

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(CardControl));
        public string ImageSource
        {
            get => (string)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public string VideoSource
        {
            get => (string)GetValue(VideoSourceProperty);
            set => SetValue(VideoSourceProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Information
        {
            get => (string)GetValue(InformationProperty);
            set => SetValue(InformationProperty, value);
        }

        public CardControl()
        {
            InitializeComponent();
            ShowImage(); // По умолчанию показываем изображение
        }

        private void ShowImage_Click(object sender, RoutedEventArgs e)
        {
            ShowImage();
        }

        private void ShowVideo_Click(object sender, RoutedEventArgs e)
        {
            ShowVideo();
        }

        private void ShowImage()
        {
            _showVideo = false;
            ContentImage.Visibility = Visibility.Visible;
            ContentVideo.Visibility = Visibility.Collapsed;
            PlayPauseButton.Content = "Воспроизвести";
            ContentVideo.Pause();
            PlayPauseButton.Visibility = Visibility.Collapsed;
        }

        private void ShowVideo()
        {
            _showVideo = true;
            ContentImage.Visibility = Visibility.Collapsed;
            ContentVideo.Visibility = Visibility.Visible;
            PlayPauseButton.Visibility = Visibility.Visible;

            if (PlayPauseButton.Content.ToString() == "Пауза") return;

            PlayPauseButton.Content = "Воспроизвести";
        }

        private void TogglePlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (!_showVideo) return;

            if (ContentVideo.Source == null) return;

            if (PlayPauseButton.Content.ToString() == "Пауза")
            {
                // Если видео воспроизводится - ставим на паузу
                ContentVideo.Pause();
                PlayPauseButton.Content = "Воспроизвести";
            }
            else
            {
                // Если на паузе или не начато - воспроизводим
                ContentVideo.Play();
                PlayPauseButton.Content = "Пауза";
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
    }
}