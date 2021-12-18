using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.GameLogic
{
    public abstract class WorldEntity : IDisposable
    {
        public Vector2 Position;
        public Vector2 ScreenPosition = Vector2.Zero;
        public string InstanceID = Guid.NewGuid().ToString();
        public Texture2D Texture = Graphics.Sprites.GetSprite("/missing_texture.png");
        public Color BlendColor = Color.White;
        public GameWorld World;
        internal Vector2 CameraPosition = Vector2.Zero;
        public Rectangle Area;
        public byte Opacity = 255;
        public Vector2 SpriteOrigin = Vector2.Zero;
        public float SpriteRotation = 0f;
        public Vector2 SpriteScale = Vector2.One;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public float SpriteLayerDepth = 1f;
        public Vector2 AreaSize = new Vector2(-1, -1);

        public void UpdateScreenPosition(Camera2D camera)
        {
            ScreenPosition = new Vector2(Position.X + camera.CameraPosition.X, Position.Y + camera.CameraPosition.Y);
            CameraPosition = camera.CameraPosition;
        }
        
        internal void UpdateArea()
        {
            if (AreaSize == new Vector2(-1, -1))
            {
                Area = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
                 
            }else
            {
                Area = new Rectangle((int)Position.X, (int)Position.Y, (int)AreaSize.X, (int)AreaSize.Y); 

            }
        }

        public virtual void Update(float delta) 
        { 
            UpdateArea();
        }

        public virtual void DoAction(string ActionType, WorldEntity entity)
        { 

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.FromNonPremultiplied(BlendColor.R, BlendColor.G, BlendColor.B, (int)(Opacity)), SpriteRotation, SpriteOrigin, SpriteScale, SpriteEffects, SpriteLayerDepth);
        }

        public void Dispose()
        {
            World.Entities.Remove(this);
        }

    }
}