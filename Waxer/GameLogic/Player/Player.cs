using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Waxer.GameLogic.Player.Inventory;
using Waxer.Graphics;

namespace Waxer.GameLogic.Player
{
    public class PlayerEntity : ControlableCharacter
    {
        InventoryUI inventoryUI;

        public PlayerEntity(Vector2 initialPosition, Map parentMap)
        {
            Position = initialPosition;
            ParentMap = parentMap;
 
            // Set SpriteOrigin to Bottom Center
            SpriteOrigin = new Vector2(16, 0);

            // Create the InventoryUI Element
            inventoryUI = new InventoryUI();

            Texture = Sprites.MissingTexture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            spriteBatch.End();
            spriteBatch.Begin();
            
            inventoryUI.Draw(spriteBatch);

        }

        public void BulletDamage(Bullet bullet)
        {
            if (bullet.ShooterInstanceID == InstanceID) { return; }
            Life -= bullet.Damage;
            BlendColor.R = 255;
            BlendColor.G = 0;
            BlendColor.B = 0;
 
        }
  

        public override void Update(float delta)
        {
            base.Update(delta);

            UpdateChracter(delta);
        }
    }
}