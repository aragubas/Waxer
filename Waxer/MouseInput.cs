/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Waxer
{
    public static class MouseInput
    {
        public static Rectangle Position;
        public static Vector2 PositionVector2;
        public static Rectangle Left_UpClickPos;
        public static Rectangle Left_DownClickPos;
        public static Rectangle Right_UpClickPos;
        public static Rectangle Right_DownClickPos;
        static MouseState oldState;
 
        public static void Update()
        {
            Left_UpClickPos = Rectangle.Empty;
            Left_DownClickPos = Rectangle.Empty;
            Right_UpClickPos = Rectangle.Empty;
            Right_DownClickPos = Rectangle.Empty;
            
            MouseState newState = Mouse.GetState();
            Position = new Rectangle(newState.Position.X, newState.Position.Y, 1, 1);
            
            // Limit the mouse position inside the window
            if (Position.X <= 0) { Position.X = 0; }
            if (Position.Y <= 0) { Position.Y = 0; }
            if (Position.X >= GameMain.Reference.Window.ClientBounds.Width) { Position.X = GameMain.Reference.Window.ClientBounds.Width; }
            if (Position.Y >= GameMain.Reference.Window.ClientBounds.Height) { Position.Y = GameMain.Reference.Window.ClientBounds.Height; }
            
            
            PositionVector2 = new Vector2(Position.X, Position.Y);

             
            // Check for Left Button Up
            if (newState.LeftButton == ButtonState.Released && oldState.LeftButton == ButtonState.Pressed)
            {
                Left_UpClickPos = new Rectangle(newState.Position.X, newState.Position.Y, 1, 1);
            }

            // Check for Left Button Bottom
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Pressed)
            {
                Left_DownClickPos = new Rectangle(newState.Position.X, newState.Position.Y, 1, 1);
            }

            // Check for Right Button Up
            if (newState.RightButton == ButtonState.Released && oldState.RightButton == ButtonState.Pressed)
            {
                Right_UpClickPos = new Rectangle(newState.Position.X, newState.Position.Y, 1, 1);
            }

            // Check for Right Button Bottom
            if (newState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Pressed)
            {
                Right_DownClickPos = new Rectangle(newState.Position.X, newState.Position.Y, 1, 1);
            }


            oldState = newState;
        }

    }
}