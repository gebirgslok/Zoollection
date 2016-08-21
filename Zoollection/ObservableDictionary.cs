using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Zoollection
{
    //[ComVisible(false)]
    //public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, 
    //    IReadOnlyDictionary<TKey, TValue>, INotifyCollectionChanged,
    //    ISerializable, IDeserializationCallback
    //{
    //    public event NotifyCollectionChangedEventHandler CollectionChanged;

    //    private readonly Dictionary<TKey, TValue> _dictionary;

    //    public int Count => _dictionary.Count;
    //    //public bool IsReadOnly { get; }

    //    public IEnumerable<TKey> Keys => _dictionary.Keys;

    //    public IEnumerable<TValue> Values => _dictionary.Values;

    //    public TValue this[TKey key] => _dictionary[key];

    //    public ObservableDictionary()
    //    {
    //        _dictionary = new Dictionary<TKey, TValue>();
    //        _dictionary
    //    }

    //    protected virtual void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
    //    {
    //        CollectionChanged?.Invoke(this, e);
    //    }

    //    public void Clear()
    //    {
    //        bool mustRaiseCollectionChanged = Count > 0;
    //        _dictionary.Clear();
    //        if (mustRaiseCollectionChanged)
    //        {
    //            var eventArgs = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    //            RaiseCollectionChanged(eventArgs);
    //        }
    //    }

    //    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    //    {
    //        return _dictionary.GetEnumerator();
    //    }

    //    public void GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        _dictionary.GetObjectData(info, context);
    //    }

    //    public void OnDeserialization(object sender)
    //    {
    //        _dictionary.OnDeserialization(sender);
    //    }

    //    public bool ContainsKey(TKey key)
    //    {
    //        return _dictionary.ContainsKey(key);
    //    }

    //    public bool TryGetValue(TKey key, out TValue value)
    //    {
    //        return _dictionary.TryGetValue(key, out value);
    //    }

    //    #region exlicit interface implementation
    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }

    //    void ICollection.CopyTo(Array array, int index)
    //    {
    //        ICollection collection = _dictionary;
    //        collection.CopyTo(array, index);
    //    }

    //    public bool Contains(object key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Add(object key, object value)
    //    {
    //        throw new NotImplementedException();
    //    }



    //    IDictionaryEnumerator IDictionary.GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Remove(object key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Add(TKey key, TValue value)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove(TKey key)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Add(KeyValuePair<TKey, TValue> item)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Contains(KeyValuePair<TKey, TValue> item)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Remove(KeyValuePair<TKey, TValue> item)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    object ICollection.SyncRoot
    //    {
    //        get
    //        {
    //            ICollection collection = _dictionary;
    //            return collection.SyncRoot;
    //        }
    //    }

    //    bool ICollection.IsSynchronized
    //    {
    //        get
    //        {
    //            ICollection collection = _dictionary;
    //            return collection.IsSynchronized;
    //        }
    //    }

    //    ICollection IDictionary.Keys
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    ICollection IDictionary.Values
    //    {
    //        get
    //        {
    //            IDictionary dic = _dictionary;
    //            return dic.Values;
    //        }
    //    }

    //    bool IDictionary.IsReadOnly
    //    {
    //        get
    //        {
    //            IDictionary dictionary = _dictionary;
    //            return dictionary.IsReadOnly;
    //        }
    //    }

    //    public bool IsFixedSize
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    ICollection<TKey> IDictionary<TKey, TValue>.Keys
    //    {
    //        get
    //        {
    //            IDictionary<TKey, TValue> dictionary = _dictionary;
    //            return dictionary.Keys;
    //        }
    //    }

    //    ICollection<TValue> IDictionary<TKey, TValue>.Values
    //    {
    //        get
    //        {
    //            IDictionary<TKey, TValue> dictionary = _dictionary;
    //            return dictionary.Values;
    //        }
    //    }

    //    TValue IDictionary<TKey, TValue>.this[TKey key]
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }

    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    public object this[object key]
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }

    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //    #endregion
    //}
}
