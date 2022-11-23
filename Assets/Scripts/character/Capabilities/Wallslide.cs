using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaccCharv;

namespace Zacccharv
{
    public class Wallslide : MonoBehaviour  
    {
        public Rigidbody2D _body;
        public Jump _jump;
        private CharCollisions _charCollisions;
        
        [HideInInspector] public bool _wallSliding;
        [HideInInspector] public bool _wallJumping;
        [HideInInspector] public bool _wallGrab;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _charCollisions = GetComponent<ZaccCharv.CharCollisions>();
        }

        public void WallSlidingCheck()
        {
            if (_charCollisions._rightWallhit || _charCollisions._leftWallHit)
            {
                _wallSliding = true;
                GetComponent<ZaccCharv.Dash>()._dashPhase = 0;
            }
            else
            {
                _wallSliding = false;
            }

            // If pushing into wall and going up no upwardMovement. //
            if (_wallSliding && _body.velocity.y > 0 && !_wallGrab)
            {
                _body.gravityScale = _jump._upwardMovementMultiplier + .5f;
            }
            // If pushing into wall going down decreased slide. //
            else if (_wallSliding && (_jump._controller.input.RetrieveMoveInput() != 0 || _jump._controller.input.RetrieveMoveInput2() != 0) && _body.velocity.y < 0 && !_wallGrab)
            {
                _body.gravityScale = _jump._downwardMovementMultiplier / 12;
            }
            // If pushing into wall, going down and no inputs. //
            else if (_wallSliding && _jump._controller.input.RetrieveMoveInput() == 0 && _jump._controller.input.RetrieveMoveInput2() == 0 && !_wallGrab && _body.velocity.y < 0)
            {
                _body.gravityScale = _jump._downwardMovementMultiplier + .5f;
               _jump._velocity.y = Mathf.Max(_jump._fallClamp, _body.velocity.y);
            }
            WallGrabCheck();
        }
                public void WallGrabCheck()
        {
            Debug.Log(Input.GetButton("WallGrab"));
            if (_wallSliding)
            {
                if (Input.GetButton("WallGrab"))
                {
                    _wallGrab = true;
                    _jump._jumpPhase = _jump._maxAirJumps - 1;
                    
                }
                if (!Input.GetButton("WallGrab"))
                {
                    _wallGrab = false;
                }
            }
            if (!_wallSliding || Input.GetButton("Jump"))
            {
                _jump._jumpPhase = 1;
                _wallGrab = false;
            }

            if (_wallGrab && !Input.GetButton("Jump"))
            {
                _body.gravityScale = 0;
                _body.velocity = new Vector2(0, 0);
                _wallGrab = true;
            }
            else if (_wallGrab && Input.GetButton("Jump"))
            {
                _body.gravityScale = _jump._upwardMovementMultiplier + .5f;
            }
        }
        public void NotWallJumping()
        {
            _wallJumping = false;
        }
    }
}