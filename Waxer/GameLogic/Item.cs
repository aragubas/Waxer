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
using MonoGame.Extended;
using Waxer.Graphics;

namespace Waxer.GameLogic
{

    public enum MouseButton
    {
        Left_Down, Left_Up,
        Right_Down, Right_Up,
        MouseHover
    }

    public struct ItemUseContext
    {
        public Vector2 UseWorldPosition;
        public Vector2 UseScreenPosition;
        public Vector2 PlayerScreenPosition;
        public Vector2 PlayerWorldPosition;
        public MouseButton ActionMouseButton;
        public Vector2 MousePosition;
        public GameWorld World;

        public ItemUseContext(Vector2 useWorldPosition, Vector2 useScreenPosition, Vector2 playerScreenPosition, Vector2 playerWorldPosition, MouseButton actionMouseButton, Vector2 mousePosition, GameWorld world)
        {
            UseWorldPosition = useWorldPosition;
            UseScreenPosition = useScreenPosition;
            PlayerScreenPosition = playerScreenPosition;
            PlayerWorldPosition = playerWorldPosition;
            ActionMouseButton = actionMouseButton;
            MousePosition = mousePosition;
            World = world;
        }
    }

    public abstract class Item : IDisposable
    {
        public string Name;
        public string Description;
        public bool Stackable = true;
        public int StackSize = 16;
        public int RangeOfUse = 4 * 32;
        public GameWorld World;
        public int Stack = 1; 
        public string ID = Guid.NewGuid().ToString();
        public int InventoryIndex = -1;

        public Texture2D IconTexture; 

        public Item(GameWorld World, int InventoryIndex)
        {
            this.World = World;
            this.InventoryIndex = InventoryIndex;
        }

        public virtual void DoAction(ItemUseContext context, float delta)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // DEBUG OPTION
            // Draw range box 
            if (Settings.Debug_RenderItemsRangeBox && Settings.DebugMode)
            {
                spriteBatch.DrawCircle(new CircleF(World.Player.Position + World.Camera.CameraPosition, RangeOfUse), 32, Color.Red);
            }
        }

        public bool IncreaseStack(int HowMuch = 1)
        {
            if (HowMuch <= 0) { return false; }
            if ((Stack + HowMuch) <= StackSize) { Stack += HowMuch; return true; };
            return false;
        } 
 
        public virtual void Update(float delta)
        {

        }

        public virtual void Deactivate()
        {
            
        }
        
        public bool IsInRange(Vector2 PlayerPosition, Vector2 UsedInPosition)
        {
            return Vector2.Distance(PlayerPosition, UsedInPosition) <= RangeOfUse;
        }

        public void Dispose()
        {
            // Remove instance from player inventory 
            World.Player.Inventory.Remove(this);

             
        }
    }
    
}