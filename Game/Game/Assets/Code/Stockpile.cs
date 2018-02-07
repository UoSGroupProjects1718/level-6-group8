using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json.Bson;
using UnityEngine;

public class StockpileStats
{
    public Dictionary<string, uint> items;
    public float Value;
}

public class Stockpile
{
    private Dictionary<string, uint> items = new Dictionary<string, uint>();
    private Factory factory;
    private uint itemLimit;

    public long ItemCount { get { return items.Sum(x => x.Value); } }
    public Factory Factory { get { return factory; } }
    public Dictionary<string, uint> Items { get { return items; } }
    public float Value { get; set; }

    // Constructor
    public Stockpile(uint _itemLimit, Factory parent)
    {
        itemLimit = _itemLimit;
        factory = parent;
    }

    // Methods

    /// <summary>
    /// Returns true if the stockpile contains a certain item,
    /// else returns false.
    /// </summary>
    /// <param name="item">The item to check that the stockpile contains</param>
    /// <returns></returns>
    public bool Contains(Item item)
    {
        return items.ContainsKey(item.DisplayName);
    }

    /// <summary>
    /// Returns true if the stockpile is full.
    /// Else, returns false
    /// </summary>
    /// <returns></returns>
    public bool IsFull()
    {
        return items.Sum(x => x.Value) >= itemLimit;
    }

    public void AddOrIncrement(Item item, uint val)
    {
        /*
            Lyut:
                I just quickly commented out this function and added this little bit because
                it wasn't working properly and wanted to make sure that it would save and 
                load fine.

                -Joe
        */
        if(!IsFull())
        {
            uint itemsToAdd = (uint)Math.Min(val, itemLimit - ItemCount);
            Value += item.Cost * itemsToAdd;
            if (Contains(item))
            {
                items[item.DisplayName] += itemsToAdd;
            }
            else
            {
                items.Add(item.DisplayName, itemsToAdd);
            }
            Debug.Log(string.Format("Added {0} items to {1}", itemsToAdd, factory.FactoryName));
        } else
        {
            // Debug.Log("Factory cannot hold any more in stockpile.");
        }
        

        //if (items.Count == 0)
        //{
        //    items.Add(item.DisplayName, val);

        //    Debug.Log(string.Format("Amount of {0}: {1}", item.DisplayName, items[item.DisplayName]));
        //    return;
        //}

        //if (items.Sum(x => x.Value) < itemLimit)
        //{
        //    // If it contains the key
        //    if (items.ContainsKey(item.DisplayName))
        //    {
        //        // Add
        //        items[item.DisplayName] += val;
        //    }
        //    else
        //    {
        //        items.Add(item.DisplayName, val);
        //    }
        //}
    }

    /// <summary>
    /// Calls the SaveLoad utility class to save the stockpile to json
    /// </summary>
    public void SaveToFile()
    {
        SaveLoad.SaveStockpile(this);
    }

    /// <summary>
    /// Loads the stockpile data in from json
    /// </summary>
    public void LoadFromFile()
    {
        // Grab the data from json for this stockpile
        StockpileStats ss = SaveLoad.LoadStockpile(this);

        // If the data exists
        if (ss != null)
        {
            // Apply the data
            items = ss.items;
        }
        else
        {
            // Else, default values
            items = new Dictionary<string, uint>();
        }


    }

    //public string SerialiseStockpile(string factoryToSerialiseName, Stockpile factoryToSerialiseStockpile)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    sb.AppendFormat("[{0}]", factoryToSerialiseName);
    //    sb.Append("{");
    //    foreach (var itemPair in factoryToSerialiseStockpile.items)
    //    {
    //        sb.AppendFormat("({0}):{1}|", itemPair.Key, itemPair.Value);
    //    }
    //    sb.Append("};");
    //    Debug.Log("Serialised Stockpile: " + sb.ToString());
    //    return sb.ToString();
    //}

    //public void DeserialiseStockpile(string serialisedFactory)
    //{
    //    var factories = serialisedFactory.Split(';');
    //    foreach (var factoryString in factories)
    //    {
    //        var factoryName = factoryString.Split(']')[0].Substring(1);
    //        Debug.Log("Factory Name: " + factoryName);
    //        if (factoryName == factory.name)
    //        {
    //            Clear();
    //            var factoryInfo = factoryString.Split('}')[0].Substring(1);
    //            var itemPairs = factoryInfo.Split('|');
    //            foreach (var itemPair in itemPairs)
    //            {
    //                var item = itemPair.Split(':')[0].Substring(1, itemPair.Split(':')[0].Length - 1);
    //                Debug.Log(item);
    //                var itemCount = itemPair.Split(':')[1].Substring(0, itemPair.Split(':')[1].Length - 1);
    //                Debug.Log(itemCount);
    //                items.Add(item, uint.Parse(itemCount));
    //            }
    //            return;
    //        }
    //    }
    //    Debug.LogError("Could not find factory with same name as base factory in serialised string.");

    //}

    ///// <summary>
    ///// Saves the stockpile of a factory to a file with the key being the factory name
    ///// </summary>
    //public void SaveStockpileToDevice()
    //{
    //    ////PlayerPrefs.SetString(factory.name, SerialiseStockpile());
    //}

    public void Clear()
    {
        //TODO: increment a resouce here such as PrimaryMoney (potentially rename as sellitems)

        GameManager.Instance.Player.AddPrimaryMoney((uint)Math.Floor(Value));
        items.Clear();
        Value = 0;
    }
}
