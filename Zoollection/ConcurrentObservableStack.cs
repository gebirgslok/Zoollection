using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Zoollection
{
    [DebuggerDisplay("Count = {Count}")]
    [Serializable]
    [ComVisible(false)]
    public class ConcurrentObservableStack<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T>,
        INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly ConcurrentStack<T> _concurrentStack;

        public int Count => _concurrentStack.Count;

        public ConcurrentObservableStack()
        {
            _concurrentStack = new ConcurrentStack<T>();
        }

        public ConcurrentObservableStack(IEnumerable<T> collection)
        {
            _concurrentStack = new ConcurrentStack<T>(collection);
        }

        protected virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public void CopyTo(T[] array, int index)
        {
            _concurrentStack.CopyTo(array, index);
        }

        public void Clear()
        {
            bool mustRaiseCollectionChanged = Count > 0;
            _concurrentStack.Clear();

            if (mustRaiseCollectionChanged)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                RaiseCollectionChanged(eventArgs);
            }
        }

        /// <summary>
        /// Inserts an object at the top of the ConcurrentObservableStack&lt;T&gt;.
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item)
        {
            _concurrentStack.Push(item);
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item });
            RaiseCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Inserts multiple objects at the top of the ConcurrentObservableStack&lt;T&gt; atomically.
        /// </summary>
        /// <param name="items"></param>
        public void PushRange(T[] items)
        {
            _concurrentStack.PushRange(items);
            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(items));
            RaiseCollectionChanged(eventArgs);
        }

        /// <summary>
        /// Inserts multiple objects at the top of the ConcurrentObservableStack&lt;T&gt; atomically.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        public void PushRange(T[] items, int startIndex, int count)
        {
            _concurrentStack.PushRange(items, startIndex, count);
            var itemSubset = items.ExtractSubArray(startIndex, count);

            if (itemSubset.Length > 0)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,
                    new List<T>(itemSubset));
                RaiseCollectionChanged(eventArgs);
            }
        }

        public T[] ToArray()
        {
            return _concurrentStack.ToArray();
        }

        public bool TryPeek(out T item)
        {
            return _concurrentStack.TryPeek(out item);
        }

        public bool TryPop(out T item)
        {
            var success = _concurrentStack.TryPop(out item);
            if (success)
            {
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                    new List<T> { item });
                RaiseCollectionChanged(eventArgs);
            }
            return success;
        }

        /// <summary>
        /// Attempts to pop and return multiple objects from the top of the ConcurrentObservableStack&lt;T&gt; atomically.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public int TryPopRange(T[] items)
        {
            int numOfPoppedItems = _concurrentStack.TryPopRange(items);
            if (numOfPoppedItems > 0)
            {
                var oldItems = items.ExtractSubArray(0, numOfPoppedItems);
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, 
                    new List<T>(oldItems));
                RaiseCollectionChanged(eventArgs);
            }

            return numOfPoppedItems;
        }

        /// <summary>
        /// Attempts to pop and return multiple objects from the top of the ConcurrentObservableStack&lt;T&gt; atomically.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int TryPopRange(T[] items, int startIndex, int count)
        {
            int numOfPoppedItems = _concurrentStack.TryPopRange(items, startIndex, count);
            if (numOfPoppedItems > 0)
            {
                var oldItems = items.ExtractSubArray(startIndex, numOfPoppedItems);
                var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                    new List<T>(oldItems));
                RaiseCollectionChanged(eventArgs);
            }
            return numOfPoppedItems;
        }

        #region explicit interface implementations
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            ICollection collection = _concurrentStack;
            collection.CopyTo(array, index);
        }


        public bool TryAdd(T item)
        {
            throw new NotImplementedException();
        }

        public bool TryTake(out T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _concurrentStack.GetEnumerator();
        }

        object ICollection.SyncRoot
        {
            get
            {
                ICollection collection = _concurrentStack;
                return collection.SyncRoot;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                ICollection collection = _concurrentStack;
                return collection.IsSynchronized;
            }
        }

        #endregion
    }
}
