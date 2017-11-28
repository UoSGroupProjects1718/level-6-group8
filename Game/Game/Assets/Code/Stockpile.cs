//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class Stockpile
//{
//    private Dictionary<Item, uint> items = new Dictionary<Item, uint>();
//    private readonly uint itemLimit;
//    public long ItemCount
//    {
//        get { return PlayerPrefs.GetInt("TotalItemCount"); }
//    }

//    public Stockpile(uint itemLimit = 0)
//    {
//        this.itemLimit = itemLimit;
//        PlayerPrefs.SetInt("ItemLimit", (int)itemLimit);
//        PlayerPrefs.Save();
//    }

//    public bool Contains(Item item)
//    {
//        return PlayerPrefs.HasKey(item.name);
//    }

//    public bool IsFull()
//    {
//        return ItemCount >= itemLimit;
//    }

//    public void AddOrIncrement(Item item)
//    {
//        if (ItemCount < itemLimit)
//        {
//            if (PlayerPrefs.HasKey(item.name))
//            {
//                PlayerPrefs.SetInt("ITEM: " + item.name, PlayerPrefs.GetInt(item.name) + 1);
//                PlayerPrefs.SetInt("TotalItemCount", PlayerPrefs.GetInt("TotalItemCount") +1);
//            } else
//            {
//                PlayerPrefs.SetInt("ITEM: " + item.name, 1);
//                PlayerPrefs.SetInt("TotalItemCount", PlayerPrefs.GetInt("TotalItemCount") + 1);
//            }
//        }
//        PlayerPrefs.Save();
//    }

//    public void ClearItems()
//    {
//        //TODO: increment a resouce here such as money (potentially rename as sellitems)
//        var maxItems = PlayerPrefs.GetInt("ItemLimit");
//        PlayerPrefs.DeleteAll();
//        PlayerPrefs.SetInt("TotalItemCount", 0);
//        PlayerPrefs.SetInt("ItemLimit", maxItems);
//    }


//}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class Stockpile
{
    private Dictionary<string, uint> items = new Dictionary<string, uint>();
    public Factory factory;
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
        return items.ContainsKey(item.name);
    }

    public bool IsFull()
    {
        return items.Sum(x => x.Value) >= itemLimit;
    }

    public void AddOrIncrement(Item item)
    {
        if (items.Sum(x => x.Value) < itemLimit)
        {
            if (items.ContainsKey(item.name)) items[item.name]++;
            else
            {
                items.Add(item.name, 1);
            }
        }
    }

    public string SerialiseStockpile(string factoryToSerialiseName, Stockpile factoryToSerialiseStockpile)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("[{0}]", factoryToSerialiseName);
        sb.Append("{");
        foreach (var itemPair in factoryToSerialiseStockpile.items)
        {
            sb.AppendFormat("({0}):{1}|", itemPair.Key, itemPair.Value);
        }
        sb.Append("};");
        Debug.Log("Serialised Stockpile: " + sb.ToString());
        return sb.ToString();
    }

    public void DeserialiseStockpile(string serialisedFactory)
    {
        var factories = serialisedFactory.Split(';');
        foreach (var factoryString in factories)
        {
            var factoryName = factoryString.Split(']')[0].Substring(1);
            Debug.Log("Factory Name: " + factoryName);
            if (factoryName == factory.name)
            {
                Clear();
                var factoryInfo = factoryString.Split('}')[0].Substring(1);
                var itemPairs = factoryInfo.Split('|');
                foreach (var itemPair in itemPairs)
                {
                    var item = itemPair.Split(':')[0].Substring(1, itemPair.Split(':')[0].Length - 1);
                    Debug.Log(item);
                    var itemCount = itemPair.Split(':')[1].Substring(0, itemPair.Split(':')[1].Length - 1);
                    Debug.Log(itemCount);
                    items.Add(item, uint.Parse(itemCount));
                }
                return;
            }
        }
        Debug.LogError("Could not find factory with same name as base factory in serialised string.");

    }

    /// <summary>
    /// Saves the stockpile of a factory to a file with the key being the factory name
    /// </summary>
    public void SaveStockpileToDevice()
    {
        ////PlayerPrefs.SetString(factory.name, SerialiseStockpile());
    }

    public void Clear()
    {
        //TODO: increment a resouce here such as money (potentially rename as sellitems)
        items.Clear();
    }


}
