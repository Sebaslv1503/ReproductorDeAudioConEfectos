using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProyectMediaPlayerVisual
{
    public class LissajousEffect : VisualizerEffect
    {
        public override void Draw(Graphics g, Rectangle bounds, float time)
        {
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.Clear(Color.Black);

            // Parámetros dinámicos que cambian con el tiempo para animación
            float a = 2 + (float)Math.Sin(time * 0.5f); // oscila entre 1 y 3
            float b = 3 + (float)Math.Cos(time * 0.4f); // oscila entre 2 y 4
            float delta = time; // rotación dinámica

            int points = 1000;
            PointF[] path = new PointF[points];

            for (int i = 0; i < points; i++)
            {
                float t = (float)(i * 2 * Math.PI / points);
                float x = (float)(Math.Sin(a * t + delta));
                float y = (float)(Math.Sin(b * t));

                x = bounds.Width / 2 + x * bounds.Width / 3;
                y = bounds.Height / 2 + y * bounds.Height / 3;

                path[i] = new PointF(x, y);
            }

            // Color neon dinámico que varía con el tiempo
            Color neonColor = ColorFromHSV((time * 50) % 360, 1, 1);
            using (Pen pen = new Pen(neonColor, 2))
            {
                g.DrawLines(pen, path);
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
