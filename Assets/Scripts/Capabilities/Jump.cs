using UnityEngine;
using ZaccCharv;

[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
    [SerializeField, Range(0, 5)] public int _maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] public float _downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;

    [SerializeField] private Controller _controller = null;
    [SerializeField] private Move _move;

    private Animator _animator;
    private Rigidbody2D _body;
    private Ground _ground;
    private Vector2 _velocity;

    public int _jumpPhase;
    private float _defaultGravityScale, _jumpSpeed;
    private Vector2 _normal;

    public bool _desiredJump, _onGround, leftWallHit, rightWallhit;

    // Start is called before the first frame update
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<Controller>();
        _animator = GetComponent<Animator>();
        _move = GetComponent<Move>();

        _defaultGravityScale = 1f;
    }


    public float timeRemaining;
    private bool timerIsRunning = false;
    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= .1f;
            }
            else
            {
                timerIsRunning = false;
                Debug.Log("Time has run out!");
            }
        }
        
        _desiredJump |= _controller.input.RetrieveJumpInput();
    }

    private void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        if ((rightWallhit || leftWallHit) && _body.velocity.y < 0 && !Input.GetButton("Jump"))
        {
            if (rightWallhit)
            {
                Vector2 _wallJumpVel = new Vector2(-100, _velocity.y);
                _body.velocity = _wallJumpVel;
            }
            else
            {
                _body.gravityScale = _downwardMovementMultiplier / 10;
                int _newJumpPhase = Mathf.Max(_jumpPhase, 0);
                _jumpPhase -= _newJumpPhase;
            }
        }

        if (_onGround)
        {
            _jumpPhase = 0;
        }
        if (_desiredJump)
        {
            _desiredJump = false;
            JumpAction();
        }
        if (_body.velocity.y > 0)
        {
            _body.gravityScale = _upwardMovementMultiplier;
        }
        if (((_body.velocity.y < 0 || !Input.GetButton("Jump")) && !((rightWallhit || leftWallHit))) && timerIsRunning == false)
        {
            _body.gravityScale = _downwardMovementMultiplier + .5f;
        }
        else if (_body.velocity.y == 0)
        {
            _body.gravityScale = _defaultGravityScale;
        }

        /////////////////////////////////////////////////////////////////// HEREHEREHERE
        if (timerIsRunning == true)
        {
            int _direction = 1;

            if (leftWallHit)
            {
                _direction = 1;
            }
            if (rightWallhit)
            {
                _direction = -1;
            }
                Vector2 _wallJumpVel = new Vector2(25 * _direction, _velocity.y + 10);
                _body.velocity = _wallJumpVel;
                _body.gravityScale = _downwardMovementMultiplier + .5f;
            if ((rightWallhit || leftWallHit))
            {
                timerIsRunning = false;
            }
        }
        else
        {
            _body.velocity = _velocity;
        }
        /////////////////////////////////////////////////////////////////////

        SidesRaycast();
    }

    private void JumpAction()
    {
        if (_onGround || _jumpPhase < _maxAirJumps && !(rightWallhit || leftWallHit))
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
        if (rightWallhit || leftWallHit && _body.velocity.y < 0)
        {
            _jumpPhase += 2;

            _animator.SetTrigger("Jumped");
            _animator.SetBool("Falling", false);
            _body.gravityScale = _downwardMovementMultiplier + .5f;

            if (rightWallhit)
            {
                timeRemaining = .4f;
                timerIsRunning = true;
                Debug.Log("I jumped off a wall");
            }
            if (leftWallHit)
            {
                timeRemaining = .4f;
                timerIsRunning = true;
                Debug.Log("I jumped off a wall");
            }
        }
    }

    private void SidesRaycast()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 7;

        // This would cast rays only against colliders in layer 7.

        var RightCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), .5f, layerMask);
        var LeftCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), .5f, layerMask);
        var UpCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), .5f, layerMask);
        var DownCast = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.down), .5f, layerMask);

        if (RightCast && (!UpCast && !DownCast))
        {
            Debug.DrawLine(transform.position, transform.TransformDirection(Vector2.right), Color.yellow);

            rightWallhit = true;
        }
        else if (LeftCast && (!UpCast && !DownCast))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.left), Color.yellow);

            leftWallHit = true;
        }
        else
        {
            leftWallHit = false; 
            rightWallhit = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.right) * 1000, Color.white);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.left) * 1000, Color.white);
        }
    }
}

