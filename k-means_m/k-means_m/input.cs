using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace k_means_m
{
    class Input
    {
        Point[] p = null;
        public Input()
        {
            init();
        }

        void init()
        {
            p = new Point[] { new Point(1, 1), new Point(6, 6), new Point(7, 7), new Point(3, 2), new Point(6, 8), new Point(2, 1), new Point(2, 3) };

        }

        public Point[] getPoint()
        {
            return p;
        }

    }
}
