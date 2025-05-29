using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProyectMediaPlayerVisual
{
    public class SpinningStarsEffect : VisualizerEffect
    {
        private int starCount = 10;
        private float starRadius = 60;
        private float orbitRadius = 120;

        public override void Draw(Graphics g, Rectangle bounds, float time)
        {
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.Clear(Color.Black);

            float centerX = bounds.Width / 2f;
            float centerY = bounds.Height / 2f;

            for (int i = 0; i < starCount; i++)
            {
                float angle = (float)(i * 2 * Math.PI / starCount + time);
                float x = centerX + (float)(orbitRadius * Math.Cos(angle));
                float y = centerY + (float)(orbitRadius * Math.Sin(angle));

                float rotation = time * 2 + i;

                using (GraphicsPath starPath = CreateStar(new PointF(x, y), starRadius, 5, rotation))
                using (Pen pen = new Pen(ColorFromHSV((angle * 180 / Math.PI + time * 50) % 360, 1, 1), 2f))
                {
                    g.DrawPath(pen, starPath);
                }
            }
        }

        private GraphicsPath CreateStar(PointF center, float radius, int points, float rotation)
        {
            GraphicsPath path = new GraphicsPath();
            double angle = rotation;

            PointF firstPoint = PointF.Empty;
            PointF prevPoint = PointF.Empty;

            for (int i = 0; i < points * 2; i++)
            {
                double r = (i % 2 == 0) ? radius : radius / 2.5;
                double theta = i * Math.PI / points + angle;

                float x = center.X + (float)(r * Math.Cos(theta));
                float y = center.Y + (float)(r * Math.Sin(theta));
                PointF currentPoint = new PointF(x, y);

                if (i == 0)
                {
                    firstPoint = currentPoint;
                }
                else
                {
                    path.AddLine(prevPoint, currentPoint);
                }

                prevPoint = currentPoint;
            }

            // Cerrar figura uniendo el último punto con el primero
            path.AddLine(prevPoint, firstPoint);
            path.CloseFigure();

            return path;
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
