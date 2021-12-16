using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;

namespace Waxer.GameLogic
{
    public class PlayerEntity : MapEntity
    {
        public float MoveSpeed = 64f;
        public float MinimumSpeed = 64f;
        public float Acceleration = 3f;
        public float JumpMultiplier = 8f;
        public int PlayerLife = 300;
        KeyboardState oldState;
        Vector2 AimVector = Vector2.Zero;
        float GravityMultiplier = 0f;
        bool IsJumping = false;
        bool JumpAvailable = true;
        bool JumpFinished = false;
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

        public PlayerEntity(Vector2 initialPosition, Map parentMap)
        {
            Position = initialPosition;
            ParentMap = parentMap;
 
            // Set SpriteOrigin to Bottom Center
            SpriteOrigin = new Vector2(16, 0);
        }

        void RenderTileInfos(SpriteBatch spriteBatch, MapTile tile, string TileName)
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

            // Hightlights the player position    
            spriteBatch.DrawPoint(Position + ParentMap.camera.CameraPosition, Color.Orange, 2);
            spriteBatch.DrawPoint((Position - new Vector2(-16, 0)) + ParentMap.camera.CameraPosition, Color.Blue, 2);
            spriteBatch.DrawPoint((Position - new Vector2(16, 0)) + ParentMap.camera.CameraPosition, Color.Yellow, 2);
            

            if (TileUnderCursor != null)
            {                  
                Vector2 FixedPosition = new Vector2(((int)TileUnderCursor.TilePosition.X * 32) + (int)ParentMap.camera.CameraPosition.X, 
                    ((int)TileUnderCursor.TilePosition.Y * 32) + (int)ParentMap.camera.CameraPosition.Y);
                    
                // Draw tile texture box
                spriteBatch.DrawRectangle(new Rectangle((int)FixedPosition.X, (int)FixedPosition.Y, 32, 32), Color.Black);
                
                // Draw tile texture
                spriteBatch.Draw(TileUnderCursor.TileTexture, new Vector2(64, 128), Color.White);
                
                // Draw red box around the tile
                spriteBatch.DrawRectangle(new Rectangle(64, 128, 32, 32), Color.Red);

                // Draw tile cordinates
                spriteBatch.DrawString(ParentMap.DebugFont, $"X:{TileUnderCursor.TilePosition.X}\n" + 
                                                            $"Y:{TileUnderCursor.TilePosition.Y}", FixedPosition, Color.Black);

            }

        }

        void CheckForSideIntersection()
        {
            
        }

        void UpdateInput(GameTime gameTime)
        {
            KeyboardState State = Keyboard.GetState();
            Vector2 MoveVector = new Vector2();
            Rectangle FixedArea = new Rectangle(Area.X - 15, Area.Y, 32, 32);
 
            if (Utils.CheckKeyUp(oldState, State, Keys.W) || Utils.CheckKeyUp(oldState, State, Keys.Space))
            {
                if (JumpAvailable) 
                { 
                    SetUpJump(gameTime);                    
                }
            }
 
            if (Utils.CheckKeyDown(oldState, State, Keys.D))
            {
                GetTilesAround();

                if (TileRight.IsColideable && !TileRight.GetArea().Intersects(FixedArea) || !TileRight.IsColideable)
                {
                    if (TileTopRight.IsColideable && !TileTopRight.GetArea().Intersects(FixedArea) || !TileTopRight.IsColideable)
                    {
                        if (TileBottomRight.IsColideable && !TileBottomRight.GetArea().Intersects(FixedArea) || !TileBottomRight.IsColideable)
                        {
                            MoveVector.X = 1;
                        }
                    }
                }

            }

            if (Utils.CheckKeyDown(oldState, State, Keys.A))
            {
                GetTilesAround();

                if (TileLeft.IsColideable && !TileLeft.GetArea().Intersects(FixedArea) || !TileLeft.IsColideable)
                { 
                    if (TileTopLeft.IsColideable && !TileTopLeft.GetArea().Intersects(FixedArea) || !TileTopLeft.IsColideable)
                    {
                        if (TileBottomLeft.IsColideable && !TileBottomLeft.GetArea().Intersects(FixedArea) || !TileBottomLeft.IsColideable)
                        {
                            MoveVector.X = -1;

                        }
                    }
                }
                
            }

            if (Utils.CheckKeyDown(oldState, State, Keys.LeftShift) || Utils.CheckKeyDown(oldState, State, Keys.RightShift))
            {
                GetTilesAround();

                MoveSpeed *= Acceleration;
            }


            // Normalize the vector, because the built in function to do so is broken
            if (MoveVector.X > 1) { MoveVector.X = 1; }
            if (MoveVector.X < -1) { MoveVector.X = -1; }
            if (MoveVector.Y > 1) { MoveVector.Y = 1; }
            if (MoveVector.Y < -1) { MoveVector.Y = -1; }

            // Update the player position with the Move Vector
            Position += MoveVector * MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds; 
            GetTilesAround();
            CheckForSideIntersection();
 
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
            PlayerLife -= bullet.Damage;
            BlendColor.R = 255;
            BlendColor.G = 0;
            BlendColor.B = 0;
 
        }

