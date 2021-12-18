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

        public virtual void DoAction(ItemUseContext context)
        {
            
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // DEBUG OPTION
            // Draw range box 
            if (Settings.Debug_RenderItemsRangeBox)
            {
                spriteBatch.DrawCircle(new CircleF(World.Player.Position + World.Camera.CameraPosition, RangeOfUse), 32, Color.Red);
            }
        }

        public bool IncreaseStack(int HowMuch = 1)
        {
            if (Stack + HowMuch <= StackSize) { Stack += HowMuch; return true; };
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