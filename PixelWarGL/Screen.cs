using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PixelWarGL
{
    static class Screen
    {
        /// <summary>
        /// The default scaling of the screen. 
        /// </summary>
        public static Point DefaultSize { get; set; }

        /// <summary>
        /// The current size of the screen. 
        /// </summary>
        public static Point CurrentSize { get; set; }

        /// <summary>
        /// Gets the scaling amongst the X co-ordinate. 
        /// </summary>
        public static double ScalingX
        {
            get { return (double)CurrentSize.X / DefaultSize.X; }
        }

        /// <summary>
        /// Gets the scaling amongst the Y co-ordinate. 
        /// </summary>
        public static double ScalingY
        {
            get { return (double)CurrentSize.Y / DefaultSize.Y; }
        }


        static Screen()
        {
            DefaultSize = new Point(640, 480);
        }

        /// <summary>
        /// Updates the currently recorded size of the screen. 
        /// </summary>
        /// <param name="p"></param>
        public static void SetSize(Point p)
        {
            CurrentSize = p;
        }

        public static void DrawUi(this SpriteBatch sb, Texture2D tex, Rectangle bounds, Color c)
        {
            var newCoords = new Rectangle(
                (int)(bounds.X * ScalingX),
                (int)(bounds.Y * ScalingY),
                (int)(bounds.Width * ScalingX),
                (int)(bounds.Height * ScalingY)
                );

            sb.Draw(tex, newCoords, c);
        }

        public static void DrawUi(this SpriteBatch sb, Texture2D tex, Vector2 pos, Color c)
        {
            var newCoords = new Rectangle(
                (int)(pos.X * ScalingX),
                (int)(pos.Y * ScalingY),
                (int)(tex.Width * ScalingX),
                (int)(tex.Height * ScalingY));

            sb.Draw(tex, newCoords, c);
        }
    }
}
