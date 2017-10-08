using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour {
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("DeathOnContact"))
        {
            die();
        }
    }

    void die()
    {
        ObjectPooling.objectsToPause.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
}
