using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    [CreateAssetMenu(fileName = "NewStatusBar", menuName = "StatusBar")]
    public class StatusBar : ScriptableObject
    {
        public float statusBarTotal;
        public float statusBarCurrent;
        public float IncrementAmount[] = float["addToBarTotal", "subtractFromBarTotal", "addToCurrent", "subtractFromCurrent"]; 

        void AddToBarTotal()
        {
            statusBarTotal += IncrementAmount["addToBarTotal"];
        }

        void SubtractFromBarTotal()
        {
            statusBarTotal -= IncrementAmount["subtractFromBarTotal"];
        }

        void AddToCurrent()
        {
            statusBarCurrent += IncrementAmount["addToCurrent"];
        } 

        void SubtractFromCurrent()
        {
            statusBarCurrent -= IncrementAmount["subtractFromCurrent"];
        }
    }
}
