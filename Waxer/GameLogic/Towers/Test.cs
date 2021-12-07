using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Waxer.GameLogic.Towers
{
    public class Test : TowerBase
    {
        int _shootDelay = 0;
        public int ShootingDelay = 15;
        
        public Test(Vector2 InitialPosition, Map parentMap)
        {
            Position = InitialPosition;
            Texture = Graphics.Sprites.GetSprite("/towers/test.png");
            ParentMap = parentMap;
        }

        private void Shoot()
        {
            Bullet newBullet = new Bullet(new Vector2(Position.X + 16, Position.Y + 16), ParentMap, InstanceID);
            newBullet.Direction = -(Position - (ParentMap.player.Position));
            newBullet.Direction.Normalize();

            ParentMap.Entities.Add(newBullet);
        }

        public override void Update()
        {
            base.Update();            
            _shootDelay--;
            if (_shootDelay < 0) { _shootDelay = 0; }

            float Distance = Vector2.Distance(new Vector2(Position.X + 16, Position.Y - 16), new Vector2(ParentMap.player.Position.X + 16, ParentMap.player.Position.Y + 16));
            if (Distance <= 300 && _shootDelay <= 0)
            {
                _shootDelay = ShootingDelay;
                Shoot();
            }
        }

    }
}