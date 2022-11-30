using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ZaccCharv
{
    public class PulsingLight : MonoBehaviour
    {
        private Light2D _light2D;
        public float _currentBrightness;

        private Coroutine _coroutine;

        public float _brightnessB;
        public float _duration;

        void Start()
        {
            _light2D = GetComponent<Light2D>();

            _coroutine = StartCoroutine(LerpFixedSpeed(_brightnessB, _duration));
        }

        public float SmoothProgress(float progress)
        {
            progress = Mathf.Lerp(-Mathf.PI/2, Mathf.PI/2, progress);
            progress = Mathf.Sin(progress);
            progress = (progress / 2f) + .5f;
            return progress;
        }

        private IEnumerator LerpFixedSpeed(float _brightnessB, float _duration)
        {
            while (true)
            {

                float timer = 0;
                float progress = 0;

                float _brightnessA = _light2D.intensity;

                float a = _brightnessA;
                float b = _brightnessB;

                float lerpdBrightness = _brightnessA;

                while (timer < _duration)
                {
                    progress = (timer / _duration);
                    progress = SmoothProgress(progress);

                    if (_duration < _duration/2)
                    {
                        a = _brightnessB;
                        b = _brightnessA;
                    } else if (_duration > _duration/2)
                    {
                        a = _brightnessB;
                        b = _brightnessA;
                    }

                    lerpdBrightness = Mathf.Lerp(a, b, progress);
                    _light2D.intensity = lerpdBrightness;

                    timer += Time.deltaTime;
                    Debug.Log("This is the timer: " + timer + ", This is progress: " + progress + ", This is brightness: " + _light2D.intensity);
                    yield return null;
                }

                timer = 1;
            }
        }
    }
}
