using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer
{
    public class MapTile
    {
        public Vector2 ScreenPosition;
        public Vector2 TilePosition;
        public Texture2D TileTexture;
        public bool IsColideable;
        int TileID = 0;
        Color BlendColor = Color.White;
        public Chunk ParentChunk;

        public MapTile(Chunk ParentChunk, Vector2 TilePosition, Vector2 ScreenPosition, int TileID, bool IsColideable)
        {
            this.TilePosition = TilePosition;
            this.ScreenPosition = ScreenPosition;
            this.IsColideable = IsColideable;
            SetTileID(TileID);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (this.TileTexture == null) { return; }
            spriteBatch.Draw(TileTexture, ScreenPosition, BlendColor);
        }

        public Rectangle GetScreenPosition()
        {
            return new Rectangle((int)(ScreenPosition.X), (int)(ScreenPosition.Y), 32, 32);
        }

        public Rectangle GetArea(Camera2D camera)
        {
            return new Rectangle((int)TilePosition.X + (int)camera.CameraPosition.X,
                                (int)TilePosition.Y + (int)camera.CameraPosition.Y, 32, 32);
        }

        public Rectangle GetArea()
        {
            return new Rectangle((int)ScreenPosition.X, (int)ScreenPosition.Y, 32, 32);
        }


        public void SetTileID(int newTileID)
        {
            TileID = newTileID;

            // Update the texture
            TileTexture = Graphics.Sprites.GetSprite($"tiles/{TileID}.png");
        }

        public int GetTileID()
        {
            return TileID;
        }
    }

}