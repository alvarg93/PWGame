using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game4
{
    public class Layer
    {
        List<List<Tile>> tiles;

        public List<List<Tile>> Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }

        List<List<string>> attributes, contents;
        List<string> motion, solid;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        Vector2 tileDimensions;
        Vector2 startingPoint;


        public Vector2 Bounds
        {
            get { return new Vector2(TileDimensions.X * Tiles[0].Count, TileDimensions.Y * Tiles.Count); }
        }

        public Vector2 StartingPoint
        {
            get { return startingPoint; }
            set { startingPoint = value; }
        }

        public Vector2 TileDimensions
        {
            get { return tileDimensions; }
            set { tileDimensions = value; }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<List<Tile>>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            solid = new List<string>();
            motion = new List<string>();
            fileManager = new FileManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider,"Content");
            tileDimensions = new Vector2(32, 32);
            int layerYCount = 0;

            fileManager.LoadContent("Load/Maps/" + map.Id + ".cme", attributes, contents, layerID);
            string[] parts;
            for(int i=0;i<attributes.Count;i++)
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet":
                            tileSheet = content.Load<Texture2D>(contents[i][j]);
                            break;
                        case "Solid":
                            solid.Add(contents[i][j]);
                            break;
                        case "Motion":
                            motion.Add(contents[i][j]);
                            break;
                        case "StartLayer":
                            if (contents[i][j].Contains("Generate"))
                            {
                                parts = contents[i][j].Split(':');
                                List<List<string>> mapCells = Generate(int.Parse(parts[1]), int.Parse(parts[2]), 
                                    int.Parse(parts[3]), bool.Parse(parts[4]));
                                for (int l = 0; l < mapCells.Count; l++)
                                {
                                    List<Tile> tempTiles = new List<Tile>();
                                    Tile.Motion tempMotion = Tile.Motion.Static;
                                    Tile.State tempState;
                                    for (int k = 0; k < mapCells[l].Count; k++)
                                    {
                                        parts = mapCells[l][k].Split(',');
                                        tempTiles.Add(new Tile());

                                        if (solid.Contains(mapCells[l][k]))
                                            tempState = Tile.State.Solid;
                                        else
                                            tempState = Tile.State.Passive;

                                        foreach (string m in motion)
                                        {
                                            if (m.Contains(mapCells[l][k]))
                                            {
                                                string[] getMotion = m.Split(':');
                                                tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                                break;
                                            }
                                        }
                                        tempTiles[k].SetTile(tempState, tempMotion, new Vector2(tileDimensions.X * k, tileDimensions.Y * l), tileSheet,
                                            new Rectangle(int.Parse(parts[0]) * (int)tileDimensions.X, int.Parse(parts[1]) * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y));
                                    }
                                    tiles.Add(tempTiles);
                                }

                            }
                            else
                            {
                                List<Tile> tempTiles = new List<Tile>();
                                Tile.Motion tempMotion = Tile.Motion.Static;
                                Tile.State tempState;
                                for (int k = 0; k < contents[i].Count; k++)
                                {
                                    parts = contents[i][k].Split(',');
                                    tempTiles.Add(new Tile());

                                    if (parts[0] == "8" && parts[1] == "0")
                                        startingPoint = new Vector2(tileDimensions.X * k, tileDimensions.Y * layerYCount);

                                    if (solid.Contains(contents[i][k]))
                                        tempState = Tile.State.Solid;
                                    else
                                        tempState = Tile.State.Passive;

                                    foreach (string m in motion)
                                    {
                                        if (m.Contains(contents[i][k]))
                                        {
                                            string[] getMotion = m.Split(':');
                                            tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                            break;
                                        }
                                    }
                                    tempTiles[k].SetTile(tempState, tempMotion, new Vector2(tileDimensions.X * k, tileDimensions.Y * layerYCount), tileSheet,
                                        new Rectangle(int.Parse(parts[0]) * (int)tileDimensions.X, int.Parse(parts[1]) * (int)tileDimensions.Y, (int)tileDimensions.X, (int)tileDimensions.Y));
                                }
                                layerYCount++;
                                tiles.Add(tempTiles);
                            }
                            break;
                    }
                }

        }

        public void UnloadContent()
        {
            content.Unload();
            tiles.Clear();
            attributes.Clear();
            contents.Clear();
            motion.Clear();
            solid.Clear();
            fileManager = null;
            tileSheet.Dispose();
            tileDimensions = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Draw(spriteBatch);
                }
            }
        }

        public List<List<char>> maze;
        private List<List<string>> Generate(int seed, int height, int width, bool rooms){
            maze = new List<List<char>>();
            for (int i = 0; i < height; i++)
                maze.Add(new List<char>());
	        int ex, ey;
            int M_UP = 0;
            int M_DOWN = 1;
            int M_LEFT = 2;
            int M_RIGHT = 3;

	        for (int i = 0; i < height; i++)
	        for (int j = 0; j < width; j++)
		        maze[i].Add('#');
            Random rand = new Random(seed);
	        int sy = height -1;
	        int sx = (int)rand.Next() % (width - 2) + 1;

            maze[sy][sx] = 'S';
            startingPoint = new Vector2(2 * sx * tileDimensions.X, 2 * sy * tileDimensions.Y);

	        //--------------------------DFS-----------------------------------
	        int molex = sx, moley = sy - 1;
	        maze[moley][molex] = ' ';
	        int molex_old=molex, moley_old=moley;
	        Stack<int> old = new Stack<int>();
	        int chx, chy, dir;
	        bool up, down, right, left, valid = false, problem = false, wb = true;
	        valid = false;
	        up = down = right = left = false;
	        while (!valid)
            {
                chx = 0; chy = 0;
		        wb = true;
		        while (wb)
		        {
			        dir = rand.Next() % 4;
			        if (!up&&dir == M_UP){
				        wb = false; chx = 0; chy = -1; up = true;
                    }
			        else if (!down&&dir == M_DOWN) {
				        wb = false; chx = 0; chy = 1; down = true;
                    }
			        else if (!right&&dir == M_RIGHT){
				        wb = false; chx = 1; chy = 0; right = true;
                    }
			        else if (!left&&dir == M_LEFT){
				        wb = false; chx = -1; chy = 0; left = true;
                    }
		        }
		        problem = false;
		        if ((molex + chx == 0) || (molex + chx == width - 1))
			        problem = true;
		
		        else if ((moley + chy == 0) || (moley + chy == height - 1)) 
			        problem = true;
		
		        else if (maze[moley + chy][molex + chx] == ' ')
			        problem = true;
		
		        else if (maze[moley + 2 * chy][molex + 2 * chx] == ' ')
			        problem = true;
		
		        else {
			        if (chy != 0)
			        {
				        if (maze[moley + 2 * chy][molex + 1] == ' ' || maze[moley + 2 * chy][molex - 1] == ' ')
					        problem = true;
				
				        if (!rooms && (maze[moley + chy][molex + 1] == ' ' || maze[moley + chy][molex - 1] == ' '))
					        problem = true;

			        }
			        else {
				        if (maze[moley + 1][molex + 2 * chx] == ' ' || maze[moley - 1][molex + 2 * chx] == ' ')
					        problem = true;
				
				        if (!rooms && (maze[moley + 1][molex + chx] == ' ' || maze[moley - 1][molex + chx] == ' '))
					        problem = true;
			        }
		        }

		        if (!problem)
		        {
			        molex_old = molex;
			        moley_old = moley;
			        molex += chx;
			        moley += chy;
			        maze[moley][molex] = ' ';
			        old.Push(moley_old);
			        old.Push(molex_old);
			        up = false; down = false; right = false; left = false;
		        }
		        else if (up && down && right && left)
		        {
			        molex = molex_old;
			        moley = moley_old;
			        if (molex == sx&&moley == sy - 1) valid = true;
			        else 
			        {
				        molex_old = old.Peek();
				        old.Pop();
				        moley_old = old.Peek();
				        old.Pop();
                        up = false; down = false; right = false; left = false;
			        }
		        }


	        }

	        valid = false;
	        while (!valid)
	        {
		        ey = 0;
		        ex = (int)rand.Next() % (width - 2) + 1;

		        if (maze[1][ex] == ' '){ maze[ey][ex] = 'E'; valid = true;}
	        }

            List<List<string>> mapLines = new List<List<string>>();


            for (int i = 0; i < 2 * height; i++)
            {
                List<string> cells = new List<string>();
                for (int j = 0; j < 2 * width; j++)
                {
                    if (maze[i / 2][j / 2] == '#')
                        cells.Add("4,6");
                    else if (maze[i / 2][j / 2] == 'S')
                        cells.Add("8,0");
                    else if (maze[i / 2][j / 2] == 'E')
                        cells.Add("15,6");
                    else
                        cells.Add("1,0");
                }
                mapLines.Add(cells);
            }


            return mapLines;
        }
    }
}
