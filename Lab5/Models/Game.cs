using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab5.Models
{
    public class Game
    {
        float horizontalSpeed = 2.5f;
        List<Wall> walls = new List<Wall>();
        Bird[] birds = new Bird[20];
        Graphics g;
        public bool show;
        int ended = 0;
        int step = 0;

        public Game(Graphics g, bool show = true)
        {
            this.g = g;
            for (int i = 0; i < 20; i++)
            {
                birds[i] = new Bird(g, i);
            }
            walls.Add(new Wall(g));
            walls.Last().x -= 600f;
            walls.Add(new Wall(g));
            walls.Last().x -= 400f;
            walls.Add(new Wall(g));
            walls.Last().x -= 200f;
            walls.Add(new Wall(g));
            this.show = show;

            using (StreamWriter writer = new StreamWriter("log.txt", false))
            {
                writer.WriteLine("Начало");
            }

            if (show)
            {
                Show();
            }

            Teach();
        }

        public void Teach()
        {
            while (step < 1000)
            {
                while (ended != 20)
                {
                    MakeTurn();
                    Thread.Sleep(10);
                }
                birds = birds.OrderByDescending(x => x.Record).ToArray();
                Log();
                Refresh();
                g.Clear(Color.White);
                step++;
            }
        }

        public void Refresh()
        {
            ended = 0;
            horizontalSpeed = 2.5f;
            walls.Clear();
            walls.Add(new Wall(g));
            walls.Last().x -= 600f;
            walls.Add(new Wall(g));
            walls.Last().x -= 400f;
            walls.Add(new Wall(g));
            walls.Last().x -= 200f;
            walls.Add(new Wall(g));
            Genetic();
        }

        public void Genetic()
        {
            var newBirds = new Bird[10];
            for (int i = 0; i < 10; i++)
            {
                newBirds[i] = new Bird(g, i + 5);
                for (int k = 0; k < Bird.input; k++)
                {
                    for (int j = 0; j < Bird.hidden; j++)
                    {
                        newBirds[i].IHWeights[k, j] = (birds[i * 2].IHWeights[k, j] + birds[i * 2 + 1].IHWeights[k, j]) / 2;
                    }
                }

                for (int k = 0; k < Bird.hidden; k++)
                {
                    for (int j = 0; j < Bird.output; j++)
                    {
                        newBirds[i].HOWeights[k, j] = (birds[i * 2].HOWeights[k, j] + birds[i * 2 + 1].HOWeights[k, j]) / 2;
                    }
                }
            }
            for(int i = 0; i < 20; i++)
            {
                birds[i].Refresh();
                if (i > 4 && i < 15)
                {
                    birds[i] = newBirds[i - 5];
                }
                if (i > 14)
                {
                    birds[i] = new Bird(g, i);
                }
            }
        }

        public void MakeTurn()
        {
            for (int i = 0; i < walls.Count; i++)
            {
                walls[i].Hide();
                walls[i].x -= horizontalSpeed;
            }
            foreach (Bird bird in birds)
            {
                if (!bird.GameOver)
                {
                    var input = new float[8];
                    input[0] = bird.verticalSpeed;
                    input[1] = horizontalSpeed;
                    input[2] = bird.y;
                    input[3] = 500 - bird.y;
                    input[4] = walls[0].ty - bird.y;
                    input[5] = walls[0].by - bird.y;
                    //input[6] = walls[0].x - bird.x;
                    //input[7] = walls[0].x + walls[0].width - bird.x;
                    bird.MakeTurn(input);
                    CheckBreak(bird);
                }
            }
            

            if (walls.FirstOrDefault() != null && walls.FirstOrDefault().x < 5f)
            {
                walls.RemoveAt(0);
            }
            if (walls.LastOrDefault() != null && walls.LastOrDefault().x + 400f < 1000f)
            {
                walls.Add(new Wall(g));
            }

            horizontalSpeed += 0.01f;

            if (show)
            {
                Show();
            }
        }

        private void Show()
        {
            //g.Clear(Color.White);
            foreach (Bird bird in birds)
            {
                if (!bird.GameOver) bird.Show();
            }
            foreach (Wall wall in walls)
            {
                wall.Show();
            }
        }

        private void CheckBreak(Bird bird)
        {
            if (bird.y < 0 || bird.y > 500)
            {
                bird.GameOver = true;
                ended++;
                return;
            }
            foreach (Wall wall in walls)
            {
                if ((bird.x > wall.x && bird.x < wall.x + wall.width) && (bird.y < wall.ty || bird.y > wall.by))
                {
                    bird.GameOver = true;
                    ended++;
                    return;
                }
            }
        }

        public void Log()
        {
            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine(step.ToString());
                foreach (Bird item in birds)
                {
                    writer.WriteLine(item.Record.ToString());
                }
            }
        }
    }
}
