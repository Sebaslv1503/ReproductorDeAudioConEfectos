using AxWMPLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Figures
{
    internal class MusicPlayer
    {
        private AxWindowsMediaPlayer player;
        private List<string> playlist = new List<string>();
        private int currentIndex = 0;
        public event Action<string> OnSongChanged;
        public MusicPlayer(AxWindowsMediaPlayer mediaPlayer)
        {
            player = mediaPlayer;
        }

        public void LoadFolder()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] files = Directory.GetFiles(folderDialog.SelectedPath, "*.mp3");
                    playlist.Clear();
                    playlist.AddRange(files);
                    currentIndex = 0;

                    if (playlist.Count > 0)
                        PlayCurrent();
                    else
                        MessageBox.Show("No se encontraron archivos .mp3 en la carpeta.");
                }
            }
        }

        private void PlayCurrent()
        {
            if (playlist.Count > 0 && currentIndex >= 0 && currentIndex < playlist.Count)
            {
                player.Ctlcontrols.stop();
                player.URL = playlist[currentIndex];
                player.Ctlcontrols.play();

                OnSongChanged?.Invoke(Path.GetFileNameWithoutExtension(playlist[currentIndex]));
            }
        }


        public void Play() => player.Ctlcontrols.play();

        public void Pause() => player.Ctlcontrols.pause();

        public void Stop() => player.Ctlcontrols.stop();

        public void Next()
        {
            if (playlist.Count == 0) return;
            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        public void Previous()
        {
            if (playlist.Count == 0) return;
            currentIndex = (currentIndex - 1 + playlist.Count) % playlist.Count;
            PlayCurrent();
        }

        public void SetVolume(int volume)
        {
            player.settings.volume = volume;
        }

        public int GetVolume()
        {
            return player.settings.volume;
        }

        public double GetProgress()
        {
            if (player.currentMedia != null && player.currentMedia.duration > 0)
                return player.Ctlcontrols.currentPosition / player.currentMedia.duration;
            return 0;
        }

        public void SetProgress(double value)
        {
            if (player.currentMedia != null)
                player.Ctlcontrols.currentPosition = value * player.currentMedia.duration;
        }
        public void HandleMediaEnded()
        {
            currentIndex++;

            if (currentIndex < playlist.Count)
                PlayCurrent();
            else
                currentIndex = 0; 
        }



    }
}
