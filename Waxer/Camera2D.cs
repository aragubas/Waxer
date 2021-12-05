using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer
{
    public class Camera2D
    {
        public Vector2 CameraPosition = Vector2.Zero;
        public float Smoothness = 0.6f;
        Matrix transformMatrix ;
        Vector2 _animatedCameraPosition = Vector2.Zero;
        public Viewport viewport;

        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(_animatedCameraPosition.X, _animatedCameraPosition.Y, 0);
        }

        public bool IsOnScreen(Rectangle thisThing)
        { 
            bool LeftEdge = -thisThing.X - thisThing.Width > CameraPosition.X;
            bool BottomEdge = -thisThing.Y - thisThing.Height > CameraPosition.Y;
            bool RightEdge = -thisThing.X < (CameraPosition.X - viewport.Width);
            bool TopEdge = -thisThing.Y < (CameraPosition.Y - viewport.Height);
            
            return !LeftEdge && !BottomEdge && !RightEdge && !TopEdge;
        }
  
        public void CenterTo(Vector2 positionToCenter, Viewport viewport)
        {
            this.viewport = viewport;
            CameraPosition.X = viewport.Width / 2 - positionToCenter.X;
            CameraPosition.Y = viewport.Height / 2 - positionToCenter.Y;            
        } 

        public void Update()
        {
            _animatedCameraPosition.X = MathHelper.Lerp(CameraPosition.X, _animatedCameraPosition.X, Smoothness);
            _animatedCameraPosition.Y = MathHelper.Lerp(CameraPosition.Y, _animatedCameraPosition.Y, Smoothness);
 
        }

    }
}