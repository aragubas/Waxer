/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using System;
using Microsoft.Xna.Framework;

namespace Waxer.GameLogic
{
    public class RigidBody : WorldEntity
    {
        public float MaximumSpeed = 196f;
        public float JumpMultiplier = 1f;
        public float MoveSpeed = 78f;
        public float Acceleration = 1.8f;
        public float JumpHeight = 14f;

        internal float _gravityMultiplier = 0f;
        internal bool _isJumping = false;
        internal bool _jumpAvailable = true;
        internal float _jumpProgress = 0f;
        internal float _jumpAccumulatedForce = 0f; 
        internal bool _jumpEnding = false;
        internal float _lastFriction = 0f;
        internal float _moveAcceleration = 0f;
        internal MapTile _tileUnderCursor = null;         
        internal MapTile _tileBehind = null;
        internal string _ceiraText = "";
        internal float _deaceleration = 0;
        internal float _lastDelta = 0.0f;
        internal float _movePosition;


        // Movement
        // Update MoveSpeed curve
        //
        void UpdateMoveSpeed(float delta)
        {
            _moveAcceleration -= _deaceleration;
            
            if (_moveAcceleration < 0) { _moveAcceleration = 0; }
            if (_moveAcceleration > MaximumSpeed) { _moveAcceleration = MaximumSpeed; }

            LimitPhysicsSpeed();
        } 

        void StopJumping()
        { 
            _jumpEnding = true; 
        }

        // Physics
        // Get tiles behind the player and under cursor
        //
        void GetTilesAround(Vector2 Position)
        {
            // Tile Under Cursor
            MapTile newTile = World.GetTile(World.GetTilePosition(MouseInput.PositionVector2 - World.Camera.CameraPosition));
            if (newTile != null) { _tileUnderCursor = newTile; }

            // Tile Behind
            newTile = World.GetTile(World.GetTilePosition(Position));
            if (newTile != null) { _tileBehind = newTile; } 
        }
        

        void PlayerHitGround()
        {
            _gravityMultiplier = 0; 
            EndJumping();
            _jumpAvailable = true;

        }
          
        internal void EndJumping()
        {
            _isJumping = false;
            JumpMultiplier = 0;
            _jumpProgress = 0;
            _jumpAccumulatedForce = 0; 
            _jumpEnding = false;
        }
  
        internal void BeginJump()
        {
            _isJumping = true;
            _jumpAvailable = false;
            _jumpProgress = 4;
             
        }
 
        // Update the jump, or should i call it... Up force?
        void UpdateJump(float delta)
        {
            bool isColiding = false;
            float Force = _jumpProgress * delta;
            _ceiraText += $"\nJump Force {Force.ToString()}\nJumping: {_isJumping}\nAccumulated Force: {_jumpAccumulatedForce}"; 
            
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
            
            _ceiraText += $"\nJump Progress: {_jumpProgress}\nJump Ending: {_jumpEnding}";
            
            if (_isJumping)
            {
                // Ending jumping if colliding
                if (isColiding) { EndJumping(); return; }

                _jumpProgress = _deaceleration * JumpHeight;

                // Update player position
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
                _lastFriction = TileBottom.TileInformation.Friction;
                PlayerHitGround();
            }

            // DEBUG: Debug Text
            _ceiraText += $"\nGravityMultiplier: {_gravityMultiplier}";

            if (!isColiding)
            { 
                Position.Y += Force;
                UpdateArea();

                // Increase Gravity Multiplier
                _gravityMultiplier += 16 * delta; 

                _lastFriction = 0;
            }

            if (_isJumping && isColiding) 
            { 
                _isJumping = false; 
            }

        }

        internal void LimitPhysicsSpeed()
        {
            // Physics calculation maximum move speed
            if (_moveAcceleration > Settings.MaximumMoveSpeed) { _moveAcceleration = Settings.MaximumMoveSpeed; } 
   
        }

        // Update left-right forces for movement
        void UpdateAppliedForces(float delta)
        {
            LimitPhysicsSpeed();
            
            _movePosition = Math.Clamp(_movePosition, -1, 1);
            float ForceMultiplied = _movePosition * _moveAcceleration;
            
            Vector2 NextStep = Position + new Vector2(ForceMultiplied * delta, 0);
            
            // Right colision
            for(int i = 0; i < 16; i++)
            {
                MapTile rightTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(i, 0)));
                Rectangle rightArea = new Rectangle(((int)Position.X + 15) + i, (int)Position.Y, Area.Width, Area.Height);

                MapTile rightBottomTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(i, i)));
                Rectangle rightBottomArea = new Rectangle(((int)Position.X + 15) + i, (int)Position.Y + i, Area.Width, Area.Height);

                bool RightColision = rightTile.TileInformation.IsColideable && rightTile.GetArea().Intersects(rightArea);
                bool RightBottomColision = rightBottomTile.TileInformation.IsColideable && rightBottomTile.GetArea().Intersects(rightBottomArea);
  
                if (RightColision || RightBottomColision)
                {
                   NextStep = new Vector2(rightTile.GetArea().X - 16, NextStep.Y);
                   break; 
                }
            } 

            // Left colision
            for(int i = 0; i < 16; i++)
            {
                MapTile leftTile = World.GetTile(World.GetTilePosition(NextStep - new Vector2(i, 0)));
                Rectangle leftArea = new Rectangle(((int)Position.X - 15) - i, (int)Position.Y, Area.Width, Area.Height);

                MapTile leftBottomTile = World.GetTile(World.GetTilePosition(NextStep - new Vector2(i, -i)));
                Rectangle leftBottomArea = new Rectangle(((int)Position.X - 15) - i, (int)Position.Y + i, Area.Width, Area.Height);
                
                bool LeftColision = leftTile.TileInformation.IsColideable && leftTile.GetArea().Intersects(leftArea);
                bool LeftBottomColision = leftBottomTile.TileInformation.IsColideable && leftBottomTile.GetArea().Intersects(leftBottomArea);
 
                if (LeftColision || LeftBottomColision)
                { 
                   NextStep = new Vector2(leftTile.GetArea().Right + 16, NextStep.Y);
                   break;
                } 
            }

             
            
            Position = NextStep; 
        }


        internal void UpdateBody(float delta)
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
 
            UpdateJump(delta); 
            UpdateGravity(delta); 
            UpdateMoveSpeed(delta);
 
            GetTilesAround(Position);
            if (!_tileBehind.TileInformation.IsColideable) { UpdateAppliedForces(delta); }

        }

    }
}