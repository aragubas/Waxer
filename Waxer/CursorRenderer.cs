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
            
            spriteBatch.Draw(cursorTexure, MouseInput.PositionVector2, Color.White);
  
            spriteBatch.End();
        }

    }
}