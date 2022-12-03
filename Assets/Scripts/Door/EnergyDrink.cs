using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Events;

namespace ZaccCharv
{
    public class EnergyDrink : MonoBehaviour
    {
        public delegate void EnergyDrinkPickup();
        public static event EnergyDrinkPickup OnEnergyPickup;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                OnEnergyPickup?.Invoke();
            }
        }
    }
}
