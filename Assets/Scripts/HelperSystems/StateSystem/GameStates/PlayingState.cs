using LKS.Managers;
using UnityEngine;

namespace LKS.States
{
    public class PlayingState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            InputManager.Start();
        }

        public override void OnExit()
        {
            base.OnExit();
            InputManager.Stop();
        }
    }
}