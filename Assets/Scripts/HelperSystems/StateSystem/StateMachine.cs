using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LKS.States
{
    public class StateMachine<TState> where TState : IState
    {
#region Constants & Fields
        private TState[] _initialState;
#endregion

#region Properties
        public HashSet<TState> CurrentStates { get; private set; }
        public bool HasStates => CurrentStates != null && CurrentStates.Count > 0;
#endregion

#region Constructors
        public StateMachine(params TState[] initialStates)
        {
            _initialState = new TState[initialStates.Length];
            CurrentStates = new HashSet<TState>();

            for (int i = 0; i < initialStates.Length; i++)
            {
                _initialState[i] = initialStates[i];
                AddState(initialStates[i]);
            }
        }
#endregion

#region Public Methods
        public T GetCurrentState<T>() where T : TState
        {
            if (TryGetState(out T result))
            {
                return result;
            }

            return default(T);
        }

        public bool HasState<T>() where T : TState
        {
            return HasStates && TryGetState(out T _);
        }

        public void AddStates(TState[] states)
        {
            for (int i = 0; i < states.Length; i++)
            {
                AddState(states[i]);
            }
        }

        public void AddState(TState state)
        {
            if (state != null && !CurrentStates.Contains(state))
            {
                CurrentStates.Add(state); 
                state.OnEnter();
            }
        }

        public void RemoveState(TState state)
        {
            if (state != null && CurrentStates.Contains(state))
            {
                state.OnExit();
                CurrentStates.Remove(state);
            }
        }

        public void RemoveState<T>() where T : TState
        {
            if (TryGetState(out T state))
            {
                RemoveState(state);
            }
        }

        public void ChangeState(params TState[] states)
        {
            if (states == null)
            {
                Debug.LogWarning($"Unable to change state: no new state is set");
                return;
            }

            if(CurrentStates.Count == 0)
            {
                AddStates(states);
                return;
            }
            else if (CurrentStates.Count == 1)
            {
                ExitAllStates();
                AddStates(states);
                return;
            }

            List<TState> statesToRemove = new List<TState>(CurrentStates);

            foreach (TState state in states)
            {
                if (state == null)
                    continue;

                if (CurrentStates.Contains(state))
                {
                    state.UpdateState();
                    statesToRemove.Remove(state);
                }
            }

            for (int i = 0; i < statesToRemove.Count; i++)
            {
                RemoveState(statesToRemove[i]);
            }

            foreach (TState state in states)
            {
                if (!CurrentStates.Contains(state))
                {
                    AddState(state);
                }
            }
        }

        public void UpdateStates()
        {
            if (!HasStates)
                return;

            foreach (TState state in CurrentStates)
            {
                state?.UpdateState();
            }
        }

        public void RestartState()
        {
            if (!HasStates)
                return;

            foreach (TState state in CurrentStates)
            {
                state?.OnExit();
                state?.OnEnter();
            }
        }

        public void Reset()
        {
            ChangeState(_initialState.ToArray());
        } 
#endregion

        private bool TryGetState<T>(out T state) where T : TState
        {
            foreach (TState st in CurrentStates)
            {
                if (st is T result)
                {
                    state = result;
                    return true;
                }
            }

            state = default(T);
            return false;
        }

        private void ReplaceState(TState state, TState newState)
        {
            if (HasStates)
            {
                RemoveState(state);
            }

            AddState(newState);
        }

        private void ExitAllStates()
        {
            while (HasStates)
            {
                RemoveState(CurrentStates.First());
            }
        }
    }
}