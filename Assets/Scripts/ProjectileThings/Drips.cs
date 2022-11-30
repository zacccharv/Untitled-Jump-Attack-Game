using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv 
{
    public class Drips : MonoBehaviour 
    {
        public float _offsetTime;
        private float _repeatTime = .3f;
        public GameObject _prefab;
        int num = 0;
        int modulater = 18;
        private void Start() 
        {
            InvokeRepeating("DripDrop", _offsetTime, _repeatTime);
        }

        void DripDrop() 
        {
            num++;
            int randNum = Mathf.RoundToInt(Random.Range(0, 2));

            if (num % modulater < 7)
            {
                if (num % modulater >= 3)
                {
                    if (randNum == 0)
                    {
                        Instantiate(_prefab, transform.position, Quaternion.identity, gameObject.transform);
                    }
                }
                else if (num % modulater == 0 || num % modulater == 2)
                {
                    Instantiate(_prefab, transform.position, Quaternion.identity, gameObject.transform);
                }
            }
        }

    }
}