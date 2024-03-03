using UnityEngine;

namespace LKS.Inputs
{
    public struct InputInfo
    {
        public Vector2 Position { get; private set; }
        public float PressTime { get; private set; }

        public InputInfo(Vector2 position, float pressTime = 0)
        {
            Position = position;
            PressTime = pressTime;
        }

        public void Update(Vector2 position, float pressTime = 0)
        {
            Position = position;
            PressTime = pressTime;
        }
    }
}