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

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }

        Dictionary<string, GameScreen> gameScreens = new Dictionary<string, GameScreen>();

        Stack<GameScreen> screenStack = new Stack<GameScreen>();
        
        GameScreen currentScreen;
        
        GameScreen newScreen;
        
        Vector2 dimensions;

        bool transition;

        Animation animation;
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
            animation = new Animation();
            fade = new FadeAnimation();
        }
        public void LoadContent(ContentManager Content) {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content,inputManager);

            nullImage = this.content.Load<Texture2D>("null");
            fadeTexture = this.content.Load<Texture2D>("fade");
            animation.LoadContent(content, fadeTexture, "",Vector2.Zero);
            animation.Scale = dimensions.X;
        }
        public void Update(GameTime gameTime) {
            Camera.Instance.Update();
            if (!transition)
                currentScreen.Update(gameTime);
            else Transition(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (transition)
            {
                animation.Draw(spriteBatch);
            }
        }
        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            Camera.Instance.SetFocalPoint(new Vector2(Dimensions.X / 2, Dimensions.Y / 2));
            transition = true;
            newScreen = screen;
            animation.IsActive = true;
            animation.Alpha = 0.0f;
            fade.Increase = true;
            fade.ActivateValue = 1.0f;
            this.inputManager = inputManager;
        }
        public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
        {
            Camera.Instance.SetFocalPoint(new Vector2(Dimensions.X / 2, Dimensions.Y / 2));
            transition = true;
            newScreen = screen;
            animation.IsActive = true;
            fade.ActivateValue = 1.0f;
            animation.Alpha = alpha;
            fade.Increase = true;
            this.inputManager = inputManager;
        }
        #endregion

        #region PrivateMethods

        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime,ref animation);
            if (animation.Alpha == 1.0 && fade.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, inputManager);
                Console.WriteLine("Transition has finished");
            }
            else if (animation.Alpha == 0.0f)
            {
                transition = false;
                animation.IsActive = false;
            }
        }

        #endregion
    }
}
