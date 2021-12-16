using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Waxer.GameLogic.Towers
{
    public class Test : TowerBase
    { 
        float ShootDelay = 0;
        public int ShootingDelaySeconds = 3;
        float GravityMultiplier;
        bool CanShoot = false;

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);            
            ShootDelay -= 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (ShootDelay < 0) 
            { 
                ShootDelay = ShootingDelaySeconds; 
                CanShoot = true;
            }

            float Distance = Vector2.Distance(new Vector2(Position.X + 16, Position.Y - 16), new Vector2(ParentMap.player.Position.X + 16, ParentMap.player.Position.Y + 16));
            if (Distance <= 300 && CanShoot)
            {
                CanShoot = false;
                Shoot();
            }

            try
            {
                MapTile tileUnder = ParentMap.GetTile(ParentMap.GetTilePosition(Position) - -Vector2.UnitY);
                
                if (!tileUnder.IsColideable)
                {
                    Position.Y += (ParentMap.MapEnvironment.Gravity * GravityMultiplier) * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    GravityMultiplier += 16f * (float)gameTime.ElapsedGameTime.TotalSeconds;


                }else  // Player hits the ground
                { 
                    GravityMultiplier = 0f; 
                }

            }catch(System.Collections.Generic.KeyNotFoundException)
            {
                Position = Vector2.Zero;
            }

        }

    }
}