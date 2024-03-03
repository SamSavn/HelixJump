namespace LKS.Iterations
{
    public interface IIterable
    {
        IIterable Previous { get; set; }
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