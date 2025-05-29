using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProyectMediaPlayerVisual
{
    public class PulsingCirclesEffect : VisualizerEffect
    {
        private int circleCount = 20;
        private float[] phases;

        public PulsingCirclesEffect()
        {
            phases = new float[circleCount];
            Random rand = new Random();
            for (int i = 0; i < circleCount; i++)
            {
                phases[i] = (float)(rand.NextDouble() * 2 * Math.PI);
            }
        }

        public override void Draw(Graphics g, Rectangle bounds, float time)
        {
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.Clear(Color.Black);

            float centerX = bounds.Width / 2f;
            float centerY = bounds.Height / 2f;
            float maxRadius = Math.Min(bounds.Width, bounds.Height) / 2f;

            for (int i = 0; i < circleCount; i++)
            {
                float phase = phases[i];
                float radius = maxRadius * 0.1f + (float)Math.Sin(time + phase) * maxRadius * 0.4f;

                // Posición circular para cada círculo
                double angle = 2 * Math.PI * i / circleCount;
                float offsetX = (float)Math.Cos(angle) * maxRadius * 0.4f;
                float offsetY = (float)Math.Sin(angle) * maxRadius * 0.4f;

                float x = centerX + offsetX - radius;
                float y = centerY + offsetY - radius;

                RectangleF circle = new RectangleF(x, y, radius * 2, radius * 2);

                using (Brush brush = new LinearGradientBrush(circle,
                    ColorFromHSV((time * 60 + i * 15) % 360, 1, 1),
                    Color.FromArgb(30, 255, 255, 255),
                    LinearGradientMode.ForwardDiagonal))
                {
                    g.FillEllipse(brush, circle);
                }
            }
        }

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value *= 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            switch (hi)
            {
                case 0: return Color.FromArgb(255, v, t, p);
                case 1: return Color.FromArgb(255, q, v, p);
                case 2: return Color.FromArgb(255, p, v, t);
                case 3: return Color.FromArgb(255, p, q, v);
                case 4: return Color.FromArgb(255, t, p, v);
                default: return Color.FromArgb(255, v, p, q);
            }
        }
    }
}
