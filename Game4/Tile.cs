using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game4
{
    public class Tile
    {
        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };

        State state;

        public State State1
        {
            get { return state; }
            set { state = value; }
        }
        Motion motion;
        Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Texture2D tileImage;

        public Texture2D TileImage
        {
            get { return tileImage; }
            set { tileImage = value; }
        }

        float range;
        int counter;
        bool increase;
        float moveSpeed;

        Animation animation;

        public Animation Animation
        {
            get { return animation; }
            set { animation = value; }
        }

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;
            for(int y=tileArea.Y;y<tileArea.Y+tileArea.Height;y++)
                for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                {
                    croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                    index++;
                }

            croppedImage.SetData<Color>(croppedImageData);
            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Vector2 position, Texture2D tileSet, Rectangle tileArea){
            this.state = state;
            this.motion = motion;
            this.position = new Vector2(position.X,position.Y);
            increase = true;

            tileImage = CropImage(tileSet, tileArea);
            range = 50;
            counter = 0;
            moveSpeed = 100;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
        }

        public void Update(GameTime gameTime)
        {
            counter++;
            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }

            if (motion == Motion.Horizontal)
            {
                if (increase)
                    position.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    position.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (motion == Motion.Vertical)
            {
                if (increase)
                    position.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    position.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            animation.Position = position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }

    }
}
