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
    public class GameplayScreen : GameScreen
    {
        List<Entity> miscObjects;

        public List<Entity> MiscObjects
        {
            get { return miscObjects; }
            set { miscObjects = value; }
        }

        Player player;

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        Map map;
        List<Enemy> enemies;

        public List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            miscObjects = new List<Entity>();
            player = new Player();
            map = new Map();
            map.Player = player;
            map.LoadContent(Content, "Map1");
            player.LoadContent(Content, inputManager);
            player.Position = map.StartingPoint;
            player.MiscObjects = miscObjects;
            enemies = new List<Enemy>();
            Enemy enemy;
            for (int i = 0; i < 10; i++)
            {
                enemy = new Enemy();
                enemy.EnemyID = "Ghost";
                enemy.Position = map.GetRandomFreeSpot();
                enemy.LoadContent(Content, inputManager);
                enemies.Add(enemy);
            }

            for (int i = 0; i < 3; i++)
            {
                enemy = new Enemy();
                enemy.EnemyID = "SubBoss";
                enemy.Position = map.GetRandomFreeSpot();
                enemy.LoadContent(Content, inputManager);
                enemies.Add(enemy);
            }

            enemy = new Enemy();
            enemy.EnemyID = "Boss";
            enemy.Position = map.GetRandomFreeSpot();
            enemy.LoadContent(Content, inputManager);
            enemies.Add(enemy);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            foreach (Enemy enemy in enemies)
            {
                enemy.UnloadContent();
            }
            map.UnloadContent();
        }

        bool gameOver = false;
        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            if (player.Dead || enemies.Count == 0)
            {
                gameOver = true;
                if (player.Dead)
                    Console.WriteLine("Player dead...");
                else
                    Console.WriteLine("You won!");
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
            else
            {
                player.Update(gameTime, inputManager, this, map, map.Layers[0]);

                for (int i = 0; i < miscObjects.Count; i++)
                {
                    Entity misc = miscObjects[i];
                    if (misc.Dead)
                    {
                        misc.UnloadContent();
                        miscObjects.Remove(misc);
                    }
                    else
                        misc.Update(gameTime, inputManager, this, map, map.Layers[0]);
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    Enemy enemy = enemies[i];
                    if (enemy.Dead)
                    {
                        enemies.Remove(enemy);
                    }
                    else
                        enemy.Update(gameTime, inputManager, this, map, map.Layers[0]);
                }
                map.Update(gameTime);
            }
        }



        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            for (int i = 0; i < miscObjects.Count; i++)
            {
                Entity misc = miscObjects[i];
                if (!misc.Dead)
                {
                    misc.Draw(spriteBatch);
                }
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                Enemy enemy = enemies[i];
                if (!enemy.Dead)
                    enemy.Draw(spriteBatch);
            }
                    
            player.Draw(spriteBatch);
            
        }

    }
}
