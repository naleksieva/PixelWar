using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class Projectile : GameObject 
    {
        const double gravitationForce = 1;
        public double Radius = 0.5;
        public Vector Velocity;

        public Projectile (double aim, double power)
        {
            Velocity = new Vector(Math.Cos(aim), Math.Sin(aim)) * power;
        }


        public override void Update(int msElapsed)
        {

            Position += Velocity * msElapsed / 1000;
            Velocity.Y -= msElapsed * gravitationForce / 1000;
            
        }
    }
}
