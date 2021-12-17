using Microsoft.Xna.Framework.Graphics;
using Waxer.UserInterface;

namespace Waxer.GameLogic.Player.Inventory
{
    public class InventoryUI : DrawableGUI
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawBackground(spriteBatch);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
        }
    }
}