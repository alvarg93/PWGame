using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game4
{
    public class Map
    {
        List<Layer> layers;
        string id;

        Player player;

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public List<Layer> Layers
        {
            get { return layers; }
            set { layers = value; }
        }


        Collision collision;

        public Collision Collision
        {
            get { return collision; }
            set { collision = value; }
        }

        Vector2 startingPoint;

        public Vector2 StartingPoint
        {
            get { return startingPoint; }
            set { startingPoint = value; }
        }

        public void LoadContent(ContentManager content, string mapId)
        {
            int numLayers = 1;
            layers = new List<Layer>();
            this.id = mapId;
            for (int i = 0; i < numLayers; i++)
            {
                Layer layer = new Layer();
                layer.LoadContent(this, "Layer"+(i+1).ToString());
                layers.Add(layer);
                startingPoint = layer.StartingPoint;
            }
            collision = new Collision();
            collision.LoadContent(content, mapId);
        }

        public void UnloadContent()
        {
            foreach(Layer layer in layers)
                layer.UnloadContent();
            collision.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            foreach (Layer layer in layers)
                layer.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Layer layer in layers)
                layer.Draw(spriteBatch);

        }

        public Vector2 GetRandomFreeSpot()
        {
            Vector2 freeSpot = new Vector2();
            bool valid = false;
            while (!valid)
            {
                int x = GlobalRandom.Instance.Next(0, layers[0].maze[0].Count), y = GlobalRandom.Instance.Next(0, layers[0].maze.Count);
                if (layers[0].maze[y][x] == ' ')
                {
                    freeSpot = new Vector2(2 * x * layers[0].TileDimensions.X + layers[0].TileDimensions.X, 
                        2 * y * layers[0].TileDimensions.Y + layers[0].TileDimensions.Y);
                    valid = true;
                }
            }
            return freeSpot;
        }

    }
}
