using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv 
{
    public class Drips : MonoBehaviour 
    {
        public float _offsetTime;
        public float _repeatTime;
        public GameObject _prefab;
        private GameObject _newDrip;

        private void Start() 
        {
            InvokeRepeating("DripDrop", _offsetTime, _repeatTime);
        }

        void DripDrop() 
        {
            _newDrip = Instantiate(_prefab, transform.position, Quaternion.Identity, this.gameObject);
        }

    }
}