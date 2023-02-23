using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace particulas2
{
    public partial class Form1 : Form
    {
        static List<Pelota> balls;
        static Bitmap bmp;
        static Graphics g;
        static Random rand = new Random();
        Image particleImage = Image.FromFile("C:\\Users\\Omar\\Documents\\Udlap\\Sexto semestre\\Graficación y  videojuegos\\assets\\exp.png");
        static float deltaTime;

        public Form1()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (PCT_CANVAS.Width == 0)
                return;

            balls = new List<Pelota>();
            bmp = new Bitmap(PCT_CANVAS.Width, PCT_CANVAS.Height);
            g = Graphics.FromImage(bmp);
            deltaTime = 1;
            PCT_CANVAS.Image = bmp;

            ballGen();
        }

        private void ballGen()
        {
            for (int b = 0; b < 300; b++)
                balls.Add(new Pelota(rand, PCT_CANVAS.Size, b));
        }
        private void Pelotas_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void Pelotas_SizeChanged(object sender, EventArgs e)
        {
            Init();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            g.Clear(Color.Black);

            Parallel.For(0, balls.Count, b =>//ACTUALIZAMOS EN PARALELO
            {
                Pelota P;
                balls[b].Update(deltaTime, balls);
                P = balls[b];
            });

            Pelota p;
            for (int b = 0; b < balls.Count; b++)//PINTAMOS EN SECUENCIA
            {
                p = balls[b];
                g.FillEllipse(new SolidBrush(p.c), p.x - p.radio, p.y - p.radio, p.radio * 2, p.radio * 2);
                //La imagen funciona pero se desborda la memoria, es la siguiente línea
                //g.DrawImage(particleImage, p.x - p.radio, p.y - p.radio, p.radio * 2, p.radio * 2);
            }

            for (int i = balls.Count - 1; i >= 0; i--)
            {
                Pelota particle = balls[i];
                if (particle.IsDead)
                {
                    balls.RemoveAt(i);
                }
            }

            if (balls.Count == 0)
                ballGen();

            PCT_CANVAS.Invalidate();
            deltaTime += .1f;
        }
    }
}
