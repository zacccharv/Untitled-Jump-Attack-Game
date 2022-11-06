using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public CameraTriggers cameraTriggers;
    public bool triggered;


    void KeepMoving(bool triggered)
    {
        Vector3 towards = new Vector3(.2f,0,0);
        Debug.Log(triggered);
        if (triggered == false)
        {
            transform.Translate(towards * (Time.deltaTime * 15));
        }
    }

    private void FixedUpdate()
    {
        triggered = cameraTriggers.isTriggered;
        KeepMoving(triggered);
    }
}
