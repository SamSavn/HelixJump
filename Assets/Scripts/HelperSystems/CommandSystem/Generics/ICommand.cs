using UnityEngine;

namespace LKS.Inputs
{
    public interface ICommand
    {
        void Execute(Transform transform);
    }
}