/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;

namespace Waxer.Animation
{
    class ColorFlasher
    {
        public bool FadeOut;
        float colorVal;
        bool direction = true;
        public float Speed = 255; // One flashe in a second
        public float MinimunValue = 0;
        public float MaximunValue = 255;

        public ColorFlasher(float Speed = 255, bool FadeOut = false)
        {
            this.Speed = Speed;
            this.FadeOut = FadeOut;
        }
        
        internal void Increase(float delta)
        {
            colorVal += Speed * delta;

            if (colorVal > MaximunValue || colorVal > 255) 
            { 
                colorVal = MaximunValue; 
                direction = false;
            }
        }

        internal void Decrease(float delta)
        {
            colorVal -= Speed * delta;
            
            if (colorVal < MinimunValue || colorVal < 0) 
            { 
                colorVal = MinimunValue;
                direction = true;
            }

        }

        public void Update(float delta)
        {
            if (!FadeOut)
            {
                if (direction)
                {
                    Increase(delta);
                    
                }else
                {
                    Decrease(delta);
                }

            }else
            {
                Decrease(delta);
            }
        }

        public byte GetColor()
        {
            return Convert.ToByte(colorVal);
        }
    }

}
