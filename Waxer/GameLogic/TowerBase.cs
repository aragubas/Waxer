/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Waxer.GameLogic
{
    public abstract class TowerBase : WorldEntity
    {

        public override void Update(float delta)
        {
            Area = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);

        }

    }
}