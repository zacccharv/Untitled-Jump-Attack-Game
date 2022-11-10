using UnityEngine;
using UnityEngine.TextCore.Text;
using ZaccCharv;

[RequireComponent(typeof(Controller))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    [SerializeField] private Controller _controller = null;
    public Vector2 _direction;
    public Vector2 _velocity, _desiredVelocity;
    public Rigidbody2D _body;
    private Ground _ground;
    public PlatformVelocity _platform;
    public Jump _jump;

    private float _maxSpeedChange, _acceleration;
    private bool _onGround;
    public bool _onPlatform;

    void Start()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _jump = GetComponent<Jump>();
        _controller = GetComponent<Controller>();
    }

    void Update()
    {
        _direction.x = _controller.input.RetrieveMoveInput();

        if (_controller.input.RetrieveJumpInput())
        {
            _onPlatform = false;
        }
    }

    void FixedUpdate()
    {
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _ground.Friction, 0f);
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;

        if (_onPlatform)
        {
            _velocity = _platform._velocity;

            if (_direction.x != 0)
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange) + (_desiredVelocity.x);
            }
        }
        if (!_onPlatform)
        {
            _velocity = _body.velocity;

            if (_jump._onGround && !_controller.input.RetrieveJumpInput() && _direction.x == 0)
            {
                _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed/2 - _ground.Friction, 0f);
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
                if (_jump._wallGrab)
                {
                    _desiredVelocity = Vector2.zero;
                }
            } else
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
                if (_jump._wallGrab && !_jump._wallJumping)                
                {
                    _velocity = Vector2.zero;
                    _desiredVelocity = Vector2.zero;
                }
                else if (_jump._wallGrab && _jump._wallJumping)
                {
                    _jump.WallJumpAction();
                }
            }

        }

        _body.velocity = _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Platform" && collision != null)
        {
            Debug.Log("I have Entered");
            _platform = collision.gameObject.GetComponent<PlatformVelocity>();
            _onPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("I have left");
            _onPlatform = false;
        }
    }
}