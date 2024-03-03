using LKS.Managers;
using UnityEngine;

namespace LKS.States.GameStates
{
    public class PlayingState : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            InputManager.Start();
            GameManager.Ball.StartGame();
        }

        public override void OnExit()
        {
            base.OnExit();
            InputManager.Stop();
        }
    }
}