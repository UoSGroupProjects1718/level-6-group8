using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    rawIngredient,
    compound,
    potion,
    waste
}

public abstract class Item : MonoBehaviour, IMoveable
{
    /*
        Because a global value does not fit every item, each item must be tweaked 
        to sit perfectly on the production line. These variables store the values for 
        the y position, the rotation and the scale of each item (potion, ingredient) 
        that travels along the production line.
    */
    [Header("Position, Rotation and scale in levels")]
    [SerializeField]
    protected float prodline_yHeight;
    [SerializeField]
    protected Vector3 prodline_rotation;
    [SerializeField]
    protected Vector3 prodline_scale;

    public float ProductionLine_YHeight { get { return prodline_yHeight; } }
    public Vector3 ProductionLine_Rotation { get { return prodline_rotation; } }
    public Vector3 ProductionLine_Scale { get { return prodline_scale; } }

    [Header("Item variables")]
    [SerializeField]
    private ItemType itemType;
    [SerializeField]
    private float cost;
    [SerializeField]
    string displayName;
    [SerializeField]
    Sprite itemSprite;


    public ItemType ItemType { get { return itemType; } }
    public float Cost
    {
        get { return cost; }
        set { cost = value; }
    }
    public string DisplayName { get { return displayName; } }
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

