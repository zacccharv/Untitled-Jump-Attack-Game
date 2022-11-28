using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zacccharv;

namespace ZaccCharv
{
    public class Dash : MonoBehaviour
    {
        private Rigidbody2D rb;
        private Jump _jump;

        private float _direction;
        [HideInInspector] public bool _canDash = true, _isDashing;
        public float dashingPower, dashingTime, dashingCoolDown, _dashPhase = 0;

        private IEnumerator coroutine;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            _jump = GetComponent<Jump>();
        }

        void Update()
        {
            if (Input.GetButtonDown("Dash"))
            {
                if (GetComponent<CharCollisions>()._touchingBottom || GetComponent<Jump>()._wallSliding || _dashPhase < 1 )
                {
                    if (_jump._wallSliding) _jump._jumpPhase = 0;
                    
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
            
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(1 * dashingPower * _direction, 0f);
            yield return new WaitForSeconds(dashingTime);

            Debug.Log("donedashing");

            rb.velocity = new Vector2(.5f * dashingPower * _direction, 0f);

            _dashPhase = 1;
            _isDashing = false;
            _canDash = true;
        }
    }
}
