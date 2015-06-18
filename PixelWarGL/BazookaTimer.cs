using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    public static class BazookaTimer
    {
        const int t_period = 2000;

        const int min_circles = 1;
        const int max_circles = 10;

        const int sq_start_size = 10;
        const int sq_incr_size = 2;
        const int circle_offset = 6;

        

        //public static void CircleCount(int totalMsElapsed)
        //{
        //    totalMsElapsed %= (t_period * 2);
        //    var t_actual = Math.Abs(totalMsElapsed - t_period);

        //    n_circles = (int)Math.Round((double)(max_circles - min_circles) * t_actual / t_period + min_circles);

        //}



        public static void DrawCircles(SpriteBatch sb, int x, int y, double angle, double ratio)
        {
            int n_circles = (int)(ratio * (max_circles - min_circles) + min_circles);
            var pts = new Point[n_circles];

            
            pts[0] = new Point(x, y);
            for (int i = 1; i < n_circles; i++)
            {
                pts[i] = new Point(x + i * circle_offset, y)
                    .Rotate(pts[0], angle);
            }

            for (int i = n_circles - 1; i >= 0; i--)
            {
                var p = pts[i];
                var sz = sq_start_size + i * sq_incr_size;
                var c = Color.Lerp(Color.Yellow, Color.Brown, (float)i / max_circles);
                sb.DrawUi(PixelGameGl.TexCircle, new Rectangle(p.X - sz / 2, p.Y - sz / 2, sz, sz), c);
            }
        }



    }
}
