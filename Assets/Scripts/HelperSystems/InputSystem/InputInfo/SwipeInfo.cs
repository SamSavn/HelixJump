using LKS.Utils;
using UnityEngine;

namespace LKS.Inputs
{
    public struct SwipeInfo
    {
        public Vector2 StartPosition { get; private set; }
        public Vector2 EndPosition { get; private set; }
        public Vector2 Direction { get; private set; }
        public Direction DirectionX { get; private set; }
        public Direction DirectionY { get; private set; }

        public void Reset()
        {
            Update(Vector2.zero, Vector2.zero, Vector2.zero);
        }

        public void Update(Vector2 startPostion, Vector2 endPosition, Vector2 direction)
        {
            StartPosition = startPostion;
            EndPosition = endPosition;
            Direction = direction;

            if (Direction != Vector2.zero)
            {
                DirectionX = Direction.x > 0 ? Utils.Direction.Right : Utils.Direction.Left;
                DirectionY = Direction.y > 0 ? Utils.Direction.Up : Utils.Direction.Down;
            }
            else
            {
                DirectionX = Utils.Direction.None;
                DirectionY = Utils.Direction.None;
            }
        }
    }
}