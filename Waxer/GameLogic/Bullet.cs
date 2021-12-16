using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer.GameLogic
{
    public class Bullet : MapEntity
    {
        float Timer = 0;
        public int Timespan = 3;
        public Vector2 Direction;
        public float Speed = 128f;
        public int Damage = 1;
        public string ShooterInstanceID;
        MapTile TileUnder = null; 

        public Bullet(Vector2 initialPosition, Map parentMap, string ShooterInstanceID)
        {
            Position = initialPosition;
            ParentMap = parentMap;
            Texture = Graphics.Sprites.GetSprite("/bullet.png");
            this.ShooterInstanceID = ShooterInstanceID;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Texture.Bounds, BlendColor, (float)Math.Atan2(Direction.Y, Direction.X), Vector2.Zero, Vector2.One, SpriteEffects.None, 1f);
            spriteBatch.DrawRectangle(Area, Color.Green);
        }

        public override void Update(GameTime gameTime)
        {
            Timer += 1 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (Timer >= Timespan) { ParentMap.Entities.Remove(this); return; }
            
            Position += Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Area = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

            if (Area.Intersects(ParentMap.player.Area))
            {
                ParentMap.player.BulletDamage(this);
                Dispose();
                return;
            }


            try
            {
                TileUnder = ParentMap.GetTile(ParentMap.GetTilePosition(Position));

                if (TileUnder.IsColideable)
                { 
                    Dispose();
                }
                
            } catch(System.Collections.Generic.KeyNotFoundException)
            {
                TileUnder = null;
                Dispose();
            }

             
        }   
    }
     
}