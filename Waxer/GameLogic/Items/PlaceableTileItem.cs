using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Waxer.GameLogic.Items
{
    public class PlaceableTileItem : Item
    {
        public TileInfo PlaceableTileInfo;

        public PlaceableTileItem(GameWorld World, TileInfo TileInfo) : base(World)
        { 
            PlaceableTileInfo = TileInfo;
            StackSize = 16; 
            IconTexture = TileInfo.Texture;
            Name = $"{TileInfo.Name} (Placeable)";
        }

        public override void DoAction(ItemUseContext context)
        {
            if (context.ActionMouseButton == MouseButton.Right_Up)
            {
                Vector2 FixedMousePosition = World.GetTilePosition(context.MousePosition - context.World.Camera.CameraPosition);
                MapTile tile = context.World.GetTile(FixedMousePosition);
                
                if (tile != null)
                {
                    if (!tile.TileInformation.IsColideable)
                    {
                        World.SetTile(FixedMousePosition, PlaceableTileInfo);
                        Stack--;
 
                        // Item needs deletion
                        if (Stack <= 0)
                        {
                            Dispose();
                            return;
                        }
                    }
                }

            }            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            
        }
    }
}