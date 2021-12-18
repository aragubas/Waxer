using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Waxer.GameLogic.Items
{
    public class Shovel : Item
    {
        public Vector2 sinasPos = Vector2.Zero;
        public Texture2D IngameTexture;
        bool UseAnimation = false;
        float Rotation = 0f;
        bool RightSide = true;


        public Shovel(GameWorld World) : base(World)
        {
            this.World = World;
            IconTexture = Graphics.Sprites.GetSprite("/items/shovel.png");
            IngameTexture = Graphics.Sprites.GetSprite("/items/ingame/shovel.png");
            StackSize = 8;
            Name = "Shovel";
        }

        public override void DoAction(ItemUseContext context)
        {
            if (context.ActionMouseButton == MouseButton.Left_Up)
            {
                sinasPos = context.PlayerScreenPosition;

                if (IsInRange(context.PlayerScreenPosition, context.MousePosition))
                {
                    MapTile tile = context.World.GetTile(context.World.GetTilePosition(context.MousePosition - context.World.Camera.CameraPosition));
                    
                    if (tile != null)
                    {
                        if (tile.TileInformation.IsColideable)
                        {
                            bool QuantityIncreased = false;

                            foreach(Item item in World.Player.Inventory)
                            {
                                if (item.GetType() == typeof(PlaceableTileItem))
                                {
                                    PlaceableTileItem convertedItem = (PlaceableTileItem)item;
                                     
                                    // A item placeable tile item has been found with the same TileID, increase quantity
                                    if (convertedItem.PlaceableTileInfo.TileID == tile.TileInformation.TileID)
                                    {
                                        QuantityIncreased = convertedItem.IncreaseQuantity(1);
                                    }
                                }
                            }

                            if (!QuantityIncreased)
                            {
                                World.Player.Inventory.Add(new PlaceableTileItem(World, tile.TileInformation));
                            }

                            context.World.SetTile(tile.TilePosition, TilesInfo.TileInfos[0]);
                            SetUpUseAnimation();
                            RightSide = (context.MousePosition - context.PlayerScreenPosition).X >= 0;

                        }
                    }
                }
            
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (UseAnimation)
            {   
                SpriteEffects effects = SpriteEffects.None;
                float OldRotation = Rotation;
                
                if (!RightSide)
                {
                    effects = SpriteEffects.FlipHorizontally;       
                    Rotation = -Rotation;
                } 

                spriteBatch.Draw(IngameTexture, World.Player.Position + World.Camera.CameraPosition, null, Color.White, MathHelper.ToRadians(Rotation), new Vector2(IngameTexture.Width / 2, IngameTexture.Height / 2), 1f, effects, 1f);

                Rotation = OldRotation; 
            }
        }
        
        public override void Deactivate()
        {
            UseAnimation = false;
            Rotation = 0;
        }

        void SetUpUseAnimation()
        {
            UseAnimation = true;
            Rotation = 0;
        }

        public override void Update(float delta)
        {
            if (UseAnimation)
            {
                Rotation += 500 * delta;

                if (Rotation >= 100)
                {
                    UseAnimation = false;
                    Rotation = 0;
                }
            }

            
        }
        
    }
}