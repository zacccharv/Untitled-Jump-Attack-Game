using UnityEngine;
using ZaccCharv;

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
        private CharCollisions _charCollisions;
        public Transform _rayCenter;
        private Animator _animator;
        private Rigidbody2D _body;
        public Vector2 _velocity;

        private float _defaultGravityScale, _jumpSpeed;
        private bool _touchingBottom;
        public bool _desiredJump, _earlyJump;
        public int _jumpPhase;

    #endregion

    #region Wall Jumping and Sliding Vars
        private bool _wallSliding;
        [HideInInspector] public bool _wallJumping;
        [HideInInspector] public bool _wallGrab;

    #endregion

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<Controller>();
        _animator = GetComponent<Animator>();
        _charCollisions = AddComponent<CharCollisions>();

        _defaultGravityScale = 1f;
    }

    void Update()
    {
        _desiredJump |= _controller.input.RetrieveJumpInput();
        _charCollisions.WallFloorHitCheck(); 
        _rayCenter = _charCollisions._rayCenter;
    }

    private void FixedUpdate()
    {
        _velocity = _body.velocity;

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
        if ((_touchingBottom || _jumpPhase < _maxAirJumps) && !_wallJumping)
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
        else if (!_touchingBottom && _wallJumping)
        {
            _jumpPhase += 1;

            float _wallHitDirection = 0;

            if (_charCollisions.rightWallhit)
            {
                _wallHitDirection = -1;
                if (_wallGrab)
                {
                    _wallHitDirection = 1 * -GetComponent<CharacterAnimator>().FlipDirection;
                }
            }
            else if (_charCollisions._leftWallHit)
            {
                _wallHitDirection = 1;
                if (_wallGrab)
                {
                    _wallHitDirection = -1 * -GetComponent<CharacterAnimator>().FlipDirection;
                }
            }

            _animator.SetTrigger("Jumped");
            _animator.SetBool("Falling", false);

            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight);

            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y/2, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_body.velocity.y/4);
            }

            if ((_charCollisions._rightWallhit && _wallHitDirection == 1) || (_charCollisions._leftWallHit && _wallHitDirection == -1))
            {
                _velocity.x += 10 * _wallHitDirection;
            }
            else if ((_charCollisions._rightWallhit && _wallHitDirection == -1) || (_charCollisions._leftWallHit && _wallHitDirection == 1))
            {
                _velocity.x += 2f * _wallHitDirection;
            }
            _wallJumping = true;
            _velocity.y += _jumpSpeed;
        }
    }
    private void JumpActionCheck()
    {
        if (_touchingBottom)
        {
            _jumpPhase = 0;
        }
        if (_earlyJump && _body.velocity.y < 0)
        {   _earlyJump = false;
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
        if (Input.GetButton("Jump") && _body.velocity.y > 0 && !_wallSliding)
        {
            _body.gravityScale = _upwardMovementMultiplier;
        }
        else if (!Input.GetButton("Jump") && !_wallSliding)
        {
            _body.gravityScale = _downwardMovementMultiplier + .5f;

            if (_body.velocity.y < 0 && !Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Max(_fallClamp, _body.velocity.y);  
            }
            Debug.Log("falling");
        }
        else if (_body.velocity.y == 0)
        {
            Debug.Log("standing");
            _body.gravityScale = _defaultGravityScale;
        }

        // 2nd Jump check
        if (_touchingBottom == false && _jumpPhase == 2)
        {
            _body.gravityScale = _doubleJumpMultiplier + .5f;
        }
    }

    private void WallSlidingCheck()
    {
        if (_charCollisions._charCollisions._rightWallhit || _charCollisions._charCollisions._leftWallHit)
        {
            _wallSliding = true;
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
        if (_wallSliding)
        {
            if (Input.GetButton("WallGrab"))
            {
                _wallGrab = true;
                _jumpPhase = 1;
            }
            if (!Input.GetButton("WallGrab"))
            {
                _wallGrab = false;
            }
        }
        if (!_wallSliding || Input.GetButton("Jump"))
        {
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
        Debug.Log("not wall jumping");
        _wallJumping = false;
    }
}