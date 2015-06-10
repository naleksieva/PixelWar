using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    abstract class GameObject
    {
        public static readonly Vector G = new Vector(0, 200);

        public Vector Position;

        public Vector Velocity;

        public PixelGame Game;

        public bool IsMoving = false;

        public Texture2D Texture;

        public event Action OnCollision;
        public bool ShouldDestroy { get; protected set; }

        public Vector Size 
        { 
            get { return new Vector(Width, Height); } 
        }

        public Vector Center
        {
            get { return Position + Size / 2; }
            set 
            {
                Position = value - Size / 2;
            }
        }

        public double Diagonal
        {
            get { return Size.Length(); }
        }

        public int Width
        {
            get
            {
                return Texture.Width;
            }
        }
        public int Height
        {
            get
            {
                return Texture.Height;
            }
        }

        private Color[] texColors = new Color[0];

        public GameObject(PixelGame g)
        {
            this.Game = g;
        }

        public virtual void Update(int msElapsed)
        {
            // Update position, if moving
            if (IsMoving)
            {
                // update the color data for this gameobject
                getColorData();

                // calculate new position (no collision)
                Velocity += G * msElapsed / 1000;
                var dPos = Velocity * msElapsed / 1000;

                // get final new position, by attributing collision checks
                var finalPos = resolveCollision(dPos);

                Position += finalPos;

                // if there was collision, stop moving lol
                if (dPos != finalPos)
                {
                    Velocity = new Vector();
                    if(OnCollision != null)
                        OnCollision();


                    //if there is no land beneath our feet, start falling
                    if (landCheck(finalPos))
                    {
                        IsMoving = false;
                    }
                }
               
            }
        }

        public abstract void Draw(SpriteBatch sb);


        internal bool landCheck(Vector d)
        {
            d.Y += 1;
            return (isColliding(d));
        }

        /// <summary>
        /// Returns whether this game object would collide with the map if it was displaced. 
        /// 
        /// </summary>
        /// <param name="d">The displacement of the unit from his current position. </param>
        /// <returns></returns>
        private bool isColliding(Vector d)
        {
            //check collision w/ terrain
            var terrain = this.Game.Terrain;
            var pos = (this.Position + d).ToPoint();

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    if (pos.X + x >= terrain.GetLength(0) || pos.Y + y >= terrain.GetLength(1) || pos.Y + y < 0 || pos.X + x < 0)
                        continue;

                    var ourColor = texColors.Get(Texture.Width, x, y);
                    var haveColor = (ourColor.A > 0);

                    var mapTile = terrain[pos.X + x, pos.Y + y];
                    var haveTile = mapTile != TerrainType.Air;

                    if (haveColor && haveTile)
                        return true;
                }

            //check collision w/ other gameobjects

            var otherCollision = this.Game.Objects
                .Where(o => (o != this))
                .Any(o => this.IsCollidingWith(d, o));



            return otherCollision;
        }
        private bool IsCollidingWith(Vector d, GameObject o)
        {
            var aPos = (this.Position + d).ToPoint();
            var bPos = o.Position.ToPoint();

            var dx = Math.Abs(aPos.X - bPos.X);
            var dy = Math.Abs(aPos.Y - bPos.Y);

            if (dx > (this.Size.X + o.Size.X) / 2 || dy > (this.Size.Y + o.Size.Y) / 2)
                return false;

            for (int ax = 0; ax < Width; ax++)
                for (int ay = 0; ay < Height; ay++)
                {
                    var bx = ax + aPos.X - bPos.X;
                    var by = ay + aPos.Y - bPos.Y;

                    if (bx >= o.Width || by >= o.Height || bx < 0 || by < 0)
                        continue;

                    var aColor = this.texColors.Get(this.Texture.Width, ax, ay);
                    var bColor = o.texColors.Get(o.Texture.Width, bx, by);

                    var aHasColor = (aColor.A > 0);
                    var bHasColor = (bColor.A > 0);

                    //var mapTile = terrain[pos.X + x, pos.Y + y];
                    //var haveTile = mapTile != TerrainType.Air;

                    if (aHasColor && bHasColor)
                        return true;
                }

            return false;
        }

        /// <summary>
        /// Resolves potential collisions occurring in the moving of the gameobject using the given vector, by moving the gameobject back in the direction of its moving. 
        /// </summary>
        /// <param name="newPos"></param>
        /// <returns></returns>
        private Vector resolveCollision(Vector newPos)
        {
            //get the vector used to backtrack
            var bd = newPos / -10;

            //use the overload which takes a backtrack vector
            var finalPos = resolveCollision(newPos, bd, 10);

            if (finalPos.HasValue)
                return finalPos.Value;
            else
                return Vector.Zero;
        }

        /// <summary>
        /// Resolves potential collisions occuring from move in first direction, by backtracking in the second direction. 
        /// </summary>
        /// <param name="newPos"></param>
        /// <param name="backtrack"></param>
        /// <returns></returns>
        internal Vector? resolveCollision(Vector newPos, Vector backtrack, int nTries)
        {
            var backLength = backtrack.Length();
            while (isColliding(newPos))
            {
                newPos = newPos + backtrack;
                nTries--;
                if (nTries < 0)
                    return null;
            }
                

            return newPos;
        }

        internal Vector seekCollision(Vector newPos, Vector backtrack)
        {
            while (!isColliding(newPos))
                newPos = newPos + backtrack;

            newPos -= backtrack;
            return newPos;
        }


        private void getColorData()
        {
            var sz = Texture.Height * Texture.Width;
            if (texColors.Length != sz)
                texColors = new Color[sz];
            Texture.GetData<Color>(texColors);
        }

    }
}
