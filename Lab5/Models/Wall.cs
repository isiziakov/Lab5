using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Models
{
    public class Wall
    {
        private Graphics g;
        public float x;
        public float width;
        public float size = 80f;
        public float ty, by;

        public Wall(Graphics g)
        {
            this.g = g;
            width = Helpful.random.Next(60) + 60;
            x = 1000 - width;
            by = Helpful.random.Next(300) + 100;
            ty = by - size;
        }

        public void Show()
        {
            SolidBrush brush = new SolidBrush(Color.Green);
            g.FillRectangle(brush, x, 0, width, ty);
            g.FillRectangle(brush, x, by, width, 500);
        }

        public void Hide()
        {
            SolidBrush brush = new SolidBrush(Color.White);
            g.FillRectangle(brush, x, 0, width, ty);
            g.FillRectangle(brush, x, by, width, 500);
        }
    }
}
