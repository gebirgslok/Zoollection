using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace Zoollection
{
    [ComVisible(false)]
    public class ObservableQueue<T> : ICollection, IReadOnlyCollection<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly Queue<T> _queue;

        public int Count => _queue.Count;

        public ObservableQueue(int capacity)
        {
            _queue = new Queue<T>(capacity);
        }

        public ObservableQueue(IEnumerable<T> collection)
        {
            _queue = new Queue<T>(collection);
        }

        public ObservableQueue()
        {
            _queue = new Queue<T>();
        }

        protected virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the ObservableQueue&lt;T&gt;, 
        /// if that number is less than 90 percent of current capacity.
        /// </summary>
        public void TrimExcess()
        {
            _queue.TrimExcess();
        }

        /// <summary>
        /// Returns the object at the beginning of the ObservableQueue&lt;T&gt; without removing it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The ObservableQueue&lt;T&gt; is empty.</exception>
        public T Peek()
        {
            return _queue.Peek();
        }

        /// <summary>
        /// Removes and returns the object at the beginning of the ObservableQueue&lt;T&gt;.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">The ObservableQueue&lt;T&gt; is empty.</exception>
        public T Dequeue()
        {
            T item = _queue.Dequeue();
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                new List<T> { item });
            RaiseCollectionChanged(eventArgs);
            return item;
        }

        /// <summary>
        /// Copies the ObservableQueue&lt;T&gt; elements to a new array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return _queue.ToArray();
        }

        /// <summary>
        /// Copies the ObservableQueue&lt;T&gt; elements to an existing one-dimensional Array, 
        /// starting at the specified array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _queue.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Determines whether an element is in the ObservableQueue&lt;T&gt;.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _queue.Contains(item);
        }

        public void Clear()
        {
            bool mustRaiseCollectionChanged = Count > 0;
            _queue.Clear();

            if (mustRaiseCollectionChanged)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                RaiseCollectionChanged(eventArgs);
            }
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item });
            RaiseCollectionChanged(eventArgs);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        #region explicit interface implementation
        object ICollection.SyncRoot
        {
            get
            {
                ICollection collection = _queue;
                return collection.SyncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ICollection collection = _queue;
                return collection.IsSynchronized;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection collection = _queue;
            collection.CopyTo(array, index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
