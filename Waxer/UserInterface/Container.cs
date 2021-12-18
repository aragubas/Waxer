using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer.UserInterface
{
    public abstract class Container
    {
        public List<Control> Elements = new();
        public Rectangle Area;
        public Color BlendColor = Color.White;
        public Color BackgroundColor = Color.Transparent;
        public float Rotation;
        public byte Opacity;

        internal void DrawElements(SpriteBatch spriteBatch)
        {
            foreach(Control element in Elements)
            {
                element.Draw(spriteBatch);
            }
        } 

        internal void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(new Rectangle(0, 0, Area.Width, Area.Height), Color.FromNonPremultiplied(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, 180));
            spriteBatch.DrawRectangle(new Rectangle(0, 0, Area.Width, Area.Height), Color.FromNonPremultiplied(BackgroundColor.R + 64, BackgroundColor.G + 64, BackgroundColor.B + 64, BackgroundColor.A + 64));
        }


        public virtual void Draw(SpriteBatch spriteBatch)
        {
                        
        }

        public virtual void Update(float delta)
        {

        }

    }
}