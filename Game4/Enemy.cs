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
    public class Enemy : Character
    {
        string enemyID;
        int randVal;
        int decisionCounter = 0, decisionCounterLimit = 600;
        int visionRange;
        int dropRate = 10;
        int range = 10;
        protected HealthBar hpBar;

        public int VisionRange
        {
            get { return visionRange; }
            set { visionRange = value; }
        }

        public string EnemyID
        {
            get { return enemyID; }
            set { enemyID = value; }
        }
        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            base.LoadContent(content, inputManager);
            fileManager = new FileManager();
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            moveSpeed = 150;
            Vector2 totalFrames = Vector2.Zero;
            Vector2 modelFrames = Vector2.Zero;
            Vector2 modelFramesOffset = Vector2.Zero;
            VisionRange = 150;

            fileManager.LoadContent("Load/"+enemyID+".cme", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    //[Image][TotalFrames][ModelFrames][ModelFramesOffset][Health][Damage]
                    switch (attributes[i][j])
                    {
                        case "Health":
                            Health = int.Parse(contents[i][j]);
                            break;
                        case "TotalFrames":
                            string[] frames = contents[i][j].Split(' ');
                            totalFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "ModelFrames":
                            frames = contents[i][j].Split(' ');
                            modelFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "ModelFramesOffset":
                            frames = contents[i][j].Split(' ');
                            modelFramesOffset = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "Damage":
                            damage = int.Parse(contents[i][j]);
                            break;
                        case "Range":
                            range = int.Parse(contents[i][j]);
                            break;
                        case "DecisionCounter":
                            decisionCounterLimit = int.Parse(contents[i][j]);
                            break;
                        case "DropRate":
                            dropRate = int.Parse(contents[i][j]);
                            break;
                        case "Image":
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                    }
                }
            }

            int cellHeight = image.Height / (int)totalFrames.Y;
            int cellWidth = image.Width / (int)totalFrames.X;

            Color[] imageData = new Color[image.Width * image.Height];
            image.GetData<Color>(imageData);
            Rectangle sourceRect = new Rectangle(cellWidth * (int)modelFramesOffset.X, cellHeight * (int)modelFramesOffset.Y,
                cellWidth * (int)modelFrames.X, cellHeight * (int)modelFrames.Y);
            Color[] imagePiece = GetImageData(imageData, image.Width, sourceRect);

            Texture2D subtexture = new Texture2D(image.GraphicsDevice, sourceRect.Width, sourceRect.Height);
            image.Dispose();
            image = subtexture;
            subtexture.SetData<Color>(imagePiece);

            moveAnimation.Frames = modelFrames;
            moveAnimation.LoadContent(content, image, enemyID, Position);
            hpBar = new HealthBar(content, Health, Position + new Vector2(0, moveAnimation.FrameHeight));
            hpBar.SetHealth(Health);
        }

        Color[] GetImageData(Color[] colorData, int width, Rectangle rectangle)
        {
            Color[] color = new Color[rectangle.Width * rectangle.Height];
            for (int x = 0; x < rectangle.Width; x++)
                for (int y = 0; y < rectangle.Height; y++)
                    color[x + y * rectangle.Width] = colorData[x + rectangle.X + (y + rectangle.Y) * width];
            return color;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }




        public override void Update(GameTime gameTime, InputManager inputManager, GameplayScreen gpScreen, Map map, Layer layer)
        {
            if (Health <= 0)
            {
                Dead = true;
                DropItem(gpScreen);
            }
            if (InCameraRange())
            {
                moveAnimation.IsActive = true;
                bool keyDown = false;
                DecideDirection(gameTime, gpScreen, map);
                Vector2 testPosition = Position;

                switch (randVal)
                {
                    case 0:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                        testPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 1:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                        testPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 2:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                        testPosition.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 3:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                        testPosition.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 4:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                        testPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        testPosition.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 5:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
                        testPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        testPosition.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 6:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                        testPosition.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        testPosition.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                    case 7:
                        moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                        testPosition.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        testPosition.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        keyDown = true;
                        break;
                }
                if (!keyDown)
                {
                    moveAnimation.IsActive = false;
                }
                foreach (List<Tile> tilesRow in layer.Tiles)
                    foreach (Tile tile in tilesRow)
                    {
                        if (tile.State1 == Tile.State.Solid)
                        {
                            if (Collision.CheckCollision(
                                new Rectangle((int)testPosition.X, (int)testPosition.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight),
                                new Rectangle((int)tile.Position.X, (int)tile.Position.Y, tile.TileImage.Width, tile.TileImage.Height)))
                            {
                                if (Position.X > tile.Position.X - moveAnimation.FrameWidth && Position.X < tile.Position.X + tile.TileImage.Width)
                                    testPosition.Y = Position.Y;
                                if (Position.Y > tile.Position.Y - moveAnimation.FrameHeight && Position.Y < tile.Position.Y + tile.TileImage.Height)
                                    testPosition.X = Position.X;
                            }
                        }
                    }

                if (testPosition.X < 0) testPosition.X = 0;
                if (testPosition.Y < 0) testPosition.Y = 0;
                if (testPosition.X > layer.Bounds.X - moveAnimation.FrameWidth) testPosition.X = layer.Bounds.X - moveAnimation.FrameWidth;
                if (testPosition.Y > layer.Bounds.Y - moveAnimation.FrameHeight) testPosition.Y = layer.Bounds.Y - moveAnimation.FrameHeight;

                Position = testPosition;

                moveAnimation.Position = Position;
                ssAnimation.Update(gameTime, ref moveAnimation);
            }
            else
            {
                moveAnimation.IsActive = false;
            }
        }

        private void DecideDirection(GameTime gameTime, GameplayScreen gpScreen, Map map)
        {
            int dist = (int)(Position - map.Player.Position).Length();
            if(decisionCounter==0)
            if (dist > VisionRange)
            {
                randVal = GlobalRandom.Instance.Next(0, 9);
                
            }
            else
            {
                if (dist<=range) Attack(gpScreen, enemyID + "Hit");
                    if (map.Player.Position.X < Position.X && map.Player.Position.Y < Position.Y)
                    {
                        if (Math.Abs(map.Player.Position.Y - Position.Y) > moveAnimation.FrameHeight)
                            randVal = 5;
                        else randVal = 1;

                    }
                    else if (map.Player.Position.X > Position.X && map.Player.Position.Y < Position.Y)
                    {
                        if (Math.Abs(map.Player.Position.Y - Position.Y) > moveAnimation.FrameHeight)
                            randVal = 4;
                        else randVal = 0;
                    }
                    else if (map.Player.Position.Y > Position.Y)
                    {
                        if (Math.Abs(map.Player.Position.X - Position.X) > moveAnimation.FrameWidth)
                            randVal = 7;
                        else randVal = 3;
                    }
                    else if (map.Player.Position.Y < Position.Y)
                    {
                        if (Math.Abs(map.Player.Position.X - Position.X) > moveAnimation.FrameWidth)
                            randVal = 6;
                        else randVal = 2;
                    }

                decisionCounter+=5;
            }
            decisionCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (decisionCounter > decisionCounterLimit)
            {
                decisionCounter = 0;
            }
        }

        void Attack(GameplayScreen gpScreen, string attackID)
        {
            Console.WriteLine("Attacking! "+attackID);
            int dir = (int)moveAnimation.CurrentFrame.Y;
            EnemyHit pH;
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
            pH = new EnemyHit(attackID, new Rectangle((int)Position.X, (int)Position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight), direction, gpScreen,AttackModifier);
            pH.LoadContent(content, inputManager);
            gpScreen.MiscObjects.Add(pH);
        }

        void DropItem(GameplayScreen gpScreen)
        {
            int randVal = GlobalRandom.Instance.Next(0,dropRate);
            if (randVal < 4)
            {
                DropItem dI;
                switch (randVal)
                {
                    case 0:
                        dI = new DropItem(content, Position, "HP", 10);
                        break;
                    case 1:
                        dI = new DropItem(content, Position, "HP", 30);
                        break;
                    case 2:
                        dI = new DropItem(content, Position, "HP", 50);
                        break;
                    default:
                        dI = new DropItem(content, Position, "STR", 10);
                        break;
                }
                gpScreen.MiscObjects.Add(dI);
            }
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            hpBar.SetHealth(Health);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (InCameraRange())
            {
                moveAnimation.Draw(spriteBatch);
                hpBar.Draw(spriteBatch, Position + new Vector2(0, moveAnimation.FrameHeight));
            }
        }
    }
}
