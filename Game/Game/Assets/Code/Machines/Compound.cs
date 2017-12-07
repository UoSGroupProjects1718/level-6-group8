using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compound : Item
{
    [Header("Compound components")]
    [SerializeField]
    private List<string> components;

    public List<string> Components { get { return components; } }

    public void SetComponents(List<string> comp)
    {
        components = comp;
    }
}
