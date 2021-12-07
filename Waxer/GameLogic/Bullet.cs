using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer.GameLogic
{
    public class Bullet : MapEntity
    {
        int _timer = 0;
        public int Timespan = 120;
        public Vector2 Direction;
        public float Speed = 8;
        public bool ShootingMode;
        public int Damage = 1;
        public string ShooterInstanceID;

        public Bullet(Vector2 initialPosition, Map parentMap, string ShooterInstanceID)
        {
            Position = initialPosition;
            ParentMap = parentMap;
            Texture = Graphics.Sprites.GetSprite("/bullet.png");
            this.ShooterInstanceID = ShooterInstanceID;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Texture.Bounds, BlendColor, (float)Math.Atan2(Direction.Y, Direction.X), new Vector2(Texture.Width / 2, Texture.Height / 2), Vector2.One, SpriteEffects.None, 1f);
            spriteBatch.DrawRectangle(Area, Color.Green);
        }

        public override void Update()
        {
            _timer++;
            if (_timer > Timespan) { ParentMap.Entities.Remove(this); return; }
            BlendColor.A--;
            
            Position += Direction * Speed;
            Area = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            if (Area.Intersects(ParentMap.player.Area))
            {
                Console.WriteLine("Sinas");
                ParentMap.player.BulletDamage(this);
            }

             
        }   
    }
     
}