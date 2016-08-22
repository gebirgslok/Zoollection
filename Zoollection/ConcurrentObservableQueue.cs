using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Zoollection
{
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    public class ConcurrentObservableQueue<T> : IProducerConsumerCollection<T>, 
        IReadOnlyCollection<T>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly ConcurrentQueue<T> _concurrentQueue;

        public int Count => _concurrentQueue.Count;

        public ConcurrentObservableQueue(IEnumerable<T> collection)
        {
            _concurrentQueue = new ConcurrentQueue<T>(collection);
        }

        public ConcurrentObservableQueue()
        {
            _concurrentQueue = new ConcurrentQueue<T>();
        }

        protected virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Adds an object to the end of the ConcurrentObservableQueue&lt;T&gt;>.
        /// </summary>
        /// <param name="item"></param>
        public void Enqueue(T item)
        {
            _concurrentQueue.Enqueue(item);
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item });
            RaiseCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Tries to return an object from the beginning of the ConcurrentObservableQueue&lt;T&gt; without removing it.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool TryPeek(out T item)
        {
            return _concurrentQueue.TryPeek(out item);
        }

        /// <summary>
        /// Tries to remove and return the object at the beginning of the concurrent queue.
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if an element was removed and returned from the beginning 
        /// of the ConcurrentObservableQueue&lt;T&gt; successfully; otherwise, false.</returns>
        public bool TryDequeue(out T item)
        {
            var success = _concurrentQueue.TryDequeue(out item);
            if (success)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>{ item });
                RaiseCollectionChanged(eventArgs);
            }
            return success;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <exception cref="ArgumentNullException">array is a null reference.</exception>
        /// <exception cref="ArgumentOutOfRangeException">index is less than zero.</exception>
        /// <exception cref="ArgumentException">index is equal to or greater than the length of the array -or- 
        /// The number of elements in the source ConcurrentObservableQueue&lt;T&gt;> is greater than the available 
        /// space from index to the end of the destination array.</exception>
        public void CopyTo(T[] array, int index)
        {
            _concurrentQueue.CopyTo(array, index);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _concurrentQueue.GetEnumerator();
        }

        public T[] ToArray()
        {
            return _concurrentQueue.ToArray();
        }

        #region explicit interface implementations
        bool ICollection.IsSynchronized
        {
            get
            {
                ICollection collection = _concurrentQueue;
                return collection.IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                ICollection collection = _concurrentQueue;
                return collection.SyncRoot;
            }
        }

        bool IProducerConsumerCollection<T>.TryAdd(T item)
        {
            IProducerConsumerCollection<T> queue = _concurrentQueue;
            return queue.TryAdd(item);
        }

        bool IProducerConsumerCollection<T>.TryTake(out T item)
        {
            IProducerConsumerCollection<T> queue = _concurrentQueue;
            return queue.TryTake(out item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection collection = _concurrentQueue;
            collection.CopyTo(array, index);
        }
        #endregion
    }
}
