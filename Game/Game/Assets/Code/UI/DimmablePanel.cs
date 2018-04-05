using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DimmablePanel : MonoBehaviour
{
    public abstract void Highlight();
    public abstract void Dim();
}