using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace Game4
{
    public class Entity
    {
        private int health = 100;
        public Rectangle MyArea
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }
        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        protected int damage;
        protected float moveSpeed;
        public Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected FileManager fileManager;

        protected ContentManager content;
        protected InputManager inputManager;
        protected List<List<string>> attributes, contents;

        protected Texture2D image;
        private Vector2 position;

        bool dead = false;

        public bool Dead
        {
            get { return dead; }
            set { dead = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual void LoadContent(ContentManager content, InputManager inputManager)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");

            contents = new List<List<string>>();
            attributes = new List<List<string>>();
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }
        public virtual void Update(GameTime gameTime, InputManager inputManager, GameplayScreen gpScreen, Map map, Layer layer)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public virtual void TakeDamage(int damage)
        {
            Health = Health - damage;
        }

        public bool InCameraRange()
        {
            return (Camera.Instance.FocalPoint - this.Position).Length() < ScreenManager.Instance.Dimensions.Length()/2;
        }
    }
}
