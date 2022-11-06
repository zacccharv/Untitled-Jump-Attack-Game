using UnityEngine;

    [RequireComponent(typeof(Controller))]
    public class Jump : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
        [SerializeField, Range(0, 5)] public int _maxAirJumps = 0;
        [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;
        [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;

        [SerializeField] private Controller _controller = null;

        private Animator _animator;
        private Rigidbody2D _body;
        private Ground _ground;
        private Vector2 _velocity;

        public int _jumpPhase;
        private float _defaultGravityScale, _jumpSpeed;

        public bool _desiredJump, _onGround;


        // Start is called before the first frame update
        void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _ground = GetComponent<Ground>();
            _controller = GetComponent<Controller>();
            _animator = GetComponent<Animator>();

            _defaultGravityScale = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            _desiredJump |= _controller.input.RetrieveJumpInput();
        }

        private void FixedUpdate()
        {
            _onGround = _ground.OnGround;
            _velocity = _body.velocity;

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
            if (_body.velocity.y < 0 || !Input.GetButton("Jump"))
            {
                _body.gravityScale = _downwardMovementMultiplier + .5f;
            }
            else if(_body.velocity.y == 0)
            {
                _body.gravityScale = _defaultGravityScale;
            }

            _body.velocity = _velocity;
        }
        private void JumpAction()
        {
            if (_onGround || _jumpPhase < _maxAirJumps)
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
        }
    }

