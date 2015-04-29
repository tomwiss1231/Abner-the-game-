using System;
using UnityEngine;

namespace Assets.Scripts.Util
{
    [Serializable]
    public struct Point
    {
        [SerializeField]
        private int _x;

        [SerializeField]
        private int _y;

        [SerializeField]
        public int X { get { return _x; } set { _x = value; } }

        [SerializeField]
        public int Y { get { return _y; } set { _y = value; } }

        public Point(int x, int y)
        {
            this._x = x;
            this._y = y;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
