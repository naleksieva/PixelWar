using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class Player
    {
        public PixelGame Game;
        public Animal Animal;
        public readonly int Number;
        public double MaxLife
        {
            get
            {
                return Animal.MaxLife;
            }
        }

        public double CurrentLife
        {
            get
            {
                return Animal.Life;
            }
        }

        public bool IsActive { get; private set; }

        public event Action<Animal> AnyProjectileShot
        {
            add
            {
                Animal.ProjectileShot += value;
            }
            remove
            {
                Animal.ProjectileShot -= value;
            }
        }


        public bool IsDead
        {
            get
            {
                return Animal.IsDead;
            }
        }
        public event Action<Player> OnDeath;

        public Player(PixelGame g, int i)
        {
            // TODO: Complete member initialization
            this.Game = g;
            this.Number = i;

            this.Animal = new Animal(Game);
            this.Animal.Position = new Vector(160 + i * 320, 50);
            this.Animal.OnDeath += Animal_OnDeath;

            //this.Animal = new Animal()
            //{
            //    Position = new Vector(160 + i * 320, 50),
            //};
        }

        private void Animal_OnDeath(Animal obj)
        {
            OnDeath?.Invoke(this);
        }

        public void Activate ()
        {
            IsActive = true;
            Animal.Activate();
        }

        public void Deactivate()
        {
            IsActive = false;
            Animal.Deactivate();
        }

        internal void Draw(SpriteBatch sb)
        {
            Animal.Draw(sb);
        }

        internal void Update(int msElapsed)
        {
            
            Animal.Update(msElapsed);

        }
    }
}
