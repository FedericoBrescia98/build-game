using build_and_expand.Content;
using build_and_expand.States;
using build_and_expand.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;

namespace build_and_expand.Objects
{
    public class Map
    {
        public GameContent GameContent { get; set; }
        public Grid grid = new Grid((int)C.MAPDIM.X, (int)C.MAPDIM.Y);
        private readonly string[] _linesMap = File.ReadAllLines("defaultMap.txt");
        private List<AnimatedTexture> _waterAnimation = new List<AnimatedTexture>();
        public Map(GameContent gameContent)
        {
            GameContent = gameContent;

            // initialize new map from file
            for(int i = 0; i < grid.Width; i++)
            {
                char[] line = _linesMap[i].ToCharArray();
                for(int j = 0; j < grid.Height; j++)
                {
                    int x = i * C.TILETEXTURESIZE.X;
                    int y = j * C.TILETEXTURESIZE.Y;
                    grid[i, j] = line[j] switch
                    {
                        'g' => new Tile(gameContent, new TileObject(), new Point(x, y)),
                        't' => new Tile(gameContent, Resource.Tree(), new Point(x, y)),
                        'o' => new Tile(gameContent, Resource.Ore(), new Point(x, y)),
                        'h' => new Tile(gameContent, Building.BasicHouse(), new Point(x, y)),
                        _ => new Tile(gameContent, new TileObject(), new Point(x, y)),
                    };
                }
            }

            // initialize water animation
            Texture2D waterAnimationTexture = GameContent.GetTileTexture(-1);
            for(int i = -C.MAXEDGEMAP;
                i < (C.MAXEDGEMAP + C.MAPDIM.X);
                i += 1)
            {
                for(int j = -C.MAXEDGEMAP;
                    j < (C.MAXEDGEMAP + C.MAPDIM.Y);
                    j += 1)
                {
                    if(i < 0 || j < 0 || j >= C.MAPDIM.Y || i >= C.MAPDIM.X)
                    {
                        int x = i * C.TILETEXTURESIZE.X;
                        int y = j * C.TILETEXTURESIZE.Y;

                        _waterAnimation.Add(new AnimatedTexture(waterAnimationTexture, new Point(0, 0))
                        {
                            Loop = true,
                            AnimFrameTime = 0.5f,
                            Position = new Point(x, y),
                        });
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // draw water
            Texture2D waterTexture = GameContent.GetTileTexture(0);
            int waterIdx = 0;
            for(int i = -C.MAXEDGEMAP;
                i < (C.MAXEDGEMAP + C.MAPDIM.X);
                i += 1)
            {
                for(int j = -C.MAXEDGEMAP;
                    j < (C.MAXEDGEMAP + C.MAPDIM.Y);
                    j += 1)
                {
                    if(i < 0 || j < 0 || j >= C.MAPDIM.Y || i >= C.MAPDIM.X)
                    {
                        int x = i * C.TILETEXTURESIZE.X;
                        int y = j * C.TILETEXTURESIZE.Y;

                        spriteBatch.Draw(waterTexture,
                                            new Rectangle(x, y,
                                                waterTexture.Width,
                                                waterTexture.Height),
                                                Color.White);

                        _waterAnimation[waterIdx].Draw(gameTime, spriteBatch);
                        waterIdx++;
                    }
                }
            }

            // draw grid tiles
            for(int i = 0; i < grid.Width; i++)
            {
                for(int j = 0; j < grid.Height; j++)
                {
                    SetRoadTexture(grid[i, j].Object, grid[i, j].PositionToMap);
                    grid[i, j].Draw(gameTime, spriteBatch);
                }
            }
        }

        public void Update(GameTime gameTime, GameState gameState)
        {
            for(int i = 0; i < grid.Width; i++)
            {
                for(int j = 0; j < grid.Height; j++)
                {
                    grid[i, j].Update(gameTime, gameState);
                }
            }
        }

        // logics for roads turns texture
        public void SetRoadTexture(TileObject obj, Point tp)
        {
            if(obj.ObjectId == 1000)
            {
                obj.TerrainId = 1000;
                if(tp.X != 0 && grid[tp.X - 1, tp.Y].Object.ObjectId == 1000)
                {
                    obj.TerrainId = 1000;
                    if(tp.Y != 0 && grid[tp.X, tp.Y - 1].Object.ObjectId == 1000)
                    {
                        obj.TerrainId = 1005;
                        if(tp.X != C.MAPDIM.X - 1 && grid[tp.X + 1, tp.Y].Object.ObjectId == 1000)
                        {
                            obj.TerrainId = 1006;
                            if(tp.Y != C.MAPDIM.Y - 1 && grid[tp.X, tp.Y + 1].Object.ObjectId == 1000)
                            {
                                obj.TerrainId = 1010;
                                return;
                            }
                            return;
                        }
                        if(tp.Y != C.MAPDIM.Y - 1 && grid[tp.X, tp.Y + 1].Object.ObjectId == 1000)
                        {
                            obj.TerrainId = 1009;
                            return;
                        }
                        return;
                    }
                    if(tp.Y != C.MAPDIM.Y - 1 && grid[tp.X, tp.Y + 1].Object.ObjectId == 1000)
                    {
                        obj.TerrainId = 1004;
                        if(tp.X != C.MAPDIM.X - 1 && grid[tp.X + 1, tp.Y].Object.ObjectId == 1000)
                        {
                            obj.TerrainId = 1008;
                            return;
                        }
                        return;
                    }
                    return;
                }
                if(tp.Y != 0 && grid[tp.X, tp.Y - 1].Object.ObjectId == 1000)
                {
                    obj.TerrainId = 1001;
                    if(tp.X != C.MAPDIM.X - 1 && grid[tp.X + 1, tp.Y].Object.ObjectId == 1000)
                    {
                        obj.TerrainId = 1002;
                        if(tp.Y != C.MAPDIM.Y - 1 && grid[tp.X, tp.Y + 1].Object.ObjectId == 1000)
                        {
                            obj.TerrainId = 1007;
                            return;
                        }
                        return;
                    }
                    return;
                }
                if(tp.X != C.MAPDIM.X - 1 && grid[tp.X + 1, tp.Y].Object.ObjectId == 1000)
                {
                    obj.TerrainId = 1000;
                    if(tp.Y != C.MAPDIM.Y - 1 && grid[tp.X, tp.Y + 1].Object.ObjectId == 1000)
                    {
                        obj.TerrainId = 1003;
                        return;
                    }
                    return;
                }
                if(tp.Y != C.MAPDIM.Y - 1 && grid[tp.X, tp.Y + 1].Object.ObjectId == 1000)
                {
                    obj.TerrainId = 1001;
                    if(tp.X != 0 && grid[tp.X - 1, tp.Y].Object.ObjectId == 1000)
                    {
                        obj.TerrainId = 1004;
                        if(tp.Y != 0 && grid[tp.X, tp.Y - 1].Object.ObjectId == 1000)
                        {
                            obj.TerrainId = 1009;
                            return;
                        }
                        return;
                    }
                    return;
                }
            }
        }
    }
}
