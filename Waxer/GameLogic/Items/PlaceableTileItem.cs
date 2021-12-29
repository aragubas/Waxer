/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.GameLogic.Items
{
    public class PlaceableTileItem : Item
    {
        public TileInfo PlaceableTileInfo;

        public PlaceableTileItem(GameWorld World, TileInfo TileInfo) : base(World, -1)
        {
            PlaceableTileInfo = TileInfo;
            StackSize = 256; 
            IconTexture = TileInfo.Texture;
            Name = $"{TileInfo.Name} [Placeable]";
        }

        public override void DoAction(ItemUseContext context)
        {
            if (context.ActionMouseButton == MouseButton.Right_Up)
            {
                Vector2 FixedMousePosition = World.GetTilePosition(context.MousePosition - context.World.Camera.CameraPosition);
                MapTile tile = context.World.GetTile(FixedMousePosition);
                
                if (tile != null)
                {
                    if (!tile.TileInformation.IsColideable)
                    {
                        World.SetTile(FixedMousePosition, PlaceableTileInfo);
                        Stack--;
 
                        // Item needs deletion
                        if (Stack <= 0)
                        {
                            Dispose();
                            return;
                        }
                    }
                }

            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            
        }
    }
}