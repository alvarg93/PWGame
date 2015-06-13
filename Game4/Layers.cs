using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Game4
{
    public class Layers
    {
        List<List<List<Vector2>>> tileMap;
        List<List<Vector2>> layer;
        List<Vector2> tile;
        List<List<string>> attributes, contents;

        ContentManager content;
        FileManager fileManager;

        Texture2D tileSet;
        Vector2 tileDimensions;

        int layerNumber;

        public int LayerNumber
        {
            set { layerNumber = value; }
        }

        public void LoadContent(ContentManager content, string mapId)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            fileManager = new FileManager();

            tile = new List<Vector2>();
            layer = new List<List<Vector2>>();
            tileMap = new List<List<List<Vector2>>>();

            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            layerNumber = 0;

            fileManager.LoadContent("Load/Maps/" + mapId + ".cme",attributes,contents,"Layers");

            for(int i=0;i<attributes.Count;i++)
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet":
                            tileSet = this.content.Load<Texture2D>("TileSets/"+contents[i][j]);
                            break;
                        case "TileDimensions":
                            string[] parts = contents[i][j].Split(',');
                            tileDimensions = new Vector2(int.Parse(parts[0]), int.Parse(parts[1]));
                            break;
                        case "StartLayer":
                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                parts = contents[i][j].Split(',');
                                tile.Add(new Vector2(int.Parse(parts[0]), int.Parse(parts[1])));
                            }
                            if (tile.Count > 0)
                            {
                                layer.Add(tile);
                            }
                            tile = new List<Vector2>();
                            break;
                        case "EndLayer":
                            if (layer.Count > 0)
                            {
                                tileMap.Add(layer);
                            }
                            layer = new List<List<Vector2>>();
                            break;   
                    }
                }

        }

        public void UnloadContent()
        {
            fileManager = null;
            content.Unload();
            tileMap.Clear();
            tileMap = null;
            layer.Clear();
            layer = null;
            tile = null;
            attributes.Clear();
            contents.Clear();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < tileMap.Count;k++ )
                for (int i = 0; i < tileMap[k].Count; i++)
                {
                    for (int j = 0; j < tileMap[k][i].Count; j++)
                    {
                        spriteBatch.Draw(tileSet, new Vector2(j * tileDimensions.X, i * tileDimensions.Y),
                            new Rectangle((int)tileMap[k][i][j].X * (int)tileDimensions.X,
                                (int)tileMap[k][i][j].Y * (int)tileDimensions.Y,
                                (int)tileDimensions.X, (int)tileDimensions.Y), Color.White);
                    }
                }
        }
    }
}
