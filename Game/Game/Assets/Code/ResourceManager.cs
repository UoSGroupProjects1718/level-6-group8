using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header("Transparent sprite")]
    [SerializeField]
    Sprite transparent;

    [Header("Output complete sprite")]
    [SerializeField]
    Sprite outputComplete;

    public Sprite TransparentImage { get { return transparent; } }
    public Sprite OutputComplete { get { return outputComplete; } }
}
