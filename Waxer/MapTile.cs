/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Waxer
{
    public struct TileInfo
    {
        public int TileID;
        public bool IsColideable;
        public float SpeedAcceleration = 1f;
        public string Name;
        public float BreakTime;
        public Texture2D Texture;

        public TileInfo(int tileID, bool isColideable, float speedAcceleration, string name, float breakTime, Texture2D texture)
        {
            TileID = tileID;
            IsColideable = isColideable;
            SpeedAcceleration = speedAcceleration;
            Name = name;
            BreakTime = breakTime;
            Texture = texture;
        }
    }
    
    public static class TilesInfo
    {
        public static Dictionary<int, TileInfo> TileInfos = new Dictionary<int, TileInfo>();
 
        #pragma warning disable 0649
        struct JsnTile
        {
            [JsonProperty("id")]
            public int ID;
 
            [JsonProperty("colideable")]
            public bool Colideable;

            [JsonProperty("speedAcceleration")]
            public float SpeedAcceleration;

            [JsonProperty("name")]
            public string Name;

            [JsonProperty("breakTime")]
            public float BreakTime;

            [JsonProperty("texturePath")]
            public string TexturePath;
            
        }
        #pragma warning restore 0649

        public static void LoadTileInfos()
        {
            TileInfos.Clear();
            
            // Get all json files in the tile folders inside data
            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(Settings.DataPath, "tiles/"));
            FileInfo[] files = dirInfo.GetFiles("*.json", SearchOption.AllDirectories);

            foreach(FileInfo tileFile in files)
            {
                try
                {
                    string strJson = File.ReadAllText(tileFile.FullName);

                    JsnTile jsonTile = JsonConvert.DeserializeObject<JsnTile>(strJson);
                    TileInfo newTile = new TileInfo(jsonTile.ID, jsonTile.Colideable, jsonTile.SpeedAcceleration, jsonTile.Name,
                                                    jsonTile.BreakTime, Graphics.Sprites.GetSprite(jsonTile.TexturePath));

                    TileInfos.Add(jsonTile.ID, newTile); 
                    
                    Utils.ConsoleWriteWithTitle("LoadTilesInfo", $"Tile loaded: \"{newTile.Name}\" successfully.");

                }catch(Newtonsoft.Json.JsonException ex)
                {
                    Utils.ConsoleWriteWithTitle("LoadTilesInfo ERROR", $"Error while parsing tile info file \"{Path.GetRelativePath(Settings.DataPath, tileFile.FullName)}\"\n{ex.Message}", true);
                
                }catch(UnauthorizedAccessException ex)
                {
                    Utils.ConsoleWriteWithTitle("LoadTilesInfo ERROR", $"Unauthorized access to tile info file \"{Path.GetRelativePath(Settings.DataPath, tileFile.FullName)}\"\n{ex.Message}\n", true);

                }

            }

        }

    }

    public class MapTile
    {
        public Vector2 ScreenPosition;
        public Vector2 TilePosition;
        public TileInfo TileInformation;
        int TileID = 0;
        Color BlendColor = Color.White;
        public Chunk ParentChunk;
        public string TileUID = Guid.NewGuid().ToString();

        public MapTile(Chunk ParentChunk, Vector2 TilePosition, Vector2 ScreenPosition, int TileID)
        {
            this.TilePosition = TilePosition;
            this.ScreenPosition = ScreenPosition;
            SetTileID(TileID);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TileInformation.Texture, ScreenPosition, BlendColor);
        }

        public Rectangle GetScreenPosition()
        {
            return new Rectangle((int)(ScreenPosition.X), (int)(ScreenPosition.Y), 32, 32);
        }

        public Rectangle GetArea(Camera2D camera)
        {
            return new Rectangle((int)TilePosition.X + (int)camera.CameraPosition.X,
                                (int)TilePosition.Y + (int)camera.CameraPosition.Y, 32, 32);
        }

        public Rectangle GetArea()
        {
            return new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, 32, 32);
        }


        public void SetTileID(int newTileID)
        {
            TileID = newTileID;
            
            if (TilesInfo.TileInfos.ContainsKey(newTileID))
            {
                TileInformation = TilesInfo.TileInfos[newTileID];
            }

        }

        public int GetTileID()
        {
            return TileID;
        }
    }

}