using System;
using System.Collections.Generic;

namespace LKS.Iterations
{
    public class Iterator : IDisposable
    {
        private List<IIterable> _iterables;
        private IIterable _current;

        public Iterator(IEnumerable<IIterable> iterables)
        {
            _iterables = new List<IIterable>(iterables);

            if (_iterables.Count > 0)
            {
                _current = _iterables[0]; 
            }
        }

        public void Iterate()
        {
            if (_current != null)
            {
                _current.Iterate(); 
            }
        }

        public void Dispose()
        {
            if (_iterables != null)
            {
                _iterables.Clear(); 
            }
        }
    }
}