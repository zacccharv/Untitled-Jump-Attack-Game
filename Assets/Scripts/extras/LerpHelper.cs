using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ZaccCharv
{
    [RequireComponent(typeof(LerpOut))]
    public class LerpHelper : MonoBehaviour
    {
        private Coroutine _coroutine;
        [SerializeField] private LerpOut _lerpOut = null;
        [HideInInspector] public float _lerpStartValue;
        public float _valueB;
        public float _duration;

        void Start()
        {

            _lerpStartValue = _lerpOut.lerpValue.ValueA(gameObject);

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

                float _valueA = _lerpStartValue;

                float lerpdValue = _valueA;
                bool forward = true;

                while (timer < _duration && forward) 
                {
                    progress = (timer / _duration);
                    progress = SmoothProgress(progress);

                    lerpdValue = Mathf.Lerp(_valueA, _valueB, progress);
                    
                    _lerpStartValue = lerpdValue;

                    Debug.Log(_lerpStartValue);

                    // can't be swapped
                    timer += Time.deltaTime;
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

                    _lerpStartValue = lerpdValue;
                    Debug.Log(_lerpStartValue);

                    // can't be swapped
                    timer += Time.deltaTime;

                    yield return null;           
                }
            }
        }
    }
}
