using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    abstract class GameObject
    {
        public Vector Position;

        public double Radius = 0.5;

        public abstract void Update(int msElapsed);

    }
}
