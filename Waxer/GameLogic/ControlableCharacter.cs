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
            if (!Settings.DebugMode) { return; }
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
                NextPos.X -= 1f;
            }

            if (Utils.CheckKeyDown(oldState, newState, Keys.LeftShift))
            {
                MoveSpeed *= Acceleration;
            }
 
            Force = NextPos;

            oldState = newState;
        }

        Vector2 Force;


        void UpdateAppliedForces(float delta)
        {
            Force = Utils.ClampVector(Force, -1, 1); 
            Vector2 ForceMultiplied = Force * MoveSpeed;
            Vector2 NextStep = Position + (ForceMultiplied * delta);

            //GetTilesAround(NextStep);

            // Right colision
            for(int i = 0; i < 16; i++)
            {
                MapTile rightTile = World.GetTile(World.GetTilePosition(NextStep + new Vector2(i, 0)));
                Rectangle rightArea = new Rectangle(((int)Position.X + 16) + i, (int)Position.Y, Area.Width, Area.Height);
 
                if (rightTile.TileInformation.IsColideable && rightTile.GetArea().Intersects(rightArea))
                {
                   NextStep = new Vector2(rightTile.GetArea().X - 16, Position.Y);
                   Console.WriteLine($"Right Colision at iteration {i}");
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
                   NextStep = new Vector2(leftTile.GetArea().Right + 16, Position.Y);
                   Console.WriteLine($"Left Colision at iteration {i}");
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
            UpdateMoveSpeed(delta);
            UpdateAppliedForces(delta);

            LastDelta = delta;

        }

    }
}