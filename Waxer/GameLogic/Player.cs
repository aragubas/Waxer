using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Waxer.GameLogic
{
    public class PlayerEntity : MapEntity
    {
        public float moveSpeed = 2;
          
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.DrawRectangle(new Rectangle((int)Position.X, (int)Position.Y, 32, 32), Color.Red);
        }

        public void CheckInput()
        {
            KeyboardState newState = Keyboard.GetState();
            Vector2 moveVector = new Vector2();

            if (newState.IsKeyDown(Keys.W))
            {
                moveVector.Y = -1;
            }

            if (newState.IsKeyDown(Keys.S))
            {
                moveVector.Y = 1;
            }

            if (newState.IsKeyDown(Keys.D))
            {
                moveVector.X = 1;
            }

            if (newState.IsKeyDown(Keys.A))
            {
                moveVector.X = -1;
            }

            if (newState.IsKeyDown(Keys.LeftShift))
            {
                moveSpeed += 0.3f;
            }


            if (moveVector.X > 1) { moveVector.X = 1; }
            if (moveVector.X < -1) { moveVector.X = -1; }
            if (moveVector.Y > 1) { moveVector.Y = 1; }
            if (moveVector.Y < -1) { moveVector.Y = -1; }

            Position += moveVector * moveSpeed; 

        }

        public void Update()
        {
            CheckInput();
            
            moveSpeed -= 0.01f;
            if (moveSpeed < 2) { moveSpeed = 2; }
            if (moveSpeed > 3) { moveSpeed = 3; }
            
        }
    }
}