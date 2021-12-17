using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Waxer.GameLogic
{
    public abstract class TowerBase : MapEntity
    {
        public string TowerDisplayName;
        bool Grabbed;
        

        private void UpdateGrab()
        {
            if (MouseInput.Left_DownClickPos.Intersects(new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, Area.Width, Area.Height)))
            {
                Grabbed = true;
            }

            if (Grabbed) 
            { 
                Position = new Vector2(MouseInput.Position.X - CameraPosition.X, MouseInput.Position.Y - CameraPosition.Y); 

                if (MouseInput.Left_UpClickPos != Rectangle.Empty)
                {
                    Grabbed = false;
                }
            }
        }

        public override void Update(float delta)
        {
            Area = new Rectangle((int)Position.X, (int)Position.Y, 32, 32);
            UpdateGrab();

        }

    }
}