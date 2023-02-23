using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace particulas2
{
    public class Pelota
    {
        int index;
        Size space;
        public Color c;
        // Variables de posición
        public float x;
        public float y;
        //static Random rand = new Random();

        // Variables de velocidad
        private float vx;
        private float vy;
        const float gravity = 0.05f;

        // Variable de radio
        public float radio;

        //Variables de edad
        private int age;
        public bool IsDead ;

        // Constructor
        public Pelota(Random rand, Size size, int index)
        {
            this.radio =1;
            this.x = rand.Next(280, 320);
            this.y = rand.Next(150, 200);
            c = Color.FromArgb(255,231, 105, 38);
            // Velocidades iniciales
            this.vx = rand.Next(-2, 3);
            this.vy = rand.Next(-2, 3);

            //edades
            age = 200;
            this.IsDead = false;

            this.index = index;
            space = size;
        }

        // Método para actualizar la posición de la pelota en función de su velocidad
        public void Update(float deltaTime, List<Pelota> balls)
        {
            for (int b = index + 1; b < balls.Count; b++)
            {
                Collision(balls[b]);
            }

            if ((x - radio) <= 0 || (x + radio) >= space.Width)
            {
                if (x - radio <= 0)
                    x = radio + 3;
                else
                    x = space.Width - radio - 3;

                vx *= -.005f;
                vy *= .075f;
            }

            if ((y - radio) <= 0 || (y + radio) >= space.Height)
            {
                if (y - radio <= 0)
                    y = radio + 3;
                else
                    y = space.Height - radio - 3;

                vx = 0;
                vy = 0;
            }


            age -= 1;
            if (age <= 0)
            {
                IsDead = true;
            }

            vy += gravity;


            int alpha = (int)Math.Min(255, 255 * this.age / 300);
            if (alpha <= 0) alpha = 0;
            this.c = Color.FromArgb(alpha, c.R, c.G, c.B);

            this.x += this.vx * 2;
            this.y += this.vy * 2;

        }



        // Método para manejar colisiones entre pelotas
        public void Collision(Pelota otraPelota)
        {
            float distancia = (float)Math.Sqrt(Math.Pow((otraPelota.x - this.x), 2) + Math.Pow((otraPelota.y - this.y), 2));

            if (distancia < (this.radio + otraPelota.radio))//ESTO SIGNIFICA COLISIÓN...
            {
                // Calculamos las velocidades finales de cada pelota en función de su masa y velocidad inicial
                float masaTotal = this.radio + otraPelota.radio;
                float masaRelativa = this.radio / masaTotal;

                float v1fx = this.vx - masaRelativa * (this.vx - otraPelota.vx) / 100;
                float v1fy = this.vy - masaRelativa * (this.vy - otraPelota.vy) / 100;

                float v2fx = otraPelota.vx - masaRelativa * (otraPelota.vx - this.vx) / 100;
                float v2fy = otraPelota.vy - masaRelativa * (otraPelota.vy - this.vy) / 100;

                // Actualizamos las velocidades de las pelotas
                this.vx = v1fx;     // -----AQUI CAMBIAMOS EL ANGULO---------
                this.vy = v1fy;     // -----AQUI CAMBIAMOS EL ANGULO--------------

                otraPelota.vx = v2fx;//-----AQUI CAMBIAMOS EL ANGULO----------------------
                otraPelota.vy = v2fy;//-----AQUI CAMBIAMOS EL ANGULO----------------------

                // Movemos las pelotas para evitar que se superpongan
                float distanciaOverlap = (this.radio + otraPelota.radio) - distancia;
                float dx = (this.x - otraPelota.x) / distancia;
                float dy = (this.y - otraPelota.y) / distancia;

                this.x += dx * distanciaOverlap / 2f;
                this.y += dy * distanciaOverlap / 2f;

                otraPelota.x -= dx * distanciaOverlap / 2f;
                otraPelota.y -= dy * distanciaOverlap / 2f;
            }
        }

    }
}
