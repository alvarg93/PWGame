using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game4
{
    public class SpriteSheetAnimation : Animation
    {
        private int frameCounter;
        private int switchFrame;

        public SpriteSheetAnimation()
        {
            frameCounter = 0;
            switchFrame = 100;
        }

        public override void Update(GameTime gameTime, ref Animation a)
        {
            Vector2 curFrame = a.CurrentFrame;

            if (a.IsActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    curFrame.X++;
                    if (curFrame.X * a.FrameWidth >= a.Image.Width) curFrame.X = 0;

                }
            }
            else
            {
                frameCounter = 0;
                curFrame.X = 0;
            }

            a.CurrentFrame = curFrame;
            a.SourceRect = new Rectangle((int)a.CurrentFrame.X * a.FrameWidth, (int)a.CurrentFrame.Y * a.FrameHeight, a.FrameWidth, a.FrameHeight);
        }

    }
}
