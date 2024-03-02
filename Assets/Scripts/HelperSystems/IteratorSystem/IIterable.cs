namespace LKS.Iterations
{
    public interface IIterable
    {
        IIterable Next { get; set; }
        void OnIteration();

        void Iterate()
        {
            OnIteration();

            if(Next != null)
            {
                Next.Iterate();
            }
        }
    }
}