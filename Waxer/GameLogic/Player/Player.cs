using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Waxer.GameLogic.Player.Inventory;

namespace Waxer.GameLogic.Player
{
    public class PlayerEntity : MapEntity
    {
        public float MoveSpeed = 64f;
        public float MinimumSpeed = 64f;
        public float Acceleration = 1.8f;
        public float JumpMultiplier = 8f;
        public int Life = 300;
        KeyboardState oldState;
        Vector2 AimVector = Vector2.Zero;
        float GravityMultiplier = 0f;
        bool IsJumping = false;
        bool JumpAvailable = true;
        float JumpProgress = 0f;
        MapTile TileUnderCursor = null;         
        MapTile TileBehind = null;
        MapTile TileRight = null;
        MapTile TileLeft = null;
        MapTile TileTop = null;
        MapTile TileBottom = null;
        MapTile TileTopLeft = null;
        MapTile TileTopRight = null;
        MapTile TileBottomLeft = null;
        MapTile TileBottomRight = null;
        
        float LastDelta = 0.0f;
        InventoryUI inventoryUI;

        public PlayerEntity(Vector2 initialPosition, Map parentMap)
        {
            Position = initialPosition;
            ParentMap = parentMap;
 
            // Set SpriteOrigin to Bottom Center
            SpriteOrigin = new Vector2(16, 0);

            // Create the InventoryUI Element
            inventoryUI = new InventoryUI();
        }

        // Used for Debugging
        internal void RenderTileInfos(SpriteBatch spriteBatch, MapTile tile, string TileName)
        {
            Vector2 FixedPosition = new Vector2(((int)tile.TilePosition.X * 32) + (int)ParentMap.camera.CameraPosition.X, 
                ((int)tile.TilePosition.Y * 32) + (int)ParentMap.camera.CameraPosition.Y);

            Color sinasColor = Color.Red;
            sinasColor.A = (byte)30;

            spriteBatch.DrawRectangle(new Rectangle((int)FixedPosition.X, (int)FixedPosition.Y, 32, 32), sinasColor);

            // Draw tile cordinates
            spriteBatch.DrawString(ParentMap.DebugFont, $"X:{tile.TilePosition.X}\n" + 
                                                        $"Y:{tile.TilePosition.Y}\n{TileName}", FixedPosition, sinasColor);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            spriteBatch.End();
            spriteBatch.Begin();
            
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

            // Hightlights the player position    
            if (Settings.Debug_RenderPlayerPositionPoints)
            {
                spriteBatch.DrawPoint(Position + ParentMap.camera.CameraPosition, Color.Red, 2);
                spriteBatch.DrawPoint((Position - new Vector2(-16, 0)) + ParentMap.camera.CameraPosition, Color.Blue, 2);
                spriteBatch.DrawPoint((Position - new Vector2(16, 0)) + ParentMap.camera.CameraPosition, Color.Yellow, 2);
            }
            

            if (TileUnderCursor != null)
            {                  
                Vector2 FixedPosition = new Vector2(((int)TileUnderCursor.TilePosition.X * 32) + (int)ParentMap.camera.CameraPosition.X, 
                    ((int)TileUnderCursor.TilePosition.Y * 32) + (int)ParentMap.camera.CameraPosition.Y);

                // Draw tile cordinates
                spriteBatch.DrawString(ParentMap.DebugFont, $"X:{TileUnderCursor.TilePosition.X}\n" + 
                                                            $"Y:{TileUnderCursor.TilePosition.Y}", FixedPosition, Color.Black);

            }

            // Draw last delta
            spriteBatch.DrawString(ParentMap.DebugFont, $"delta: {LastDelta}\n" + 
                                                        $"move_speed: {MoveSpeed}", new Vector2(12, 50), Color.Red);

        }

