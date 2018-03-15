using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScrollableList : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField]
    protected GameObject itemPrefab;

    [Header("Columns")]
    [SerializeField]
    protected int columnCount;

    // protected list of objects
    protected List<GameObject> objectList;
    protected bool loaded = false;

    // Public getter
    public List<GameObject> ObjectList { get { return objectList; } }

    public abstract void Fill();
}