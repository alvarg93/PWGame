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
    public class PlayerHit : Entity
    {
        string hitID;
        protected int dmgModifier=0,range=10;
        GameplayScreen gpScreen;
        Vector2 direction;
        Rectangle casterRect;
        string[] hitSounds = { "Whooosh!", "Booom", "GRbhrg!!!" };
        int hitSoundsCnt = 0;

        public string HitID
        {
            get { return hitID; }
            set { hitID = value; }
        }

        public PlayerHit(string hitID, Rectangle casterRect, Vector2 direction, GameplayScreen gpScreen, int dmgModifier)
        {
            fileManager = new FileManager();
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            moveSpeed = 150;
            this.hitID = hitID;
            this.casterRect = casterRect;
            this.direction = direction;
            this.gpScreen = gpScreen;
            this.dmgModifier = dmgModifier;
        }


        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            base.LoadContent(content, inputManager);

            Vector2 totalFrames = Vector2.Zero;
            Vector2 modelFrames = Vector2.Zero;
            Vector2 modelFramesOffset = Vector2.Zero;
            fileManager.LoadContent("Load/" + hitID + ".cme", attributes, contents);
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
                        case "Damage":
                            damage = int.Parse(contents[i][j]);
                            break;
                        case "Range":
                            range = int.Parse(contents[i][j]);
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

            Position = (new Vector2(casterRect.X + casterRect.Width / 2, casterRect.Y + casterRect.Height / 2) + range * direction) - new Vector2(cellWidth/2,cellHeight/2);

            moveAnimation.Frames = modelFrames;
            moveAnimation.LoadContent(content, image, hitSounds[GlobalRandom.Instance.Next(0, 3)], Position);
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



        private bool dmgTaken = false;
        private bool lastFrame = false;
        public override void Update(GameTime gameTime, InputManager inputManager, GameplayScreen gpScreen, Map map, Layer layer)
        {
            if (lastFrame && moveAnimation.CurrentFrame.X == 0) Dead = true;
            if (!dmgTaken)
            {
                dmgTaken = true;

                for (int i = 0; i < gpScreen.Enemies.Count; i++)
                {
                    Enemy enemy = gpScreen.Enemies[i];
                    if(!enemy.Dead)
                    if (Collision.CheckCollision(new Rectangle((int)Position.X, (int)Position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight), 
                        new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.moveAnimation.FrameWidth, enemy.moveAnimation.FrameHeight)))
                    {
                        enemy.TakeDamage(damage+dmgModifier);
                        Console.WriteLine(enemy.EnemyID + " " + enemy.Health);
                    }
                }
            }

            moveAnimation.Position = Position;
            moveAnimation.IsActive = true;
            ssAnimation.Update(gameTime, ref moveAnimation);
            if (moveAnimation.CurrentFrame.X == 2) lastFrame = true;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
        }
    }
}