        void UpdateGravity(GameTime gameTime)
        {
            try
            {
                Rectangle FixedArea = new Rectangle(Area.X - 16, Area.Y + 1, 32, 32);
                Rectangle FixedAreaLeft = new Rectangle(Area.X - 14, Area.Y + 1, 32, 32);
                Rectangle AreaSinas = new Rectangle(Area.X - 16, Area.Y + 1, 32, 32);

                bool ColidingBottom = (TileBottom.GetArea().Intersects(FixedArea) && TileBottom.IsColideable);
                bool ColidingBottomLeft = (TileBottomLeft.GetArea().Intersects(FixedAreaLeft) && TileBottomLeft.IsColideable);
                bool ColidingBottomRight = (TileBottomRight.GetArea().Intersects(FixedArea) && TileBottomRight.IsColideable);

                if (ColidingBottom) { Position.Y = TileBottom.GetArea().Y - 32; }
                if (ColidingBottomLeft) { Position.Y = TileBottomLeft.GetArea().Y - 32; }
                if (ColidingBottomRight) { Position.Y = TileBottomRight.GetArea().Y - 32; }


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
                    float Force = (ParentMap.MapEnvironment.Gravity * GravityMultiplier) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Position.Y += Force;

                    GravityMultiplier += 16f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                }

            }catch(System.Collections.Generic.KeyNotFoundException)
            {
                Position = Vector2.Zero;
            }

        }

        void EndJumping()
        {
            JumpProgress = 16f;
            IsJumping = false;
            JumpAvailable = true;
            JumpFinished = false;
        }
        
        void UpdateJump(GameTime gameTime)
        {
            if (IsJumping && !JumpAvailable)
            { 
                JumpProgress += MoveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (JumpProgress > JumpMultiplier) 
                {
                    JumpProgress = JumpMultiplier;
                }

                Position.Y -= (ParentMap.MapEnvironment.Gravity * JumpProgress) * (float)gameTime.ElapsedGameTime.TotalSeconds;                
            }
        }

        void SetUpJump(GameTime gameTime)
        {
            Position.Y -= (ParentMap.MapEnvironment.Gravity * JumpProgress) * (float)gameTime.ElapsedGameTime.TotalSeconds;  
            IsJumping = true; 
            JumpAvailable = false; 
            JumpProgress = 12f;

        }

        void GetTilesAround()
        {
            if (Position.X < 32) { Position.X = 32; }
            if (Position.Y < 32) { Position.Y = 32; }
            if (Position.X > ParentMap.MapEnvironment.WorldSize.X - 32) { Position.X = ParentMap.MapEnvironment.WorldSize.X - 32; }
            if (Position.Y > ParentMap.MapEnvironment.WorldSize.Y - 32) { Position.Y = ParentMap.MapEnvironment.WorldSize.Y - 32; }

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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (BlendColor.G < 255)
            {
                BlendColor.G += Convert.ToByte((2f * (float)gameTime.ElapsedGameTime.TotalSeconds) * 255);
            }            

            if (BlendColor.B < 255)
            {
                BlendColor.B += Convert.ToByte((2f * (float)gameTime.ElapsedGameTime.TotalSeconds) * 255);
            }            


            MoveSpeed -= 38 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (MoveSpeed < MinimumSpeed) { MoveSpeed = MinimumSpeed; }
            if (MoveSpeed > 128) { MoveSpeed = 128; }
            
            AimVector = -(Position - (MouseInput.PositionVector2 - ParentMap.camera.CameraPosition));


            GetTilesAround();

            UpdateGravity(gameTime);
            GetTilesAround();

            UpdateJump(gameTime);
            GetTilesAround();

            UpdateInput(gameTime);
            GetTilesAround();
 
        }
    }
}