using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zacccharv;

namespace ZaccCharv
{
    public class Dash : MonoBehaviour
    {
        private Rigidbody2D rb;

        private float _direction;
        [HideInInspector] public bool _canDash = true, _isDashing;
        public float dashingPower, dashingTime, dashingCoolDown, _dashPhase = 0;

        private IEnumerator coroutine;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {


            if (Input.GetButtonDown("Dash"))
            {
                if (GetComponent<CharCollisions>()._touchingBottom || GetComponent<Jump>()._wallSliding || _dashPhase < 1 )
                {
                    coroutine = Dashing();
                    StartCoroutine(coroutine);
                    _dashPhase += 1;
                    _direction = GetComponent<CharacterAnimator>().FlipDirection;
                    Debug.Log("this is direction" + _direction);

                }
            }

        }

        IEnumerator Dashing()
        {
            _direction = GetComponent<CharacterAnimator>().FlipDirection;

            _canDash = false;
            _isDashing = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(1 * dashingPower * _direction, 0f);
            yield return new WaitForSeconds(dashingTime);

            rb.gravityScale = originalGravity;
            _isDashing = false;

            yield return new WaitForSeconds(dashingCoolDown);

            _canDash = true;
        }
    }
}
