using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{    
    public void PanCamera(float x, float y, float panTime)
    {
        StartCoroutine(MoveTo(x, y, panTime));
    }

    public void PanCamera(Vector2 pos, float panTime)
    {
        StartCoroutine(MoveTo(pos.x, pos.y, panTime));
    }

    private IEnumerator MoveTo(float x, float y, float panTime)
    {
        Vector3 target = new Vector3(x, transform.position.y, y);

        float timeCount = 0;
        while (timeCount < panTime)
        {
            timeCount += Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target,    timeCount / panTime);
            yield return null;
        }

    }
    
    
}