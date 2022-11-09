using UnityEngine;
using ZaccCharv;

[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    #region Jump Class Only Vars

    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] public int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] public float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;

    private Animator _animator;
    private Rigidbody2D _body;
    private Ground _ground;
    private Vector2 _velocity;

    [SerializeField] private Controller _controller = null;

    private float _defaultGravityScale, _jumpSpeed;

    #endregion


    public bool _desiredJump, _onGround;

    public int _jumpPhase;

    #region Wall Jumping and Sliding Vars

    public bool _leftWallHit, _rightWallhit;
    private bool _wallSliding;
    private bool _wallJumping;
    public float _wallJumpTime;

    #endregion

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<Controller>();
        _animator = GetComponent<Animator>();

        _defaultGravityScale = 1f;
    }

    void Update()
    {
        _desiredJump |= _controller.input.RetrieveJumpInput();
    }

    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        JumpActionCheck();

        WallHitCheck();

        WallSlidingCheck();

        JumpVelocityCheck();

        if (_wallJumping == true)
        {
            WallJumpAction();
            Invoke("NotWallJumping", _wallJumpTime);
        }
        else
        {
            // This has to go last
            _body.velocity = _velocity;
        }

    }
    private void JumpVelocityCheck()
    {
        if (_body.velocity.y > 0)
        {
            _body.gravityScale = _upwardMovementMultiplier;
        }
        if (((_body.velocity.y < 0 || !_controller.input.RetrieveJumpInput()) && !(_wallSliding)) || _wallJumping == false)
        {
            _body.gravityScale = _downwardMovementMultiplier + .5f;
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }
    }

    private void JumpAction()
    {
        if (_onGround || _jumpPhase < _maxAirJumps && !(_rightWallhit || _leftWallHit))
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
        if (_wallSliding && _body.velocity.y < 0)
        {
            _jumpPhase += 2;

            _animator.SetTrigger("Jumped");
            _animator.SetBool("Falling", false);
            _body.gravityScale = _downwardMovementMultiplier + .5f;

            if (_wallSliding)
            {
                _wallJumping = true;
                Debug.Log("I jumped off a wall");
            }
        }
    }

    private void JumpActionCheck()
    {
        if (_onGround)
        {
            _jumpPhase = 0;
        }
        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }
    }

    private void WallHitCheck()
    {
        // Bit shift the index of the layer (7) to get a bit mask
        int layerMask = 1 << 7;

        // This would cast rays only against colliders in layer 7.

        var RightCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), .5f, layerMask);
        var LeftCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), .5f, layerMask);
        var UpCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), .5f, layerMask);
        var DownCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), .5f, layerMask);

        if (RightCast && (!UpCast && !DownCast))
        {
            Debug.DrawLine(transform.position, transform.TransformDirection(Vector2.right), Color.yellow);

            _rightWallhit = true;
        }
        else if (LeftCast && (!UpCast && !DownCast))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.left), Color.yellow);

            _leftWallHit = true;
        }
        else
        {
            _leftWallHit = false;
            _rightWallhit = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 1000, Color.white);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.left) * 1000, Color.white);
        }
    }

    private void WallSlidingCheck()
    {
        if (_rightWallhit || _leftWallHit)
        {
            _wallSliding = true;
        }
        else
        {
            _wallSliding = false;
        }
        if (_wallSliding && !_controller.input.RetrieveJumpInput())
        {
            _body.gravityScale = _downwardMovementMultiplier / 10;
            int _newJumpPhase = Mathf.Max(_jumpPhase, 0);
            _jumpPhase -= _newJumpPhase;
        }
    }

    private void WallJumpAction()
    {
        float _direction = _controller.input.RetrieveMoveInput() * -1;
        _body.gravityScale = _downwardMovementMultiplier + .5f;

        if (_rightWallhit || _leftWallHit)
        {
            _wallJumping = false;
        }

        Vector2 _wallJumpVelocity = new Vector2(15 * _direction, 30);
        _body.velocity = _wallJumpVelocity;
    }

    private void NotWallJumping()
    {
        _wallJumping = false;
    }

}