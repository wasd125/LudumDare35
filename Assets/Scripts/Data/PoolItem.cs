using UnityEngine;
using System.Collections;

public class PoolItem<T> {

	public bool Active { get; set; }
    public T MyComponent { get; private set; }

    public PoolItem(T component)
    {
        MyComponent = component;
    }

}
