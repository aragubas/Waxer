/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

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
        GameWorld gameMap;
        public static GameMain Reference;

		public GameMain()
		{
			graphics = new GraphicsDeviceManager(this);
 
            IsFixedTimeStep = false;
            //TargetElapsedTime = TimeSpan.FromMilliseconds(1000 / 144);
      
            // Make sure its running with VSync 
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.IsFullScreen = false; 
            graphics.ApplyChanges(); 

            Reference = this;
		}

        protected override void Initialize()
        {
			if (!Directory.Exists(Settings.ContentFolder)) { throw new DirectoryNotFoundException("Cannot find content folder."); }
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Loads all game sprite into ram
            Graphics.Sprites.LoadAllSprites(GraphicsDevice);
 
            // Creates a new cursor renderer
            cursorRender = new CursorRenderer();
            
            // Load tile information
            TilesInfo.LoadTileInfos();

            // Creates a new game map
            gameMap = new GameWorld();
		}

        protected override void Update(GameTime gameTime)
        {
            // Update mouse position
            MouseInput.Update();

            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
             
            // Limit the game to a stable "delta value"
            if (delta > Settings.StableDelta) 
            {
                delta = Settings.StableDelta; 
            }
 
            gameMap.Update(delta);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
 
            // Draw the game map and entities
            gameMap.Draw(spriteBatch);
 
            // Draw mouse cursor
			cursorRender.Draw(spriteBatch);
        }

    }

}
