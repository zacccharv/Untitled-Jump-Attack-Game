using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    public class DisappearingPlatform : MonoBehaviour
    {
        private bool isOn;
        public float waitTime;
        private Rigidbody2D rb;
        private SpriteRenderer rend;
        public Color color1;
        public Color color2;

        IEnumerator coroutine;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rend = GetComponent<SpriteRenderer>();

            InvokeRepeating("FlipFlop", waitTime, waitTime);
        }

        private void Update()
        {
        }

        void FlipFlop()
        {
                if (!isOn)
                {
                    isOn = true;
                    rend.color = color1; 
                    rb.simulated = true;
                } else if (isOn)
                {
                    isOn = false;
                    rend.color = color2;
                    rb.simulated = false;
                }
        }
    }
}
