using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Waxer.GameLogic
{
    public class PlayerEntity : MapEntity
    {
        public float moveSpeed = 2;
        public int PlayerLife = 300;
        KeyboardState oldState;
        Vector2 AimVector = Vector2.Zero;

        public PlayerEntity(Vector2 initialPosition, Map parentMap)
        {
            Position = initialPosition;
            ParentMap = parentMap;
        }
 
        public void CheckInput()
        {
            KeyboardState state = Keyboard.GetState();
            Vector2 moveVector = new Vector2();

            if (state.IsKeyDown(Keys.W))
            {
                moveVector.Y = -1;
            }

            if (state.IsKeyDown(Keys.S))
            {
                moveVector.Y = 1;
            }

            if (state.IsKeyDown(Keys.D))
            {
                moveVector.X = 1;
            }

            if (state.IsKeyDown(Keys.A))
            {
                moveVector.X = -1;
            }

            if (state.IsKeyDown(Keys.LeftShift))
            {
                moveSpeed += 0.3f;
            }


            if (moveVector.X > 1) { moveVector.X = 1; }
            if (moveVector.X < -1) { moveVector.X = -1; }
            if (moveVector.Y > 1) { moveVector.Y = 1; }
            if (moveVector.Y < -1) { moveVector.Y = -1; }

            Position += moveVector * moveSpeed; 

            if (state.IsKeyDown(Keys.Space) && oldState.IsKeyDown(Keys.Space))
            {
                Bullet newBullet = new Bullet(new Vector2(Position.X + 16, Position.Y + 16), ParentMap, InstanceID);
                newBullet.Direction = AimVector;
                newBullet.ParentMap = ParentMap;
                newBullet.Direction.Normalize();
                
                ParentMap.Entities.Add(newBullet);
            }

            oldState = state;
        }

        public void BulletDamage(Bullet bullet)
        {
            if (bullet.ShooterInstanceID == InstanceID) { return; }
            PlayerLife -= bullet.Damage;
            BlendColor.R = 255;
            BlendColor.G = 0;
            BlendColor.B = 0;
 
        }
  
        public override void Update()
        {
            base.Update();
            CheckInput();
            
            BlendColor.R = (byte)MathHelper.Lerp(BlendColor.R, 255, 0.2f);
            BlendColor.G = (byte)MathHelper.Lerp(BlendColor.G, 255, 0.2f);
            BlendColor.B = (byte)MathHelper.Lerp(BlendColor.B, 255, 0.2f);

            moveSpeed -= 0.01f;
            if (moveSpeed < 2) { moveSpeed = 2; }
            if (moveSpeed > 3) { moveSpeed = 3; }
             
            AimVector = -(Position - (MouseInput.PositionVector2 - ParentMap.camera.CameraPosition));
              
        }
    }
}