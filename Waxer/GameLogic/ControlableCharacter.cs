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
        public float Acceleration = 10f;
        public float JumpMultiplier = 12f;
        public int Life = 300;
        private KeyboardState oldState;
        internal Vector2 AimVector = Vector2.Zero;
        internal float GravityMultiplier = 0f;
        internal bool IsJumping = false;
        internal bool JumpAvailable = true;
        internal float JumpProgress = 0f;
        internal float JumpSpeed = 0f;
        internal float JumpForce = 0f; 
        internal MapTile TileUnderCursor = null;         
        internal MapTile TileBehind = null;
        internal MapTile TileRight = null;
        internal MapTile TileLeft = null;
        internal MapTile TileTop = null;
        internal MapTile TileBottom = null;
        internal MapTile TileTopLeft = null;
        internal MapTile TileTopRight = null;
        internal MapTile TileBottomLeft = null;
        internal MapTile TileBottomRight = null;
        internal MapTile SinasTile = null;
        internal Rectangle SinasRect = Rectangle.Empty; 
        internal MapTile CeiraTile = null;
        internal Rectangle CeiraRect = Rectangle.Empty; 
 
        internal float LastDelta = 0.0f;

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
                 
                if (SinasTile != null)
                {
                    spriteBatch.FillRectangle(new Rectangle(SinasRect.X + (int)World.Camera.CameraPosition.X, SinasRect.Y + (int)World.Camera.CameraPosition.Y, SinasRect.Width, SinasRect.Height), Color.Yellow); 
                    RenderTileInfos(spriteBatch, SinasTile, "TOP");

                    SinasTile = null;
                    SinasRect = Rectangle.Empty;
                }
                 
                /*
                if (CeiraTile != null)
                {
                    spriteBatch.FillRectangle(new Rectangle(CeiraRect.X + (int)World.Camera.CameraPosition.X, CeiraRect.Y + (int)World.Camera.CameraPosition.Y, CeiraRect.Width, CeiraRect.Height), Color.Orange); 
                    RenderTileInfos(spriteBatch, CeiraTile, "TOP");
                }
                */


            }

            // Draw last delta
            spriteBatch.DrawString(World.DebugFont, $"force_dir: {Force.ToString()}\nforce_multiplied: {(Force * MoveSpeed).ToString()}\nmove_speed: {MoveSpeed}\ndelta: {LastDelta}", new Vector2(12, 180), Color.Red);
 
            // Hightlights the player position    
            if (Settings.Debug_RenderPlayerPositionPoints)
            {
                spriteBatch.DrawPoint(Position + World.Camera.CameraPosition, Color.Green, 2);
                spriteBatch.DrawPoint((Position + new Vector2(16, 0)) + World.Camera.CameraPosition, Color.Blue, 2);
                spriteBatch.DrawPoint((Position - new Vector2(16, 0)) + World.Camera.CameraPosition, Color.Yellow, 2);
            }
            
            // Draws information about the tile under the mouse cursor
            if (TileUnderCursor != null)
            {                  
                Vector2 FixedPosition = new Vector2(((int)TileUnderCursor.TilePosition.X * 32) + (int)World.Camera.CameraPosition.X, 
                    ((int)TileUnderCursor.TilePosition.Y * 32) + (int)World.Camera.CameraPosition.Y);

                // Draw tile cordinates
                spriteBatch.DrawString(World.DebugFont, $"X:{TileUnderCursor.TilePosition.X}\n" + 
                                                            $"Y:{TileUnderCursor.TilePosition.Y}", FixedPosition, Color.Black);

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
            if (newTile != null) { TileUnderCursor = newTile; }

            // Tile Behind
            newTile = World.GetTile(World.GetTilePosition(Position));
            if (newTile != null) { TileBehind = newTile; }

            // Tile Top
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(0, -1));
            if (newTile != null) { TileTop = newTile; }

            // Tile Bottom
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(0, 1));
            if (newTile != null) { TileBottom = newTile; }

            // Tile Left
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(-1, 0));
            if (newTile != null) { TileLeft = newTile; }

            // Tile Right
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(1, 0));
            if (newTile != null) { TileRight = newTile; }

            // Corner Edges Tiles

            // Tile TopLeft
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(-1, -1));
            if (newTile != null) { TileTopLeft = newTile; }

            // Tile TopRight
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(1, -1));
            if (newTile != null) { TileTopRight = newTile; }

            // Tile BottomLeft
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(-1, 1));
            if (newTile != null) { TileBottomLeft = newTile; }

            // Tile BottomRight 
            newTile = World.GetTile(TileBehind.TilePosition + new Vector2(1, 1));
            if (newTile != null) { TileBottomRight = newTile; }
 
        }
        
        void UpdateInput()
        {
            KeyboardState newState = Keyboard.GetState();
            Vector2 NextPos = Vector2.Zero;

            if (Utils.CheckKeyDown(oldState, newState, Keys.D))
            {
                NextPos.X = 1f;
            }

            if (Utils.CheckKeyDown(oldState, newState, Keys.A))
            {
                NextPos.X = -1f;
            }
 
            if (Utils.CheckKeyUp(oldState, newState, Keys.F1))
            {
                MoveSpeed = 64;
            }
 
            if (Utils.CheckKeyDown(oldState, newState, Keys.W) || Utils.CheckKeyDown(oldState, newState, Keys.Space))
            {
                if (JumpAvailable)
                {
                    SetUpJump();
                }
            }

            if (Utils.CheckKeyDown(oldState, newState, Keys.LeftShift))
            {
                MoveSpeed *= Acceleration;
            }

            Force = NextPos;

            oldState = newState;
        }

        Vector2 Force;

        void PlayerHitGround()
        {
            GravityMultiplier = 0; 
            EndJumping();

        }
        
        void EndJumping()
        {
            IsJumping = false;
            JumpMultiplier = 0;
            JumpAvailable = true;
            JumpProgress = 0;
             
        }

        void SetUpJump()
        {
            IsJumping = true;
            JumpAvailable = false;
            JumpProgress = 12f;
        }

        void UpdateJump(float delta)
        {
            float Force = (World.WorldEnvironment.Gravity * JumpProgress) * delta;
            bool IsTopColiding = false;

            

        }

        void UpdateGravity(float delta)
        {
            
            float Force = (World.WorldEnvironment.Gravity * GravityMultiplier) * delta;
            bool isColiding = false;
            
            // Detect bottom colision
            for(int i = 0; i < 16; i++)
            {
                Rectangle BottomArea = new Rectangle((int)Position.X, (int)Position.Y, Area.Width,Area.Height);
                Rectangle BottomAreaRight = new Rectangle((int)Position.X + 15, (int)Position.Y, Area.Width,Area.Height);
                Rectangle BottomAreaLeft = new Rectangle((int)Position.X - 15, (int)Position.Y, Area.Width,Area.Height);

                MapTile TileBottom = World.GetTile(World.GetTilePosition(Position + new Vector2(0, 32 + i)));
                MapTile TileBottomRight = World.GetTile(World.GetTilePosition(Position + new Vector2(15, 32 + i)));
                MapTile TileBottomLeft = World.GetTile(World.GetTilePosition(Position + new Vector2(-15, 32 + i)));

                if (TileBottom.TileInformation.IsColideable)
                {
                    SinasTile = TileBottom;
                    SinasRect = BottomArea;
                }

                isColiding = TileBottom.TileInformation.IsColideable && BottomArea.Intersects(TileBottom.GetArea()) || 
                             TileBottomLeft.TileInformation.IsColideable && BottomAreaLeft.Intersects(TileBottomLeft.GetArea()) ||
                             TileBottomRight.TileInformation.IsColideable && BottomAreaRight.Intersects(TileBottomRight.GetArea());

                if (isColiding)
                {
                    Position.Y = TileBottom.GetArea().Y - 31;
                    PlayerHitGround();
                    break;
                }
            }

            if (!isColiding) 
            {
                Position.Y += Force;
                UpdateArea();

                GravityMultiplier += 16f * delta;
            }


        }

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


            /*
            // Top colision
            for(int i = 0; i < 16; i++)
            {  
                // Top Center
                MapTile topTile = World.GetTile(World.GetTilePosition(NextStep - new Vector2(0, i + 16)));
                Rectangle topArea = new Rectangle((int)Position.X, (int)Position.Y - i, Area.Width, Area.Height);
                 
                if (i == 0)
                {
                    SinasTile = topTile;
                    SinasRect = topArea;
                }
   
                if (topTile.TileInformation.IsColideable && topTile.GetArea().Intersects(topArea))
                { 
                   NextStep = new Vector2(NextStep.X, topTile.GetArea().Bottom + i); 
                   Console.WriteLine($"Top Colision at iteration {i}");
                   EndJumping(); 
                   break;
                }
 
            }
 
            // Bottom colision
            for(int i = 0; i < 16; i++)
            {  
                // Bottom Center
                MapTile bottomTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(0, 32 + i)));
                Rectangle bottomArea = new Rectangle((int)Position.X, (int)Position.Y - i, Area.Width, Area.Height);
     
                if (bottomTile.TileInformation.IsColideable && bottomTile.GetArea().Intersects(bottomArea))
                { 
                   NextStep = new Vector2(NextStep.X, bottomTile.GetArea().Y - (31 - i)); 
                   if (IsJumping) { Console.WriteLine("Bottom Colision"); }
                   PlayerHitGround();
                   break;
                }
 
                // Bottom Left
                MapTile bottomLeftTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(16 - i, 32 + i)));
                Rectangle bottomLeftArea = new Rectangle(((int)Position.X - 16) + i, (int)Position.Y - i, Area.Width, Area.Height);

                if (i == 0) { SinasTile = bottomLeftTile; SinasRect = bottomLeftArea; }
 
                if (bottomLeftTile.TileInformation.IsColideable && bottomLeftTile.GetArea().Intersects(bottomLeftArea))
                {   
                   NextStep = new Vector2(NextStep.X, bottomLeftTile.GetArea().Y - (31 - i));
                   if (IsJumping) { Console.WriteLine("Bottom Left Colision"); }
                   PlayerHitGround();
                   break;
                }
 
                // Bottom Right
                MapTile bottomRightTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(-(16 - i), 32 + i)));
                Rectangle bottomRightArea = new Rectangle(((int)Position.X - 16) - i, (int)Position.Y - i, Area.Width, Area.Height);

                if (i == 0) { CeiraTile = bottomRightTile; CeiraRect = bottomRightArea; }
 
                if (bottomRightTile.TileInformation.IsColideable && bottomRightTile.GetArea().Intersects(bottomRightArea))
                {
                   NextStep = new Vector2(NextStep.X, bottomRightTile.GetArea().Y - (31 - i));
                   if (IsJumping) { Console.WriteLine("Bottom Right Colision"); }
                   PlayerHitGround();
                   break;
                }

            }
            */
            
            
            Position = NextStep; 
        }

        // 
        // Update pipeline for a Controlable Character
        //
        internal void UpdateChracter(float delta)
        {
            if (Position.X < 0) { Position.X = 0; UpdateArea(); }
            if (Position.Y < 0) { Position.Y = 0; UpdateArea(); }
            
            if (BlendColor.G < 255)
            {
                BlendColor.G += Convert.ToByte((2f * delta * 255));
            }            

            if (BlendColor.B < 255)
            {
                BlendColor.B += Convert.ToByte((2f * delta * 255));
            }            
            
            AimVector = -(Position - (MouseInput.PositionVector2 - World.Camera.CameraPosition));

            GetTilesAround(Position);

            UpdateInput();
            float oldDelta = delta;
 
            // Limit delta to a stable value
            if (delta > Settings.Physics_StableDelta) { delta = Settings.Physics_StableDelta; }
  
            UpdateMoveSpeed(delta);
            UpdateAppliedForces(delta);
            UpdateJump(delta);
            UpdateGravity(delta);

            delta = oldDelta;

            LastDelta = delta;

        }

    }
}