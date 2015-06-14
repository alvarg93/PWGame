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
    public class SplashScreen : GameScreen
    {
        SpriteFont font;
        List<Animation> animations;
        List<Texture2D> images;

        FileManager fileManager;

        FadeAnimation fade;

        int imageNumber = 0;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");

            fileManager = new FileManager();
            animations = new List<Animation>();
            fade = new FadeAnimation();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.cme", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            animations.Add(new Animation());
                            break;
                    }
                }
            }

            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].LoadContent(content, images[i], "", Vector2.Zero);
                animations[i].IsActive = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            if (imageNumber >= animations.Count || inputManager.KeyPressed(Keys.Escape))
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            else
            {
                Animation anim = animations[imageNumber];
                fade.Update(gameTime, ref anim);
                animations[imageNumber] = anim;

                Console.WriteLine(imageNumber + " " + animations.Count);
                if (animations[imageNumber].Alpha == 0.0f)
                {
                    imageNumber++;
                    fade = new FadeAnimation();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (imageNumber < animations.Count)
                animations[imageNumber].Draw(spriteBatch);
        }

    }
}