        void CheckForSideIntersection(float delta)
        {  
            Rectangle FixedArea = new Rectangle(Area.X, Area.Y, 32, 32);
            Vector2 MoveVector = new Vector2();
  
            Rectangle FixedAreaRight = new Rectangle((Area.X + 16), Area.Y, 32, 32);
            MapTile FixedRightTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaRight));
            Rectangle FixedAreaRightBottom = new Rectangle((Area.X + 16), Area.Y + 32, 32, 32);
            MapTile FixedRightBottomTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaRightBottom));
            Rectangle FixedAreaRightTop = new Rectangle((Area.X + 16), Area.Y - 32, 32, 32);
            MapTile FixedRightTopTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaRightTop));
 
            if (FixedRightTile.IsColideable && !FixedRightTile.GetArea().Intersects(Area))
            {
                if (FixedRightTopTile.IsColideable && !FixedRightTopTile.GetArea().Intersects(Area))
                {
                    if (FixedRightBottomTile.IsColideable && !FixedRightBottomTile.GetArea().Intersects(Area))
                    {
                        MoveVector.X = -1;
                        Console.WriteLine("Conter X-");
                    }
                }
            }

            Rectangle LeftColision = new Rectangle(Area.X - 16, Area.Y, Area.Width, Area.Height);
            Rectangle FixedAreaLeft = new Rectangle((Area.X - 16), Area.Y, 32, 32);
            MapTile FixedLeftTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaLeft));
            Rectangle FixedAreaLeftBottom = new Rectangle((Area.X - 16), Area.Y + 32, 32, 32);
            MapTile FixedLeftBottomTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaLeftBottom));
            Rectangle FixedAreaLeftTop = new Rectangle((Area.X - 16), Area.Y - 32, 32, 32);
            MapTile FixedLeftTopTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaLeftTop));

            if (FixedLeftTile.IsColideable && !FixedLeftTile.GetArea().Intersects(LeftColision))
            {
                if (FixedLeftTopTile.IsColideable && !FixedLeftTopTile.GetArea().Intersects(LeftColision))
                {
                    if (FixedLeftBottomTile.IsColideable && !FixedLeftBottomTile.GetArea().Intersects(LeftColision))
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
            MapTile FixedRightTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaRight));
            Rectangle FixedAreaRightBottom = new Rectangle((Area.X + 16), Area.Y + 32, 32, 32);
            MapTile FixedRightBottomTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaRightBottom));
            Rectangle FixedAreaRightTop = new Rectangle((Area.X + 18), Area.Y - 32, 32, 32);
            MapTile FixedRightTopTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaRightTop));
 
            if (Utils.CheckKeyDown(oldState, State, Keys.D))
            {
                if (FixedRightTile.IsColideable && !FixedRightTile.GetArea().Intersects(RightColision) || !FixedRightTile.IsColideable)
                {
                    if (FixedRightTopTile.IsColideable && !FixedRightTopTile.GetArea().Intersects(RightColision) || !FixedRightTopTile.IsColideable)
                    {
                        if (FixedRightBottomTile.IsColideable && !FixedRightBottomTile.GetArea().Intersects(RightColision) || !FixedRightBottomTile.IsColideable)
                        {
                            MoveVector.X = 1;
                        }
                    }
                }

            }

            Rectangle LeftColision = new Rectangle(Area.X - 18, Area.Y, Area.Width, Area.Height);
            Rectangle FixedAreaLeft = new Rectangle((Area.X - 18), Area.Y, 32, 32);
            MapTile FixedLeftTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaLeft));
            Rectangle FixedAreaLeftBottom = new Rectangle((Area.X - 18), Area.Y + 32, 32, 32);
            MapTile FixedLeftBottomTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaLeftBottom));
            Rectangle FixedAreaLeftTop = new Rectangle((Area.X - 18), Area.Y - 32, 32, 32);
            MapTile FixedLeftTopTile = ParentMap.GetTile(ParentMap.GetTilePosition(FixedAreaLeftTop));

            if (Utils.CheckKeyDown(oldState, State, Keys.A))
            {
                if (FixedLeftTile.IsColideable && !FixedLeftTile.GetArea().Intersects(LeftColision) || !FixedLeftTile.IsColideable)
                {
                    if (FixedLeftTopTile.IsColideable && !FixedLeftTopTile.GetArea().Intersects(LeftColision) || !FixedLeftTopTile.IsColideable)
                    {
                        if (FixedLeftBottomTile.IsColideable && !FixedLeftBottomTile.GetArea().Intersects(LeftColision) || !FixedLeftBottomTile.IsColideable)
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
                Bullet newBullet = new Bullet(new Vector2(Position.X + 16, Position.Y + 16), ParentMap, InstanceID);
                newBullet.Direction = AimVector;
                newBullet.ParentMap = ParentMap;
                newBullet.Direction.Normalize();
                
                ParentMap.Entities.Add(newBullet);
            }

            oldState = State;
        }

        public void BulletDamage(Bullet bullet)
        {
            if (bullet.ShooterInstanceID == InstanceID) { return; }
            Life -= bullet.Damage;
            BlendColor.R = 255;
            BlendColor.G = 0;
            BlendColor.B = 0;
 
        }
 
        void UpdateGravity(float delta)
        {
            Rectangle FixedArea = new Rectangle(Area.X - 16, Area.Y + (int)ParentMap.MapEnvironment.Gravity, 32, 32);
            Rectangle FixedAreaLeft = new Rectangle(Area.X - 14, Area.Y + (int)ParentMap.MapEnvironment.Gravity, 32, 32);
            Rectangle AreaSinas = new Rectangle(Area.X - 16, Area.Y + (int)ParentMap.MapEnvironment.Gravity, 32, 32);

            bool ColidingBottom = (TileBottom.GetArea().Intersects(FixedArea) && TileBottom.IsColideable);
            bool ColidingBottomLeft = (TileBottomLeft.GetArea().Intersects(FixedAreaLeft) && TileBottomLeft.IsColideable);
            bool ColidingBottomRight = (TileBottomRight.GetArea().Intersects(FixedArea) && TileBottomRight.IsColideable);

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
                float Force = (ParentMap.MapEnvironment.Gravity * GravityMultiplier) * delta;
                Position.Y += Force;
                UpdateArea();

                GravityMultiplier += 16f * delta;
                
            }

        }
 
        void EndJumping()
        {
            JumpProgress = 16f;
            IsJumping = false;
            JumpAvailable = true;
        }
        
        void UpdateJump(float delta)
        {
            if (IsJumping && !JumpAvailable)
            {  
                float JumpForce = (ParentMap.MapEnvironment.Gravity * JumpProgress) * delta;
                
                UpdateArea();
                Rectangle FixedArea = new Rectangle(Area.X, Area.Y - 1, Area.Width, Area.Height);
                Rectangle FixedAreaLeft = new Rectangle(Area.X - 16, Area.Y - 1, Area.Width, Area.Height);
                Rectangle FixedAreaRight = new Rectangle(Area.X + 16, Area.Y - 1, Area.Width, Area.Height);
                
                MapTile FixedTopTile = ParentMap.GetTile(ParentMap.GetTilePosition(new Vector2(FixedArea.X, FixedArea.Y)));
                MapTile FixedTopLeft = ParentMap.GetTile(ParentMap.GetTilePosition(new Vector2(FixedAreaLeft.X, FixedAreaLeft.Y)));
                MapTile FixedTopRight = ParentMap.GetTile(ParentMap.GetTilePosition(new Vector2(FixedAreaRight.X, FixedAreaRight.Y)));
 
                if (FixedTopTile.GetArea().Intersects(FixedArea) && FixedTopTile.IsColideable ||
                    FixedTopLeft.GetArea().Intersects(FixedAreaLeft) && FixedTopLeft.IsColideable ||
                    FixedTopRight.GetArea().Intersects(FixedAreaRight) && FixedTopRight.IsColideable)
                {
                    Console.WriteLine("No room to jump.");
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

                Position.Y -= (ParentMap.MapEnvironment.Gravity * JumpProgress) * delta;
                UpdateArea();
                GetTilesAround(); 
                UpdateMoveSpeed(delta);

            }
        }

        void SetUpJump(float delta)
        {
            //Position.Y -= (ParentMap.MapEnvironment.Gravity * JumpProgress) * delta;
            //UpdateArea();

            IsJumping = true; 
            JumpAvailable = false; 
            JumpProgress = 12f;

        }

        void GetTilesAround()
        {
            // Tile Under Cursor
            MapTile newTile = ParentMap.GetTile(ParentMap.GetTilePosition(MouseInput.PositionVector2 - ParentMap.camera.CameraPosition));
            if (newTile != null) { TileUnderCursor = newTile; }

            // Tile Behind
            newTile = ParentMap.GetTile(ParentMap.GetTilePosition(Position));
            if (newTile != null) { TileBehind = newTile; }

            // Tile Top
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(0, -1));
            if (newTile != null) { TileTop = newTile; }

            // Tile Bottom
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(0, 1));
            if (newTile != null) { TileBottom = newTile; }

            // Tile Left
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(-1, 0));
            if (newTile != null) { TileLeft = newTile; }

            // Tile Right
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(1, 0));
            if (newTile != null) { TileRight = newTile; }

            // Corner Edges Tiles

            // Tile TopLeft
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(-1, -1));
            if (newTile != null) { TileTopLeft = newTile; }

            // Tile TopRight
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(1, -1));
            if (newTile != null) { TileTopRight = newTile; }

            // Tile BottomLeft
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(-1, 1));
            if (newTile != null) { TileBottomLeft = newTile; }

            // Tile BottomRight 
            newTile = ParentMap.GetTile(TileBehind.TilePosition + new Vector2(1, 1));
            if (newTile != null) { TileBottomRight = newTile; }
 
        }

        void UpdateMoveSpeed(float delta)
        {
            MoveSpeed -= 38 * delta;
            if (MoveSpeed < MinimumSpeed) { MoveSpeed = MinimumSpeed; }
            if (MoveSpeed > 256) { MoveSpeed = 256; }
        }

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

        public override void Update(float delta)
        {
            base.Update(delta);

            if (BlendColor.G < 255)
            {
                BlendColor.G += Convert.ToByte((2f * delta * 255));
            }            

            if (BlendColor.B < 255)
            {
                BlendColor.B += Convert.ToByte((2f * delta * 255));
            }            
            
            AimVector = -(Position - (MouseInput.PositionVector2 - ParentMap.camera.CameraPosition));


            GetTilesAround();

            UpdateGravity(delta);
            GetTilesAround();

            UpdateInput(delta);
            GetTilesAround();
            
            CheckPlayerStuck();
            UpdateJump(delta);

            if (TileUnderCursor != null)
            {
                Rectangle FixedArea = new Rectangle(TileUnderCursor.GetArea().X + (int)ParentMap.camera.CameraPosition.X,
                    TileUnderCursor.GetArea().Y + (int)ParentMap.camera.CameraPosition.Y, 32, 32);

                if (MouseInput.Left_UpClickPos.Intersects(FixedArea))
                {
                    TileUnderCursor.SetTileID(0);
                    TileUnderCursor.IsColideable = false;
                    GetTilesAround();

                }

                if (MouseInput.Right_UpClickPos.Intersects(FixedArea))
                {
                    TileUnderCursor.SetTileID(1);
                    TileUnderCursor.IsColideable = true;
                    GetTilesAround();
 
                }
            }

            LastDelta = delta;
        }
    }
}