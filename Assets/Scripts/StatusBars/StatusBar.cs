using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    [CreateAssetMenu(fileName = "NewStatusBar", menuName = "StatusBar")]
    public class StatusBar : ScriptableObject
    {
        public int statusBarTotal;
        public int statusBarCurrent;

        [SerializeField] private int addToBarTotal;
        [SerializeField] private int subtractFromBarTotal;
        [SerializeField] private int addToCurrent;
        [SerializeField] private int subtractFromCurrent;

        void AddToBarTotal()
        {
            statusBarTotal += addToBarTotal;
        }

        void SubtractFromBarTotal()
        {
            statusBarTotal -= subtractFromBarTotal;
        }

        void AddToCurrent()
        {
            statusBarCurrent += Mathf.Min(addToCurrent,(statusBarTotal-statusBarCurrent));
        } 

        public void SubtractFromCurrent()
        {
            statusBarCurrent -= Mathf.Min(subtractFromCurrent, statusBarCurrent);
        }
    }
}
