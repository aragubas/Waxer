using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Waxer.GameLogic;
using System;
using MonoGame.Extended;
using Waxer.GameLogic.Player;
using Waxer.GameLogic.Player.Inventory;

namespace Waxer
{
    public struct MapProperties
    {
        public Vector2 TileSize;
    }

    public struct EnvironmentSettings
    {
        public float Gravity = 28f;
    }

    public class GameWorld
    {
        public MapProperties Properties;
        public PlayerEntity Player;
        public Camera2D Camera = new Camera2D();
        public List<WorldEntity> Entities = new List<WorldEntity>();
        public EnvironmentSettings WorldEnvironment = new EnvironmentSettings();
        public SpriteFont DebugFont;
        public Vector2 DebugFontSize;
        public Dictionary<Vector2, Chunk> chunks = new();
        InventoryUI inventoryUI;

        public GameWorld()
        {
            Properties = new MapProperties();
            Properties.TileSize = new Vector2(32, 32);
            
            Chunk initialChunk = new Chunk(new Vector2(0, 0), this);
            chunks.Add(new Vector2(0, 0), initialChunk);

            Player = new PlayerEntity(new Vector2(8  * 32, 10 * 32), this);
            Entities.Add(new GameLogic.Towers.Test(new Vector2(4 * 32, 9 * 32), this));
            
            inventoryUI = new InventoryUI(Player);

        } 

        // Draw the map on its batch
        void DrawMap(SpriteBatch spriteBatch)
        {
            if (DebugFont == null) 
            {
                DebugFont = Graphics.Fonts.GetSpriteFont(Graphics.Fonts.GetFontDescriptor("/PressStart2P", 8, spriteBatch.GraphicsDevice));
                DebugFontSize = DebugFont.MeasureString("H"); 
            }

            spriteBatch.Begin(transformMatrix: Camera.GetMatrix());
            int Iterations = 0;
            int chunksCount = 0;

            Chunk[] chunkList = new Chunk[chunks.Count];
            chunks.Values.CopyTo(chunkList, 0);
 
            for(int i = 0; i < chunkList.Length; i++)
            {
                if (Camera.IsOnScreen(chunkList[i].Area))
                {
                    chunksCount++;
                    Iterations += chunkList[i].Draw(spriteBatch, Camera);

                }

            }

            spriteBatch.End();
        }

        public void DrawEntities(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: Camera.GetMatrix());

            for(int i = 0; i < Entities.Count; i++)
            {
                Entities[i].Draw(spriteBatch);
            }
            
            Player.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void UpdateEntities(float delta)
        {
            for(int i = 0; i < Entities.Count; i++)
            {
                Entities[i].UpdateScreenPosition(Camera);
                Entities[i].Update(delta);
            }
 
 
        }

        public Vector2 GetTilePosition(Vector2 pos)
        {
            int xPos = 0;
            int yPos = 0;

            xPos = (int)pos.X / (int)Properties.TileSize.X;
            yPos = (int)pos.Y / (int)Properties.TileSize.Y;

            return new Vector2(xPos, yPos);
        }

        public Vector2 GetTilePosition(Rectangle rect)
        {
            int xPos = 0;
            int yPos = 0;

            xPos = rect.X / (int)Properties.TileSize.X;
            yPos = rect.Y / (int)Properties.TileSize.Y;

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

        public bool SetTile(Vector2 pos, TileInfo newInfos)
        {
            foreach(Chunk chunk in chunks.Values)
            {
                if (chunk.tiles.ContainsKey(pos))
                {
                    chunk.tiles[pos].TileInformation = newInfos;
                    return true;
                }
            }
            return false; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Center the camera to the center of the player
            Camera.CenterTo(new Vector2(Player.Position.X + Player.Texture.Width / 2, Player.Position.Y + Player.Texture.Height / 2), spriteBatch.GraphicsDevice.Viewport);

            DrawMap(spriteBatch);
            DrawEntities(spriteBatch);

            inventoryUI.Draw(spriteBatch);

        } 

        public void Update(float delta)
        {
            Camera.Update(delta);
            
            Player.Update(delta);    
            
            UpdateEntities(delta);

            inventoryUI.Update(delta);
            
        }

    }
}