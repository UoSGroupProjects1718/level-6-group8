using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stockpile
{
    private Dictionary<Item, uint> items = new Dictionary<Item, uint>();
    uint itemLimit;
    public long ItemCount
    {
        get { return items.Sum(x => x.Value); }
    }

    public Stockpile(uint itemLimit = 0)
    {
        this.itemLimit = itemLimit;
    }

    public bool Contains(Item item)
    {
        return items.ContainsKey(item);
    }

    public bool IsFull()
    {
        return items.Sum(x => x.Value) >= itemLimit;
    }

    public void AddOrIncrement(Item item)
    {
        if (items.Sum(x => x.Value) < itemLimit)
        {
            if (items.ContainsKey(item)) items[item]++;
            else
            {
                items.Add(item, 1);
            }
        }
    }

    public void Clear()
    {
        //TODO: increment a resouce here such as money (potentially rename as sellitems)
        items.Clear();
    }


}
