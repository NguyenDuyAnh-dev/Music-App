using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {
        private List<string> _playlist = new List<string>();
        private IWavePlayer _waveOut; //Đối tượng để phát nhạc.
        private AudioFileReader _audioFileReader;
        private int _currentSongIndex = -1;
        private DispatcherTimer _timer; //Bộ đếm thời gian để cập nhật thời gian phát nhạc.
        private const string PlaylistFilePath = "playlist.txt"; // Đường dẫn tệp danh sách phát
        private bool _isPaused = false;

        public MainWindow()
        {
            InitializeComponent();
            SetupTimer();
            LoadPlaylist(); // Tải danh sách bài hát khi khởi động
        }

        private void SetupTimer()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1); //xác định khoảng thời gian giữa bộ điếm là 1s
            _timer.Tick += UpdatePlaybackTime; //mỗi khi sự kiện Tick xảy ra (tức là sau mỗi 1 giây), phương thức UpdatePlaybackTime sẽ được gọi.
        }

        private void LoadPlaylist()
        {
            if (File.Exists(PlaylistFilePath))
            {
                string[] songs = File.ReadAllLines(PlaylistFilePath); // đọc và lưu vô mảng
                foreach (var song in songs)//duyet het mảng
                {
                    if (File.Exists(song))
                    {
                        _playlist.Add(song);
                        Playlist.Items.Add(Path.GetFileName(song)); // toòn tại thì thêm đg dẫn vào danh sách nhạc
                    }
                }
            }
        }

        private void SavePlaylist()
        {
            File.WriteAllLines(PlaylistFilePath, _playlist); // ghi tất cả các đường dẫn tệp nhạc trong danh sách _playlist vào tệp playlist.txt
        }

        private void AddSongs_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog //đối tượng này sẽ hiển thị cửa sổ để người dùng có thể chọn các tệp từ hệ thống.
            {
                //Filter = "Audio Files|*.mp3;*.wav;*.aac;*.flac",
                Filter = "Audio Files|*.mp3;*.wav;*.aac;*.flac|Video Files|*.mp4",
                Multiselect = true //cho phép người dùng chọn nhiều tệp cùng một lúc. Nếu thiết lập này là false, người dùng chỉ có thể chọn một tệp duy nhất.
            };

            if (openFileDialog.ShowDialog() == true) // check xem có mở cửa sổ chọn tệp hay ko
            {
                foreach (var file in openFileDialog.FileNames) //lặp qua danh sách tệp đã chọn
                {
                    _playlist.Add(file); //add đường dẫn vào
                    Playlist.Items.Add(Path.GetFileName(file)); // thêm tên tệp ko có đường dẫn vào
                }
                SavePlaylist(); // Lưu danh sách sau khi thêm bài hát
            }
        }

        private void RemoveSong_Click(object sender, RoutedEventArgs e)
        {
            if (Playlist.SelectedIndex >= 0) 
            {
                int selectedIndex = Playlist.SelectedIndex;// tìm đến index bài hát đc select
                _playlist.RemoveAt(selectedIndex); //xóa khoi list
                Playlist.Items.RemoveAt(selectedIndex); // xóa khỏi UI

                if (_currentSongIndex == selectedIndex)
                {
                    StopCurrentSong(); // ngưng bài hát
                    _currentSongIndex = -1; // Đặt lại chỉ số bài hát hiện tại
                    SongInfo.Text = ""; // Xóa thông tin bài hát
                }
                else if (_currentSongIndex > selectedIndex)
                {
                    _currentSongIndex--; // Giảm chỉ số bài hát hiện tại nếu bài hát bị xóa nằm trước chỉ số hiện tại
                }
                SavePlaylist(); // Lưu danh sách sau khi xóa bài hát
            }
        }

        private void Playlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Playlist.SelectedIndex >= 0)
            {
                _currentSongIndex = Playlist.SelectedIndex; // tìm đến index bài hát đc select
                PlayCurrentSong();
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            //PlayCurrentSong();
            if (_isPaused && _waveOut != null) // nếu đâng pause hoặc chưa phát nhạc
            {
                _waveOut.Play(); //cho chạy nhạc
                _isPaused = false;
                _timer.Start(); // Tiếp tục cập nhật thời gian phát
            }
            else
            {
                PlayCurrentSong(); //chơi rồi mà bị pause bấm play để chơi tiếp
            }
        }

        private void PlayCurrentSong()
        {
            if (_currentSongIndex >= 0 && _currentSongIndex < _playlist.Count) // check bài hát có trong danh sách hay ko
            {
                StopCurrentSong(); // để dừng bài hát hiện tại nếu có.Khi bắt đầu phát một bài hát mới.

                _waveOut = new WaveOut();
                _audioFileReader = new AudioFileReader(_playlist[_currentSongIndex]); // đọc và lấy bài hát hiện tại
                _waveOut.Init(_audioFileReader); //Khởi tạo đối tượng WaveOut với dữ liệu âm thanh từ _audioFileReader. Điều này giúp chuẩn bị phát nhạc từ tệp âm thanh
                _waveOut.Play();
                _waveOut.PlaybackStopped += OnPlaybackStopped; // Đăng ký sự kiện khi phát nhạc dừng
                SongInfo.Text = $"Playing: {Path.GetFileName(_playlist[_currentSongIndex])}"; // cập nhật tên bài dang hát
                _timer.Start();
            }
        }

        private void OnPlaybackStopped(object sender, EventArgs e)
        {
            Next_Click(sender, new RoutedEventArgs()); // Chuyển sang bài hát tiếp theo
        }

        private void StopCurrentSong()
        {
            _timer.Stop();
            _waveOut?.Stop();
            _audioFileReader?.Dispose(); //giải phóng tài nguyên được cấp phát cho đối tượng
            _waveOut?.Dispose(); //giải phóng tài nguyên được cấp phát cho đối tượng
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            _waveOut?.Pause();
            _isPaused = true;
            _timer.Stop();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (ShuffleMode.IsChecked == true)
            {
                Random rand = new Random();
                _currentSongIndex = rand.Next(0, _playlist.Count);
            }
            else
            {
                if (_currentSongIndex < _playlist.Count - 1)
                {
                    _currentSongIndex++;
                }
                else
                {
                    _currentSongIndex = 0; // Quay lại bài đầu tiên nếu đã đến cuối danh sách
                }
            }
            Playlist.SelectedIndex = _currentSongIndex;
            PlayCurrentSong();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSongIndex > 0)
            {
                _currentSongIndex--;
                Playlist.SelectedIndex = _currentSongIndex;
                PlayCurrentSong();
            }
            else if (_currentSongIndex == 0)
            {
                _currentSongIndex = _playlist.Count - 1; // Quay lại bài hát cuối cùng nếu đang ở bài đầu tiên
                Playlist.SelectedIndex = _currentSongIndex;
                PlayCurrentSong();
            }
        }

        private void UpdatePlaybackTime(object sender, EventArgs e)
        {
            if (_audioFileReader != null)
            {
                CurrentTimeTextBlock.Text = $"{_audioFileReader.CurrentTime:mm\\:ss}";
                TotalTimeTextBlock.Text = $"{_audioFileReader.TotalTime:mm\\:ss}";
                PlaybackSlider.Maximum = _audioFileReader.TotalTime.TotalSeconds;
                PlaybackSlider.Value = _audioFileReader.CurrentTime.TotalSeconds;
            }
        }

        private void PlaybackSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_audioFileReader != null && PlaybackSlider.Value >= 0)
            {
                _audioFileReader.CurrentTime = TimeSpan.FromSeconds(PlaybackSlider.Value);
            }
        }
    }
}
