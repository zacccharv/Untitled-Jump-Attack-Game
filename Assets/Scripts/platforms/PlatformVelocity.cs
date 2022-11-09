using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlatformVelocity : MonoBehaviour
{
    public GameObject Rect;

    public Transform start;
    public Transform end;

    private Vector3 _previous;
    public Vector2 _velocity;

    private Coroutine LerpCoroutine;
    public float LerpSpeed = 3;
    public AnimationCurve Curve;

    private void Start()
    {
        LerpCoroutine = StartCoroutine(LerpRectFixedSpeed());
    }

    void FixedUpdate()
    {
        _velocity = (transform.position - _previous) / Time.deltaTime;
        _previous = transform.position;
    }

    private IEnumerator LerpRectFixedSpeed()
    {
        while (true)
        {
            float distance = Vector3.Distance(start.position, end.position);
            float remainingDistance = distance;

            bool forward = true;

            while (remainingDistance > 0 && forward == true)
            {
                Rect.transform.position = Vector3.Lerp(start.position, end.position, Curve.Evaluate(1 - (remainingDistance / distance)));
                remainingDistance -= LerpSpeed * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            remainingDistance = distance;
            forward = false;

            while (remainingDistance > 0 && forward == false)
            {
                Rect.transform.position = Vector3.Lerp(end.position, start.position, Curve.Evaluate(1 - (remainingDistance / distance)));
                remainingDistance -= LerpSpeed * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
