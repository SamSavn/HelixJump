using System;

namespace LKS.Iterations
{
    public interface IIterable
    {
        Action<IIterable> OnIterationComplete { get; set; }
        IIterable Previous { get; set; }
        IIterable Next { get; set; }
        void OnIteration();

        void Iterate()
        {
            OnIteration();

            if (Next != null)
            {
                Next.Iterate();
            }
        }

        void ResetIterable()
        {
            OnIterationComplete = null;
            Previous = null;
            Next = null;
        }
    }
}