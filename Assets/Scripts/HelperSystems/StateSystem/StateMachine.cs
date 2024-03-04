using UnityEngine;

namespace LKS.States
{
    public class StateMachine<TState> where TState : IState
    {
#region Constants & Fields
        private TState _initialState;
#endregion

#region Properties
        public TState CurrentState { get; private set; }
#endregion

#region Constructors
        public StateMachine(TState initialState)
        {
            _initialState = initialState;
            ChangeState(initialState);
        }
#endregion

#region Public Methods
        public T GetCurrentState<T>() where T : TState
        {
            if (CurrentState is T result)
            {
                return result;
            }

            return default(T);
        }

        public bool IsInState<T>() where T : TState
        {
            return CurrentState != null && CurrentState.GetType() == typeof(T);
        }

        public void ChangeState(TState state)
        {
            if (state == null)
            {
                Debug.LogWarning($"Unable to change state: new state is null");
                return;
            }

            if (IsInState<TState>())
            {
                UpdateState();
                return;
            }

            if (CurrentState != null)
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

        public void RestartState()
        {
            if (CurrentState == null)
                return;

            CurrentState.OnEnter();
        }

        public void Reset()
        {
            ChangeState(_initialState);
        } 
#endregion
    }
}