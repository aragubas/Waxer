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
using Waxer.GameLogic.Items;
using Waxer.GameLogic.Player.Inventory;
using Waxer.Graphics;

namespace Waxer.GameLogic.Player
{
    public class PlayerEntity : ControlableCharacter
    {
        public int SelectedInventoryItem = 0;
        public List<Item> Inventory = new();
        KeyboardState oldState;

        public PlayerEntity(Vector2 initialPosition, GameWorld parentMap)
        {
            Position = initialPosition;
            World = parentMap;
 
            // Set SpriteOrigin to Bottom Center
            SpriteOrigin = new Vector2(16, 0);

            Inventory.Add(new Shovel(World, 0));

            Texture = Sprites.MissingTexture;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw Player Sprite
            base.Draw(spriteBatch);
            
            spriteBatch.End();
            spriteBatch.Begin();
            
            base.DrawDebug(spriteBatch);

            // Draw selected item
            for(int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].InventoryIndex == SelectedInventoryItem)
                {
                    Inventory[i].Draw(spriteBatch);
                }
            }

        }

        public void BulletDamage(Bullet bullet)
        {
            if (bullet.ShooterInstanceID == InstanceID) { return; }
            Life -= bullet.Damage;
            BlendColor.R = 255;
            BlendColor.G = 0;
            BlendColor.B = 0;
        }

        void DeactivateSelectedItem()
        {
            for(int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].InventoryIndex == SelectedInventoryItem)
                {
                    Inventory[i].Deactivate();                     
                }
            }
        }

        public bool AddItemInventory(Item item)
        {
            if (Inventory.Count > PlayerState.MaximumInventorySlots) { return false; }
            
            item.InventoryIndex = Inventory.Count;
            Inventory.Add(item);
            
            return true;
        }

        int LastScrollValue = 0;
        void UpdateInventorySelectedItemSlot()
        {
            KeyboardState newState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();


            if (newMouseState.ScrollWheelValue - LastScrollValue >= 1)
            {
                DeactivateSelectedItem();
                SelectedInventoryItem++;

                if (SelectedInventoryItem > 9) { SelectedInventoryItem = 0; }
            }

            if (newMouseState.ScrollWheelValue - LastScrollValue <= -1)
            {
                DeactivateSelectedItem();
                SelectedInventoryItem--;

                if (SelectedInventoryItem < 0) { SelectedInventoryItem = 9; }
            }

            LastScrollValue = newMouseState.ScrollWheelValue;


            if (Utils.CheckKeyUp(oldState, newState, Keys.D1))
            {
                if (SelectedInventoryItem != 0) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 0;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D2))
            {
                if (SelectedInventoryItem != 1) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 1;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D3))
            {
                if (SelectedInventoryItem != 2) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 2;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D4))
            {
                if (SelectedInventoryItem != 3) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 3;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D5))
            {
                if (SelectedInventoryItem != 4) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 4;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D6))
            {
                if (SelectedInventoryItem != 5) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 5;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D7))
            {
                if (SelectedInventoryItem != 6) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 6;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D8))
            {
                if (SelectedInventoryItem != 7) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 7;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D9))
            {
                if (SelectedInventoryItem != 8) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 8;
            }

            if (Utils.CheckKeyUp(oldState, newState, Keys.D0))
            {
                if (SelectedInventoryItem != 9) { DeactivateSelectedItem(); }
                SelectedInventoryItem = 9;
            }

            oldState = newState;
        }

        public bool InventoryItemExists(int Index)
        {
            Item itemFound = Inventory.Find(item => item.InventoryIndex == Index);
            return itemFound != null;
        }

        public override void Update(float delta)
        {
            base.Update(delta);

            // Update controlable character logic
            UpdateChracter(delta);

            // Update Inventory Slot Selection
            UpdateInventorySelectedItemSlot();

            // Update selected item
            for(int i = 0; i < Inventory.Count; i++)
            {
                if (Inventory[i].InventoryIndex == SelectedInventoryItem)
                {
                    Inventory[i].Update(delta);

                    if (_tileUnderCursor != null)
                    {
                        Rectangle FixedArea = new Rectangle(_tileUnderCursor.GetArea().X + (int)World.Camera.CameraPosition.X,
                            _tileUnderCursor.GetArea().Y + (int)World.Camera.CameraPosition.Y, 32, 32);

                        if (MouseInput.Left_UpClickPos.Intersects(FixedArea))
                        {
                            Inventory[i].DoAction(new ItemUseContext(_tileUnderCursor.TilePosition, new Vector2(FixedArea.X, FixedArea.Y), Position + World.Camera.CameraPosition, _tileBehind.TilePosition, MouseButton.Left_Up, MouseInput.PositionVector2, World));
                        }

                        if (MouseInput.Right_UpClickPos.Intersects(FixedArea)) 
                        {
                            Inventory[i].DoAction(new ItemUseContext(_tileUnderCursor.TilePosition, new Vector2(FixedArea.X, FixedArea.Y), Position + World.Camera.CameraPosition, _tileBehind.TilePosition, MouseButton.Right_Up, MouseInput.PositionVector2, World));
                        }

                        if (MouseInput.Left_DownClickPos.Intersects(FixedArea))
                        {
                            Inventory[i].DoAction(new ItemUseContext(_tileUnderCursor.TilePosition, new Vector2(FixedArea.X, FixedArea.Y), Position + World.Camera.CameraPosition, _tileBehind.TilePosition, MouseButton.Left_Down, MouseInput.PositionVector2, World));
                        }

                        if (MouseInput.Right_DownClickPos.Intersects(FixedArea))
                        {
                            Inventory[i].DoAction(new ItemUseContext(_tileUnderCursor.TilePosition, new Vector2(FixedArea.X, FixedArea.Y), Position + World.Camera.CameraPosition, _tileBehind.TilePosition, MouseButton.Right_Down, MouseInput.PositionVector2, World));
                        }
                        
                    }
                }

            }            

        }
    }
}