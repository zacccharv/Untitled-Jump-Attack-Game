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
        private IEnumerator coroutine;

        public int staminaMax;
        public int total;
        public int current;
        public float staminaBurnRate;

        private void Start()
        {
            coroutine = Timer(staminaBurnRate, total);
            StartCoroutine(coroutine);
        }

        private void Update()
        {
            total = staminaMax;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 10 && coroutine != null)
            {
                current = total;
                StopCoroutine(coroutine);
            }
            if (collision.gameObject.tag == "Energy")
            {
                current = total;
                StopCoroutine(coroutine);

                coroutine = Timer(staminaBurnRate, total);
                StartCoroutine(coroutine);



                collision.gameObject.SetActive(false);
            }
            if (collision.gameObject.tag == "Faerie")
            {
                StopCoroutine(coroutine);

                coroutine = Timer(staminaBurnRate / 2, current);
                StartCoroutine(coroutine);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 10 )
            {
                coroutine = Timer(staminaBurnRate, total);
                StartCoroutine(coroutine);
            }
            if (collision.gameObject.tag == "Faerie")
            {
                StopCoroutine(coroutine);

                coroutine = Timer(staminaBurnRate, current);
                StartCoroutine(coroutine);
            }

        }

        IEnumerator Timer(float waitTime, int _total)
        {
            current = _total;

            for (int i = total; i > 0; i--)
            { 
                SubtractFromCurrent();
                if (current == 5)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                yield return new WaitForSeconds(waitTime);
            }
        }

        void SubtractFromCurrent()
        {
            current -= 5;
        }
    }
}
