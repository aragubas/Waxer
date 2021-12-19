using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer.UserInterface
{
    public abstract class Tooltip : Control
    { 
        // Tooltips has a darker background 
        new internal void DrawBackgroundRelativeCords(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(0, 0, Area.Width, Area.Height), Color.FromNonPremultiplied(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, Opacity - 100));
            spriteBatch.DrawRectangle(new Rectangle(0, 0, Area.Width, Area.Height), Color.FromNonPremultiplied(BackgroundColor.R + 64, BackgroundColor.G + 64, BackgroundColor.B + 64, Opacity - 32));
        }
        
    }
}