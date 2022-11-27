using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ZaccCharv
{
    public class Lazer : MonoBehaviour
    {

        private Vector3 _startWidth;
        private Vector3 _endWidth;

        public float _startWidthX;
        public float _endWidthX;

        private Coroutine LerpCoroutine;
        public float _lerpSpeed = 3;
        public float _offset;
        public AnimationCurve _curve;

        private void Start()
        {
            LerpCoroutine = StartCoroutine(LerpRectFixedSpeed());
            transform.localScale = new Vector3(_startWidthX, transform.localScale.y, transform.localScale.z);
        }

        private IEnumerator LerpRectFixedSpeed()
        {
            bool _offsetDone = false;
            while (true)
            {
                _startWidth = new Vector3(_startWidthX, transform.localScale.y, transform.localScale.z);
                _endWidth = new Vector3(_endWidthX, transform.localScale.y, transform.localScale.z);

                float _width = Vector3.Distance(_startWidth, _endWidth);
                float _remainingWidth = _width;

                bool forward = true;

                if(!_offsetDone)
                {
                    _offsetDone = true;
                    yield return new WaitForSeconds(_offset);
                }

                while (_remainingWidth > 0 && forward == true)
                {
                    transform.localScale = Vector3.Lerp(_startWidth, _endWidth, _curve.Evaluate(1 - (_remainingWidth / _width)));
                    _remainingWidth -= _lerpSpeed * Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }

                _remainingWidth = _width;
                forward = false;

                while (_remainingWidth > 0 && forward == false)
                {
                    transform.localScale = Vector3.Lerp(_endWidth, _startWidth, _curve.Evaluate(1 - (_remainingWidth / _width)));
                    _remainingWidth -= _lerpSpeed * Time.deltaTime;
                    yield return new WaitForFixedUpdate();
                }
            }
        }
    }
}
