using LKS.GameElements;
using LKS.States;

namespace LKS.Managers
{
    public static class GameManager
    {
#region Constants & Fields
        private static StateMachine<GameState> _stateMachine;
#endregion

#region Properties
        public static GameCamera GameCamera { get; private set; }
        public static Tower Tower { get; private set; }
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
            GameCamera = gameCamera;
        }

        public static void SetTower(Tower tower)
        {
            Tower = tower;
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