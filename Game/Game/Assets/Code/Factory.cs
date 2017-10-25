using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField]
    private bool unlocked;
    private float efficiency;

    [Header("Unlock level")]
    [SerializeField]
    private int levelToUnlock;

    [Header("Factory name")]
    [SerializeField]
    private string factoryName;

    [Header("Factory sprite")]
    [SerializeField]
    private Sprite factorySprite;

    public bool IsUnlocked { get { return unlocked; } }
    public int LevelToUnlock { get { return levelToUnlock; } }
    public float FactoryEfficiency { get { return efficiency; } }
    public string FactoryName { get { return factoryName; } }
    public Sprite FactorySprite { get { return factorySprite; } }
   

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    private void OnMouseDown()
    {
        // Set this as the currently-selected factory in the overworld
        GameObject.Find("OverworldController").GetComponent<Overworld>().SetCurrentFactory(this);

        // Update the canvas to open a pannel with this factories stats
        GameObject.Find("Canvas").GetComponent<OverworldCanvas>().DisplayFactory(this);  
    }

    public void UnlockFactory()
    {
        unlocked = true;
    }

    public void CalculateEfficiency()
    {

    }

    public void LoadInternalFactory()
    {

    }

    public void SaveToFile()
    {

    }
}
