using LKS.Utils;
using UnityEngine;

namespace LKS.Inputs
{
    public struct SwipeInfo
    {
        public Vector2 StartPosition { get; private set; }
        public Vector2 EndPosition { get; private set; }
        public Vector2 DirectionVector { get; private set; }
        public Vector2 Distance { get; private set; }
        public (Direction x, Direction y) Direction { get; private set; }

        public void Reset()
        {
            Update(Vector2.zero, Vector2.zero, Vector2.zero);
        }

        public void Update(Vector2 startPostion, Vector2 endPosition, Vector2 direction)
        {
            StartPosition = startPostion;
            EndPosition = endPosition;
            DirectionVector = direction;

            Distance = new Vector2(Mathf.Abs(StartPosition.x - EndPosition.x), 
                                   Mathf.Abs(StartPosition.y - EndPosition.y));

            if (DirectionVector != Vector2.zero)
            {
                Direction =
                (
                    x: DirectionVector.x > 0 ? Utils.Direction.Right : Utils.Direction.Left,
                    y: DirectionVector.y > 0 ? Utils.Direction.Up : Utils.Direction.Down
                );
            }
            else
            {
                Direction = (x: Utils.Direction.None, y: Utils.Direction.None);
            }
        }
    }
}