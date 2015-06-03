using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelWarGL
{
    static class Ext
    {
        public static Color Get(this Color[] c, int w, int x, int y)
        {
            var i = y * w + x;
            return c[i];

        }

        public static void Set(this Color[] c, int w, int x, int y, Color val)
        {
            var i = y * w + x;
            c[i] = val;
        }

        public static Vector2 ToVector2(this Vector v)
        {
            return new Vector2((float)v.X, (float)v.Y);
        }

    }


}
