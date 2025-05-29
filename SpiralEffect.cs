using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProyectMediaPlayerVisual
{
    public class SpiralEffect : VisualizerEffect
    {
        public override void Draw(Graphics g, Rectangle bounds, float time)
        {
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.Clear(Color.Black);

            float centerX = bounds.Width / 2f;
            float centerY = bounds.Height / 2f;

            int points = 2000;
            double angleStep = 0.1;
            double radiusStep = 0.5;

            PointF[] spiralPoints = new PointF[points];

            for (int i = 0; i < points; i++)
            {
                double angle = i * angleStep + time; // tiempo da rotación
                double radius = i * radiusStep * (0.5 + 0.5 * Math.Sin(time * 0.5)); // expansión y contracción animada

                float x = (float)(centerX + radius * Math.Cos(angle));
                float y = (float)(centerY + radius * Math.Sin(angle));

                spiralPoints[i] = new PointF(x, y);
            }

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddLines(spiralPoints);

                using (Pen pen = new Pen(ColorFromHSV((time * 40) % 360, 1, 1), 1.5f))
                {
                    g.DrawPath(pen, path);
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
