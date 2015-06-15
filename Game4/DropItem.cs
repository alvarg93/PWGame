using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    public class DropItem : Entity
    {
        string type;
        int value;

        public DropItem(ContentManager content, Vector2 position, string type, int value)
        {
            moveAnimation = new Animation();
            this.type = type;
            this.value = value;
            Position = position;
            this.content = new ContentManager(content.ServiceProvider, "Content");
            image = this.content.Load<Texture2D>(type + "Drop");
            Console.WriteLine("New drop! " + type + " = " + value);
            moveAnimation.LoadContent(this.content, image, "", position);
        }

        public override void Update(GameTime gameTime, InputManager inputManager, GameplayScreen gpScreen, Map map, Layer layer)
        {
            base.Update(gameTime, inputManager, gpScreen, map, layer);
            Player player = new Player();
            if (gpScreen.Player.MyArea.Intersects(MyArea))
                PickUp(ref player);
            else
            {
                for (int i = 0; i < gpScreen.Enemies.Count; i++)
                {
                    Enemy enemy = gpScreen.Enemies[i];
                    if (!enemy.Dead)
                    {
                        if (enemy.MyArea.Intersects(MyArea))
                        {
                            PickUp(ref enemy);
                            break;
                        }
                    }
                }
            }
        }

        public override void UnloadContent()
        {
        }

        public void PickUp(ref Player c)
        {
            Console.WriteLine("Player picked up " + type);
            switch (type)
            {
                case "HP":
                    c.Health = c.Health + value;
                    break;
                case "STR":
                    c.AttackModifier += value;
                    break;
            }
            Dead = true;
        }
        public void PickUp(ref Enemy c)
        {
            Console.WriteLine("Enemy picked up " + type);
            switch (type)
            {
                case "HP":
                    c.Health = c.Health + value;
                    break;
                case "STR":
                    c.AttackModifier += value;
                    break;
            }
            Dead = true;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!Dead)
                moveAnimation.Draw(spriteBatch);
        }


    }
}
