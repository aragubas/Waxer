using System;
using Microsoft.Xna.Framework;

namespace Waxer.GameLogic
{
    public abstract class MapEntity
    {
        public Vector2 Position;
        public string InstanceID = Guid.NewGuid().ToString();

 
    }
}