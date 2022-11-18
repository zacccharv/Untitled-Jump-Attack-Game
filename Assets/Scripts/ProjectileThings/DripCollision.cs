using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DripCollision : MonoBehaviour
// RENAME to drip prefab
{
    private Rigidbody2D rb;
    public AnimationCurve curve;
    public float curveTimeStart;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curveTimeStart = Time.time;
    }
    void Update()
    {
        rb.velocity = new Vector2(0, curve.Evaluate(Time.time- curveTimeStart) * -5);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 7)
        {
            Destroy(gameObject);
        }
    }
}
