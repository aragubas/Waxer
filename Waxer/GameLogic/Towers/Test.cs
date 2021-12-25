/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

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

        public Test(Vector2 InitialPosition, GameWorld parentMap)
        {
            Position = InitialPosition;
            Texture = Graphics.Sprites.GetSprite("/towers/test.png");
            World = parentMap;
        }
 
        private void Shoot()
        {
            Bullet newBullet = new Bullet(new Vector2(Position.X + 16, Position.Y + 16), World, InstanceID);
            newBullet.Direction = -(Position - (World.Player.Position));
            newBullet.Direction.Normalize();

            World.Entities.Add(newBullet);
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            ShootDelay -= 1 * delta;
            if (ShootDelay < 0) 
            { 
                ShootDelay = ShootingDelaySeconds; 
                CanShoot = true;
            }

            float Distance = Vector2.Distance(new Vector2(Position.X + 16, Position.Y - 16), new Vector2(World.Player.Position.X + 16, World.Player.Position.Y + 16));
            if (Distance <= 300 && CanShoot)
            {
                CanShoot = false;
                Shoot();
            }

            try
            {
                MapTile tileUnder = World.GetTile(World.GetTilePosition(Position) - -Vector2.UnitY);
                
                if (!tileUnder.TileInformation.IsColideable)
                {
                    Position.Y += (World.WorldEnvironment.Gravity * GravityMultiplier) * delta;

                    GravityMultiplier += 16f * delta;


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