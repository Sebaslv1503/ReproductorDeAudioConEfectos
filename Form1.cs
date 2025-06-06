﻿using AxWMPLib;
using Figures;
using MediaPlayerUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectMediaPlayerVisual
{
    public partial class Form1 : Form
    {
        private Effects effects = new Effects();
        private Timer marqueeTimer = new Timer();
        private MusicPlayer musicPlayer;
        private AudioVisualizer visualizer;
        private AudioVisualizer visualizer2;
        private VisualEffectManager effectManager;


        public Form1()
        {
            InitializeComponent();
            InicializarControles();
            effects.StartMarquee(lblTitulo, marqueeTimer, 2);
            musicPlayer = new MusicPlayer(axPlayer);
            volumenSlider.Value = musicPlayer.GetVolume();
            timerProgress.Start();
            axPlayer.PlayStateChange += axPlayer_PlayStateChange_1;
            musicPlayer.OnSongChanged += (title) => lblTitulo.Text = title;
            visualizer = new AudioVisualizer(panelVisualizer);
            visualizer2 = new AudioVisualizer(panel1);
            visualizer2.Start();
            visualizer.Start();
            

        }



        private void InicializarControles()
        {           
            this.BackColor = ColorTranslator.FromHtml("#1e1e2f");
            this.Text = "🎶 Reproductor de Música";
            this.StartPosition = FormStartPosition.CenterScreen;
            

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            musicPlayer.LoadFolder();

        }
            

        private void btnPlay_Click(object sender, EventArgs e) => musicPlayer.Play();

        private void btnPause_Click(object sender, EventArgs e) => musicPlayer.Pause();

        private void btnStop_Click(object sender, EventArgs e) => musicPlayer.Stop();

        private void btnNext_Click(object sender, EventArgs e) => musicPlayer.Next();

        private void btnPrevious_Click(object sender, EventArgs e) => musicPlayer.Previous();

        private void volumenSlider_Scroll(object sender, EventArgs e)
        {
            musicPlayer.SetVolume(volumenSlider.Value);
        }

        private void timerProgress_Tick(object sender, EventArgs e)
        {
            if (axPlayer.currentMedia != null && axPlayer.currentMedia.duration > 0)
            {
                double progress = musicPlayer.GetProgress();
                int trackValue = (int)Math.Round(progress * trackProgress.Maximum);

                if (trackValue >= trackProgress.Minimum && trackValue <= trackProgress.Maximum)
                {
                    trackProgress.Value = trackValue;
                }

                
                double currentSeconds = axPlayer.Ctlcontrols.currentPosition;
                int minutes = (int)currentSeconds / 60;
                int seconds = (int)currentSeconds % 60;
                lblTiempoActual.Text = $"{minutes:D2}:{seconds:D2}";
            }
            else
            {
                lblTiempoActual.Text = "00:00";
            }
        }


        private void trackProgress_Scroll(object sender, EventArgs e)
        {
            double newValue = trackProgress.Value / 100.0;
            musicPlayer.SetProgress(newValue);
        }

        private void axPlayer_PlayStateChange_1(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            switch (e.newState)
            {
                case 3: 
                    if (effectManager == null)
                        effectManager = new VisualEffectManager(picCanvas);
                    else
                        effectManager.Restart(); 

                    break;

                case 8: 
                    musicPlayer.HandleMediaEnded();
                    break;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            visualizer.Stop();
            base.OnFormClosing(e);
        }

    }
}
