using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class Game
    {
        static readonly Point DefaultSize = new Point(640, 480);
        
        
        public TerrainType[,] Terrain = new TerrainType[DefaultSize.X, DefaultSize.Y];



        private void generateTerrain()
        {
            throw new NotImplementedException();
        }



    }
}
