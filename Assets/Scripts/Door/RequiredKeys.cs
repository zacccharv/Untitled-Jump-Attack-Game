using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    public class RequiredKeys : MonoBehaviour
    {
        [SerializeField] private int _requiredKeys;
        [SerializeField] private int _currentKeys;

        private void OnEnable()
        {
            EnergyDrink.OnEnergyPickup += UpdateKeys;
        }

        private void OnDisable()
        {
            EnergyDrink.OnEnergyPickup -= UpdateKeys;
        }

        void UpdateKeys()
        {
            _currentKeys++;

            if (_currentKeys == _requiredKeys)
            {
                OpenDoor();
            }
        }

        void OpenDoor()
        {
            gameObject.SetActive(false);
        }
    }
}
