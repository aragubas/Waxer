using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Waxer.GameLogic;
using System;

namespace Waxer
{
    public struct MapProperties
    {
        public Vector2 TileSize;
    }

    public struct EnvironmentSettings
    {
        public float Gravity = 28f;
        public Vector2 WorldSize = Vector2.Zero;
    }

    public class Map
    {
        public MapProperties properties;
        public Dictionary<Point, MapTile> tiles = new();
        public PlayerEntity player;
        public Camera2D camera = new Camera2D();
        public List<MapEntity> Entities = new List<MapEntity>();
        public EnvironmentSettings MapEnvironment = new EnvironmentSettings();
        public SpriteFont DebugFont;


        public Map()
        {
            properties = new MapProperties();
            properties.TileSize = new Vector2(32, 32);
            
            // Fill with air tiles
            for(int x = 0; x < 32; x++)
            { 
                for(int y = 0; y < 32; y++)
                {
                    MapTile tile = new MapTile();
                    tile.TilePosition = new Vector2(x, y);
                    tile.ScreenPosition = new Vector2(x * properties.TileSize.X, y * properties.TileSize.Y);
                    tiles.Add(new Point(x, y), tile);
                }
            }
            // Set the camera limit to the map
            camera.ScreenLimit = new Vector2(32 * 32, 32 * 32);
            
            for (int x = 0; x < 32; x++)
            {
                for (int y = 20; y < 32; y++)
                {
                    tiles[new Point(x, y)].SetTileID(1);
                    tiles[new Point(x, y)].IsColideable = true;
                }
            }

            MapEnvironment.WorldSize = new Vector2(1024, 1024);
             
            // Just for testing colision          
            tiles[new Point(1, 19)].SetTileID(1);
            tiles[new Point(1, 19)].IsColideable = true;
            
            tiles[new Point(1, 18)].SetTileID(1);
            tiles[new Point(1, 18)].IsColideable = true;

            tiles[new Point(2, 19)].SetTileID(1);
            tiles[new Point(2, 19)].IsColideable = true;

            tiles[new Point(8, 19)].SetTileID(1);
            tiles[new Point(8, 19)].IsColideable = true;
            

            player = new PlayerEntity(new Vector2(5 * 32, 12 * 32), this);
            Entities.Add(new GameLogic.Towers.Test(new Vector2(32, 32), this));
            
        }
   
        // Draw the map on its batch
        void DrawMap(SpriteBatch spriteBatch)
        {
            if (DebugFont == null) 
            {
                DebugFont = Graphics.Fonts.GetSpriteFont(Graphics.Fonts.GetFontDescriptor("/PressStart2P", 8, spriteBatch.GraphicsDevice));
            }

            spriteBatch.Begin(transformMatrix: camera.GetMatrix());
            int i = 0;
 
            foreach(MapTile tile in tiles.Values)
            {
                if (camera.IsOnScreen(tile.GetScreenPosition()))
                {
                    i++;
                    tile.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            
            spriteBatch.Begin();
            
            spriteBatch.DrawString(DebugFont, $"Tiles on Screen: {i}", new Vector2(0, 25), Color.Red);

            spriteBatch.End();
        }

        public void DrawEntities(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.GetMatrix());

            for(int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Draw(spriteBatch);
            }
            
            player.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void UpdateEntities(GameTime gameTime)
        {
            for(int i = 0; i < Entities.Count; i++)
            {
                Entities[i].UpdateScreenPosition(camera);
                Entities[i].Update(gameTime);
            }
 
 
        }

        public Vector2 GetTilePosition(Vector2 pos)
        {
            int xPos = 0;
            int yPos = 0;

            xPos = (int)pos.X / (int)properties.TileSize.X;
            yPos = (int)pos.Y / (int)properties.TileSize.Y;
   
            return new Vector2(xPos, yPos);
        }

        public Vector2 GetTilePosition(Rectangle rect)
        {
            int xPos = 0;
            int yPos = 0;

            xPos = rect.X / (int)properties.TileSize.X;
            yPos = rect.Y / (int)properties.TileSize.Y;
   
            return new Vector2(xPos, yPos);
        }


        public MapTile GetTile(Vector2 pos)
        {
            try
            {
                return tiles[pos.ToPoint()];

            } catch (System.Collections.Generic.KeyNotFoundException)
            {
                return null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Center the camera to the center of the player
            camera.CenterTo(new Vector2(player.Position.X + player.Texture.Width / 2, player.Position.Y + player.Texture.Height / 2), spriteBatch.GraphicsDevice.Viewport);

            DrawMap(spriteBatch);
            DrawEntities(spriteBatch);
  
        } 

        public void Update(GameTime gameTime)
        {
            camera.Update();
            
            player.Update(gameTime);    
            
            UpdateEntities(gameTime);

        }

    }
}