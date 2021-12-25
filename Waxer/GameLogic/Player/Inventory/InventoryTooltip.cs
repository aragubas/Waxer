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
using Waxer.Animation;
using Waxer.UserInterface;

namespace Waxer.GameLogic.Player.Inventory
{
    public class InventoryTooltip : Tooltip
    {
        public InventoryItem SelectedInventoryItem = null;
        Point Size = new Point(200, 50); 
        ValueSmoother WidthValueSmooth;
        ValueSmoother HeightValueSmooth;
        readonly PlayerEntity Player;
        Vector2 FontSize = Vector2.Zero;

        public InventoryTooltip(PlayerEntity playerEntity)
        { 
            Area = new Rectangle(MouseInput.Position.X, MouseInput.Position.Y, 10, 10); 
            this.Player = playerEntity;

            WidthValueSmooth = new ValueSmoother(400, Size.X, 10);
            HeightValueSmooth = new ValueSmoother(400, Size.Y, 10);
        }
 
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (SelectedInventoryItem == null) { return; }
            if (FontSize == Vector2.Zero)
            {
                FontSize = SelectedInventoryItem.parentInventoryUI.player.World.DebugFont.MeasureString("H"); 
            }
 
            Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;
             
            // Set new viewport
            spriteBatch.GraphicsDevice.Viewport = new Viewport(Area);
            spriteBatch.Begin();
 
            DrawBackgroundRelativeCords(spriteBatch);
            Point tempSize = new Point(2, 2); 

            if (!SelectedInventoryItem.parentInventoryUI.player.InventoryItemExists(SelectedInventoryItem.Index))
            {
                // Render "Empty Slot" at top 
                string Text = "Empty";
                tempSize.X += ((int)FontSize.X * Text.Length) + 2; 
                tempSize.Y += (int)FontSize.Y + 2; 
                spriteBatch.DrawString(SelectedInventoryItem.parentInventoryUI.player.World.DebugFont, Text, new Vector2(2, 2), Color.White); 
                
                Size = tempSize;
            }else
            {
  
                // Finds instance of selected item 
                for(int i = 0; i < Player.Inventory.Count; i++)
                {
                    // Item found
                    if (Player.Inventory[i].InventoryIndex == SelectedInventoryItem.Index)
                    {   
                        // Render item name at top                        
                        tempSize.X += ((int)FontSize.X * Player.Inventory[i].Name.Length) + 2; 
                        tempSize.Y += (int)FontSize.Y + 2;
                        spriteBatch.DrawString(SelectedInventoryItem.parentInventoryUI.player.World.DebugFont, $"{Player.Inventory[i].Name}", new Vector2(2, 2), Color.White); 

            
                        string QuantityText = $"Stack: {Player.Inventory[i].Stack} / {Player.Inventory[i].StackSize}"; 
                        int TextWidth = ((int)FontSize.X * QuantityText.Length) + 4;
                        int TextHeight = ((int)FontSize.Y * 3) + 1; 
                        
                        if (tempSize.X < TextWidth) { tempSize.X = TextWidth; }
                        if (tempSize.Y < TextHeight) { tempSize.Y = TextHeight; }

                        spriteBatch.DrawString(SelectedInventoryItem.parentInventoryUI.player.World.DebugFont, QuantityText, new Vector2(2, FontSize.Y * 2), Color.White); 


                        Size = tempSize; 
                        break;
                    }
                }

            }

            // Restore old viewport
            spriteBatch.End();
            spriteBatch.GraphicsDevice.Viewport = oldViewport;
            
        } 

        public void ResetAnimation()
        {
            WidthValueSmooth.TargetValue = 10;
            HeightValueSmooth.TargetValue = 10;
            Area.Width = 10;
            Area.Height = 10;
            
        } 

        public override void Update(float delta)
        {
            WidthValueSmooth.TargetValue = Size.X;
            HeightValueSmooth.TargetValue = Size.Y;

            Area.Width = WidthValueSmooth.Update(delta);
            Area.Height = HeightValueSmooth.Update(delta);
 
            if (SelectedInventoryItem == null) 
            { 
                WidthValueSmooth.TargetValue = 10;
                HeightValueSmooth.TargetValue = 10;
                return;
            }
            
            Area.X = MouseInput.Position.X + 18;
            Area.Y = MouseInput.Position.Y;

        }
    }
}