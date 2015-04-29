using System;
using UnityEngine;

namespace Assets.Scripts.Util.Interfaces
{
    [Serializable]
    public abstract class GridObject
    {
        [SerializeField]
        private Point _point;

        [SerializeField]
        public int X { get { return _point.X; } }

        [SerializeField]
        public int Y { get { return _point.Y; } }

        [SerializeField]
        public Point Location { get { return _point; } } 


        public GridObject(Point point)
        {
            _point = point;
        }

        public GridObject(int x, int y) : this(new Point(x, y)) { }

        public override string ToString()
        {
            return _point.ToString();
        }
    }
}

