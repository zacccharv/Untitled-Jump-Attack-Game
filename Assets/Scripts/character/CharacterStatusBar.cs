using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ZaccCharv
{
    public class CharacterStatusBar : MonoBehaviour
    {
        [SerializeField] private bool safe = true;

        private IEnumerator coroutine;

        public int staminaMax;
        public int total;
        public int current;

        private void Update()
        {
            total = staminaMax;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 10)
            {
                safe = true;
                current = total;
                StopCoroutine(coroutine);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 10)
            {
                safe = false;
                coroutine = Timer(1.75f, total);
                StartCoroutine(coroutine);
            }
        }

        IEnumerator Timer(float waitTime, int total)
        {
            current = total;

            for (int i = total; i > 0; i--)
            { 
                SubtractFromCurrent();
                if (current == 0)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                yield return new WaitForSeconds(waitTime);
            }
        }

        void SubtractFromCurrent()
        {
            current -= 10;
        }
    }
}
