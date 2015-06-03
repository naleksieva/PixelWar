using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PixelWarGL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                KeyboardState.YDirection += 1;
            if (e.KeyCode == Keys.Down)
                KeyboardState.YDirection -= 1;

            if (e.KeyCode == Keys.Space)
                KeyboardState.FireKey = true;
            Console.WriteLine("dasdasd");
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
                KeyboardState.YDirection -= 1;
            if (e.KeyCode == Keys.Down)
                KeyboardState.YDirection += 1;

            if (e.KeyCode == Keys.Space)
                KeyboardState.FireKey = false;
        }
    }   

    public static class KeyboardState
    {
        public static int XDirection;
        public static int YDirection;
        public static bool FireKey;

    }

}
