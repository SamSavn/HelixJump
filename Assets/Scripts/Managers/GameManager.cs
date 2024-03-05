using LKS.GameElements;
using LKS.States;
using LKS.States.BallStates;
using LKS.States.GameStates;
using LKS.States.TowerStates;
using System;
using UnityEngine;

namespace LKS.Managers
{
    public static class GameManager
    {
#region Constants & Fields
        public static event Action<BallState> BallStateChanged;
        private static StateMachine<GameState> _stateMachine;
#endregion

#region Properties
        public static GameCamera GameCamera { get; private set; }
        public static Tower Tower { get; private set; }
        public static Ball Ball { get; private set; }
#endregion

#region Constructors
        static GameManager()
        {
            _stateMachine = new StateMachine<GameState>(new MainMenuState());
        }
#endregion

#region Public Methods
        public static void SetGameCamera(GameCamera gameCamera)
        {
            GameCamera ??= gameCamera;
        }

        public static void SetTower(Tower tower)
        {
            Tower ??= tower;
        }

        public static void SetBall(Ball ball)
        {
            Ball ??= ball;
        }

        public static bool CanActivatePlatform(Platform platform)
        {
            return Tower.CanActivatePlatform(platform);
        }

        public static void OnBallStateChanged<TState>(TState state) where TState : BallState
        {
            if(state == null) 
                return;

            BallStateChanged?.Invoke(state);            
            
            if (state.GetType() == typeof(DeadState))
            {
                StopGame();
            }
        }
#endregion

#region States Methods
        public static void ResetGame()
        {
            _stateMachine.Reset();
        }

        public static void StartGame()
        {
            _stateMachine.ChangeState(new PlayingState());
        }

        public static void StopGame()
        {
            _stateMachine.ChangeState(new EndGameState());
        }

        public static void PauseGame()
        {
            _stateMachine.ChangeState(new PauseState());
        } 
#endregion
    }
}