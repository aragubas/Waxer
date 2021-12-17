using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.UserInterface
{
    public abstract class GUIRenderer
    {
        public List<DrawableGUI> Elements = new();

        internal void DrawElements(SpriteBatch spriteBatch)
        {
            foreach(DrawableGUI element in Elements)
            {
                element.Draw(spriteBatch);
            }
        } 

        public virtual void Draw(SpriteBatch spriteBatch)
        {
                        
        }

        public virtual void Update(float delta)
        {

        }

    }
}