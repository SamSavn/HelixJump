using LKS.GameUpdate;
using UnityEngine;

namespace LKS.States
{
    public interface IState : IUpdatable
    {
        void OnEnter();
        void UpdateState();
        void OnExit();
    }
}