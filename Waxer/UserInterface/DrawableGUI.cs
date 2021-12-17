using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer.UserInterface
{
    public abstract class DrawableGUI
    {
        public Color BlendColor = Color.White;
        public Color BackgroundColor = Color.Transparent;
        public Rectangle Area;
        public float Rotation;
        public byte Opacity;

        internal void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Area, Color.FromNonPremultiplied(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, 180));
            spriteBatch.DrawRectangle(Area, Color.FromNonPremultiplied(BackgroundColor.R + 64, BackgroundColor.G + 64, BackgroundColor.B + 64, BackgroundColor.A + 64));
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            
        }

        public virtual void Update(float delta)
        {

        }

    }
}