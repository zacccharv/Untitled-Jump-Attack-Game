using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZaccCharv
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D rb;
        public float xVel;
        public float yVel;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
             Debug.Log(gameObject.transform.parent);
             rb.velocity = new Vector2(xVel, yVel);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                Destroy(gameObject);
            }
        }
    }
}
