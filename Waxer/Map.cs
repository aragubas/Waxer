using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Waxer
{
    public struct MapProperties
    {
        public Vector2 TileSize;
    }


    public class MapTile
    {
        public Vector2 MapPosition;
        public Texture2D TileTexture;
        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TileTexture, MapPosition, Color.White);
        }

    }


    public class Map
    {
        MapProperties properties;
        List<MapTile> tiles = new List<MapTile>();

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
                    tile.MapPosition = new Vector2(x * properties.TileSize.X, y * properties.TileSize.Y);
                    tiles.Add(tile);
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            foreach(MapTile tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
  
            spriteBatch.End();
        }

    }
}