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
    public class ScreenManager
    {
        #region Variables
        
        private static ScreenManager instance;

        ContentManager content;

        Dictionary<string, GameScreen> gameScreens = new Dictionary<string, GameScreen>();

        Stack<GameScreen> screenStack = new Stack<GameScreen>();
        
        GameScreen currentScreen;
        
        GameScreen newScreen;
        
        Vector2 dimensions;

        bool transition;

        FadeAnimation fade;

        InputManager inputManager;

        Texture2D fadeTexture;
        Texture2D nullImage;

        
        #endregion
        #region Properties
        public static ScreenManager Instance
        {
            get{
                if(instance==null) instance = new ScreenManager();
                return instance;
            }
        }
        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }
            set { nullImage = value; }
        }
        #endregion

        #region MainMethods
        public void Initialize() {
            currentScreen = new SplashScreen();
            inputManager = new InputManager();
            fade = new FadeAnimation();
        }
        public void LoadContent(ContentManager Content) {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content,inputManager);

            nullImage = this.content.Load<Texture2D>("null");
            fadeTexture = this.content.Load<Texture2D>("fade");
            fade.LoadContent(content, fadeTexture, "",Vector2.Zero);
            fade.Scale = dimensions.X;
        }
        public void Update(GameTime gameTime) {
            if (!transition)
                currentScreen.Update(gameTime);
            else Transition(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch) {
            currentScreen.Draw(spriteBatch);
            if (transition)
                fade.Draw(spriteBatch);
        }
        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateValue = 1.0f;
            this.inputManager = inputManager;
        }
        public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.ActivateValue = 1.0f;
            fade.Alpha = alpha;
            fade.Increase = true;
            this.inputManager = inputManager;
        }
        #endregion

        #region PrivateMethods

        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime);
            if (fade.Alpha == 1.0 && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, inputManager);
            }
            else if (fade.Alpha == 0.0f)
            {
                transition = false;
                fade.IsActive = false;
            }
        }

        #endregion
    }
}
