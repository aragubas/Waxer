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
        public float Acceleration = 1.8f;
        public float JumpMultiplier = 8f;
        public int Life = 300;
        private KeyboardState oldState;
        internal Vector2 AimVector = Vector2.Zero;
        internal float GravityMultiplier = 0f;
        internal bool IsJumping = false;
        internal bool JumpAvailable = true;
        internal float JumpProgress = 0f;
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
            // Draw all the surronding colision check tiles
            if (Settings.Debug_RenderColidersTiles)
            {
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

            }

            // Draw last delta
            spriteBatch.DrawString(World.DebugFont, $"delta: {LastDelta}\n" + 
                                                        $"move_speed: {MoveSpeed}", new Vector2(12, 50), Color.Red);

            // Hightlights the player position    
            if (Settings.Debug_RenderPlayerPositionPoints)
            {
                spriteBatch.DrawPoint(Position + World.Camera.CameraPosition, Color.Red, 2);
                spriteBatch.DrawPoint((Position - new Vector2(-16, 0)) + World.Camera.CameraPosition, Color.Blue, 2);
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
 
        #region Physics

        // Physics
        // Get all tiles around the player, including all quadrants
        //
        void GetTilesAround()
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


        // Physics
        // Check if player isn't stuck on something
        //
        void CheckPlayerStuck()
        {
            Rectangle FixedArea = TileBehind.GetArea();
            FixedArea.Inflate(-1, -1);

            if (FixedArea.Intersects(TileTopRight.GetArea()))
            {
                Position = TileTopRight.ScreenPosition;

            }
            else if (FixedArea.Intersects(TileTopLeft.GetArea()))
            {
                Position = TileTopLeft.ScreenPosition;

            }
            else if (FixedArea.Intersects(TileTop.GetArea()))
            {
                Position = TileTop.ScreenPosition;

            }

            UpdateArea();
        }

        // Physics
        // Update Gravity
        //
        void UpdateGravity(float delta)
        {
            Rectangle FixedArea = new Rectangle(Area.X - 16, Area.Y + (int)World.MapEnvironment.Gravity, 32, 32);
            Rectangle FixedAreaLeft = new Rectangle(Area.X - 14, Area.Y + (int)World.MapEnvironment.Gravity, 32, 32);
            Rectangle AreaSinas = new Rectangle(Area.X - 16, Area.Y + (int)World.MapEnvironment.Gravity, 32, 32);

            bool ColidingBottom = (TileBottom.GetArea().Intersects(FixedArea) && TileBottom.TileInformation.IsColideable);
            bool ColidingBottomLeft = (TileBottomLeft.GetArea().Intersects(FixedAreaLeft) && TileBottomLeft.TileInformation.IsColideable);
            bool ColidingBottomRight = (TileBottomRight.GetArea().Intersects(FixedArea) && TileBottomRight.TileInformation.IsColideable);

            if (ColidingBottom) { Position.Y = TileBottom.GetArea().Y - 32; UpdateArea(); }
            //if (ColidingBottomLeft) { Position.Y = TileBottomLeft.GetArea().Y - 32; UpdateArea(); }
            //if (ColidingBottomRight) { Position.Y = TileBottomRight.GetArea().Y - 32; UpdateArea(); }


            if (ColidingBottom || ColidingBottomLeft || ColidingBottomRight)
            {
                // Player hits the ground
                GravityMultiplier = 0f;
                EndJumping();

                
                /*
                Console.WriteLine( 
                    $"B{TileBottom.GetArea().Intersects(AreaSinas) && TileBottom.IsColideable}\n" + 
                    $"R{TileBottomRight.GetArea().Intersects(FixedArea) && TileBottomRight.IsColideable}\n" + 
                    $"L{TileBottomLeft.GetArea().Intersects(FixedAreaLeft) && TileBottomLeft.IsColideable}\n"
                    
                );
                */

            }else  
            { 
                // Pulls the player to the ground, to simulate gravity
                float Force = (World.MapEnvironment.Gravity * GravityMultiplier) * delta;
                Position.Y += Force;
                UpdateArea();

                GravityMultiplier += 16f * delta;
                
            }

        }

        // Physics
        // Check if character is not inside a tile
        //
        void CheckForSideColision(float delta)
        {  
            Rectangle FixedArea = new Rectangle(Area.X, Area.Y, 32, 32);
            Vector2 MoveVector = new Vector2();
  
            Rectangle FixedAreaRight = new Rectangle((Area.X + 16), Area.Y, 32, 32);
            MapTile FixedRightTile = World.GetTile(World.GetTilePosition(FixedAreaRight));
            Rectangle FixedAreaRightBottom = new Rectangle((Area.X + 16), Area.Y + 32, 32, 32);
            MapTile FixedRightBottomTile = World.GetTile(World.GetTilePosition(FixedAreaRightBottom));
            Rectangle FixedAreaRightTop = new Rectangle((Area.X + 16), Area.Y - 32, 32, 32);
            MapTile FixedRightTopTile = World.GetTile(World.GetTilePosition(FixedAreaRightTop));
 
            if (FixedRightTile.TileInformation.IsColideable && !FixedRightTile.GetArea().Intersects(Area))
            {
                if (FixedRightTopTile.TileInformation.IsColideable && !FixedRightTopTile.GetArea().Intersects(Area))
                {
                    if (FixedRightBottomTile.TileInformation.IsColideable && !FixedRightBottomTile.GetArea().Intersects(Area))
                    {
                        MoveVector.X = -1;
                        Console.WriteLine("Conter X-");
                    }
                }
            }

            Rectangle LeftColision = new Rectangle(Area.X - 16, Area.Y, Area.Width, Area.Height);
            Rectangle FixedAreaLeft = new Rectangle((Area.X - 16), Area.Y, 32, 32);
            MapTile FixedLeftTile = World.GetTile(World.GetTilePosition(FixedAreaLeft));
            Rectangle FixedAreaLeftBottom = new Rectangle((Area.X - 16), Area.Y + 32, 32, 32);
            MapTile FixedLeftBottomTile = World.GetTile(World.GetTilePosition(FixedAreaLeftBottom));
            Rectangle FixedAreaLeftTop = new Rectangle((Area.X - 16), Area.Y - 32, 32, 32);
            MapTile FixedLeftTopTile = World.GetTile(World.GetTilePosition(FixedAreaLeftTop));

            if (FixedLeftTile.TileInformation.IsColideable && !FixedLeftTile.GetArea().Intersects(LeftColision))
            {
                if (FixedLeftTopTile.TileInformation.IsColideable && !FixedLeftTopTile.GetArea().Intersects(LeftColision))
                {
                    if (FixedLeftBottomTile.TileInformation.IsColideable && !FixedLeftBottomTile.GetArea().Intersects(LeftColision))
                    {
                        MoveVector.X = 1;
                        Console.WriteLine("Conter X+");
                    }
                }
            }


            // Update the player position with the Move Vector
            //UpdateMoveSpeed(delta);
            Position += MoveVector * MoveSpeed * delta;
            UpdateArea();


        }


        #endregion

        #region Movement

        // Movement
        // Update MoveSpeed curve
        //
        void UpdateMoveSpeed(float delta)
        {
            MoveSpeed -= 38 * delta;
            if (MoveSpeed < MinimumSpeed) { MoveSpeed = MinimumSpeed; }
            if (MoveSpeed > 256) { MoveSpeed = 256; }
        }

        // Movement
        // Updates character keyboard input
        //
        void UpdateInput(float delta)
        {
            UpdateArea();
            KeyboardState State = Keyboard.GetState();
            Vector2 MoveVector = new Vector2();

            if (Utils.CheckKeyUp(oldState, State, Keys.W) || Utils.CheckKeyUp(oldState, State, Keys.Space))
            {
                if (JumpAvailable) 
                { 
                    SetUpJump(delta);

                }
            }

            Rectangle RightColision = new Rectangle(Area.X + 2, Area.Y, Area.Width, Area.Height);
            Rectangle FixedAreaRight = new Rectangle((Area.X + 18), Area.Y, 32, 32);
            MapTile FixedRightTile = World.GetTile(World.GetTilePosition(FixedAreaRight));
            Rectangle FixedAreaRightBottom = new Rectangle((Area.X + 16), Area.Y + 32, 32, 32);
            MapTile FixedRightBottomTile = World.GetTile(World.GetTilePosition(FixedAreaRightBottom));
            Rectangle FixedAreaRightTop = new Rectangle((Area.X + 18), Area.Y - 32, 32, 32);
            MapTile FixedRightTopTile = World.GetTile(World.GetTilePosition(FixedAreaRightTop));
 
            if (Utils.CheckKeyDown(oldState, State, Keys.D))
            {
                if (FixedRightTile.TileInformation.IsColideable && !FixedRightTile.GetArea().Intersects(RightColision) || !FixedRightTile.TileInformation.IsColideable)
                {
                    if (FixedRightTopTile.TileInformation.IsColideable && !FixedRightTopTile.GetArea().Intersects(RightColision) || !FixedRightTopTile.TileInformation.IsColideable)
                    {
                        if (FixedRightBottomTile.TileInformation.IsColideable && !FixedRightBottomTile.GetArea().Intersects(RightColision) || !FixedRightBottomTile.TileInformation.IsColideable)
                        {
                            MoveVector.X = 1;
                        }
                    }
                }

            }

            Rectangle LeftColision = new Rectangle(Area.X - 18, Area.Y, Area.Width, Area.Height);
            Rectangle FixedAreaLeft = new Rectangle((Area.X - 18), Area.Y, 32, 32);
            MapTile FixedLeftTile = World.GetTile(World.GetTilePosition(FixedAreaLeft));
            Rectangle FixedAreaLeftBottom = new Rectangle((Area.X - 18), Area.Y + 32, 32, 32);
            MapTile FixedLeftBottomTile = World.GetTile(World.GetTilePosition(FixedAreaLeftBottom));
            Rectangle FixedAreaLeftTop = new Rectangle((Area.X - 18), Area.Y - 32, 32, 32);
            MapTile FixedLeftTopTile = World.GetTile(World.GetTilePosition(FixedAreaLeftTop));

            if (Utils.CheckKeyDown(oldState, State, Keys.A))
            {
                if (FixedLeftTile.TileInformation.IsColideable && !FixedLeftTile.GetArea().Intersects(LeftColision) || !FixedLeftTile.TileInformation.IsColideable)
                {
                    if (FixedLeftTopTile.TileInformation.IsColideable && !FixedLeftTopTile.GetArea().Intersects(LeftColision) || !FixedLeftTopTile.TileInformation.IsColideable)
                    {
                        if (FixedLeftBottomTile.TileInformation.IsColideable && !FixedLeftBottomTile.GetArea().Intersects(LeftColision) || !FixedLeftBottomTile.TileInformation.IsColideable)
                        {
                            MoveVector.X = -1;
                        }
                    }
                }

            }

            if (Utils.CheckKeyDown(oldState, State, Keys.LeftShift) || Utils.CheckKeyDown(oldState, State, Keys.RightShift))
            {
                MoveSpeed *= Acceleration;
            }
 

            // Normalize the vector, because the built in function to do so is broken
            if (MoveVector.X > 1) { MoveVector.X = 1; }
            if (MoveVector.X < -1) { MoveVector.X = -1; }
            if (MoveVector.Y > 1) { MoveVector.Y = 1; }
            if (MoveVector.Y < -1) { MoveVector.Y = -1; }

            // Update the player position with the Move Vector
            UpdateMoveSpeed(delta);
            Position += MoveVector * MoveSpeed * delta;
            UpdateArea();
            
            GetTilesAround();
            //CheckForSideIntersection(delta);
 
            if (Utils.CheckKeyUp(oldState, State, Keys.Space))
            {
                Bullet newBullet = new Bullet(new Vector2(Position.X + 16, Position.Y + 16), World, InstanceID);
                newBullet.Direction = AimVector;
                newBullet.World = World;
                newBullet.Direction.Normalize();
                
                World.Entities.Add(newBullet);
            }

            oldState = State;
        }

        #region Movement -> Jump
        // Movement -> Jump
        // Ends jumping
        //
        void EndJumping()
        {
            JumpProgress = 16f;
            IsJumping = false;
            JumpAvailable = true;
        }
        
        // Movement -> Jump
        // Updates the jump curve
        //
        void UpdateJump(float delta)
        {
            if (IsJumping && !JumpAvailable)
            {  
                float JumpForce = (World.MapEnvironment.Gravity * JumpProgress) * delta;
                
                UpdateArea();
                Rectangle FixedArea = new Rectangle(Area.X, Area.Y - 1, Area.Width, Area.Height);
                Rectangle FixedAreaLeft = new Rectangle(Area.X - 15, Area.Y - 1, Area.Width, Area.Height);
                Rectangle FixedAreaRight = new Rectangle(Area.X + 15, Area.Y - 1, Area.Width, Area.Height);
                
                MapTile FixedTopTile = World.GetTile(World.GetTilePosition(new Vector2(FixedArea.X, FixedArea.Y)));
                MapTile FixedTopLeft = World.GetTile(World.GetTilePosition(new Vector2(FixedAreaLeft.X, FixedAreaLeft.Y)));
                MapTile FixedTopRight = World.GetTile(World.GetTilePosition(new Vector2(FixedAreaRight.X, FixedAreaRight.Y)));
 
                if (FixedTopTile.GetArea().Intersects(FixedArea) && FixedTopTile.TileInformation.IsColideable ||
                    FixedTopLeft.GetArea().Intersects(FixedAreaLeft) && FixedTopLeft.TileInformation.IsColideable ||
                    FixedTopRight.GetArea().Intersects(FixedAreaRight) && FixedTopRight.TileInformation.IsColideable)
                {
                    //Position.Y += JumpForce;
                    UpdateArea();
                    GetTilesAround();
                    EndJumping();
                    return;
                }
 
                JumpProgress += MoveSpeed * delta;

                if (JumpProgress > JumpMultiplier) 
                {
                    JumpProgress = JumpMultiplier;
                }

                Position.Y -= (World.MapEnvironment.Gravity * JumpProgress) * delta;
                UpdateArea();
                GetTilesAround(); 
                UpdateMoveSpeed(delta);

            }
        }

        // Movement -> Jump
        // Initiates a jump
        //
        void SetUpJump(float delta)
        {
            //Position.Y -= (ParentMap.MapEnvironment.Gravity * JumpProgress) * delta;
            //UpdateArea();

            IsJumping = true; 
            JumpAvailable = false; 
            JumpProgress = 12f;

        }

        #endregion

        #endregion

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


            GetTilesAround();

            UpdateGravity(delta);
            GetTilesAround();

            UpdateInput(delta);
            GetTilesAround();
            
            CheckPlayerStuck();
            UpdateJump(delta);

            LastDelta = delta;

        }

    }
}