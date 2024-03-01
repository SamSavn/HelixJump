using UnityEngine;

namespace LKS.States
{
    public interface IState
    {
        void OnEnter();
        void UpdateState();
        void OnExit();
    }
}