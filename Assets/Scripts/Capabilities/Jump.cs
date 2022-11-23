using UnityEngine;
using System.Collections;

namespace ZaccCharv
{
    [RequireComponent(typeof(Controller))]
    public class Jump : MonoBehaviour
    {
        #region Jumping EarlyJump Falling FallClamp Vars

        [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
        [SerializeField, Range(0, 5)] public int _maxAirJumps = 0;
        [SerializeField, Range(0f, 5f)] public float _downwardMovementMultiplier = 3f;
        [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;
        [SerializeField, Range(0f, 5f)] public float _doubleJumpMultiplier = 3f;
        [SerializeField] private Controller _controller = null;
        [SerializeField] private float _fallClamp;

        // See if creating new class solves cluttering scripts
        private CharCollisions _charCollisions;
        private Animator _animator;
        private Rigidbody2D _body;
        public Vector2 _velocity;
        private float _defaultGravityScale, _jumpSpeed;
        public bool _desiredJump;
        public int _jumpPhase;

        #endregion

        #region Wall Jumping and Sliding Vars

        [HideInInspector] public bool _wallSliding;
        [HideInInspector] public bool _wallJumping;
        [HideInInspector] public bool _wallGrab;

        #endregion

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();
            _animator = GetComponent<Animator>();
            _charCollisions = gameObject.AddComponent<CharCollisions>();

            _charCollisions._rayCenter = GetComponentInChildren<Transform>();

            _defaultGravityScale = 1f;
        }

        void Update()
        {
            if (GetComponent<Dash>()._isDashing)
            {
                return;
            }

            _desiredJump |= _controller.input.RetrieveJumpInput();
        }
        private void FixedUpdate()
        {
            if (GetComponent<Dash>()._isDashing)
            {
                return;
            }

            _velocity = _body.velocity;

            _charCollisions.CharCollisionCheck();

            JumpActionCheck();

            WallSlidingCheck();

            WallGrabCheck();

            // This has to go last
            _body.velocity = _velocity;
        }

        public void JumpAction()
        {
            if (_wallSliding)
            {
                _wallJumping = true;
            }
            if ((_charCollisions._touchingBottom || _jumpPhase < _maxAirJumps) && !_wallJumping)
            {
                _jumpPhase += 1;

                _animator.SetTrigger("Jumped");
                _animator.SetBool("Falling", false);

                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);

                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                else if (_velocity.y < 0f)
                {
                    _jumpSpeed += Mathf.Abs(_body.velocity.y);
                }
                _velocity.y += _jumpSpeed;
            }
            else if (!_charCollisions._touchingBottom && _wallJumping)
            {
                _jumpPhase += 1;

                float _wallHitDirection = 0;

                if (_charCollisions._rightWallhit)
                {
                    _wallHitDirection = -1;
                }
                else if (_charCollisions._leftWallHit)
                {
                    _wallHitDirection = 1;
                }

                _animator.SetTrigger("Jumped");
                _animator.SetBool("Falling", false);

                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);
                var FlipDir = _wallHitDirection + gameObject.GetComponent<CharacterAnimator>().FlipDirection;
                var InputAxis = (Input.GetAxis("Horizontal") * 2);

                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y / 2, 0f);
                }
                else if (_velocity.y < 0f)
                {
                    _jumpSpeed += Mathf.Abs(_body.velocity.y / 4);
                }

                if (_charCollisions._leftWallHit && gameObject.GetComponent<CharacterAnimator>().FlipDirection == -1)
                {
                    _velocity.x -= 5 * FlipDir + InputAxis;
                }
                if (_charCollisions._leftWallHit && gameObject.GetComponent<CharacterAnimator>().FlipDirection == 1)
                {
                    _velocity.x += 6 * FlipDir + InputAxis;
                }
                else if (_charCollisions._rightWallhit && gameObject.GetComponent<CharacterAnimator>().FlipDirection == -1)
                {
                    _velocity.x += 6 * FlipDir + InputAxis;
                }
                else if (_charCollisions._rightWallhit && gameObject.GetComponent<CharacterAnimator>().FlipDirection == 1)
                {
                    _velocity.x -= 5 * FlipDir + InputAxis;
                }

