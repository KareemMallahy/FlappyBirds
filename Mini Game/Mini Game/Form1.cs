using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mini_Game
{
    class backGroundActor
    {
        public Bitmap Image;
        public int x, y, width, height;
    }

    class BirdActor
    {
        public Bitmap Image;
        public int x, y, width, height;
    }

    class ColumnsActor
    {
        public Bitmap Image;
        public int x, y, width, height;
    }

    public partial class Form1 : Form
    {
        List<ColumnsActor> C = new List<ColumnsActor>();
        List<backGroundActor> LBG = new List<backGroundActor>();
        List<BirdActor> Bird = new List<BirdActor>();
        Timer tt = new Timer();
        Bitmap off;
        int countScreens = 0;
        int countTick = 0;
        int score = 0;
        int prev = 1000;
        int flagDead = 0;

        void check_Dead()
        {
            for (int i = 0; i < C.Count; i++)
            {
                if (Bird[0].x > C[i].x && Bird[0].x < C[i].x + 20)
                {
                    if (Bird[0].y < C[i + 1].y + C[i + 1].height || Bird[0].y > C[i].y)
                    {
                        flagDead = 1;
                        MessageBox.Show("Game Over Your Score: " + score);
                    }
                    i++;
                }
            }
        }

        void Increase_Score()
        {
            for (int i = 0; i < C.Count; i++)
            {
                if (i != prev && i != prev + 1)
                {
                    if (Bird[0].x >= C[i].x && Bird[0].x <= C[i].x + 20)
                    {
                        if (Bird[0].y > C[i + 1].y + C[i + 1].height && Bird[0].y < C[i].y)
                        {
                            score++; 
                            prev = i;
                            i++;
                            break;
                        }
                        
                    }
                }
            }
        }

        void CreateMorePipes()
        {
            int x = C[C.Count - 1].x + 400;
            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                ColumnsActor Column;

                if (i % 2 == 0)
                {
                    Column = new ColumnsActor();
                    Column.Image = new Bitmap("pipe 1.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = r.Next(300, 500);
                    Column.x = x;
                    Column.y = this.Height - Column.height + 50;

                    C.Add(Column);
                }
                else
                {
                    Column = new ColumnsActor();
                    Column.Image = new Bitmap("pipe 2.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = C[C.Count - 1].y - 150;
                    Column.x = x;
                    Column.y = 0;
                    C.Add(Column);

                    x += 400;
                }
            }
        }


        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.Load +=new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
            tt.Tick += new EventHandler(tt_Tick);
            tt.Start();
        }

        void tt_Tick(object sender, EventArgs e)
        {
            if (flagDead == 0)
            {
                if (countTick % 2 == 0)
                {
                    LBG[countScreens].x += 20;
                    if (LBG.Count - 1 > countScreens)
                    {
                        LBG[countScreens + 1].x -= 20;
                    }
                    for (int i = 0; i < C.Count; i++)
                    {
                        C[i].x -= 20;
                    }

                    if (LBG[countScreens].x <= 1000)
                    {
                        //MessageBox.Show("Comp");
                        if (LBG[countScreens].x + this.Width >= LBG[countScreens].Image.Width)
                        {
                            //MessageBox.Show("Completed");
                            backGroundActor pnn = new backGroundActor();
                            pnn.Image = new Bitmap("backGround.png");
                            pnn.x = LBG[countScreens].x + LBG[countScreens].Image.Width;
                            pnn.y = 0;
                            pnn.width = pnn.Image.Width;
                            pnn.height = pnn.Image.Height;
                            LBG.Add(pnn);
                            countScreens++;
                        }
                    }
                    for (int i = 0; i < Bird.Count; i++)
                    {
                        Bird[i].y += 10;
                    }
                    if (countTick % 10 == 0)
                    {
                        CreateMorePipes();
                    }
                }

                check_Dead();
                Increase_Score();
                countTick++;
                DrawDubb(this.CreateGraphics());
            }
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(this.CreateGraphics());   
        }

        void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (flagDead == 0)
            {
                if (e.KeyCode == Keys.Space)
                {
                    for (int i = 0; i < Bird.Count; i++)
                    {
                        Bird[i].y -= 10;
                    }
                }

                LBG[countScreens].x += 20;
                if (LBG.Count - 1 > countScreens)
                {
                    LBG[countScreens + 1].x -= 20;
                }
                for (int i = 0; i < C.Count; i++)
                {
                    C[i].x -= 20;
                }

                Increase_Score();
                check_Dead();
                DrawDubb(this.CreateGraphics());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            create_world();
            insert_Bird();
        }

        void create_world()
        {
            backGroundActor pnn = new backGroundActor();
            pnn.Image = new Bitmap("backGround.png");
            pnn.x = 0;
            pnn.y = 0;
            pnn.width = pnn.Image.Width;
            pnn.height = pnn.Image.Height;
            LBG.Add(pnn);

            Random r = new Random();
            int x = 400;
            for (int i = 0; i < 10; i++)
            {
                ColumnsActor Column;
 
                if (i % 2 == 0)
                {
                    Column = new ColumnsActor();
                    Column.Image = new Bitmap("pipe 1.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = r.Next(300, 500);
                    Column.x = x;
                    Column.y = this.Height - Column.height + 50;

                    C.Add(Column);
                }
                else
                {
                    Column = new ColumnsActor();
                    Column.Image = new Bitmap("pipe 2.png");
                    Column.Image.MakeTransparent(Color.White);
                    Column.height = C[C.Count - 1] .y - 150;
                    Column.x = x;
                    Column.y = 0;
                    C.Add(Column);

                    x += 400;
                }
            }
        }

        void insert_Bird()
        {
            BirdActor pnn = new BirdActor();
            pnn.Image = new Bitmap("bird.png");
            pnn.Image.MakeTransparent(Color.White);
            pnn.x = 50;
            pnn.y = this.Height / 2;
            Bird.Add(pnn);
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            for (int i = 0; i < LBG.Count; i++)
            {
                g.DrawImage(LBG[i].Image,
                    new Rectangle(0, LBG[i].y, LBG[i].width, LBG[i].height),
                    new Rectangle(LBG[i].x, 0, ClientSize.Width, ClientSize.Height),
                    GraphicsUnit.Pixel);
            }

            for (int i = 0; i < C.Count; i++)
            {
                if (i % 2 == 0)
                {
                    g.DrawImage(C[i].Image,
                        new Rectangle(C[i].x, C[i].y, C[i].Image.Width, C[i].height),
                        new Rectangle(0, 0, ClientSize.Width, C[i].height),
                        GraphicsUnit.Pixel);
                }
                else
                {
                    g.DrawImage(C[i].Image,
                        new Rectangle(C[i].x, C[i].y, C[i].Image.Width, C[i].height),
                        new Rectangle(0, 0, ClientSize.Width, C[i].Image.Height),
                        GraphicsUnit.Pixel);
                }
            }

            for (int i = 0; i < Bird.Count; i++)
            {
                g.DrawImage(Bird[i].Image,
                    new Rectangle(Bird[i].x, Bird[i].y, Bird[i].Image.Width + 300, Bird[i].Image.Height + 300),
                    new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                    GraphicsUnit.Pixel);
            }
        }
    }
}
