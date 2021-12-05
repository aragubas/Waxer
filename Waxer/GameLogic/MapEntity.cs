using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.GameLogic
{
    public abstract class MapEntity
    {
        public Vector2 Position = Vector2.Zero;
        public string InstanceID = Guid.NewGuid().ToString();
        public Texture2D Texture = Graphics.Sprites.GetSprite("/missing_texture.png");
 
    }
}