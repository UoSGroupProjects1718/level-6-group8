using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSectionToFile
{
    public bool unlocked;
}

[System.Serializable]
public class TownSection : MonoBehaviour
{
    [Header("Town section stats")]
    [SerializeField]
    bool unlocked;
    [SerializeField]
    uint cost;
    [SerializeField]
    uint id;

    [Header("Factories")]
    [SerializeField]
    private Factory[] factories;
    [SerializeField]
    private GameObject[] lights;

    public bool Unlocked { get { return unlocked; } }
    public uint Cost { get { return cost; } }
    public uint ID { get { return id; } }
    public Factory[] Factories { get { return factories; } }

    /// <summary>
    /// Unlocks this section of the map. Unlocks the factories 
    /// and enables all of the lights.
    /// </summary>
    public void Unlock()
    {
        unlocked = true;

        // Unlock factories
        foreach (Factory factory in factories)
        {
            factory.Unlock();
        }

        EnableLights();
    }

    public void EnableLights()
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(true);
        }
    }

    public void SaveToFile()
    {
        SaveLoad.SaveTownSection(this);
    }

    public void LoadFromFile()
    {
        TownSectionToFile saveData = SaveLoad.LoadTownSection(id);

        if (saveData == null)
        {
            unlocked = false;
        }
        else
        {
            unlocked = saveData.unlocked;
        }
    }
}