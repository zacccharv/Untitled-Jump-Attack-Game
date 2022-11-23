using UnityEngine;
using UnityEngine.TextCore.Text;

namespace ZaccCharv
{
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
        public CharCollisions _charCollisions;
        public Jump _jump;
        public CharCollisions _charCollions;

        private float _maxSpeedChange, _acceleration, _previousVelocity;
        private bool _onGround, _onPlatform, _landed;

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _ground = GetComponent<Ground>();
            _charCollisions = GetComponent<CharCollisions>();
            _jump = GetComponent<Jump>();
            _controller = GetComponent<Controller>();
        }

        void Update()
        {
            if (GetComponent<Dash>()._isDashing)
            {
                return;
            }

            _direction.x = _controller.input.RetrieveMoveInput();

            if (_controller.input.RetrieveJumpInput())
            {
                _onPlatform = false;
            }
        }

        void FixedUpdate()
        {
            if (GetComponent<Dash>()._isDashing)
            {
                return;
            }

            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed - _ground.Friction, 0f);
            _onGround = _ground.OnGround;
            _velocity = _body.velocity;
            _previousVelocity = _body.velocity.x;

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

                if (_charCollisions._touchingBottom && !_controller.input.RetrieveJumpInput() && _direction.x == 0)
                {
                    _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(_maxSpeed/2 - _ground.Friction, 0f);
                    _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
                    if (_jump._wallGrab)
                    {
                        _desiredVelocity = Vector2.zero;
                    }
                    if (_landed)
                    {
                        _landed = false;
                        _velocity.x = _previousVelocity;
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
                        _maxAirAcceleration = _maxAcceleration/1.25f;
                    }
                }

            }

            _body.velocity = _velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.tag == "Platform" && collision != null)
            {
                _platform = collision.gameObject.GetComponent<PlatformVelocity>();
                _onPlatform = true;
            }
            if (_charCollisions._touchingBottom && !_jump._wallSliding)
            {
                _landed = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Platform")
            {
                _onPlatform = false;
            }
        }
    }
}