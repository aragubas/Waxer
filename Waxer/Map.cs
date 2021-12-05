using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Waxer.GameLogic;

namespace Waxer
{
    public struct MapProperties
    {
        public Vector2 TileSize;
    }


    public class MapTile
    {
        public Vector2 ScreenPosition;
        public Vector2 TilePosition;
        public Texture2D TileTexture;
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TileTexture, ScreenPosition, Color.White);
        }

        public Rectangle GetScreenPosition(Camera2D camera)
        {
            return new Rectangle((int)(ScreenPosition.X), (int)(ScreenPosition.Y), 32, 32);
        }
    }


    public class Map
    {
        MapProperties properties;
        List<MapTile> tiles = new List<MapTile>();
        PlayerEntity player;
        Camera2D camera = new Camera2D();
        SpriteFont debugFont;

        public Map()
        {
            properties = new MapProperties();
            properties.TileSize = new Vector2(32, 32);
            
            Texture2D grassTile = Graphics.Sprites.GetSprite("/tiles/1.png");

            for(int x = 0; x < 32; x++)
            { 
                for(int y = 0; y < 32; y++)
                {
                    MapTile tile = new MapTile();
                    tile.TileTexture = grassTile;
                    tile.TilePosition = new Vector2(x, y);
                    tile.ScreenPosition = new Vector2(x * properties.TileSize.X, y * properties.TileSize.Y);
                    tiles.Add(tile);
                }
            }
            camera.ScreenLimit = new Vector2(32 * 32, 32 * 32);
             
            player = new PlayerEntity();
        }
   
        // Draw the map on its batch
        void DrawMap(SpriteBatch spriteBatch)
        {
            if (debugFont == null) 
            {
                debugFont = Graphics.Fonts.GetSpriteFont(Graphics.Fonts.GetFontDescriptor("/PressStart2P", 8, spriteBatch.GraphicsDevice));
            }

            spriteBatch.Begin(transformMatrix: camera.GetMatrix());
            int IterationCount = 0;

            foreach(MapTile tile in tiles)
            {
                if (camera.IsOnScreen(tile.GetScreenPosition(camera)))
                {
                    IterationCount++;
                    tile.Draw(spriteBatch);
                }
            }

            spriteBatch.End();
            
            spriteBatch.Begin();
            
            spriteBatch.DrawString(debugFont, $"IterationCount {IterationCount}", new Vector2(0, 50), Color.Red);

            spriteBatch.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawMap(spriteBatch);
 
            spriteBatch.Begin(transformMatrix: camera.GetMatrix());

            player.Draw(spriteBatch);

            spriteBatch.End();
  
            camera.CenterTo(new Vector2(player.Position.X + player.Texture.Width / 2, player.Position.Y + player.Texture.Height / 2), spriteBatch.GraphicsDevice.Viewport);
            
            spriteBatch.Begin();
            
            spriteBatch.DrawString(debugFont, $"x: {camera.CameraPosition.X}\ny: {camera.CameraPosition.Y}", Vector2.Zero, Color.Red);
            
            spriteBatch.End();

        } 
  
        public void Update()
        {
            camera.Update();
              
            player.Update();

        }

    }
}