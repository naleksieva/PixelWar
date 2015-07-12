using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelWarGL
{
    class PixelGame
    {


        static readonly Point DefaultSize = new Point(640, 480);
        static readonly Color BarBackground = new Color(0, 0, 0, 50);

        const int NPlayers = 2;
        const int PostTurnDuration = 3000;
        const int MaxTurnDuration = 30000;
        const int Anchor = 50;
        const int BarHeight = 30;
        const int BarSpace = 150;

        public TerrainType[,] Terrain;

        Player[] Players = new Player[NPlayers];

        public List<Projectile> Projectiles = new List<Projectile>();

        int currentPlayerId = 0;

        int stateElapsed = 0;

        //public GameState State { get; private set; }
        public Player Winner { get; private set; }

        private GameState _state;

        public GameState State
        {
            get { return _state; }
            set
            {
                if(value != _state)
                {
                    _state = value;
                    stateElapsed = 0;
                }
            }
        }

        IEnumerable<Animal> Animals
        {
            get { return Players.Select(p => p.Animal); }
        }

        public IEnumerable<GameObject> Objects
        {
            get
            {
                foreach (var a in Animals)
                    yield return a;

                foreach (var p in Projectiles)
                    yield return p;

                //another way to do the same:
                //return Animals.Cast<GameObject>().Concat(Projectiles);
            }
        }

        public int TerrainWidth
        {
            get { return Terrain.GetLength(0); }
        }

        public int TerrainHeight
        {
            get { return Terrain.GetLength(1); }
        }


        //bool GameRunning;
        Texture2D CurrentMap;

        public PixelGame()
        {
            //var rnd = new Random();
            //currentPlayerId = rnd.Next(0, NPlayers);

            for (int i = 0; i < NPlayers; i++)
            {
                Players[i] = new Player(this, i);
                Players[i].AnyProjectileShot += PixelGame_AnyProjectileShot;
                Players[i].OnDeath += PixelGame_OnDeath;
            }
            State = GameState.Turn;
            Players[currentPlayerId].Activate();
        }

        private void PixelGame_OnDeath(Player obj)
        {
            //unused
        }

        private void PixelGame_AnyProjectileShot(Animal obj)
        {
            State = GameState.PostTurn;
        }

        private void NextPlayer()
        {
            Players[currentPlayerId].Deactivate();

            var nextPlayerId = (currentPlayerId + 1) % NPlayers;
            currentPlayerId = nextPlayerId;

            Players[currentPlayerId].Activate();
        }

        public void Update(int msElapsed)
        {
            UpdateState(msElapsed);

            // update players 
            foreach (Player pl in Players)
            {
                pl.Update(msElapsed);
            }
            // update other objects
            foreach (var pr in Projectiles)
            {
                pr.Update(msElapsed);
            }

            Projectiles.RemoveAll(p => p.IsBellowMap);
            //cleanup stale objects
           
            Projectiles.RemoveAll(p => p.ShouldDestroy);
        }

        private void UpdateState(int msElapsed)
        {
            stateElapsed = stateElapsed + msElapsed;
            switch (State)
            {
                case GameState.Turn:
                    if (stateElapsed > MaxTurnDuration)
                    {
                        State = GameState.Waiting;
                    }
                            
                    break;

                case GameState.PostTurn:
                    if (stateElapsed > PostTurnDuration)
                    {
                        State = GameState.Waiting;
                    }

                    break;
                case GameState.Waiting:
                    if (!Projectiles.Any())
                    {
                        NextPlayer();
                        State = GameState.Turn;
                    }


                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            //draw the map
            if (CurrentMap != null)
                sb.DrawUi(CurrentMap, Vector2.Zero, Color.White);

            //draw the players (their tanks)
            foreach (var pl in Players)
                pl.Draw(sb);

            //draw the flying projectiles
            foreach (var pr in Projectiles)
                pr.Draw(sb);

            //draw UI components
            drawUi(sb);
        }


        private void drawUi(SpriteBatch sb)
        {
            var screenWidth = Screen.CurrentSize.X;
            var barWidth = (screenWidth - 2 * Anchor - BarSpace) / 2;
            int LifeBar1 = (int)(barWidth * Players[0].CurrentLife / Players[0].MaxLife);
            int LifeBar2 = (int)(barWidth * Players[1].CurrentLife / Players[1].MaxLife);

            sb.Draw(PixelGameGl.TexOne, new Rectangle(Anchor, Anchor / 2, barWidth, BarHeight), BarBackground);
            sb.Draw(PixelGameGl.TexOne, new Rectangle(Anchor, Anchor / 2, LifeBar1, BarHeight), Color.Red);

            sb.Draw(PixelGameGl.TexOne, new Rectangle(Anchor + barWidth + BarSpace, Anchor / 2, barWidth, BarHeight), BarBackground);
            sb.Draw(PixelGameGl.TexOne, new Rectangle(Anchor + barWidth + BarSpace, Anchor / 2, LifeBar2, BarHeight), Color.Red);


        }

        internal void CreateExplosion(Vector center, double radius)
        {
            var startX = Math.Max(0, (int)(center.X - radius));
            var startY = Math.Max(0, (int)(center.Y - radius));
            var endX = Math.Min(TerrainWidth - 1, center.X + radius);
            var endY = Math.Min(TerrainHeight - 1, center.Y + radius);

            for (int ix = startX; ix <= endX; ix++)
                for (int iy = startY; iy <= endY; iy++)
                {

                    if (center.DistanceTo(new Vector(ix, iy)) <= radius)
                        Terrain[ix, iy] = TerrainType.Air;

                }


            foreach (var a in Animals)
            {
                var dist = a.Center.DistanceTo(center) - a.Diagonal / 2;
                var angle = center.AngleTo(a.Center);


                dist = Math.Max(0, dist);

                if (dist > radius)
                    continue;

                var damage = Math.Pow(radius - dist, 2) / radius;
                var moveVector = Vector.Zero.PolarProjection(angle, damage * 2);

                a.Velocity = moveVector;
                a.IsMoving = true;


                //a.Life -= damage;
                a.DealDamage(damage);

            }

            generateTerrain();

            checkPlayerDeath();

        }

        private void checkPlayerDeath()
        {
            var playersAlive = Players.Count(p => !p.IsDead);

            if (playersAlive < 2)
            {
                State = GameState.GameOver;

                Winner = Players.FirstOrDefault(p => !p.IsDead);

                Players[currentPlayerId].Deactivate();
            }
        }

        internal void LoadMap(Texture2D TexMap)
        {
            CurrentMap = TexMap;
            loadTerrain(TexMap);
            generateTerrain();
        }

        private void loadTerrain(Texture2D tex)
        {
            Color[] colorData = new Color[tex.Width * tex.Height];
            tex.GetData<Color>(colorData);

            Terrain = new TerrainType[tex.Width, tex.Height];

            for (int ix = 0; ix < tex.Width; ix++)
                for (int iy = 0; iy < tex.Height; iy++)
                {
                    var i = iy * tex.Width + ix;
                    var c = colorData[i];

                    if (c != new Color(255, 255, 255))
                        Terrain[ix, iy] = TerrainType.Dirt;
                    else
                        Terrain[ix, iy] = TerrainType.Air;

                }

        }

        private void generateTerrain()
        {
            Color[] colorData = new Color[Terrain.Length];

            var w = Terrain.GetLength(0);
            var h = Terrain.GetLength(1);
            for (int ix = 0; ix < w; ix++)
                for (int iy = 0; iy < h; iy++)
                {
                    var i = iy * w + ix;
                    switch (Terrain[ix, iy])
                    {
                        case TerrainType.Dirt:
                            colorData[i] = Color.Green;
                            break;
                        case TerrainType.Air:
                            colorData[i] = Color.Transparent;
                            break;
                    }
                }
            CurrentMap.SetData<Color>(colorData);

        }
    }
}