                _wallJumping = true;
                _velocity.y += _jumpSpeed;
                }
            }
        
        private void JumpActionCheck()
        {
            if (_charCollisions._touchingBottom)
            {
                _jumpPhase = 0;
                GetComponent<Dash>()._dashPhase = 0;
            }
            if (_charCollisions._earlyJump && _body.velocity.y < 0)
            {
                _charCollisions._earlyJump = false;
                _jumpPhase = 0;

            }

            if (_desiredJump && !_wallSliding && !_wallJumping)
            {
                _desiredJump = false;
                JumpAction();
            }
            else if (_desiredJump && (_wallJumping || _wallSliding) && _jumpPhase < _maxAirJumps)
            {
                _desiredJump = false;
                JumpAction();
                NotWallJumping();
            }
            //State check if going up down or on ground//
            if (Input.GetButton("Jump") && _body.velocity.y > 0.1f && !_wallSliding)
            {
                _body.gravityScale = _upwardMovementMultiplier;
            }
            else if (Input.GetButton("Jump") && _body.velocity.y < 0.1f && !_wallSliding)
            {
                _body.gravityScale = _downwardMovementMultiplier + .5f;
            }
            else if (!Input.GetButton("Jump") && !_wallSliding)
            {
                _body.gravityScale = _downwardMovementMultiplier + .5f;

                if (_body.velocity.y < 0 && !_desiredJump)
                {
                    _desiredJump = false;
                    _velocity.y = Mathf.Max(_fallClamp, _body.velocity.y);
                }
            }
            else if (_body.velocity.y == 0)
            {
                _body.gravityScale = _defaultGravityScale;
            }

            // 2nd Jump check
            if (_charCollisions._touchingBottom == false && _jumpPhase == 2)
            {
                   _body.gravityScale = _doubleJumpMultiplier + .5f;
            }
        }
        private void WallSlidingCheck()
        {
            if (_charCollisions._rightWallhit || _charCollisions._leftWallHit)
            {
                _wallSliding = true;
                GetComponent<Dash>()._dashPhase = 0;
            }
            else
            {
                _wallSliding = false;
            }

            // If pushing into wall and going up no upwardMovement. //
            if (_wallSliding && _body.velocity.y > 0 && !_wallGrab)
            {
                _body.gravityScale = _upwardMovementMultiplier + .5f;
            }
            // If pushing into wall going down decreased slide. //
            else if (_wallSliding && (_controller.input.RetrieveMoveInput() != 0 || _controller.input.RetrieveMoveInput2() != 0) && _body.velocity.y < 0 && !_wallGrab)
            {
                _body.gravityScale = _downwardMovementMultiplier / 12;
            }
            // If pushing into wall, going down and no inputs. //
            else if (_wallSliding && _controller.input.RetrieveMoveInput() == 0 && _controller.input.RetrieveMoveInput2() == 0 && !_wallGrab && _body.velocity.y < 0)
            {
                _body.gravityScale = _downwardMovementMultiplier + .5f;
                _velocity.y = Mathf.Max(_fallClamp, _body.velocity.y);
            }
            WallGrabCheck();
        }
        private void WallGrabCheck()
        {
            Debug.Log(Input.GetButton("WallGrab"));
            if (_wallSliding)
            {
                if (Input.GetButton("WallGrab"))
                {
                    _wallGrab = true;
                    _jumpPhase = _maxAirJumps - 1;
                    
                }
                if (!Input.GetButton("WallGrab"))
                {
                    _wallGrab = false;
                }
            }
            if (!_wallSliding || Input.GetButton("Jump"))
            {
                _jumpPhase = 1;
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
                _body.gravityScale = _upwardMovementMultiplier + .5f;
            }
        }
        private void NotWallJumping()
        {
            _wallJumping = false;
        }
    }
}