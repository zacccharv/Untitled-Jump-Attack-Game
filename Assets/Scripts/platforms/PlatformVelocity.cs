using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVelocity : MonoBehaviour
{
    private Vector3 previous;
    public Vector2 _velocity;

    void Update()
    {
        _velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;
    }

}
