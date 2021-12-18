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


        public Shovel(GameWorld World, int InventoryIndex) : base(World, InventoryIndex)
        {
            this.World = World;
            IconTexture = Graphics.Sprites.GetSprite("/items/shovel.png");
            IngameTexture = Graphics.Sprites.GetSprite("/items/ingame/shovel.png");
            StackSize = 8;
            Name = "Shovel";
            Description = "Dig... dig... dig...";
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
                            PlaceableTileItem tileItem = new PlaceableTileItem(World, tile.TileInformation);
                            World.Entities.Add(new PickableItem(World, tileItem, context.UseWorldPosition * 32)); 

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