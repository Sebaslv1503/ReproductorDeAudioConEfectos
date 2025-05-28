using System;
using System.Windows.Forms;

namespace MediaPlayerUI
{
    public class Effects
    {
        private int marqueeOffset = 0;

        
        public void StartMarquee(Label label, Timer timer, int speed = 2)
        {
            timer.Interval = 1000 / 60; 
            timer.Tick += (s, e) =>
            {
                if (label.Text.Length == 0) return;

                marqueeOffset -= speed;
                if (marqueeOffset < -label.PreferredWidth)
                {
                    marqueeOffset = label.Parent.Width;
                }

                label.Left = marqueeOffset;
            };
            timer.Start();
        }

       
        public void ResetMarquee(Label label)
        {
            marqueeOffset = label.Parent.Width;
            label.Left = marqueeOffset;
        }
    }
}
