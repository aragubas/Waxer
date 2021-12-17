using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Waxer.Graphics;

namespace Waxer.GameLogic
{

    public abstract class Item
    {
        public string Name;
        public string Description;
        public bool Stackable = true;
        public int StackSize = 16;
    }

    public abstract class Tool : Item
    {
        // Tools can always only be stackable up to 8 tools
        public new readonly int StackSize = 8;
        public Texture2D ToolTexture = Sprites.MissingTexture;
        public int RangeOfUse = 12;

        public bool IsInRange(Vector2 PlayerPosition, Vector2 UsedInPosition)
        {
            return Vector2.Distance(PlayerPosition, UsedInPosition) <= RangeOfUse;
        }

        public virtual void DoAction(object ActionContext = null)
        {
            
        }

    }
}