using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectMediaPlayerVisual
{
    public class VisualEffectManager
    {
        private readonly PictureBox canvas;
        private readonly Timer switchTimer;
        private readonly Timer renderTimer;
        private readonly List<VisualizerEffect> effects;
        private VisualizerEffect currentEffect;
        private Random random = new Random();
        private float time = 0;

        public VisualEffectManager(PictureBox pictureBox)
        {
            canvas = pictureBox;
            effects = new List<VisualizerEffect>
            {
                new LissajousEffect(),
                new PulsingCirclesEffect(),
                new SpiralEffect()
            };

            currentEffect = effects[random.Next(effects.Count)];

            switchTimer = new Timer { Interval = 10000 }; // Cambia cada 10 segundos
            switchTimer.Tick += (s, e) => SwitchEffect();

            renderTimer = new Timer { Interval = 40 }; // ~25 FPS
            renderTimer.Tick += (s, e) => Render();

            switchTimer.Start();
            renderTimer.Start();
        }

        private void SwitchEffect()
        {
            int index = random.Next(effects.Count);
            currentEffect = effects[index];
            time = 0;
        }

        private void Render()
        {
            time += 0.04f;
            if (canvas.Width == 0 || canvas.Height == 0) return;

            using (Bitmap bmp = new Bitmap(canvas.Width, canvas.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                currentEffect.Draw(g, new Rectangle(0, 0, bmp.Width, bmp.Height), time);
                if (canvas.Image != null)
                    canvas.Image.Dispose();

                canvas.Image = (Bitmap)bmp.Clone();
            }
        }

        public void Stop()
        {
            renderTimer.Stop();
            switchTimer.Stop();
        }
    }
}
