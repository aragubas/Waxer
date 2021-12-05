using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer
{
    public class CursorRenderer
    {
        public Texture2D cursorTexure;

        public CursorRenderer()
        {
            cursorTexure = Graphics.Sprites.GetSprite("/cursor.png");
        }   

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            spriteBatch.Draw(cursorTexure, new Vector2(MouseInput.Position.X, MouseInput.Position.Y), Color.White);
 

            spriteBatch.End();
        }

    }
}