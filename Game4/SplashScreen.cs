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
        List<FadeAnimation> fade;
        List<Texture2D> images;

        FileManager fileManager;

        int imageNumber = 0;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = content.Load<SpriteFont>("Font1");

            fileManager = new FileManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.cme", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(content.Load<Texture2D>(contents[i][j]));
                            fade.Add(new FadeAnimation());
                            break;
                    }
                }
            }

            for (int i = 0; i < fade.Count; i++)
            {
                fade[i].LoadContent(content, images[i], "", Vector2.Zero);
                fade[i].IsActive = true;
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
            if (fade[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if(imageNumber<fade.Count)
            fade[imageNumber].Update(gameTime);


            if (imageNumber>=fade.Count || inputManager.KeyPressed(Keys.Z))
                ScreenManager.Instance.AddScreen(new TitleScreen(),inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (imageNumber < fade.Count)
                fade[imageNumber].Draw(spriteBatch);
        }

    }
}
