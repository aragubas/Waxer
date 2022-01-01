/*
    Copyright(C) Aragubas - All Rights Reserved
    Unauthorized copying of this file, via any media such as Videos, Screenshots or Copy/Paste is strictly prohibited
    Propietary and Confidential
    Only those who are envolved in production of this project may modify the Source Code, but not distribute it.
    Written by Paulo Otávio <vaiogames18@gmail.com> or <dpaulootavio5@outlook.com>, December 24, 2021
*/

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
        bool _useAnimation = false;
        float _rotation = 0f;
        bool _rightSide = true;
        string _lastTileGUID = "";
        float _breakTime = 0;
        float _rotationSpeed = 400;
 
        public Shovel(GameWorld World, int InventoryIndex) : base(World, InventoryIndex)
        {
            this.World = World;
            IconTexture = Graphics.Sprites.GetSprite("/items/shovel.png");
            IngameTexture = Graphics.Sprites.GetSprite("/items/ingame/shovel.png");
            StackSize = 8;
            Name = "Shovel";
            Description = "Dig... dig... dig...";
        }
 
        public override void DoAction(ItemUseContext context, float delta)
        {
            if (context.ActionMouseButton == MouseButton.Left_Down || context.ActionMouseButton == MouseButton.MouseHover)
            {
                sinasPos = context.PlayerScreenPosition;

                if (IsInRange(context.PlayerScreenPosition, context.MousePosition))
                {
                    MapTile tile = context.World.GetTile(context.World.GetTilePosition(context.MousePosition - context.World.Camera.CameraPosition));
                     
                    if (tile != null)
                    { 
                        if (tile.TileUID != _lastTileGUID)
                        {
                            _breakTime = 0;
                            _lastTileGUID = "";
                            StopUseAnimation();

                        }

                        if (tile.TileInformation.IsColideable && context.ActionMouseButton == MouseButton.Left_Down)
                        {
                            if (_lastTileGUID != tile.TileUID)
                            {
                                _lastTileGUID = tile.TileUID;
                                _breakTime = 0; 
                                SetUpUseAnimation();

                            }

                            if (_lastTileGUID == tile.TileUID)
                            {
                                _breakTime += delta * 1;
                                
                                _rightSide = (context.MousePosition - context.PlayerScreenPosition).X >= 0; 

                                if (_breakTime >= tile.TileInformation.BreakTime)
                                {
                                    PlaceableTileItem tileItem = new PlaceableTileItem(World, tile.TileInformation);
                                    World.Entities.Add(new PickableItem(World, tileItem, context.UseWorldPosition * 32)); 

                                    context.World.SetTile(tile.TilePosition, TilesInfo.TileInfos[0]);
                                    
                                    _breakTime = 0;
                                    _lastTileGUID = "";
                                    StopUseAnimation();
                                }
 
                            }

                        }
                    }
                }
            
            } 
            
            if (context.ActionMouseButton == MouseButton.Left_Up)
            {
                StopUseAnimation();
            }


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_useAnimation)
            {   
                SpriteEffects effects = SpriteEffects.None;
                float OldRotation = _rotation;
                
                if (!_rightSide)
                {
                    effects = SpriteEffects.FlipHorizontally;       
                    _rotation = -_rotation;
                } 

                spriteBatch.Draw(IngameTexture, World.Player.Position + World.Camera.CameraPosition, null, Color.White, MathHelper.ToRadians(_rotation), new Vector2(IngameTexture.Width / 2, IngameTexture.Height / 2), 1f, effects, 1f);

                _rotation = OldRotation; 
            }
        }
        
        public override void Deactivate()
        {
            _useAnimation = false;
            _rotation = 0;
        }

        void SetUpUseAnimation()
        {
            _useAnimation = true;
            _rotation = 0;
        }

        void StopUseAnimation()
        {
            _useAnimation = false;
            _rotation = 0;
            _breakTime = 0;
        }
 
        public override void Update(float delta)
        {
            if (_useAnimation)
            {
                _rotation += _rotationSpeed * delta;

                if (_rotation >= 90)
                {
                    //UseAnimation = false;
                    _rotationSpeed = -800;
                }
                
                if (_rotation <= 0)
                {
                    _rotationSpeed = 500;

                }
            }

            
        }
        
    }
}