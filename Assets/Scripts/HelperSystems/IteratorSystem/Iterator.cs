using System;
using System.Collections.Generic;

namespace LKS.Iterations
{
    public class Iterator : IDisposable
    {
        private List<IIterable> _iterables;
        
        private IIterable First
        {
            get
            {
                if (_iterables != null && _iterables.Count > 0)
                {
                    return _iterables[0];
                }

                return null;
            }
        }

        public Iterator(IEnumerable<IIterable> iterables)
        {
            _iterables = new List<IIterable>(iterables);
        }

        public void Iterate()
        {
            if (First != null)
            {
                First.Iterate();
            }
        }

        public void Add(IIterable item)
        {
            _iterables.Add(item);
        }

        public void Remove(IIterable item)
        {
            if (_iterables.Contains(item))
            {
                _iterables.Remove(item);
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