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
        readonly int index;
        readonly InventoryUI parentInventoryUI;
        bool Selected = false;
        Dictionary<int, ColorFlasher> colorValues = new Dictionary<int, ColorFlasher>();
        public Item item;
        string LastItemID;

        public InventoryItem(int Index, InventoryUI ParentInventoryUI)
        {
            this.index = Index;
            this.parentInventoryUI = ParentInventoryUI;

            Area = new Rectangle(2 + index * 40, 2, 38, 38);

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
 
            if (index < parentInventoryUI.player.Inventory.Count)
            {
                item = parentInventoryUI.player.Inventory[index];

                if (item != null)
                {
                    spriteBatch.Draw(item.IconTexture, new Rectangle(Area.X, Area.Y, 38, 38), BackgroundColor);

                }
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Selected = parentInventoryUI.player.SelectedInventoryItem == index;
            
            if (item != null)
            {
                if (item.Stack <= 0) 
                {
                    item.Dispose();
                    item = null;
                }
            }

            foreach(ColorFlasher flasher in colorValues.Values)
            {
                flasher.FadeOut = !Selected;

                if (Selected)
                {
                    flasher.Speed = 255;
                    flasher.MinimunValue = 100;

                }else 
                { 
                    flasher.Speed = 510; 
                    flasher.MinimunValue = 50;
                }

                flasher.Update(delta);
            }

            if (Selected) 
            { 
                Area.Y = 4; 
            } else 
            { 
                Area.Y = 2; 
            }

            BackgroundColor.R = colorValues[0].GetColor();
            BackgroundColor.G = colorValues[1].GetColor();
            BackgroundColor.B = colorValues[2].GetColor();
        }

    }
}