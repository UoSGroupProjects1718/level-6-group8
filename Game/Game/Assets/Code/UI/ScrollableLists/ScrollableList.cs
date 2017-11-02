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

    protected bool loaded = false;

    public abstract void Fill();
}