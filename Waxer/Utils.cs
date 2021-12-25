/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Waxer
{
    public static class Utils
    {
        public static bool IsInRange(float range, Vector2 v1, Vector2 v2)
        {
            var dx = v1.Y - v2.X;
            var dy = v1.Y - v2.X;
            return dx * dx + dy * dy < range * range;
        }

        /// <summary>
        /// Convert strings to rect.
        /// </summary>
        /// <returns>The to rect.</returns>
        /// <param name="StringToConvert">String to convert.</param>
        public static Rectangle StringToRect(string StringToConvert)
        {
            string[] RectCode = StringToConvert.Split(',');
 
            int RectX = Int32.Parse(RectCode[0]);
            int RectY = Int32.Parse(RectCode[1]);
            int RectW = Int32.Parse(RectCode[2]);
            int RectH = Int32.Parse(RectCode[3]);

            return new Rectangle(RectX, RectY, RectW, RectH);
        }

        /// <summary>
        /// Convert string to color
        /// </summary>
        /// <returns>The to color.</returns>
        /// <param name="StringToConver">String to conver.</param>
        /// <param name="ColorAlphaOveride">Color alpha overide.</param>
        public static Color StringToColor(string StringToConver, int ColorAlphaOveride = -255)
        {
            Color ColorToReturn = Color.Magenta;

            int Color_R = 0;
            int Color_G = 0;
            int Color_B = 0;
            int Color_A = 0;


            string[] ColorCodes = StringToConver.Split(',');

            Color_R = Int32.Parse(ColorCodes[0]);
            Color_G = Int32.Parse(ColorCodes[1]);
            Color_B = Int32.Parse(ColorCodes[2]);
            if (ColorAlphaOveride != -255) { Color_A = ColorAlphaOveride; } else { Color_A = Int32.Parse(ColorCodes[3]); }

            ColorToReturn = Color.FromNonPremultiplied(Color_R, Color_G, Color_B, Color_A);

            return ColorToReturn;
        }

        public static int Interpolate(int Input, int Source, float delta)
        {
            float smooth = (float)(Settings.AnimationSpeed * delta);
            return (int)MathHelper.SmoothStep(Source, Input, smooth);
        }

        /// <summary>
        /// Wraps the text.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="spriteFont">Sprite font.</param>
        /// <param name="text">Text.</param>
        /// <param name="maxLineWidth">Max line width.</param>
        public static string WrapText(SpriteFont spriteFont, string text, float maxLineWidth)
        {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);

                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }

            return sb.ToString();
        }

        //////////////////////////////////
        // Modified version for my needs
        // Credits:
        // http://web.archive.org/web/20190618131036/http://bluelinegamestudios.com/posts/drawstring-to-fit-text-to-a-rectangle-in-xna/
        static public void DrawStringBoundaries(SpriteBatch spriteBatch, SpriteFont font, string strToDraw, Rectangle boundaries, Color textColor, float rotation = 0.0f)
        {
            Vector2 size = font.MeasureString(strToDraw);

            float xScale = (boundaries.Width / size.X);
            float yScale = (boundaries.Height / size.Y);

            // Taking the smaller scaling value will result in the text always fitting in the boundaires.
            float scale = Math.Min(xScale, yScale);

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * scale);
            int strHeight = (int)Math.Round(size.Y * scale);
            Vector2 position = new Vector2();
            position.X = (((boundaries.Width - strWidth) / 2) + boundaries.X);
            position.Y = (((boundaries.Height - strHeight) / 2) + boundaries.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            // Draw the string to the sprite batch!
            spriteBatch.DrawString(font, strToDraw, position, textColor, rotation, spriteOrigin, scale, spriteEffects, spriteLayer);
        }

        public static Vector2 ClampVector(Vector2 value, int min, int max)
        {
            Vector2 ReturnVector = Vector2.Zero;
            ReturnVector.X = MathHelper.Clamp(value.X, min, max);
            ReturnVector.Y = MathHelper.Clamp(value.Y, min, max);
            return ReturnVector;
        }

        /// <summary>
        /// Gets the substring.
        /// </summary>
        /// <returns>The substring.</returns>
        /// <param name="Input">Input.</param>
        /// <param name="Spliter">Spliter.</param>
        public static string GetSubstring(string Input, char Spliter)
        {
            return Input.Substring(Input.IndexOf(Spliter) + 1, Input.LastIndexOf(Spliter) - 1).Replace(Spliter.ToString(), "");
        }

        public static bool IsAllDigits(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return true;
        }

        public static void ConsoleWriteWithTitle(string Title, string Text, bool OverrideDebugMode = false)
        {
            if (!Settings.DebugMode && !OverrideDebugMode) { return; }
            Console.WriteLine($"{Title} ; {Text}");
        }

        public static string SplitIntoLines(string Input, char pSplit)
        {
            string[] Splited = Input.Split(pSplit);
            string Result = "";

            foreach (var split in Splited)
            {
                Result += split + "\n";
            }

            return Result;

        }

        public static bool CheckKeyUp(KeyboardState oldState, KeyboardState newState, Microsoft.Xna.Framework.Input.Keys key)
        {
            return newState.IsKeyDown(key) && oldState.IsKeyUp(key);
        }

        public static bool CheckKeyDown(KeyboardState oldState, KeyboardState newState, Microsoft.Xna.Framework.Input.Keys key)
        {
            return newState.IsKeyDown(key) && oldState.IsKeyDown(key);
        }


    }


}