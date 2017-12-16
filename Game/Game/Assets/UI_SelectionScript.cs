using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_SelectionScript : MonoBehaviour {
    
    public void Select(GameObject obj)
    {
        obj.GetComponent<Shadow>().enabled = !obj.GetComponent<Shadow>().enabled;
    }
}
