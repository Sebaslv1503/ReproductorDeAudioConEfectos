﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectMediaPlayerVisual
{
    public abstract class VisualizerEffect
    {
        public abstract void Draw(Graphics g, Rectangle bounds, float time);
    }
}
