#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace PixelWarGL
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        static PixelGameGl game;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new PixelGameGl();

            game.Run();
        }
    }
}
