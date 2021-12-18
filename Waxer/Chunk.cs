using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer
{
        public class Chunk
    {
        public Dictionary<Vector2, MapTile> tiles = new();
        public Vector2 ChunkPosition;
        public Rectangle Area;
        GameWorld ParentMap; 
        bool CheckChunks = false;
            

        public Chunk(Vector2 ChunkPosition, GameWorld ParentMap)
        {
            this.ChunkPosition = ChunkPosition;
            this.ParentMap = ParentMap;
            FillTiles();
            Area = new Rectangle((int)ChunkPosition.X * 1024, (int)ChunkPosition.Y * 1024, 1024, 1024);
        }

        public int Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            int i = 0;
            foreach(MapTile tile in tiles.Values)
            {
                if (camera.IsOnScreen(tile.GetArea()))
                {
                    i++;
                    tile.Draw(spriteBatch);
                }
            }

            if (!CheckChunks)
            {
                CheckChunks = true;

                // Left Chunk
                if (ChunkPosition.X - 1 >= 1)
                {
                    if (!ParentMap.chunks.ContainsKey(new Vector2(ChunkPosition.X - 1, ChunkPosition.Y)))
                    {                        
                        ParentMap.chunks.Add(new Vector2(ChunkPosition.X - 1, ChunkPosition.Y), new Chunk(new Vector2(ChunkPosition.X - 1, ChunkPosition.Y), ParentMap));
                    }
                }

                // Right Chunk
                if (ChunkPosition.X + 1 >= 1)
                {
                    if (!ParentMap.chunks.ContainsKey(new Vector2(ChunkPosition.X + 1, ChunkPosition.Y)))
                    {                        
                        ParentMap.chunks.Add(new Vector2(ChunkPosition.X + 1, ChunkPosition.Y), new Chunk(new Vector2(ChunkPosition.X + 1, ChunkPosition.Y), ParentMap));
                    }
                }

                // Top Chunk
                if (ChunkPosition.Y - 1 >= 1)
                {
                    if (!ParentMap.chunks.ContainsKey(new Vector2(ChunkPosition.X, ChunkPosition.Y - 1)))
                    {                        
                        ParentMap.chunks.Add(new Vector2(ChunkPosition.X, ChunkPosition.Y - 1), new Chunk(new Vector2(ChunkPosition.X, ChunkPosition.Y - 1), ParentMap));
                    }
                }
 
                // Bottom Chunk
                if (ChunkPosition.Y + 1 >= 1)
                {
                    if (!ParentMap.chunks.ContainsKey(new Vector2(ChunkPosition.X, ChunkPosition.Y + 1)))
                    {                        
                        ParentMap.chunks.Add(new Vector2(ChunkPosition.X, ChunkPosition.Y + 1), new Chunk(new Vector2(ChunkPosition.X, ChunkPosition.Y + 1), ParentMap));
                    }
                }
                 
            }
            
            return i;
        }

        public void FillTiles()
        {
            // Fill with dirt tiles
            for(int x = 0; x < 32; x++)
            { 
                for(int y = 0; y < 32; y++)
                {
                    MapTile tile = new MapTile(this, TilePosition: new Vector2((ChunkPosition.X * 32) + x, (ChunkPosition.Y * 32) + y),
                                                ScreenPosition: new Vector2((ChunkPosition.X * 1024) + (x * 32), (ChunkPosition.Y * 1024) + (y * 32)),
                                                TileID: 1);
                    
                    tiles.Add(tile.TilePosition, tile);
                }
            }
             
            // Just for testing colision          

            for (int x = 4; x < 10; x++)
            {
                for (int y = 10; y < 15; y++)
                {
                    tiles[new Vector2((ChunkPosition.X * 32) + x, (ChunkPosition.Y * 32) + y)].SetTileID(0);
                }
            }

        }

    }

}