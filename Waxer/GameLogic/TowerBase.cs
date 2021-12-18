using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Waxer.GameLogic
{
    public abstract class TowerBase : MapEntity
    {

        public override void Update(float delta)
        {
            Area = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);

        }

    }
}