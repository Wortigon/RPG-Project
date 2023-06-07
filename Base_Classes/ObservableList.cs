using System;
using System.Collections.Generic;

public class ObservableList<T> : List<T>
{
    public event Action<T> OnItemAdded;
    public event Action<T> OnItemRemoved;

    public new void Add(T item)
    {
        base.Add(item);
        OnItemAdded?.Invoke(item);
    }

    public new bool Remove(T item)
    {
        var result = base.Remove(item);
        if (result)
            OnItemRemoved?.Invoke(item);
        return result;
    }
}
