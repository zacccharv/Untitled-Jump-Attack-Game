using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CameraTriggers : MonoBehaviour
{
    public bool isTriggered = false;

    private void Start()
    {
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow cube at the transform position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 0));
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isTriggered == true) return;
        isTriggered = true;
    }
}
