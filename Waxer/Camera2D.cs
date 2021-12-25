/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Waxer.Animation;

namespace Waxer
{
    public class Camera2D
    {
        public Vector2 CameraPosition = Vector2.Zero;
        public float Smoothness = 0.2f;
        Vector2 _animatedCameraPosition = Vector2.Zero;
        public Viewport viewport;
 
        public Matrix GetMatrix()
        {
            return Matrix.CreateTranslation(CameraPosition.X, CameraPosition.Y, 0);
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
 
    }
}