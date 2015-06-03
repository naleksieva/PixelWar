using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class Animal: GameObject 
    {
        const int DefaultRadius = 3;
        const double MaxPower = 5;
        const double PowerDelta = 3;
        const double AimDelta = 2;
        const double MaxLife = 100;


        public double Aim;
        public double CurrentPower;
        public double Life = MaxLife;
        private double PowerDirection = 1;
        private double AimDirection = 1;

        private bool lastFireKey;

        public override void Update(int msElapsed)
        {
            var nowFireKey = KeyboardState.FireKey;

            if (nowFireKey)
            {
                CurrentPower += PowerDelta * PowerDirection * msElapsed / 1000;
                if (CurrentPower < 1 || CurrentPower > MaxPower)
                {
                    CurrentPower = Math.Min(MaxPower, Math.Max(1, CurrentPower));
                    PowerDirection *= 1;
                }
            }
            else if (lastFireKey)
                fireMissile();


            if (KeyboardState .YDirection!=0)
            {
                CurrentPower += AimDelta * AimDirection * msElapsed / 1000;
                if (Aim < 0 || Aim > Math.PI)
                {
                    Aim = Math.Min(Math.PI, Math.Max(0, Aim));
                    AimDirection *= 1;
                }
            }

            lastFireKey = nowFireKey;
        }

        private void fireMissile()
        {
            throw new NotImplementedException();
        }


    }
}
