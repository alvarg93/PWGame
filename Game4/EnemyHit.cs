using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    public class EnemyHit : PlayerHit
    {
        string hitID;

        public EnemyHit(string hitID, Rectangle casterRect, Vector2 direction, GameplayScreen gpScreen, int dmgModifier) : base(hitID, casterRect, direction, gpScreen, dmgModifier) { }

        private bool dmgTaken = false;
        private bool lastFrame = false;
        public override void Update(GameTime gameTime, InputManager inputManager, GameplayScreen gpScreen, Map map, Layer layer)
        {
            if (lastFrame && moveAnimation.CurrentFrame.X == 0) Dead = true;
            if (!dmgTaken)
            {
                dmgTaken = true;
                if (Collision.CheckCollision(new Rectangle((int)Position.X, (int)Position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight),
                    new Rectangle((int)gpScreen.Player.Position.X, (int)gpScreen.Player.Position.Y, gpScreen.Player.moveAnimation.FrameWidth, gpScreen.Player.moveAnimation.FrameHeight)))
                {
                    gpScreen.Player.TakeDamage(damage + dmgModifier);
                }
                
            }

            moveAnimation.Position = Position;
            moveAnimation.IsActive = true;
            ssAnimation.Update(gameTime, ref moveAnimation);
            if (moveAnimation.CurrentFrame.X == 2) lastFrame = true;

        }
    }
}
