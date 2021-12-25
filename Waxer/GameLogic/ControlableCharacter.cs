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
    public abstract class ControlableCharacter : WorldEntity
    {
        public float MoveSpeed = 64f;
        public float MinimumSpeed = 64f;
        public float Acceleration = 8f;
        public float JumpMultiplier = 12f;
        public int Life = 300;
        private KeyboardState _oldState;
        internal Vector2 _aimVector = Vector2.Zero;
        internal float _gravityMultiplier = 0f;
        internal bool _isJumping = false;
        internal bool _jumpAvailable = true;
        internal float _jumpProgress = 0f;
        internal float _jumpSpeed = 0f;
        internal float _jumpForce = 0f; 
        internal MapTile _tileUnderCursor = null;         
        internal MapTile _tileBehind = null;
        internal MapTile _tileRight = null;
        internal MapTile _tileLeft = null;
        internal MapTile _tileTop = null;
        internal MapTile _tileBottom = null;
        internal MapTile _tileTopLeft = null;
        internal MapTile _tileTopRight = null;
        internal MapTile _tileBottomLeft = null;
        internal MapTile _tileBottomRight = null;
        internal MapTile _sinasTile = null;
        internal Rectangle _sinasRect = Rectangle.Empty; 
        internal MapTile _ceiraTile = null;
        internal Rectangle _ceiraRect = Rectangle.Empty; 
        internal string _ceiraText = "";

        internal float _lastDelta = 0.0f;

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
                 
                if (_sinasTile != null)
                {
                    spriteBatch.FillRectangle(new Rectangle(_sinasRect.X + (int)World.Camera.CameraPosition.X, _sinasRect.Y + (int)World.Camera.CameraPosition.Y, _sinasRect.Width, _sinasRect.Height), Color.Yellow); 
                    RenderTileInfos(spriteBatch, _sinasTile, "BOTT");

                    _sinasTile = null;
                    _sinasRect = Rectangle.Empty;
                }
                 
                if (_ceiraTile != null)
                {
                    spriteBatch.FillRectangle(new Rectangle(_ceiraRect.X + (int)World.Camera.CameraPosition.X, _ceiraRect.Y + (int)World.Camera.CameraPosition.Y, _ceiraRect.Width, _ceiraRect.Height), Color.Orange); 
                    RenderTileInfos(spriteBatch, _ceiraTile, "TOP");

                    _ceiraTile = null;
                    _ceiraRect = Rectangle.Empty;
                }


            }

            // Draw last delta
            spriteBatch.DrawString(World.DebugFont, $"force_dir: {Force.ToString()}\nforce_multiplied: {(Force * MoveSpeed).ToString()}\nmove_speed: {MoveSpeed}\ndelta: {_lastDelta}\n{_ceiraText}", new Vector2(12, 180), Color.Red);
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

        // Movement
        // Update MoveSpeed curve
        //
        void UpdateMoveSpeed(float delta)
        {
            MoveSpeed -= 38 * delta;
            if (MoveSpeed < MinimumSpeed) { MoveSpeed = MinimumSpeed; }
            if (MoveSpeed > 2048) { MoveSpeed = 2048; }
        }


        // Physics
        // Get all tiles around the player, including all quadrants
        //
        void GetTilesAround(Vector2 Position)
        {
            // Tile Under Cursor
            MapTile newTile = World.GetTile(World.GetTilePosition(MouseInput.PositionVector2 - World.Camera.CameraPosition));
            if (newTile != null) { _tileUnderCursor = newTile; }

            // Tile Behind
            newTile = World.GetTile(World.GetTilePosition(Position));
            if (newTile != null) { _tileBehind = newTile; }

            // Tile Top
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(0, -1));
            if (newTile != null) { _tileTop = newTile; }

            // Tile Bottom
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(0, 1));
            if (newTile != null) { _tileBottom = newTile; }

            // Tile Left
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(-1, 0));
            if (newTile != null) { _tileLeft = newTile; }

            // Tile Right
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(1, 0));
            if (newTile != null) { _tileRight = newTile; }

            // Corner Edges Tiles

            // Tile TopLeft
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(-1, -1));
            if (newTile != null) { _tileTopLeft = newTile; }

            // Tile TopRight
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(1, -1));
            if (newTile != null) { _tileTopRight = newTile; }

            // Tile BottomLeft
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(-1, 1));
            if (newTile != null) { _tileBottomLeft = newTile; }

            // Tile BottomRight 
            newTile = World.GetTile(_tileBehind.TilePosition + new Vector2(1, 1));
            if (newTile != null) { _tileBottomRight = newTile; }
 
        }
        
        void UpdateInput(float delta)
        {
            KeyboardState newState = Keyboard.GetState();
            Vector2 NextPos = Vector2.Zero;

            if (Utils.CheckKeyDown(_oldState, newState, Keys.D))
            {
                NextPos.X = 1f;
            }

            if (Utils.CheckKeyDown(_oldState, newState, Keys.A))
            {
                NextPos.X = -1f;
            }
 
            if (Utils.CheckKeyUp(_oldState, newState, Keys.F1))
            {
                MoveSpeed = 64;
                EndJumping(); 
            }
 
            if (Utils.CheckKeyDown(_oldState, newState, Keys.W) || Utils.CheckKeyDown(_oldState, newState, Keys.Space))
            {
                if (_jumpAvailable)
                {
                    SetUpJump();
                }
            }

            if (Utils.CheckKeyDown(_oldState, newState, Keys.LeftShift))
            {
                MoveSpeed += MoveSpeed * Acceleration * delta;
            }

            Force = NextPos;

            _oldState = newState;
        }

        Vector2 Force;

        void PlayerHitGround()
        {
            _gravityMultiplier = 0; 
            EndJumping();
            _jumpAvailable = true;

        }
          
        void EndJumping()
        {
            _isJumping = false;
            JumpMultiplier = 0;
            _jumpProgress = 0;
        }
 
        void SetUpJump()
        {
            _isJumping = true;
            _jumpAvailable = false;
            _jumpProgress = 12f;
        }

        // Update the jump, or should i call it... Up force?
        void UpdateJump(float delta)
        {
            bool isColiding = false;            
            float Force = (World.WorldEnvironment.Gravity * _jumpProgress) * delta;
            _ceiraText += $"\nForce {Force.ToString()}\nJumping: {_isJumping}"; 
    
            // Top colision detection
            Vector2 nextPosition = new Vector2(Position.X, Position.Y - Force);
            Rectangle TopArea = new Rectangle((int)Position.X, (int)nextPosition.Y, Area.Width, Area.Height);
            Rectangle TopLeftArea = new Rectangle((int)Position.X - 15, (int)nextPosition.Y, Area.Width, Area.Height);
            Rectangle TopRightArea = new Rectangle((int)Position.X + 15, (int)nextPosition.Y, Area.Width, Area.Height);
     
            MapTile TopLeftTile = World.GetTile(World.GetTilePosition(nextPosition - new Vector2(15, 1)));
            MapTile TopTile = World.GetTile(World.GetTilePosition(nextPosition - new Vector2(0, 1)));
            MapTile TopRightTile = World.GetTile(World.GetTilePosition(nextPosition - new Vector2(-15, 1)));

            bool TopLeftColiding = TopLeftTile.TileInformation.IsColideable && TopLeftTile.GetArea().Bottom <= Position.Y;
            bool TopColiding = TopTile.TileInformation.IsColideable && TopTile.GetArea().Bottom <= Position.Y;
            bool TopRightColiding = TopRightTile.TileInformation.IsColideable && TopRightTile.GetArea().Bottom <= Position.Y;
            

            isColiding = TopLeftColiding || TopRightColiding || TopColiding;

            if (isColiding && _isJumping)
            { 
                _sinasTile = TopTile;
                _sinasRect = TopArea;

                int newY = 0;

                if (TopLeftColiding)
                {
                    newY = TopLeftTile.GetArea().Bottom;
                }
                if (TopColiding)
                {
                    newY = TopTile.GetArea().Bottom;
                }
                if (TopRightColiding)
                {
                    newY = TopRightTile.GetArea().Bottom;
                }
        
                Position.Y = newY + 1;
                EndJumping();
                return;
            }
   
            if (_isJumping)
            {
                // Ending jumping if colliding
                if (isColiding) { EndJumping(); return; }
   
                _jumpProgress += JumpMultiplier;
    
                if (_jumpProgress > 16) { _jumpProgress = 16; }  
 
                Position.Y -= Force;
            }

        }

        // Update the constant down force, also called gravity
        void UpdateGravity(float delta)
        {
            
            float Force = (World.WorldEnvironment.Gravity * _gravityMultiplier) * delta;
            bool isColiding = false;
             
            // Detect bottom colision
            Rectangle BottomAreaRight = new Rectangle((int)Position.X + 15, (int)Position.Y, Area.Width,Area.Height);
            Rectangle BottomArea = new Rectangle((int)Position.X, (int)Position.Y, Area.Width,Area.Height);
            Rectangle BottomAreaLeft = new Rectangle((int)Position.X - 15, (int)Position.Y , Area.Width,Area.Height);

            MapTile TileBottomRight = World.GetTile(World.GetTilePosition(Position + new Vector2(15, 32)));
            MapTile TileBottom = World.GetTile(World.GetTilePosition(Position + new Vector2(0, 32)));
            MapTile TileBottomLeft = World.GetTile(World.GetTilePosition(Position + new Vector2(-15, 32)));

            bool BottomTileColision = TileBottom.TileInformation.IsColideable && BottomArea.Y <= TileBottom.GetArea().Y;
            bool BottomLeftTileColision = TileBottomLeft.TileInformation.IsColideable && BottomAreaLeft.Y <= TileBottomLeft.GetArea().Y;
            bool BottomRightTileColision = TileBottomRight.TileInformation.IsColideable && BottomAreaRight.Y <= TileBottomRight.GetArea().Y;

            isColiding = BottomTileColision || BottomLeftTileColision || BottomRightTileColision;
 
            if (isColiding && !_isJumping)
            { 
                int newY = 0;

                if (BottomTileColision)
                {
                    newY = TileBottom.GetArea().Y - 32;
                } 
                if (BottomLeftTileColision)
                {
                    newY = TileBottomLeft.GetArea().Y - 32;
                }
                if (BottomRightTileColision)
                {
                    newY = TileBottomRight.GetArea().Y - 32;
                } 

                
                
                Position.Y = newY;
                PlayerHitGround();
            }

            if (!isColiding) 
            {
                Position.Y += Force;
                UpdateArea();
 
                _gravityMultiplier += 16f * delta;

            }
 
            if (_isJumping && isColiding) { _isJumping = false; }

        }

        // Update left-right forces for movement
        void UpdateAppliedForces(float delta)
        {
 
            Force = Utils.ClampVector(Force, -1, 1); 
            Vector2 ForceMultiplied = Force * MoveSpeed;
            Vector2 NextStep = Position + (ForceMultiplied * delta);
  
  
            // Right colision
            for(int i = 0; i < 16; i++)
            {
                MapTile rightTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(i, 0)));
                Rectangle rightArea = new Rectangle(((int)Position.X + 16) + i, (int)Position.Y, Area.Width, Area.Height);
 
                if (rightTile.TileInformation.IsColideable && rightTile.GetArea().Intersects(rightArea))
                {
                   NextStep = new Vector2(rightTile.GetArea().X - 16, NextStep.Y);
                   break;
                }
            } 

            // Left colision
            for(int i = 0; i < 16; i++)
            {
                MapTile leftTile = World.GetTile(World.GetTilePosition(NextStep - new Vector2(i, 0)));
                Rectangle leftArea = new Rectangle(((int)Position.X - 16) - i, (int)Position.Y, Area.Width, Area.Height);

                if (leftTile.TileInformation.IsColideable && leftTile.GetArea().Intersects(leftArea))
                { 
                   NextStep = new Vector2(leftTile.GetArea().Right + 16, NextStep.Y);
                   break;
                } 
            }

             
            
            Position = NextStep; 
        }

        // 
        // Update pipeline for a Controlable Character
        //
        internal void UpdateChracter(float delta)
        {
 
            // Fix Crashing when player position is less than 0
            if (Position.X < 0) 
            { 
                Position.X = 0; UpdateArea(); 
            }
            if (Position.Y < 0) 
            { 
                Position.Y = 0; UpdateArea(); 
            }
            
            if (BlendColor.G < 255)
            {
                BlendColor.G += Convert.ToByte((2f * delta * 255));
            }            

            if (BlendColor.B < 255)
            {
                BlendColor.B += Convert.ToByte((2f * delta * 255));
            }            
            
            _aimVector = -(Position - (MouseInput.PositionVector2 - World.Camera.CameraPosition));

            GetTilesAround(Position);
     
            UpdateInput(delta);
            UpdateMoveSpeed(delta);
            if (!_tileBehind.TileInformation.IsColideable) { UpdateAppliedForces(delta); }
            UpdateJump(delta);
            UpdateGravity(delta); 

            _lastDelta = delta;

        }

    }

}