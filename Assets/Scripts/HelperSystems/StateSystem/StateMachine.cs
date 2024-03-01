using UnityEngine;

namespace LKS.States
{
    public class StateMachine<TState> where TState : IState
    {
        private TState _initialState;
        public TState CurrentState { get; private set; }

        public StateMachine(TState initialState) 
        {
            _initialState = initialState;
            ChangeState(initialState);
        }

        public void ChangeState(TState state)
        {
            if(state == null)
            {
                Debug.LogWarning($"Unable to change state: new state is null");
                return;
            }

            if(CurrentState != null)
            {
                CurrentState.OnExit();
            }

            CurrentState = state;
            CurrentState.OnEnter();
        }

        public void UpdateState()
        {
            if (CurrentState == null)
                return;

            CurrentState.UpdateState();
        }

        public void Reset()
        {
            ChangeState(_initialState);
        }
    }
}