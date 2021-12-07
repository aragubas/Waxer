using System;
using Microsoft.Xna.Framework;

namespace Waxer.GameLogic
{
    public abstract class PlayerGunBase
    {
        public int Ammo;
        public int RecoilAmmount;
        public string InstanceID = Guid.NewGuid().ToString();
        public Map ParentMap;
 
        public virtual void Shoot(Vector2 Direction, Vector2 Location)
        {
            Bullet newBullet = new Bullet(new Vector2(Location.X + 16, Location.Y + 16), ParentMap, InstanceID);
            newBullet.Direction = Direction;
            newBullet.Direction.Normalize();
             
            ParentMap.Entities.Add(newBullet);

        }
    }

    
}