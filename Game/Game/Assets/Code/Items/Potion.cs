using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : CraftableItem
{
    [Header("Required machines")]
    [SerializeField]
    Machine[] machinesRequired;
}