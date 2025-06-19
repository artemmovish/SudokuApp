using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core.Services
{
    using System.IO;
    using System.Windows.Media;

    public class MusicPlayer
    {
        private MediaPlayer _mediaPlayer;
        private string _currentTrackPath;

        public MusicPlayer()
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += (s, e) =>
            {
                // Автоповтор (по желанию)
                _mediaPlayer.Position = TimeSpan.Zero;
                _mediaPlayer.Play();
            };
        }

        /// <summary>
        /// Воспроизводит аудиофайл.
        /// </summary>
        /// <param name="filePath">Путь к файлу (.mp3, .wav и др.)</param>
        public void Play(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден!");

            if (_currentTrackPath != filePath)
            {
                _mediaPlayer.Stop();
                _mediaPlayer.Open(new Uri(filePath));
                _currentTrackPath = filePath;
            }

            _mediaPlayer.Play();
        }

        /// <summary>
        /// Останавливает воспроизведение.
        /// </summary>
        public void Stop()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Position = TimeSpan.Zero;
        }

        /// <summary>
        /// Пауза (если музыка играет).
        /// </summary>
        public void Pause()
        {
            if (_mediaPlayer.CanPause)
                _mediaPlayer.Pause();
        }

        /// <summary>
        /// Продолжает воспроизведение после паузы.
        /// </summary>
        public void Resume()
        {
            if (_mediaPlayer.Source != null)
                _mediaPlayer.Play();
        }

        /// <summary>
        /// Устанавливает громкость (0.0 - 1.0).
        /// </summary>
        public void SetVolume(double volume)
        {
            _mediaPlayer.Volume = Math.Clamp(volume, 0.0, 1.0);
        }
    }
}
