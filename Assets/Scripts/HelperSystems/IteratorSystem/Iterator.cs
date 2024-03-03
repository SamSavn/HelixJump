using LKS.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LKS.Iterations
{
    public class Iterator : IDisposable
    {
        public event Action OnIterationCompleted;
        private List<IIterable> _iterables;
        
        public IIterable First
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
            int count = iterables.Count();
            for (int i = 0; i < count; i++)
            {
                Add(iterables.ElementAt(i));
            }
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
            _iterables ??= new List<IIterable>();
            int count = _iterables.Count;

            if (count > 0)
            {
                _iterables[^1].Next = item;

                item.Previous = _iterables[^1];
                item.Next = null;
            }
            else
            {
                item.ResetIterable();
            }

            item.OnIterationComplete = OnItemIterationCompleted;
            _iterables.Add(item);
        }

        public void Remove(IIterable item)
        {
            if (_iterables.Contains(item))
            {
                item.ResetIterable();
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

        private void OnItemIterationCompleted(IIterable iterable)
        {
            if(iterable.Next == null)
            {
                OnIterationCompleted?.Invoke();
            }
        }
    }
}