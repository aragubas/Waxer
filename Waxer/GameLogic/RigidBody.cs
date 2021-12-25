/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

using Microsoft.Xna.Framework;

namespace Waxer.GameLogic
{
    public class RigidBody : WorldEntity
    {
        internal float GravityMultiplier = 0f; 
        internal MapTile TileBehind = null;
        internal MapTile TileRight = null;
        internal MapTile TileLeft = null;
        internal MapTile TileTop = null;
        internal MapTile TileBottom = null;
        internal MapTile TileTopLeft = null;
        internal MapTile TileTopRight = null;
        internal MapTile TileBottomLeft = null;
        internal MapTile TileBottomRight = null;


        // Physics
        // Get all tiles around the player, including all quadrants
        //
        void GetTilesAround()
        {
            // Tile Behind
            MapTile newTile = World.GetTile(World.GetTilePosition(Position));
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
        // Update Gravity
        //
        void UpdateGravity(float delta)
        {
            Vector2 Size = new Vector2(32, 32);
            if (AreaSize != new Vector2(-1, -1))
            {
                Size = AreaSize;
            }
            
            Rectangle FixedArea = new Rectangle(Area.X - (int)SpriteOrigin.X, Area.Y + (int)World.WorldEnvironment.Gravity, (int)Size.X, (int)Size.Y);

            bool ColidingBottom = (TileBottom.GetArea().Intersects(FixedArea) && TileBottom.TileInformation.IsColideable);
            bool ColidingBottomLeft = (TileBottomLeft.GetArea().Intersects(FixedArea) && TileBottomLeft.TileInformation.IsColideable);
            bool ColidingBottomRight = (TileBottomRight.GetArea().Intersects(FixedArea) && TileBottomRight.TileInformation.IsColideable);
 
            if (ColidingBottom) { Position.Y = TileBottom.GetArea().Y - AreaSize.Y; UpdateArea(); }

            if (ColidingBottom || ColidingBottomLeft || ColidingBottomRight)
            {
                // Object hits the ground
                GravityMultiplier = 0f;


            }else  
            { 
                // Pulls the object to the ground, to simulate gravity
                float Force = (World.WorldEnvironment.Gravity * GravityMultiplier) * delta;
                Position.Y += Force;
                UpdateArea();

                GravityMultiplier += 16f * delta;
                
            }

        }


        internal void UpdateBody(float delta)
        {   
            if (Position.X < 32) { Position.X = 32; UpdateArea(); }
            if (Position.Y < 32) { Position.Y = 32; UpdateArea(); }
            UpdateArea(); 

            GetTilesAround();
            UpdateGravity(delta);

        }

    }
}