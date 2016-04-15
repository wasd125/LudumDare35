using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PoolManager<T> {

    public List<PoolItem<T>> PoolItems { get; private set; }

    public PoolItem<T> SpawnItem()
    {
        if (PoolItems.Where(p => p.Active == false).ToList().Count < 1)
        {
            // was fehlt hier?
            //PoolItem<T> item = new PoolItem<T>();
            //PoolItems.Add(item);
            //return item;
            // item.Active = true;
            return null;
        }
        else
        {
            return PoolItems.Where(p => p.Active == false).FirstOrDefault();
        }
    }

    public void DespawnItem(  )
    {

    }

    public PoolManager()
    {
        PoolItems = new List<PoolItem<T>>();
    }
	
}
