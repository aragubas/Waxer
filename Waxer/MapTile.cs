/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer
{
    public struct TileInfo
    {
        public int TileID;
        public bool IsColideable;
        public float Friction = 0f;
        public string Name;
        public Texture2D Texture;

        public TileInfo(int tileID, bool isColideable, float friction, string name, Texture2D texture)
        {
            TileID = tileID;
            IsColideable = isColideable;
            Friction = friction;
            Name = name;
            Texture = texture;
        }
    }

    public static class TilesInfo
    {
        public static Dictionary<int, TileInfo> TileInfos = new Dictionary<int, TileInfo>();

        public static void LoadTileInfos()
        {
            TileInfos.Clear();
             
            TileInfos.Add(0, new TileInfo(0, false, 0f, "Background Dirt", Graphics.Sprites.GetSprite("/tiles/0.png")));
            TileInfos.Add(1, new TileInfo(1, true, 12f, "Dirt", Graphics.Sprites.GetSprite("/tiles/1.png")));
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