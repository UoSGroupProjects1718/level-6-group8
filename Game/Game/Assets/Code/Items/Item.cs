using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IMoveable
{
    [SerializeField]
    private float cost;
    public float Cost
    {
        get { return cost; }
        set { cost = value; }
    }

    [SerializeField]
    string displayName;
    public string DisplayName { get { return displayName; } }

    [SerializeField]
    Sprite itemSprite;
    public Sprite ItemSprite { get { return itemSprite; } }

    //public static bool operator== (Item left, Item right)
    //{
    //    return (left.DisplayName == right.DisplayName);
    //}

    //public static bool operator != (Item left, Item right)
    //{
    //    return (left.DisplayName != right.DisplayName);
    //}

    public bool Moving
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }
}

