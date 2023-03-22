using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace build_and_expand.Objects
{
    public class Grid
    {
        private readonly Tile[,] _grid;
        private readonly int _width;
        private readonly int _height;
        public int Width => _width;
        public int Height => _height;
        public Tile[,] GridTiles => _grid;

        private readonly List<Point> _roadList = new List<Point>();

        public Grid(int width, int height)
        {
            _width = width;
            _height = height;
            _grid = new Tile[width, height];
        }

        // Adding index operator to Grid class so that can use grid[][] to access specific cell from grid. 
        public Tile this[int i, int j]
        {
            get
            {
                return _grid[i, j];
            }
            set
            {
                if (value.Object.ObjectId == 11) // if tile is a road
                {
                    _roadList.Add(new Point(i, j));
                }
                else
                {
                    _roadList.Remove(new Point(i, j));
                }
                _grid[i, j] = value;
            }
        }

        public static bool IsCellWalkable(Tile cellType, bool aiAgent = false)
        {
            if (aiAgent)
            {
                return cellType.Object.ObjectId == 11; // if tile is a road
            }
            return cellType.Object.ObjectId == 0 || cellType.Object.ObjectId == 11; // if tile is a road or grass
        }

        public Point GetRandomRoadPoint()
        {
            Random rand = new Random();
            return _roadList[rand.Next(0, _roadList.Count - 1)];
        }

        public Point GetRandomSpecialStructurePoint()
        {
            Random rand = new Random();
            return _roadList[rand.Next(0, _roadList.Count - 1)];
        }

        public List<Point> GetAdjacentCells(Point cell, bool isAgent)
        {
            return GetWalkableAdjacentCells(cell.X, cell.Y, isAgent);
        }

        public float GetCostOfEnteringCell(Point cell)
        {
            return 1;
        }

        public List<Point> GetAllAdjacentCells(int x, int y)
        {
            List<Point> adjacentCells = new List<Point>();
            if (x > 0)
            {
                adjacentCells.Add(new Point(x - 1, y));
            }
            if (x < _width - 1)
            {
                adjacentCells.Add(new Point(x + 1, y));
            }
            if (y > 0)
            {
                adjacentCells.Add(new Point(x, y - 1));
            }
            if (y < _height - 1)
            {
                adjacentCells.Add(new Point(x, y + 1));
            }
            return adjacentCells;
        }

        public List<Point> GetWalkableAdjacentCells(int x, int y, bool isAgent)
        {
            List<Point> adjacentCells = GetAllAdjacentCells(x, y);
            for (int i = adjacentCells.Count - 1; i >= 0; i--)
            {
                if (IsCellWalkable(_grid[adjacentCells[i].X, adjacentCells[i].Y], isAgent) == false)
                {
                    adjacentCells.RemoveAt(i);
                }
            }
            return adjacentCells;
        }

        public List<Point> GetAdjacentCellsOfType(int x, int y, Tile type)
        {
            List<Point> adjacentCells = GetAllAdjacentCells(x, y);
            for (int i = adjacentCells.Count - 1; i >= 0; i--)
            {
                if (_grid[adjacentCells[i].X, adjacentCells[i].Y] != type)
                {
                    adjacentCells.RemoveAt(i);
                }
            }
            return adjacentCells;
        }

        /// <summary>
        /// Returns array [Left neighbour, Top neighbour, Right neighbour, Down neighbour]
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Tile[] GetAllAdjacentCellTypes(int x, int y)
        {
            Tile[] neighbours = {};
            if (x > 0)
            {
                neighbours[0] = _grid[x - 1, y];
            }
            if (x < _width - 1)
            {
                neighbours[2] = _grid[x + 1, y];
            }
            if (y > 0)
            {
                neighbours[3] = _grid[x, y - 1];
            }
            if (y < _height - 1)
            {
                neighbours[1] = _grid[x, y + 1];
            }
            return neighbours;
        }
    }
}
