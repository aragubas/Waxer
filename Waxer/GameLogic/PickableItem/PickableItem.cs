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
using Waxer.Animation;
using Waxer.GameLogic.Items;

namespace Waxer.GameLogic
{
    public class PickableItem : RigidBody
    {
        PlaceableTileItem PickItem;
        Dictionary<int, ColorFlasher> colorValues = new Dictionary<int, ColorFlasher>();
        public bool Picked = false;
        public bool Pickable = false;
        float PickDelay = 0;

        public PickableItem(GameWorld World, PlaceableTileItem PickItem, Vector2 Position)
        {
            this.World = World;
            this.PickItem = PickItem;
            this.Position = Position;
            AreaSize = new Vector2(16, 16);
            UpdateArea();            
            
            Texture = PickItem.IconTexture;

            for(int i = 0; i < 3; i++)
            {
                ColorFlasher newFlasher = new ColorFlasher(510, false);
                newFlasher.MinimunValue = 50;
                newFlasher.MaximunValue = 230;
                colorValues.Add(i, newFlasher);
            }

            SpriteOrigin = new Vector2(0, 0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Area, null, BlendColor, SpriteRotation, SpriteOrigin, SpriteEffects, SpriteLayerDepth);
        }

        public override void Update(float delta)
        {
            UpdateArea();
            UpdateBody(delta);
            
            if (!Pickable)
            {
                PickDelay += 1 * delta;

                if (PickDelay >= 0.1)
                {
                    Pickable = true;
                }
            }

            if (Area.Intersects(World.Player.Area) && Pickable)
            {
                Picked = true;
                
                bool QuantityIncreased = false;

                foreach(Item item in World.Player.Inventory)
                {
                    if (item.GetType() == typeof(PlaceableTileItem))
                    {
                        PlaceableTileItem convertedItem = (PlaceableTileItem)item;
                            
                        // A item placeable tile item has been found with the same TileID, increase quantity without creating new item on inventory
                        if (convertedItem.PlaceableTileInfo.TileID == PickItem.PlaceableTileInfo.TileID)
                        {
                            Console.WriteLine("Increase Quantity");
                            QuantityIncreased = convertedItem.IncreaseStack(1);
                        }
                    }
                }

                if (!QuantityIncreased)
                {  
                    PickItem.InventoryIndex = World.Player.Inventory.Count;
                    World.Player.Inventory.Add(PickItem);
                    Console.WriteLine($"Add new item {World.Player.Inventory.Count}");
                }


                Dispose();
                return;
            }

 
            foreach(ColorFlasher flasher in colorValues.Values)
            {
                flasher.Update(delta);
            }

            BlendColor.R = colorValues[0].GetColor();
            BlendColor.G = colorValues[1].GetColor();
            BlendColor.B = colorValues[2].GetColor();
        }
        
    }
}