using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class Animal: GameObject 
    {
        static readonly double MaxWalkSlope = Math.Tan(Math.PI * 70 / 180);

        const int DefaultRadius = 3;

        const int MaxFireTime = 3000;
        const double MinPower = 50;
        const double MaxPower = 500;

        const double AimDelta = 2;

        public const double MaxLife = 100;

        const double WalkSpeed = 50;
        const double CrosshairLength = 70;


        KeyboardState keyInfo;

        private int Direction = 1;
        private int fireTimer;

        bool wasFireKeyDown = false;
        private bool lastFireKey;


        /// <summary>
        /// The event raised whenever this animal shoots a projectile. 
        /// </summary>
        public event Action<Animal> ProjectileShot;
        public event Action<Animal> OnDeath;

        public double Life { get; private set; }
        public bool IsDead { get; private set; }
        public double Aim { get; private set; }
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets whether the animal is yet to fire this round.
        /// </summary>
        public bool CanShoot { get; private set; }



        public Animal(PixelGame g)
            : base(g)
        {
            Life = MaxLife;
            IsMoving = true;
            Texture = PixelGameGl.TexTank;
        }

        public void Activate()
        {
            IsActive = true;
            CanShoot = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public override void Update(int msElapsed)
        {
            if (IsActive)
            {
                // update the keyboard state
                keyInfo = Keyboard.GetState();


                if (!IsDead && !IsMoving)
                {
                    handleMovement(msElapsed);

                    handleAim(msElapsed);

                    handleFire(msElapsed);
                }
            }

            base.Update(msElapsed);

        }

        private void handleAim(int msElapsed)
        {

            if (keyInfo.IsKeyDown(Keys.Down))
            {
                var dAngle = AimDelta * msElapsed / 1000;
                Aim = Math.Min(Math.PI / 2, Aim + dAngle);
            }
            if (keyInfo.IsKeyDown(Keys.Up))
            {
                var dAngle = AimDelta * msElapsed / 1000;
                Aim = Math.Max(-Math.PI / 2, Aim - dAngle);
            }
        }

        private void handleFire(int msElapsed)
        {
            //continue only if we can shoot
            if (!CanShoot)
                return;
            
            //check whether space is down
            var isFireKeyDown = keyInfo.IsKeyDown(Keys.Space);

            //update the fire timer
            if (wasFireKeyDown && isFireKeyDown)
                fireTimer += msElapsed;


            //check if its state changed since the last Update call
            if (isFireKeyDown != wasFireKeyDown)
            {
                //if so, see whether we *just* pressed or released the key
                if (isFireKeyDown)
                {
                    // just pressed
                    fireTimer = 0;

                }
                    else
                {
                    // just released
                   
                    var angle = (this.Direction == 1) ? (this.Aim) : (Math.PI  - this.Aim);
                    //fireTimer = 1000;

                    fireTimer = Math.Min(fireTimer, MaxFireTime);
                    var powerCoefficient = (double)fireTimer / MaxFireTime;
                    var actualPower = powerCoefficient * (MaxPower - MinPower) + MinPower;

                    Console.WriteLine("Timer: {0},  Power Ratio {1}, Actual Power {2}", fireTimer, powerCoefficient, actualPower);
                    var p = new Projectile(this, angle, actualPower);
                    this.Game.Projectiles.Add(p);

                    // fire the ProjectileShot event
                    if (ProjectileShot != null)
                    ProjectileShot.Invoke(this);

                    // prevent shooting again
                    this.CanShoot = false;
                }

            }
            //finally update the current state
            wasFireKeyDown = isFireKeyDown;
        }
       

        private void handleMovement(int msElapsed)
        {
            var isLeftPressed = keyInfo.IsKeyDown(Keys.Left);
            if (isLeftPressed)
            {
                tryMove(Vector.Left, msElapsed);
                Direction = -1;
            }

            var isRightPressed = keyInfo.IsKeyDown(Keys.Right);
            if (isRightPressed)
            {
                tryMove(Vector.Right, msElapsed);
                Direction = 1;

            }

            if (keyInfo.IsKeyDown(Keys.LeftControl))
            {
                IsMoving = true;
                Velocity = new Vector(20 * Direction, -150);
            }
        }


        private void tryMove (Vector direction, int msElapsed)
        {
            var dist = direction * WalkSpeed * msElapsed / 1000;
            var stepUp = Vector.Down * msElapsed / 1000;
            var stepDown = Vector.Up * msElapsed / 1000;
            var maxStepY = dist.Length() * MaxWalkSlope;
            var maxNSteps = (int)(maxStepY / stepUp.Length());
            //see if there's a collision
            var newD = resolveCollision(dist, stepUp, maxNSteps);

            if (!newD.HasValue)
                return;

            if (newD != dist)
            {
                
                //if so, try to climb it
                if (Math.Abs(newD.Value.Y / newD.Value.X) < MaxWalkSlope)
                    Position += newD.Value;

                return;
            }
            
            //see if there's ground under our feet
            var fallD = seekCollision(dist, stepDown);
            if (fallD != dist)
            {
                //if no ground, see if we walk
                if (Math.Abs(fallD.Y / fallD.X) < MaxWalkSlope)
                    Position += fallD;
                else //or fall
                {
                    IsMoving = true;
                    Velocity = direction * WalkSpeed;
                }
                return;
            }

            //there's ground, and no collision
            Position += dist;
        }

        public void DealDamage(double damage)
        {
            Life = Life - damage;
            if (Life <= 0)
            {
                IsDead = true;
                Life = 0;
                Texture = PixelGameGl.TexDeadTank;

                if (OnDeath != null)
                OnDeath.Invoke(this);
                
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            const int barHeight = 10;

            // draw the hp bar
            if (!IsDead)
            {
                var barPos = Position - new Vector(0, barHeight);
                var filledWidth = Width * Life / MaxLife;

                sb.DrawUi(PixelGameGl.TexOne, new Rectangle((int)barPos.X, (int)barPos.Y, (int)Width, (int)barHeight), new Color(0, 0, 0, 50));
                sb.DrawUi(PixelGameGl.TexOne, new Rectangle((int)barPos.X, (int)barPos.Y, (int)filledWidth, (int)barHeight), Color.DarkRed);
            }

            //draw the tank
            sb.DrawUi(Texture, Position.ToVector2(), Color.White);


            if (IsActive)
            {
                var crossD = Vector.Zero.PolarProjection(Aim, CrosshairLength);
                crossD.X *= Direction;

                sb.DrawUi(PixelGameGl.TexTarget, (Position + crossD).ToVector2(), Color.White);

                BazookaTimer.DrawCircles(sb, (int) Center.X, (int) Center.Y);

            }
        }
    }
}
