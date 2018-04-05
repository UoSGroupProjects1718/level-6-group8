using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Transparent sprite")]
    [SerializeField]
    Sprite transparent;

    public Sprite TransparentImage { get { return transparent; } }
}
