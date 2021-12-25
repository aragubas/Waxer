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
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Waxer.Animation;
using Waxer.UserInterface;

namespace Waxer.GameLogic.Player.Inventory
{
    public class InventoryUI : Container
    {
        public readonly PlayerEntity player;
        List<InventoryItem> items = new();

        readonly int OneRowHeight = 0;
        readonly int AllRowsHeight = 0;
        KeyboardState oldState;
        ValueSmoother valueSmooth;
        bool MulitpleRowsVisible = false;
        int MultipleRows_InventorySelectedItem = -1;
        int MultipleRows_InventoryItemBeingHovered = -1;
        InventoryTooltip tooltip;
        int MultipleRows_ItemsBeingHovered = 0;

        public InventoryUI(PlayerEntity PlayerInstance)
        {
            Area = new Rectangle(10, 10, 2 + 10 * 40, 42);
            
            this.OneRowHeight = 42;
            this.AllRowsHeight = 2 + 40 * 3;
            this.player = PlayerInstance;
            int Row = 0;
            int Col = -1;
  
            for(int i = 0; i < 30; i++)
            { 
                Col++;

                if (Col > 9)
                {
                    Col = 0;
                    Row++;
                }
                
                items.Add(new InventoryItem(i, Col, Row, this));
            }

            Area.Height = 4;

            valueSmooth = new ValueSmoother(250, OneRowHeight, 4);
            tooltip = new InventoryTooltip(PlayerInstance);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {       
            Viewport oldViewport = spriteBatch.GraphicsDevice.Viewport;
            Viewport viewport = new Viewport(Area);

            // Change viewport
            spriteBatch.GraphicsDevice.Viewport = viewport;
  
            spriteBatch.Begin(); 
            // Draw the background
            DrawBackground(spriteBatch);

            int NumberRowsToDraw = 10;

            if (Area.Height > OneRowHeight) { NumberRowsToDraw = items.Count; }

            for(int i = 0; i < NumberRowsToDraw; i++)
            {  
                if (items[i].Index == MultipleRows_InventorySelectedItem && MulitpleRowsVisible)
                {
                    // Draw red square on its original position
                    spriteBatch.DrawRectangle(items[i].Area, Color.White);
 
                    // Back up the original area, change area position to Mouse Position 
                    Rectangle OldItemRect = items[i].Area;
                    items[i].Area = new Rectangle(MouseInput.Position.X - Area.X, MouseInput.Position.Y - Area.Y, items[i].Area.Width, items[i].Area.Height);
 
                    items[i].Draw(spriteBatch); 

                    // Restore the original area
                    items[i].Area = OldItemRect; 
                }else
                {
                    items[i].Draw(spriteBatch); 

                }
            }

            spriteBatch.End();



            // Restore Viewport
            spriteBatch.GraphicsDevice.Viewport = oldViewport;

            // Render Stats
            spriteBatch.Begin();
            
            for(int i = 0; i < player.Inventory.Count; i++)
            {
                if (player.Inventory[i].InventoryIndex == player.SelectedInventoryItem)
                {
                    string ItemInfosString = $"{player.Inventory[i].Name}\n" + 
                                             $"{player.Inventory[i].Stack} / {player.Inventory[i].StackSize}";
  
                    spriteBatch.DrawString(player.World.DebugFont, ItemInfosString, new Vector2(Area.X, Area.Y + Area.Height + 8), Color.Red);

                }
            }

            spriteBatch.End();

            if (MulitpleRowsVisible)
            {
                tooltip.Draw(spriteBatch); 
            } 
            
        }

        void ExitFullInventory()
        {
            MultipleRows_InventorySelectedItem = -1;
            MulitpleRowsVisible = false;
            valueSmooth.TargetValue = OneRowHeight;
            tooltip.ResetAnimation();
            
        }

        void EnterFullInventory()
        {
            MultipleRows_InventorySelectedItem = -1;
            MulitpleRowsVisible = true;
            valueSmooth.TargetValue = AllRowsHeight;

        }

        void FullInventoryToggleKey()
        {
            KeyboardState newState = Keyboard.GetState();
            
            if (Utils.CheckKeyUp(oldState, newState, Keys.E))
            {
                if (MulitpleRowsVisible)
                {
                    ExitFullInventory();
                     
                }else
                {
                    EnterFullInventory();
                }
            }

            oldState = newState;
        }

        void UpdateInventoryPositioning(InventoryItem item)
        {
            if (MulitpleRowsVisible)
            {
                Rectangle FixedArea = new Rectangle(item.Area.X + Area.X,item.Area.Y + Area.Y, item.Area.Width, item.Area.Height); 

                if (MouseInput.Left_UpClickPos.Intersects(FixedArea))
                {
                    // Clicked on the same place
                    if (MultipleRows_InventorySelectedItem == item.Index)
                    {
                        // Remove selection
                        MultipleRows_InventorySelectedItem = -1;     
                        return;
                         
                    }else // User is trying to move something to something
                    {
                        if (MultipleRows_InventorySelectedItem != -1) // Clicked on an item and then in another one
                        {
                            int item2Index = -1;
                            int item1Index = -1;
                            
                            for(int i = 0; i < player.Inventory.Count; i++)
                            {
                                if (item2Index != -1 && item1Index != -1) { break; }

                                if (player.Inventory[i].InventoryIndex == MultipleRows_InventorySelectedItem)
                                { 
                                    item1Index = i; 
                                    continue;
                                }

                                if (player.Inventory[i].InventoryIndex == item.Index)
                                { 
                                    item2Index = i;
                                    continue;
                                }

                            }
                             
                            
                            if (item1Index == -1) { return; }

                            // Item 1 and two exists, move operation will happen
                            if (item2Index != -1)
                            {                                
                                // Switch places
                                player.Inventory[item1Index].InventoryIndex = player.Inventory[item2Index].InventoryIndex;
                                player.Inventory[item2Index].InventoryIndex = MultipleRows_InventorySelectedItem;

                                // Deselect
                                MultipleRows_InventorySelectedItem = -1;

                            }else // User clicked in a white space, move to selected white space
                            {
                                player.Inventory[item1Index].InventoryIndex = item.Index;
                                
                                MultipleRows_InventorySelectedItem = -1;
                            }


                        }else // User clicked on an item without any other item selected. Start move operation
                        {
                            MultipleRows_InventorySelectedItem = item.Index;

                        }

                    }
                 
                }else if (MouseInput.Position.Intersects(FixedArea))
                {
                    MultipleRows_InventoryItemBeingHovered = item.Index;
                    MultipleRows_ItemsBeingHovered++;
                }
            }
 
        }

        void UpdateInventoryItems(float delta)
        {
            foreach(InventoryItem item in items)
            {
                item.Update(delta);
                UpdateInventoryPositioning(item); 
            }   

            if (MultipleRows_ItemsBeingHovered < 1)
            {
                MultipleRows_InventoryItemBeingHovered = -1;
            }
             
            MultipleRows_ItemsBeingHovered = 0;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Area.Height = valueSmooth.Update(delta);

            FullInventoryToggleKey();
            UpdateInventoryItems(delta);
 
            if (MulitpleRowsVisible) 
            {
                if (MultipleRows_InventoryItemBeingHovered < items.Count && MultipleRows_InventoryItemBeingHovered >= 0)
                {
                    tooltip.SelectedInventoryItem = items[MultipleRows_InventoryItemBeingHovered];
                }else
                {
                    tooltip.SelectedInventoryItem = null;
                }

                tooltip.Update(delta);
            }

        }
    }
}