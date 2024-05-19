using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5.Models
{
    public class Bird
    {
        public bool GameOver = false;
        public float x = 150f;
        public float y = 250f;
        public float size = 20f;
        public float verticalSpeed = 1f;
        private Graphics g;
        private Color color;

        const double maxWeight = 5.0;

        public const int input = 6;
        public const int output = 5;
        public const int hidden = 4;

        //public int[] Input = new int[input];
        public double[] Hidden = new double[hidden];
        public double[] Output = new double[output];

        public double[,] IHWeights = new double[input, hidden];
        public double[,] HOWeights = new double[hidden, output];

        public float Record = 0f;

        public Bird(Graphics g, int n)
        {
            this.g = g;
            color = Helpful.Colors[n % 10];
            Refresh();

            for (int i = 0; i < input; i++)
            {
                for (int j = 0; j < hidden; j++)
                {
                    IHWeights[i, j] = Helpful.random.NextDouble() + Helpful.random.Next(Convert.ToInt32(maxWeight) * 2) - maxWeight;
                }
            }

            for (int i = 0; i < hidden; i++)
            {
                for (int j = 0; j < output; j++)
                {
                    HOWeights[i, j] = Helpful.random.NextDouble() + Helpful.random.Next(Convert.ToInt32(maxWeight) * 2) - maxWeight;
                }
            }
        }

        public void Refresh()
        {
            y = 250f;
            verticalSpeed = 1f;
            Record = 0f;
            GameOver = false;
        }

        public void MakeTurn(float[] Input)
        {
            Hide();
            y += verticalSpeed;
            verticalSpeed += 1f;
            Record += 0.1f;
            var res = calculate(Input);
            switch (res)
            {
                case 1: Jump(); break;
                case 2: DownJump(); break;
                case 3: LongJump(); break;
                case 4: DownLongJump(); break;
            }
        }

        int calculate(float[] Input)
        {
            double[] Hidden = new double[hidden];
            double[] Output = new double[output];

            for (int i = 0; i < input; i++)
            {
                for (int j = 0; j < hidden; j++)
                {
                    Hidden[j] += IHWeights[i, j] * Input[i];
                }
            }

            for (int i = 0; i < hidden; i++)
            {
                for (int j = 0; j < output; j++)
                {
                    Output[j] += HOWeights[i, j] * Hidden[i];
                }
            }

            var res = Math.Max(Output[0], Math.Max(Output[1], Math.Max(Output[2], Math.Max(Output[3], Output[4]))));
            for (int i = 0; i < output; i++)
            {
                if (res == Output[i]) return i;
            }
            return -1;
        }

        public void Jump()
        {
            y -= 30f;
            verticalSpeed = 1f;
        }
        public void LongJump()
        {
            y -= 150f;
            verticalSpeed = 5f;
        }

        public void DownLongJump()
        {
            y += 150f;
            verticalSpeed = -5f;
        }

        public void DownJump()
        {
            y += 30f;
            verticalSpeed = 1f;
        }

        public void Show()
        {
            if (!GameOver)
            {
                SolidBrush brush = new SolidBrush(color);
                g.FillEllipse(brush, x - size, y - size, size, size);
            }
        }

        public void Hide()
        {
            if (!GameOver)
            {
                SolidBrush brush = new SolidBrush(Color.White);
                g.FillEllipse(brush, x - size, y - size, size, size);
            }
        }
    }
}
