/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

namespace Waxer.Animation
{
    public class ValueSmoother
    {
        public bool Running = false;
        public int TargetValue = 0;
        public int CurrentValue = 0;
        public float Speed = 28;
        public bool Ended = false;

        public ValueSmoother(float Speed, int TargetValue, int CurrentValue = 0)
        {
            this.Speed = Speed;
            this.TargetValue = TargetValue;
            this.CurrentValue = CurrentValue;
        }
 
        public int Update(float delta)
        {
            
            if (CurrentValue != TargetValue)
            { 
                if (TargetValue - CurrentValue > 0)
                {
                    CurrentValue += (int)(Speed * delta);
    
                    if (CurrentValue >= TargetValue)
                    {
                        CurrentValue = TargetValue;
                    }
                }else
                {
                    CurrentValue -= (int)(Speed * delta);
     
                    if (CurrentValue <= TargetValue)
                    {
                        CurrentValue = TargetValue;
                    }

                }
            }

            return CurrentValue;
        }
    }
}