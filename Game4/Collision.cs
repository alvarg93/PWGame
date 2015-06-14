using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Game4
{
    public class Collision
    {
        public enum Type { None, Left, Right, Top, Bottom}

        ContentManager content;
        FileManager fileManager;

        List<List<string>> attributes, contents, collisionMap;

        public List<List<string>> CollisionMap
        {
            get { return collisionMap; }
            set { collisionMap = value; }
        }
        List<string> row;

        public void LoadContent(ContentManager content, string mapId)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            this.fileManager = new FileManager();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            collisionMap = new List<List<string>>();
            row = new List<string>();

            fileManager.LoadContent("Load/Maps/" + mapId+".cme", attributes, contents, "Collision");

            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < contents[i].Count; j++)
                {
                    row.Add(contents[i][j]);
                }
                collisionMap.Add(row);
                row = new List<string>();
            }
        }

        public void UnloadContent()
        {
            content.Unload();
            fileManager = null;
            attributes.Clear();
            contents.Clear();
            collisionMap.Clear();
            row.Clear();
        }

        public void Update(GameTime gameTime, ref Vector2 playerPosition, Vector2 pDimensions, Vector2 tileDimensions)
        {
        }

        public static bool CheckCollision(Rectangle rect, Rectangle rect2){
            return rect.Intersects(rect2);
        }
    }
}
