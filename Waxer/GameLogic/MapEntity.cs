using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.GameLogic
{
    public abstract class MapEntity
    {
        public Vector2 Position = Vector2.Zero;
        public Vector2 ScreenPosition = Vector2.Zero;
        public string InstanceID = Guid.NewGuid().ToString();
        public Texture2D Texture = Graphics.Sprites.GetSprite("/missing_texture.png");
        public Color BlendColor = Color.White;
        public Map ParentMap;
        internal Vector2 CameraPosition = Vector2.Zero;
        public Rectangle Area;

        public void UpdateScreenPosition(Camera2D camera)
        {
            ScreenPosition = new Vector2(Position.X + camera.CameraPosition.X, Position.Y + camera.CameraPosition.Y);
            CameraPosition = camera.CameraPosition;
        }
         
        public virtual void Update() 
        { 
            Area = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); 
        }

        public virtual void DoAction(string ActionType, MapEntity entity)
        { 

        }

        public virtual void Draw(SpriteBatch spriteBatch) 
        { 
            spriteBatch.Draw(Texture, Position, BlendColor);
        }

    }
}