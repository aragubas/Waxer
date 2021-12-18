using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Waxer.UserInterface;

namespace Waxer.GameLogic.Player.Inventory
{
    public class InventoryUI : Container
    {
        public readonly PlayerEntity player;
        List<InventoryItem> items = new();

        public InventoryUI(PlayerEntity PlayerInstance)
        {
            Area = new Rectangle(10, 10, 2 + 10 * 40, 42);

            this.player = PlayerInstance;

            for(int i = 0; i < 10; i++)
            {
                items.Add(new InventoryItem(i, this));
            }
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

            foreach(InventoryItem item in items)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();

            // Restore Viewport
            spriteBatch.GraphicsDevice.Viewport = oldViewport;

            spriteBatch.Begin();
            
            if (items[player.SelectedInventoryItem].item != null)
            {
                string ItemInfosString = $"{items[player.SelectedInventoryItem].item.Name}\n" + 
                                         $"{items[player.SelectedInventoryItem].item.Stack} / {items[player.SelectedInventoryItem].item.StackSize}";

                spriteBatch.DrawString(player.World.DebugFont, ItemInfosString, new Vector2(Area.X, Area.Y + Area.Height + 8), Color.Red);
            } 

            spriteBatch.End();

        }

        public override void Update(float delta)
        {
            base.Update(delta);

            foreach(InventoryItem item in items)
            {
                item.Update(delta);
            }

        }
    }
}