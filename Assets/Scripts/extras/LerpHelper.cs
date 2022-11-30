using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ZaccCharv
{
    public class LerpHelper : MonoBehaviour
    {
        //swappable class
        public GameObject _light2D;
        private Coroutine _coroutine;

        public float _valueB;
        public float _duration;

        void Start()
        {
            //swappable component
            _light2D = GetComponent<Light2D>();

            _coroutine = StartCoroutine(LerpFixedSpeed(_valueB, _duration));
        }

        public float SmoothProgress(float progress)
        {
            progress = Mathf.Lerp(-Mathf.PI/2, Mathf.PI/2, progress);
            progress = Mathf.Sin(progress);
            progress = (progress / 2f) + .5f;
            return progress;
        }

        private IEnumerator LerpFixedSpeed(float _valueB, float _duration)
        {
            while (true)
            {
                float timer = 0;
                float progress = 0;

                float _valueA = _light2D.intensity;

                float lerpdValue = _valueA;
                bool forward = true;

                while (timer < _duration && forward) 
                {
                    progress = (timer / _duration);
                    progress = SmoothProgress(progress);

                    //can be swapped
                    lerpdValue = Mathf.Lerp(_valueA, _valueB, progress);
                    Debug.Log("A = brightnessA");
                    Debug.Log("B = brightnessB");
                    
                    _light2D.intensity = lerpdValue;

                    // can't be swapped
                    timer += Time.deltaTime;

                    //can be swapped
                    Debug.Log("This is the timer: " + timer + ", This is progress: " + progress + ", This is Lerpd value: " + _light2D.intensity);
                    yield return null;   
                } 

                timer = 0;
                forward = false;

                while (timer < _duration && !forward)
                {
                    progress = (timer / _duration);
                    progress = SmoothProgress(progress);

                    //can be swapped
                    lerpdValue = Mathf.Lerp(_valueB, _valueA, progress);
                    Debug.Log("A = brightnessB");
                    Debug.Log("B = brightnessA");      

                    _light2D.intensity = lerpdValue;

                    // can't be swapped
                    timer += Time.deltaTime;

                    //can be swapped
                    Debug.Log("This is the timer: " + timer + ", This is progress: " + progress + ", This is lerpd value: " + _light2D.intensity);
                    yield return null;           
                }
            }
        }
    }
}
