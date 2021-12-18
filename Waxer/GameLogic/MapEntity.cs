using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.GameLogic
{
    public abstract class MapEntity : IDisposable
    {
        public Vector2 Position;
        public Vector2 ScreenPosition = Vector2.Zero;
        public string InstanceID = Guid.NewGuid().ToString();
        public Texture2D Texture = Graphics.Sprites.GetSprite("/missing_texture.png");
        public Color BlendColor = Color.White;
        public GameWorld World;
        internal Vector2 CameraPosition = Vector2.Zero;
        public Rectangle Area;
        public float Opacity = 1f;
        public Vector2 SpriteOrigin;
        public float SpriteRotation;
        public Vector2 SpriteScale = Vector2.One;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public float SpriteLayerDepth = 1f;
        
        public void UpdateScreenPosition(Camera2D camera)
        {
            ScreenPosition = new Vector2(Position.X + camera.CameraPosition.X, Position.Y + camera.CameraPosition.Y);
            CameraPosition = camera.CameraPosition;
        }
        
        internal void UpdateArea()
        {
            Area = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public virtual void Update(float delta) 
        { 
            UpdateArea();
        }

        public virtual void DoAction(string ActionType, MapEntity entity)
        { 

        }

        public virtual void Draw(SpriteBatch spriteBatch) 
        { 
            BlendColor.A = (byte)(Opacity * 255);
            spriteBatch.Draw(Texture, Position, null, BlendColor, SpriteRotation, SpriteOrigin, SpriteScale, SpriteEffects, SpriteLayerDepth);
        }

        public void Dispose()
        {
            World.Entities.Remove(this);
        }

    }
}