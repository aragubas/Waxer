using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace Waxer
{
	public class GameMain : Game
	{
		public GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
        public CursorRenderer cursorRender;
        Map gameMap;
        public static GameMain Reference;

		public GameMain()
		{
			graphics = new GraphicsDeviceManager(this);

            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 60);

            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.ApplyChanges();

            Reference = this;
		}

        protected override void Initialize()
        {
			if (!Directory.Exists(Settings.ContentFolder)) { throw new DirectoryNotFoundException("Cannot find data folder."); }
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads all game sprite into ram
            Graphics.Sprites.LoadAllSprites(GraphicsDevice);

            // Creates a new cursor renderer
            cursorRender = new CursorRenderer();
            
            // Creates a new game map
            gameMap = new Map();
		}

        protected override void Update(GameTime gameTime)
        {
            // Update mouse position
            MouseInput.Update();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            gameMap.Update(delta);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Draw the game map
            gameMap.Draw(spriteBatch);

			cursorRender.Draw(spriteBatch);
        }

    }

}
