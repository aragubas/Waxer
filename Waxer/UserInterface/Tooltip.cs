/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

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