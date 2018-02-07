using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TownSection : MonoBehaviour
{
    [Header("Town section stats")]
    [SerializeField]
    uint cost;
    [SerializeField]
    uint id;
    [SerializeField]
    bool unlocked;

    [Header("Factories")]
    [SerializeField]
    private Factory[] factories;
    [SerializeField]
    private GameObject[] lights;

    public uint Cost { get { return cost; } }
    public uint ID { get { return id; } }
    public bool Unlocked { get { return unlocked; } }

    public Factory[] Factories { get { return factories; } }

    /// <summary>
    /// Unlocks this section of the map. Unlocks the factories 
    /// and enables all of the lights.
    /// </summary>
    public void Unlock(bool unlock)
    {
        // Unlock factories
        foreach (Factory factory in factories)
        {
            factory.Unlock(unlock);
        }
        EnableLights(unlock);
    }

    public void EnableLights(bool status)
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(status);
        }
    }
}