using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    public class Animation
    {
        private Texture2D image;

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        private string text;
        private SpriteFont font;
        private Color color;
        private Rectangle sourceRect;

        public Rectangle SourceRect
        {
            get { return sourceRect; }
            set { sourceRect = value; }
        }
        private float rotation, scale, axis, alpha;
        private Vector2 origin, position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        private ContentManager content;
        private bool isActive;

        public void LoadContent(ContentManager Content, Texture2D image, string text, Vector2 position)
        {

            this.content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = image;
            this.text = text;
            this.position = position;
            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Font1");
                color = Color.White;
            }
            rotation = 0.0f;
            axis = 0.0f;
            scale = 1.0f;
            alpha = 1.0f;
            isActive = false;

            if(frames==Vector2.Zero) frames = new Vector2(1, 1);
            currentFrame = new Vector2(0, 0);
            if(image!=null)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
        }

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value;}
        }


        private Vector2 frames;
        private Vector2 currentFrame;

        public int FrameWidth { get { return image.Width / (int)frames.X; } }

        public int FrameHeight { get { return image.Height / (int)frames.Y; } }

        public Vector2 CurrentFrame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public Vector2 Frames
        {
            get { return frames; }
            set { frames = value; }
        }

        public void UnloadContent()
        {
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;

        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {

        }

        public void Draw(SpriteBatch spriteBatch) {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                spriteBatch.Draw(image, position + origin, sourceRect, Color.White * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }
            else
            {
                Console.WriteLine("IMAGE NULL!!1");
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin, color * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }
        }
    }
}
