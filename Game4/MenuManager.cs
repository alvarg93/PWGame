using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4
{
    public class MenuManager
    {
        List<string> menuItems,animationTypes, linkType, linkID;
        List<Texture2D> menuImages;
        List<List<Animation>> animation;

        ContentManager content;

        FileManager fileManager;

        Vector2 position;
        int axis;
        string align;

        List<List<string>> attributes, contents;

        Rectangle source;

        int itemNumber;

        List<Animation> tempAnimation;

        SpriteFont font;

        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
                if (menuImages.Count == i)
                {
                    menuImages.Add(ScreenManager.Instance.NullImage);
                }

            for (int i = 0; i < menuImages.Count; i++)
                if (menuItems.Count == i)
                {
                    menuItems.Add("");
                }
        }

        private void SetAnimations()
        {
            Vector2 pos = Vector2.Zero;
            Vector2 dimensions=Vector2.Zero;

            if (align.Contains("center"))
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    dimensions.X += font.MeasureString(menuItems[i]).X + menuImages[i].Width;
                    dimensions.Y += font.MeasureString(menuItems[i]).Y + menuImages[i].Height;
                }
                if(axis==1) {
                    pos.X = (ScreenManager.Instance.Dimensions.X-dimensions.X)/2; 
                }else if(axis==2) {
                    pos.Y = (ScreenManager.Instance.Dimensions.Y-dimensions.Y)/2; 
                }
            }
            else
            {
                pos = position;
            }

            for (int i = 0; i < menuImages.Count; i++)
            {
                dimensions = new Vector2(font.MeasureString(menuItems[i]).X + menuImages[i].Width, font.MeasureString(menuItems[i]).Y + menuImages[i].Height);

                if (axis == 1)
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                else
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;

                tempAnimation = new List<Animation>();
                for (int j = 0; j < animationTypes.Count; j++)
                {
                    switch (animationTypes[j])
                    {
                        case "Fade":
                            tempAnimation.Add(new FadeAnimation());
                            tempAnimation[tempAnimation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
                            tempAnimation[tempAnimation.Count - 1].Font = font;
                            break;
                    }
                }
                
                if(axis==1) {
                    pos.X+=dimensions.X;
                } else {
                    pos.Y+=dimensions.Y;
                }
                if(tempAnimation.Count>0)
                animation.Add(tempAnimation);
            }
        }

        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            animationTypes = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<List<Animation>>();
            fileManager = new FileManager();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            linkType = new List<string>();
            linkID = new List<string>();
            itemNumber = 0;
            align = "";
            fileManager.LoadContent("Load/Menus.cme", attributes, contents, id);

            
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Font":
                            font = this.content.Load<SpriteFont>(contents[i][j]);
                            break;
                        case "Item":
                            menuItems.Add(contents[i][j]);
                            break;
                        case "Image":
                            menuImages.Add(this.content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Axis":
                            axis = int.Parse(contents[i][j]);
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
                            break;
                        case "Source":
                            string[] tempSource = contents[i][j].Split(' ');
                            source = new Rectangle(int.Parse(tempSource[0]), int.Parse(tempSource[1]), int.Parse(tempSource[2]), int.Parse(tempSource[3]));
                            break;
                        case "Animation":
                            animationTypes.Add(contents[i][j]);
                            break;
                        case "Align":
                            align = contents[i][j];
                            break;
                        case "LinkType":
                            linkType.Add(contents[i][j]);
                            break;
                        case "LinkID":
                            linkID.Add(contents[i][j]);
                            break;

                    }
                }
            }

            SetMenuItems();
            SetAnimations();

        }

        public void UnloadContent()
        {
            content.Unload();
            menuImages.Clear();
            animationTypes.Clear();
            menuImages.Clear();
            animation.Clear();
            fileManager = null;
        }

        public void Update(GameTime gameTime, InputManager inputManager)
        {
            if (axis == 1)
            {
                if (inputManager.KeyPressed(Keys.Right))
                    itemNumber++;
                if (inputManager.KeyPressed(Keys.Left))
                    itemNumber--;
            }
            else
            {
                if (inputManager.KeyPressed(Keys.Down))
                    itemNumber++;
                if (inputManager.KeyPressed(Keys.Up))
                    itemNumber--;
            }

            if (inputManager.KeyDown(Keys.Enter))
            {
                if (linkType[itemNumber] == "Screen")
                {
                    Type newClass = Type.GetType("Game4." + linkID[itemNumber]);
                    ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass),inputManager);
                }
            }

            if (itemNumber < 0) itemNumber = 0;
            else if (itemNumber >= menuItems.Count) itemNumber = menuItems.Count - 1;

            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animation[i].Count; j++)
                {
                    if (itemNumber == i)
                        animation[i][j].IsActive = true;
                    else
                        animation[i][j].IsActive = false;

                    animation[i][j].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i=0;i<animation.Count;i++)
                for (int j = 0; j < animation[i].Count; j++)
                {
                    animation[i][j].Draw(spriteBatch);
                }
        }
    }
}
