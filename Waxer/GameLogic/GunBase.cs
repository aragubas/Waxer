/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using Microsoft.Xna.Framework;

namespace Waxer.GameLogic
{
    public abstract class PlayerGunBase
    {
        public int Ammo;
        public int RecoilAmmount;
        public string InstanceID = Guid.NewGuid().ToString();
        public GameWorld ParentMap;
 
        public virtual void Shoot(Vector2 Direction, Vector2 Location)
        {
            Bullet newBullet = new Bullet(new Vector2(Location.X + 16, Location.Y + 16), ParentMap, InstanceID);
            newBullet.Direction = Direction;
            newBullet.Direction.Normalize();
             
            ParentMap.Entities.Add(newBullet);

        }
    }

    
}