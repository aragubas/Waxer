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

		public GameMain()
		{
			graphics = new GraphicsDeviceManager(this);
		}

        protected override void Initialize()
        {
			if (!Directory.Exists(Settings.ContentFolder)) { throw new DirectoryNotFoundException("Cannot find data folder."); }
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
 
            Graphics.Sprites.LoadAllSprites(GraphicsDevice);


            cursorRender = new CursorRenderer();
            gameMap = new Map();
		}

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MouseInput.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            gameMap.Draw(spriteBatch);

			cursorRender.Draw(spriteBatch);
        }

    }

}
