# Zoollection
Additional collections ('a zoo of collections') that supplement the collections provided by .NET

## Supported collections
collection | details  
--- | --- 
ObservableStack | Stack that implements INotifyCollectionChanged. Stack<T> can be replaced by ObservableStack<T> without any additional changes in code.
ObservableQueue | Queue that implements INotifyCollectionChanged. Queue<T> can be replaced by ObservableQueue<T> without any additional changes in code.     
ConcurrentObservableStack | A thread-safe, observable stack. Technically ConcurrentStack that implements INotifyCollectionChanged. ConcurrentStack<T> can be replaced by ConcurrentObservableStack<T> without any additional changes in code.
ConcurrentObservableQueue | A thread-safe, observable queue. Technically ConcurrentQueue that implements INotifyCollectionChanged. ConcurrentStack<T> can be replaced by ConcurrentObservableQueue<T> without any additional changes in code.
