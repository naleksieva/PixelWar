using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class Projectile : GameObject 
    {
        public double Radius = 0.5;


        public Projectile (Animal a, double aim, double power)
            : base(a.Game)
        {
            Texture = PixelGameGl.TexMissile;
            IsMoving = true;
            Velocity = new Vector(Math.Cos(aim), Math.Sin(aim)) * power;
            var distanceFromAnimal = (Diagonal + a.Diagonal) / 2;
            var displacementFromAnimal = Vector.Zero.PolarProjection(aim, distanceFromAnimal);
            Center = a.Center + displacementFromAnimal;

            this.OnCollision += Projectile_OnCollision;
        }

        private void Projectile_OnCollision()
        {
            Game.CreateExplosion(Center, 50);
            ShouldDestroy = true;
        }

        public override void Update(int msElapsed)
        {
            base.Update(msElapsed);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.DrawUi(Texture, Position.ToVector2(), Color.White); 
        }
    }
}
