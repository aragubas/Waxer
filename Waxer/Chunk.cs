/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SimplexNoise;

namespace Waxer
{
    public class Chunk
    {

        struct ChunkBiomeSettings
        {
            public ChunkBiomeSettings()
            {
            }

            public int Emptyness { get; set; } = 100;

            public new string ToString()
            {
                return $"emptyness: {Emptyness}";
            }
        }

        public Dictionary<Vector2, MapTile> tiles = new();
        public Vector2 ChunkPosition;
        public Rectangle Area;
        GameWorld _world;
        bool CheckChunks = false;
        ChunkBiomeSettings _biomeSettings;

        public Chunk(Vector2 ChunkPosition, GameWorld ParentMap)
        {
            this.ChunkPosition = ChunkPosition;
            this._world = ParentMap;
            FillTiles();
            Area = new Rectangle((int)ChunkPosition.X * 1024, (int)ChunkPosition.Y * 1024, 1024, 1024);
        }

        public int Draw(SpriteBatch spriteBatch, Camera2D camera)
        {
            int i = 0;
            foreach (MapTile tile in tiles.Values)
            {
                if (camera.IsOnScreen(tile.GetArea()))
                {
                    i++;
                    tile.Draw(spriteBatch);
                }
            }

            if (Settings.Debug_DrawChunkBorders)
            {
                spriteBatch.DrawRectangle(Area, Color.MonoGameOrange);
                spriteBatch.DrawString(_world.DebugFont, $"biome:\n{_biomeSettings.ToString()}\nposition: {ChunkPosition}", new Vector2(Area.X + 5, Area.Y + 5), Color.MonoGameOrange);
            }

            if (!CheckChunks)
            {
                CheckChunks = true;

                // Left Chunk
                if (ChunkPosition.X - 1 >= 1)
                {
                    if (!_world.Chunks.ContainsKey(new Vector2(ChunkPosition.X - 1, ChunkPosition.Y)))
                    {
                        _world.Chunks.Add(new Vector2(ChunkPosition.X - 1, ChunkPosition.Y), new Chunk(new Vector2(ChunkPosition.X - 1, ChunkPosition.Y), _world));
                    }
                }

                // Right Chunk
                if (ChunkPosition.X + 1 >= 1)
                {
                    if (!_world.Chunks.ContainsKey(new Vector2(ChunkPosition.X + 1, ChunkPosition.Y)))
                    {
                        _world.Chunks.Add(new Vector2(ChunkPosition.X + 1, ChunkPosition.Y), new Chunk(new Vector2(ChunkPosition.X + 1, ChunkPosition.Y), _world));
                    }
                }

                // Top Chunk
                if (ChunkPosition.Y - 1 >= 1)
                {
                    if (!_world.Chunks.ContainsKey(new Vector2(ChunkPosition.X, ChunkPosition.Y - 1)))
                    {
                        _world.Chunks.Add(new Vector2(ChunkPosition.X, ChunkPosition.Y - 1), new Chunk(new Vector2(ChunkPosition.X, ChunkPosition.Y - 1), _world));
                    }
                }

                // Bottom Chunk
                if (ChunkPosition.Y + 1 >= 1)
                {
                    if (!_world.Chunks.ContainsKey(new Vector2(ChunkPosition.X, ChunkPosition.Y + 1)))
                    {
                        _world.Chunks.Add(new Vector2(ChunkPosition.X, ChunkPosition.Y + 1), new Chunk(new Vector2(ChunkPosition.X, ChunkPosition.Y + 1), _world));
                    }
                }

            }

            return i;
        }

        public void FillTiles()
        {
            Noise.Seed = -_world.Seed + (int)(ChunkPosition.X + ChunkPosition.Y);

            float Ceira = Noise.CalcPixel2D(Area.X, Area.Y, 0.1f);
            _biomeSettings = new ChunkBiomeSettings();

            if (Ceira >= 128)
            {
                _biomeSettings.Emptyness = 200;

            }
            if (Ceira >= 100)
            {
                _biomeSettings.Emptyness = 100;

            }


            // Fill with background tiles
            for (int x = 0; x < 32; x++)
            {
                for (int y = 0; y < 32; y++)
                {
                    int tileID = 1;
                    float nextNoise = Noise.CalcPixel2D(x + (int)ChunkPosition.X, y + (int)ChunkPosition.Y, 0.1f);

                    if (nextNoise < _biomeSettings.Emptyness)
                    {
                        tileID = 0;
                    }

                    MapTile tile = new MapTile(this, TilePosition: new Vector2((ChunkPosition.X * 32) + x, (ChunkPosition.Y * 32) + y),
                                                ScreenPosition: new Vector2((ChunkPosition.X * 1024) + (x * 32), (ChunkPosition.Y * 1024) + (y * 32)),
                                                TileID: tileID);

                    tiles.Add(tile.TilePosition, tile);
                }
            }


            // Initial spawn area
            if (ChunkPosition == Vector2.Zero)
            {
                // Initial Square
                for (int x = 4; x < 20; x++)
                {
                    for (int y = 5; y < 15; y++)
                    {
                        tiles[new Vector2(x, y)].SetTileID(0);
                    }
                }

                // Add some floor
                for (int x = 4; x < 20; x++)
                {
                    tiles[new Vector2(x, 15)].SetTileID(2);
                }

            }

        }

    }

}