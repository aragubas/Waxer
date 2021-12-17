using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Waxer.GameLogic;
using System;
using MonoGame.Extended;

namespace Waxer
{
    public class Chunk
    {
        public Dictionary<Vector2, MapTile> tiles = new();
        public Vector2 ChunkPosition;
        public Rectangle Area;
        Map ParentMap; 
        bool CheckChunks = false;
            

        public Chunk(Vector2 ChunkPosition, Map ParentMap)
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
                    MapTile tile = new MapTile();
                    tile.TilePosition = new Vector2((ChunkPosition.X * 32) + x, (ChunkPosition.Y * 32) + y);
                    tile.ScreenPosition = new Vector2((ChunkPosition.X * 1024) + (x * 32), (ChunkPosition.Y * 1024) + (y * 32)); 
                    tile.SetTileID(1);
                    tile.IsColideable = true;
                    tiles.Add(tile.TilePosition, tile);
                }
            }
             
            // Just for testing colision          

            for (int x = 4; x < 10; x++)
            {
                for (int y = 10; y < 15; y++)
                {
                    tiles[new Vector2((ChunkPosition.X * 32) + x, (ChunkPosition.Y * 32) + y)].SetTileID(0);
                    tiles[new Vector2((ChunkPosition.X * 32) + x, (ChunkPosition.Y * 32) + y)].IsColideable = false;
                }
            }

        }

    }

    public struct MapProperties
    {
        public Vector2 TileSize;
    }

    public struct EnvironmentSettings
    {
        public float Gravity = 28f;
    }

    public class Map
    {
        public MapProperties properties;
        public PlayerEntity player;
        public Camera2D camera = new Camera2D();
        public List<MapEntity> Entities = new List<MapEntity>();
        public EnvironmentSettings MapEnvironment = new EnvironmentSettings();
        public SpriteFont DebugFont;
        public Dictionary<Vector2, Chunk> chunks = new();

        public Map()
        {
            properties = new MapProperties();
            properties.TileSize = new Vector2(32, 32);
            
            Chunk chunk = new Chunk(new Vector2(0, 0), this);
            chunks.Add(new Vector2(0, 0), chunk);
            
            Chunk chunk2 = new Chunk(new Vector2(1, 0), this);
            chunks.Add(new Vector2(1, 0), chunk2);

            foreach(MapTile sinasTile in chunk2.tiles.Values) Console.WriteLine(sinasTile.TilePosition);

            player = new PlayerEntity(new Vector2(6 * 32, 13 * 32), this);
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
            int Iterations = 0;
            int chunksCount = 0;

            Chunk[] chunkList = new Chunk[chunks.Count];
            chunks.Values.CopyTo(chunkList, 0);
 
            for(int i = 0; i < chunkList.Length; i++)
            {
                if (camera.IsOnScreen(chunkList[i].Area))
                {
                    chunksCount++;
                    Iterations += chunkList[i].Draw(spriteBatch, camera);
                    
                    spriteBatch.DrawRectangle(chunkList[i].Area, Color.Red);

                }

            }

            spriteBatch.End();            
            spriteBatch.Begin();
            
            spriteBatch.DrawString(DebugFont, $"Tiles on Screen: {Iterations}\n" + 
                                              $"Chunks: {chunksCount}", new Vector2(0, 25), Color.Red);

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

        public void UpdateEntities(float delta)
        {
            for(int i = 0; i < Entities.Count; i++)
            {
                Entities[i].UpdateScreenPosition(camera);
                Entities[i].Update(delta);
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
            foreach(Chunk chunk in chunks.Values)
            {
                if (chunk.tiles.ContainsKey(pos))
                {
                    return chunk.tiles[pos];
                }
            }
            return null; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Center the camera to the center of the player
            camera.CenterTo(new Vector2(player.Position.X + player.Texture.Width / 2, player.Position.Y + player.Texture.Height / 2), spriteBatch.GraphicsDevice.Viewport);

            DrawMap(spriteBatch);
            DrawEntities(spriteBatch);
  
        } 

        public void Update(float delta)
        {
            camera.Update(delta);
            
            player.Update(delta);    
            
            UpdateEntities(delta);

        }

    }
}