using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PixelWarGL
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class PixelGameGl : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //That's how you declare a texture
        public static Texture2D TexBazooka, TexTank, TexMissile, TexTarget;
        public static Texture2D TexOne;
        public static Texture2D TexDeadTank;
        public static Texture2D TexCircle;


        public Texture2D TexMap;

        public static SpriteFont MainFont;


        PixelGame TheGame;

        public PixelGameGl()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            this.IsMouseVisible = true;
            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        private void Window_ClientSizeChanged(object sender, System.EventArgs e)
        {

            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;

            graphics.ApplyChanges();
            Screen.SetSize(new Point(Window.ClientBounds.Width, Window.ClientBounds.Height));
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //that's how you load a texture. 
            TexBazooka = Content.Load<Texture2D>("bazooka");
            TexTank = Content.Load<Texture2D>("tank");
            TexOne = Content.Load<Texture2D>("1");
            TexCircle = Content.Load<Texture2D>("circle");
            TexMissile = Content.Load<Texture2D>("missile");
            TexTarget = Content.Load<Texture2D>("target");
            TexDeadTank = Content.Load<Texture2D>("dead_tank");

            MainFont = Content.Load<SpriteFont>("MainFont");

            TexMap = Content.Load<Texture2D>("map");

            TheGame = new PixelGame();
            TheGame.LoadMap(TexMap);

            ////grab the color data from the texture
            //Color[] colz = new Color[TexBazooka.Width * TexBazooka.Height];
            //TexBazooka.GetData<Color>(colz);

            ////make the first 5 lines red
            //for (int i = 0; i < TexBazooka.Width * 5; i++)
            //    colz[i] = Color.Red;

            ////and update the texture with the modified color data
            //TexBazooka.SetData<Color>(colz);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var msElapsed = (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                       
            var kbd = Keyboard.GetState();

            if (kbd.IsKeyDown(Keys.RightAlt) && kbd.IsKeyDown(Keys.Enter))
            {
                graphics.PreferredBackBufferWidth = TheGame.TerrainWidth;
                graphics.PreferredBackBufferHeight = TheGame.TerrainHeight;
                graphics.ToggleFullScreen();
                graphics.ApplyChanges();
            }

            if(kbd.IsKeyDown(Keys.F2) && TheGame.State == GameState.GameOver)
            {
                TheGame = new PixelGame();
                TheGame.LoadMap(TexMap);
            }

            // TODO: Add your update logic here
            TheGame.Update(msElapsed);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            //start drawing
            spriteBatch.Begin();


            TheGame.Draw(spriteBatch);

            if (TheGame.State == GameState.GameOver)
            {
                var screenRect = new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height);
                spriteBatch.Draw(TexOne, screenRect, new Color(0, 0, 0, 50));


                var msgText = "ai gg";
                var msgSize = MainFont.MeasureString(msgText);
                var msgPosition = new Vector2(screenRect.Width / 2, screenRect.Height / 2) - msgSize / 2;
                spriteBatch.DrawString(MainFont, msgText, msgPosition, Color.White);


            }

            //spriteBatch.DrawString(MainFont, "lapai pishki", new Vector2(100, 100), Color.DeepPink);

            //draw the bazooka texture at 
            //var pos = new Vector2(15, 15);
            //spriteBatch.Draw(TexBazooka, pos, Color.White);

            //finalise drawing
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
