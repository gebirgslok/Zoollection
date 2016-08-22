using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Zoollection
{
    [DebuggerDisplay("Count = {Count}")]
    [ComVisible(false)]
    [Serializable]
    public class ObservableStack<T> : ICollection, IReadOnlyCollection<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly Stack<T> _stack;

        public int Count => _stack.Count;

        public ObservableStack(int capacity)
        {
            _stack = new Stack<T>(capacity);
        }

        public ObservableStack(IEnumerable<T> collection)
        {
            _stack = new Stack<T>(collection);
        }

        public ObservableStack()
        {
            _stack = new Stack<T>();
        }

        protected virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Removes all objects from the ObservableStack&lt;T&gt;.
        /// </summary>
        public void Clear()
        {
            bool mustRaiseCollectionChanged = _stack.Count > 0;
            _stack.Clear();

            if (mustRaiseCollectionChanged)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                RaiseCollectionChanged(eventArgs);
            }
        }

        /// <summary>
        /// Determines whether an element is in the ObservableStack&lt;T&gt;.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _stack.Contains(item);
        }

        /// <summary>
        /// Copies the ObservableStack&lt;T&gt; to an existing one-dimensional Array, 
        /// starting at the specified array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _stack.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Returns the object at the top of the ObservableStack&lt;T&gt; without removing it.
        /// </summary>
        /// <exception cref="InvalidOperationException">The ObservableStack&lt;T&gt; is empty.</exception>
        public T Peek()
        {
            return _stack.Peek();
        }

        /// <summary>
        /// Removes and returns the object at the top of the ObservableStack&lt;T&gt;.
        /// </summary>
        /// <returns></returns>#
        /// <exception cref="InvalidOperationException">The ObservableStack&lt;T&gt; is empty.</exception> 
        public T Pop()
        {
            T item = _stack.Pop();
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                new List<T> { item });
            RaiseCollectionChanged(eventArgs);
            return item;
        }

        /// <summary>
        /// Inserts an object at the top of the ObservableStack&lt;T&gt;.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            _stack.Push(item);
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                new List<T> { item });
            RaiseCollectionChanged(eventArgs);
        }

        public T[] ToArray()
        {
            return _stack.ToArray();
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the ObservableStack&lt;T&gt;, 
        /// if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess()
        {
            _stack.TrimExcess();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        #region explicit interface implementations
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection collection = _stack;
            collection.CopyTo(array, index);
        }

        object ICollection.SyncRoot
        {
            get
            {
                ICollection collection = _stack;
                return collection.SyncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ICollection collection = _stack;
                return collection.IsSynchronized;
            }
        }

        #endregion
    }
}
