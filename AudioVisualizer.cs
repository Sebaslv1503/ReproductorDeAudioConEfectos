using NAudio.Wave;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Figures
{
    public class AudioVisualizer
    {
        private Panel displayPanel;
        private Timer timer;
        private float[] barValues;
        private int barCount = 20;
        private WasapiLoopbackCapture capture;
        private float volume;

        public AudioVisualizer(Panel panel)
        {
            displayPanel = panel;
            barValues = new float[barCount];

            timer = new Timer();
            timer.Interval = 50; 
            timer.Tick += Timer_Tick;

            capture = new WasapiLoopbackCapture();
            capture.DataAvailable += OnDataAvailable;
        }

        public void Start()
        {
            capture.StartRecording();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            capture.StopRecording();
        }

        private void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            int bytesPerSample = 2;
            int sampleCount = e.BytesRecorded / bytesPerSample;

            float sum = 0f;

            for (int i = 0; i < e.BytesRecorded; i += bytesPerSample)
            {
                short sample = BitConverter.ToInt16(e.Buffer, i);
                float sample32 = sample / 32768f;
                sum += sample32 * sample32; // energía (cuadrado de la amplitud)
            }

            float rms = (float)Math.Sqrt(sum / sampleCount); // raíz cuadrada de energía promedio
            volume = rms;

            // Normaliza y actualiza barras solo si el volumen es significativo
            if (volume < 0.005f)
            {
                for (int i = 0; i < barCount; i++)
                    barValues[i] = 0;
                return;
            }

            float normalized = Math.Min(1f, volume * 10f); // ajusta el multiplicador según sensibilidad

            // Actualiza suavemente cada barra con algo de variación
            Random rand = new Random();
            for (int i = 0; i < barCount; i++)
            {
                float variance = (float)(rand.NextDouble() * 0.3 + 0.7); // pequeño ruido visual
                float targetValue = normalized * variance;
                // Suavizado para transiciones más agradables
                barValues[i] = barValues[i] * 0.6f + targetValue * 0.4f;
            }
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            using (Graphics g = displayPanel.CreateGraphics())
            {
                g.Clear(Color.Black);

                if (barCount == 0) return;

                int barWidth = Math.Max(1, displayPanel.Width / barCount); // evitar ancho 0

                for (int i = 0; i < barCount; i++)
                {
                    int height = (int)(displayPanel.Height * barValues[i]);
                    height = Math.Max(1, height); // evitar altura 0

                    Rectangle bar = new Rectangle(
                        i * barWidth,
                        displayPanel.Height - height,
                        barWidth - 2,
                        height
                    );

                    using (Brush b = new LinearGradientBrush(bar, Color.Magenta, Color.Cyan, 90f))
                    {
                        g.FillRectangle(b, bar);
                    }
                }
            }
        }


    }
}
