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
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Waxer.GameLogic
{
    public abstract class ControlableCharacter : RigidBody
    {
        public int Life = 300;
        private KeyboardState _oldState;
        internal Vector2 _aimVector = Vector2.Zero;
 

        #region DEBUG AREA
        // == DEBUG AREA ==
        // Render tile infos on screen
        //
        internal void RenderTileInfos(SpriteBatch spriteBatch, MapTile tile, string TileName)
        {
            Vector2 FixedPosition = new Vector2(((int)tile.TilePosition.X * 32) + (int)World.Camera.CameraPosition.X, 
                ((int)tile.TilePosition.Y * 32) + (int)World.Camera.CameraPosition.Y);

            Color sinasColor = Color.Red;
            sinasColor.A = (byte)30;

            spriteBatch.DrawRectangle(new Rectangle((int)FixedPosition.X, (int)FixedPosition.Y, 32, 32), sinasColor);

            // Draw tile cordinates
            spriteBatch.DrawString(World.DebugFont, $"X:{tile.TilePosition.X}\n" + 
                                                        $"Y:{tile.TilePosition.Y}\n{TileName}", FixedPosition, sinasColor);
        }

        // == DEBUG AREA ==
        // Draw debug stuff on screen
        //
        internal void DrawDebug(SpriteBatch spriteBatch)
        {
            if (!Settings.DebugMode) { return; }
            // Draw all the surronding colision check tiles
            if (Settings.Debug_RenderColidersTiles)
            {
                /*
                if (TileBehind != null)
                {
                    RenderTileInfos(spriteBatch, TileBehind, "BEHI");
                }
                 
                if (TileBottom != null)
                {
                    RenderTileInfos(spriteBatch, TileBottom, "BOTT");
                }
                            
                if (TileLeft != null)
                {
                    RenderTileInfos(spriteBatch, TileLeft, "LEFT");
                }
                            
                if (TileRight != null)
                {
                    RenderTileInfos(spriteBatch, TileRight, "RIGH");
                }
                            
                if (TileTop != null)
                {
                    RenderTileInfos(spriteBatch, TileTop, "TOP");
                }
                            
                if (TileTopLeft != null)
                {
                    RenderTileInfos(spriteBatch, TileTopLeft, "TOPL");
                }
                            
                if (TileTopRight != null)
                {
                    RenderTileInfos(spriteBatch, TileTopRight, "TOPR");
                }
                            
                if (TileBottomLeft != null)
                {
                    RenderTileInfos(spriteBatch, TileBottomLeft, "BOTL");
                }
                            
                if (TileBottomRight != null)
                {
                    RenderTileInfos(spriteBatch, TileBottomRight, "BOTR");
                }
                */
            }

            // Draw last delta
            spriteBatch.DrawString(World.DebugFont, $"lr_dir: {_movePosition.ToString()}\nmove_acceleration: {MoveSpeed * _floorSpeedAcceleration}\ndelta: {_lastDelta}\ndeacceleration: {_deaceleration}\n{_ceiraText}", new Vector2(12, 180), Color.Red);
            _ceiraText = "";

            // Hightlights the player position    
            if (Settings.Debug_RenderPlayerPositionPoints)
            {
                spriteBatch.DrawPoint(Position + World.Camera.CameraPosition, Color.Green, 2);
                spriteBatch.DrawPoint((Position + new Vector2(16, 0)) + World.Camera.CameraPosition, Color.Blue, 2);
                spriteBatch.DrawPoint((Position - new Vector2(16, 0)) + World.Camera.CameraPosition, Color.Yellow, 2);
            }
            
            // Draws information about the tile under the mouse cursor
            if (_tileUnderCursor != null)
            {                  
                Vector2 FixedPosition = new Vector2(((int)_tileUnderCursor.TilePosition.X * 32) + (int)World.Camera.CameraPosition.X, 
                    ((int)_tileUnderCursor.TilePosition.Y * 32) + (int)World.Camera.CameraPosition.Y);

                // Draw tile cordinates
                spriteBatch.DrawString(World.DebugFont, $"X:{_tileUnderCursor.TilePosition.X}\n" + 
                                                        $"Y:{_tileUnderCursor.TilePosition.Y}", FixedPosition, Color.Black);

            }

 
        }

        #endregion


 
        void UpdateInput(float delta)
        {
            KeyboardState newState = Keyboard.GetState();
            int MovePos = 0;
 
            if (Utils.CheckKeyDown(_oldState, newState, Keys.D))
            {
                MovePos = 1;
            }

            if (Utils.CheckKeyDown(_oldState, newState, Keys.A))
            {
                MovePos = -1;
            }
 
            if (Utils.CheckKeyUp(_oldState, newState, Keys.F1))
            {
                EndJumping(); 
            }

            if (Utils.CheckKeyDown(_oldState, newState, Keys.W))
            {
                if (_jumpAvailable)
                {
                    BeginJump();
                }
            }else 
            {  
                // Instead of disabling jump 
                if (_isJumping) 
                { 
                    EndJumping();
                }
            }
 
            if (Utils.CheckKeyDown(_oldState, newState, Keys.LeftShift))
            {
                _moveAccel = Acceleration; 
            
            }else 
            { 
                _moveAccel = 0;
            }
  
            _movePosition += MovePos;
 
            _oldState = newState;
        }

        // 
        // Update pipeline for a Controlable Character
        //
        internal void UpdateChracter(float delta)
        {
            
            if (BlendColor.G < 255)
            {
                BlendColor.G += Convert.ToByte((2f * delta * 255));
            }            

            if (BlendColor.B < 255)
            {
                BlendColor.B += Convert.ToByte((2f * delta * 255));
            }            
            
            _aimVector = -(Position - (MouseInput.PositionVector2 - World.Camera.CameraPosition));

            UpdateInput(delta);

            UpdateBody(delta);

            _lastDelta = delta;

        }

    }

}
