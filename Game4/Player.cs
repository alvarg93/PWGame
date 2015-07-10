using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    public class Player : Character
    {
        List<Entity> miscObjects;
        int range = 30;
        protected HealthBar hpBar;

        public List<Entity> MiscObjects
        {
            get { return miscObjects; }
            set { miscObjects = value; }
        }

        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            base.LoadContent(content, inputManager);
            fileManager = new FileManager();
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            moveSpeed = 150;
            Vector2 tempFrames= Vector2.Zero;

            fileManager.LoadContent("Load/Player.cme", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Health":
                            Health = int.Parse(contents[i][j]);
                            Console.WriteLine("My health = " + Health);
                            break;
                        case "Frames":
                            string[] frames = contents[i][j].Split(' ');
                            tempFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "Image":
                            Console.WriteLine(range);
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                        case "Range":
                            range = int.Parse(contents[i][j]);
                            Console.WriteLine(range);
                            break;
                        case "Position":
                            string[] positions = contents[i][j].Split(' ');
                            Position = new Vector2(int.Parse(positions[0]), int.Parse(positions[1]));
                            break;
                    }
                }
            }
            moveAnimation.Frames = new Vector2(4, 4);
            moveAnimation.LoadContent(content, image, "", Position);
            hpBar = new HealthBar(content, Health, Position + new Vector2(0, moveAnimation.FrameHeight));
            hpBar.SetHealth(Health);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager inputManager, GameplayScreen gpScreen, Map map, Layer layer)
        {
            if (Health <= 0) Dead = true;
            if (inputManager.KeyPressed(Keys.Space))
            {
                Attack(gpScreen, "PlayerHit");
            }

            if (inputManager.KeyPressed(Keys.V))
            {
                Attack(gpScreen, "PlayerMeleeHit");
            }
            if (inputManager.KeyPressed(Keys.Y))
            {
                TakeDamage(40);
            }



            moveAnimation.IsActive = true;
            bool keyDown = false;
            Vector2 testPosition = Position;
            if (inputManager.KeyDown(Keys.Right))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                testPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                keyDown = true;
            }
            if (inputManager.KeyDown(Keys.Left))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                testPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                keyDown = true;
            }
            if (inputManager.KeyDown(Keys.Up))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                testPosition.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                keyDown = true;
            }
            if (inputManager.KeyDown(Keys.Down))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                testPosition.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                keyDown = true;
            }
            if(!keyDown)
            {
                moveAnimation.IsActive = false;
            }
            List<Collision.Type> collisionCheck = new List<Collision.Type>();
            foreach (List<Tile> tilesRow in layer.Tiles)
                foreach (Tile tile in tilesRow)
                {
                    if (tile.State1 == Tile.State.Solid)
                    {
                        if(Collision.CheckCollision(
                            new Rectangle((int)testPosition.X, (int)testPosition.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight),
                            new Rectangle((int)tile.Position.X, (int)tile.Position.Y, tile.TileImage.Width, tile.TileImage.Height)))
                        {
                            if (Position.X > tile.Position.X - moveAnimation.FrameWidth && Position.X < tile.Position.X + tile.TileImage.Width)
                                testPosition.Y = Position.Y;
                            if (Position.Y > tile.Position.Y - moveAnimation.FrameHeight && Position.Y < tile.Position.Y + tile.TileImage.Height)
                                testPosition.X = Position.X;
                        } 

                        collisionCheck.Clear();
                    }
                }


            if (testPosition.X < 0) testPosition.X = 0;
            if (testPosition.Y < 0) testPosition.Y = 0;
            if (testPosition.X > layer.Bounds.X - moveAnimation.FrameWidth) testPosition.X = layer.Bounds.X - moveAnimation.FrameWidth;
            if (testPosition.Y > layer.Bounds.Y - moveAnimation.FrameHeight) testPosition.Y = layer.Bounds.Y - moveAnimation.FrameHeight;
            Position = testPosition;

            moveAnimation.Position = Position;
            ssAnimation.Update(gameTime, ref moveAnimation);

            Camera.Instance.SetFocalPoint(Position);
        }

        void Attack(GameplayScreen gpScreen, string attackID)
        {
            int dir = (int)moveAnimation.CurrentFrame.Y;
            PlayerHit pH;
            Vector2 direction;
            switch (dir)
            {
                case 0:
                    direction = new Vector2(0, 1);
                    break;
                case 1:
                    direction = new Vector2(-1, 0);
                    break;
                case 2:
                    direction = new Vector2(1, 0);
                    break;
                default:
                    direction = new Vector2(0, -1);
                    break;
            }
            pH = new PlayerHit(attackID, new Rectangle((int)Position.X, (int)Position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight), direction, gpScreen, AttackModifier);
            pH.LoadContent(content, inputManager);
            gpScreen.MiscObjects.Add(pH);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            hpBar.SetHealth(Health);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            hpBar.Draw(spriteBatch,Position + new Vector2(0, moveAnimation.FrameHeight));
        }
    }
}
