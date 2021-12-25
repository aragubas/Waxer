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
using Waxer.UserInterface;
using Waxer.Animation;

namespace Waxer.GameLogic.Player.Inventory
{    
    public class InventoryItem : Control
    {
        public readonly int Index;
        public readonly InventoryUI parentInventoryUI;
        bool Selected = false;
        Dictionary<int, ColorFlasher> colorValues = new Dictionary<int, ColorFlasher>();
        int RowPos = 0;
 
        public InventoryItem(int Index, int X, int Y, InventoryUI ParentInventoryUI)
        {
            this.Index = Index;
            this.parentInventoryUI = ParentInventoryUI;

            Area = new Rectangle(2 + X * 40, 2 + Y * 40, 38, 38);
            RowPos = Area.Y;

            for(int i = 0; i < 3; i++)
            {
                colorValues.Add(i, new ColorFlasher(510, true));
            }

            BackgroundColor = Color.Black;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            DrawBackground(spriteBatch);

            foreach(Item item in parentInventoryUI.player.Inventory)
            {
                if (item.InventoryIndex == Index)
                {
                    spriteBatch.Draw(item.IconTexture, new Rectangle(Area.X, Area.Y, 38, 38), BackgroundColor);                    
                }
            }
            
        } 

        public override void Update(float delta)
        {
            base.Update(delta);

            Selected = parentInventoryUI.player.SelectedInventoryItem == Index;

            // Check if index is in range with player inventory
            for(int i = 0; i < parentInventoryUI.player.Inventory.Count; i++)
            {
                if (parentInventoryUI.player.Inventory[i].InventoryIndex == Index)
                {
                    // If item stack is less than or equal to zero, dispose item
                    if (parentInventoryUI.player.Inventory[i].Stack <= 0) 
                    {
                        parentInventoryUI.player.Inventory[i].Dispose(); 
                    }
                    
                }
            }


            foreach(ColorFlasher flasher in colorValues.Values)
            {
                flasher.FadeOut = !Selected;

                if (Selected)
                {
                    flasher.Speed = 255;
                    flasher.MinimunValue = 150;

                }else 
                { 
                    flasher.Speed = 510; 
                    flasher.MinimunValue = 100;
                }

                flasher.Update(delta);
            }

            if (Selected) 
            { 
                Area.Y = RowPos + 2;

            } else 
            { 
                Area.Y = RowPos; 
            }

            BackgroundColor.R = colorValues[0].GetColor();
            BackgroundColor.G = colorValues[1].GetColor();
            BackgroundColor.B = colorValues[2].GetColor();
        }

    }
}