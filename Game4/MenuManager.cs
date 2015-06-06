using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4
{
    public class MenuManager
    {
        List<string> menuItems;
        List<string> animationTypes;
        List<Texture2D> menuImages;
        List<List<Animation>> animation;

        ContentManager content;

        FileManager fileManager;

        Vector2 position;
        int axis;

        List<List<string>> attributes;
        List<List<string>> contents;

        Rectangle source;

        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
                if (menuImages.Count == i)
                {
                    menuImages.Add(null);
                }

            for (int i = 0; i < menuImages.Count; i++)
                if (menuItems.Count == i)
                {
                    menuItems.Add("");
                }
        }

        private void SetAnimations()
        {

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
            fileManager.LoadContent("Load/Menus.cme", attributes, contents, id);

            
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Item":
                            menuItems.Add(contents[i][j]);
                            break;
                        case "Image":
                            menuImages.Add(content.Load<Texture2D>(contents[i][j]));
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
                    }
                }
            }

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

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
