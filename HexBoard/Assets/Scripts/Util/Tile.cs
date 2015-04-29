using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Util.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Util
{
    [Serializable]
    public class Tile : GridObject
    {
        [SerializeField]
        private int MAX_WEIGHT = 1000000000;

        [SerializeField]
        public static List<Point> NeighbourShift
        {
            get
            {
                return new List<Point>
                {
                    new Point(0,1),
                    new Point(1,0),
                    new Point(1,-1),
                    new Point(0,-1),
                    new Point(-1,0),
                    new Point(-1,1),
                };
            }
        }


        [SerializeField]
        private bool _passable;
    
        [SerializeField]
        private List<TileBehaviour> _neighbours;

        [SerializeField]
        private int _weight;

        [SerializeField]
        public bool Passable { get { return _passable; } set { _passable = value; } }

        [SerializeField]
        public int Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public Tile(int x, int y, bool passable) : base(x, y) {
            _passable = passable;
            _neighbours = new List<TileBehaviour>();
            Weight = MAX_WEIGHT;
        }

        public void FindNeighbours(Dictionary<Point, TileBehaviour> board, Vector2 boardSize, 
            bool equalLineLengths)
        {
            List<TileBehaviour> neighbours = new List<TileBehaviour>();
            foreach (Point point in NeighbourShift)
            {
                int neighbourX = X + point.X;
                int neighbourY = Y + point.Y;
                int xOffset = neighbourY / 2;

                if (neighbourY % 2 != 0 && !equalLineLengths &&
                    neighbourX + xOffset == boardSize.x - 1)
                    continue;
                if (neighbourX >= -xOffset &&
                    neighbourX < (int)boardSize.x - xOffset &&
                    neighbourY >= 0 && neighbourY < (int)boardSize.y)
                    neighbours.Add(board[new Point(neighbourX, neighbourY)]);
            }
            _neighbours = neighbours;
        }


        public void setP(bool val) { _passable = val; }

        public void ResetWeight() { Weight = MAX_WEIGHT; }

        public  IEnumerable<TileBehaviour> GetAllNeighbours() { return _neighbours.Where(o => o.tile.Passable); }

    }
}

