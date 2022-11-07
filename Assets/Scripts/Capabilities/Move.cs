using UnityEngine;
using ZaccCharv;

[RequireComponent(typeof(Controller))]
public class Move : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    [SerializeField] private Controller _controller = null;
    private Vector2 _direction;
    public Vector2 _velocity, _desiredVelocity;
    public Rigidbody2D _body;
    private Ground _ground;
    public PlatformVelocity platform;

    private float _maxSpeedChange, _acceleration;
    private bool _onGround;
    public bool _onPlatform;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _ground = GetComponent<Ground>();
        _controller = GetComponent<Controller>();
    }

    void Update()
    {
        _direction.x = _controller.input.RetrieveMoveInput();
        _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _ground.Friction, 0f);

        if (_controller.input.RetrieveJumpInput())
        {
            _onPlatform = false;
        }
    }

    void FixedUpdate()
    {
        _onGround = _ground.OnGround;
        _velocity = _body.velocity;

        _acceleration = _onGround ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;

        if (_onPlatform)
        {
            _velocity = platform._velocity;
            if (_direction.x != 0)
            {
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange) + (_desiredVelocity.x / 2);
            }

        }
        if (!_onPlatform)
        {
            _velocity = _body.velocity;

            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
        }


        _body.velocity = _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Debug.Log("I have Entered");
            platform = collision.gameObject.GetComponent<PlatformVelocity>();

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