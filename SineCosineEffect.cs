using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ProyectMediaPlayerVisual
{
    public class SineCosineEffect : VisualizerEffect
    {
        private float phase = 0f;

        public override void Draw(Graphics g, Rectangle bounds, float time)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            int amplitude = bounds.Height / 4;
            int midY = bounds.Height / 2;

            using (Pen sinePen = new Pen(Color.Cyan, 2))
            using (Pen cosinePen = new Pen(Color.Magenta, 2))
            {
                PointF[] sinePoints = new PointF[bounds.Width];
                PointF[] cosinePoints = new PointF[bounds.Width];

                for (int x = 0; x < bounds.Width; x++)
                {
                    float t = (x + phase) * 0.05f;
                    float ySine = midY + (float)Math.Sin(t) * amplitude;
                    float yCos = midY + (float)Math.Cos(t) * amplitude;

                    sinePoints[x] = new PointF(x, ySine);
                    cosinePoints[x] = new PointF(x, yCos);
                }

                g.DrawLines(sinePen, sinePoints);
                g.DrawLines(cosinePen, cosinePoints);
            }

            // Animar fase
            phase += 5f;
            if (phase > 10000) phase = 0;
        }
    }
}
