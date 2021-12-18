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
        int MultipleRowsInventorySelectedItem = -1;

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

                if (Col > 10)
                {
                    Col = 0;
                    Row++;
                }
                
                items.Add(new InventoryItem(i, Col, Row, this));
            }

            Area.Height = 4;

            valueSmooth = new ValueSmoother(250, OneRowHeight, 4);
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
                if (items[i].Index == MultipleRowsInventorySelectedItem && MulitpleRowsVisible)
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

            spriteBatch.DrawString(player.World.DebugFont, NumberRowsToDraw.ToString(), new Vector2(0, 0), Color.Red);


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

        }

        void ExitFullInventory()
        {
            MultipleRowsInventorySelectedItem = -1;
            MulitpleRowsVisible = false;
            valueSmooth.TargetValue = OneRowHeight;

        }

        void EnterFullInventory()
        {
            MultipleRowsInventorySelectedItem = -1;
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
                    if (MultipleRowsInventorySelectedItem == item.Index)
                    {
                        // Remove selection
                        MultipleRowsInventorySelectedItem = -1;     
                        return;
                         
                    }else // User is trying to move something to something
                    {
                        if (MultipleRowsInventorySelectedItem != -1) // Clicked on an item and then in another one
                        {
                            int item2Index = -1;
                            int item1Index = -1;
                            
                            for(int i = 0; i < player.Inventory.Count; i++)
                            {
                                if (item2Index != -1 && item1Index != -1) { break; }

                                if (player.Inventory[i].InventoryIndex == MultipleRowsInventorySelectedItem)
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
                                // Both items are the same type, try to combine them 
                                if (player.Inventory[item2Index].GetType() == player.Inventory[item1Index].GetType())
                                {
                                    // Items can combine, destroy instance of item2
                                    if (player.Inventory[item1Index].IncreaseStack(player.Inventory[item2Index].Stack))
                                    {
                                        player.Inventory[item2Index].Dispose(); 
                                    } 
                                    else
                                    {
                                        // Switch places if can't combine items
                                        player.Inventory[item1Index].InventoryIndex = player.Inventory[item2Index].InventoryIndex;
                                        player.Inventory[item2Index].InventoryIndex = MultipleRowsInventorySelectedItem;

                                    }
                                }else
                                {
                                    // Switch places if incompatible
                                    player.Inventory[item1Index].InventoryIndex = player.Inventory[item2Index].InventoryIndex;
                                    player.Inventory[item2Index].InventoryIndex = MultipleRowsInventorySelectedItem;

                                }

                                MultipleRowsInventorySelectedItem = -1;
                                
                                Console.WriteLine($"Switch Places {player.Inventory[item1Index].InventoryIndex} with {player.Inventory[item2Index].InventoryIndex}");

                            }else // User clicked in a white space, move to selected white space
                            {
                                player.Inventory[item1Index].InventoryIndex = item.Index;
                                MultipleRowsInventorySelectedItem = -1;

                                Console.WriteLine($"Switch Places White Space {player.Inventory[item1Index].InventoryIndex} with {item.Index}");

                            }


                        }else // User clicked on an item without any other item selected. Start move operation
                        {
                            MultipleRowsInventorySelectedItem = item.Index;

                        }

                    }
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

        }

        public override void Update(float delta)
        {
            base.Update(delta);

            Area.Height = valueSmooth.Update(delta);

            FullInventoryToggleKey();
            UpdateInventoryItems(delta);

        }
    }
}