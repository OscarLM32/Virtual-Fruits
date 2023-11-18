using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.StateMachine
{
    public class PlayerStateMachine : MonoBehaviour
    {
        private const float _gravity = -9.8f;
        private const int _enemyLayer = 3;
        private const int _projectileLayer = 9;

        public LayerMask playerLayer;
        public LayerMask groundLayer;
        public LayerMask platformLayer;

        public Transform groundChecker;
        public Transform wallChecker;
        //TODO: move this debugging logic to a separate file
        public TextMeshProUGUI debugText;

        public GameObject weaponGameObject;

        public Action<PlayerState> OnStateChange;

        private AudioManager _audioManager;

        private PlayerBaseState _currentState;
        private PlayerStateFactory _factory;

        private int _playerLayerID;
        private int _platformLayerID;

        private PlayerInput _playerInput;
        private Rigidbody2D _rb;
        private Animator _animator;
        private PlayerWeapon _weapon;

        //Horizontal movement variables
        private Vector2 _currentMovementInput;

        //TODO: change this variable's name. It's just horizontal movement that stores. Not so representative
        private Vector2 _currentMovement;

        private int _lastFacingDirection = 1;

        private float _speed = 6f;
        private bool _isWalking = false;
        private float _walkingSpeed;


        //Jumping variables
        private bool _isJumpPressed = false;
        private bool _jumped = false;
        private bool _requireNewJumpPress = false;
        private float _initialJumpVelocity;
        private float _maxJumpHeight = 2.7f;
        private float _maxJumpTime = 0.7f;

        private bool _isJumpDownPlatformPressed = false;
        private bool _isJumpingDownPlatform = false;

        //Double jumping variables: The same input will be used: _isJumpPressed;
        private bool _doubleJumped = false;
        private float _initialDoubleJumpVelocity;
        private float _maxDoubleJumpHeight = 1.7f;
        private float _maxDoubleJumpTime = 0.5f;

        //Wall jumping variables: Uses _isJumpPressed as input
        private const int _maxWallJumps = 3;
        private bool _wallJumped = false;
        private int _wallJumpsCount = 0;
        //Each field contains --> InitialJumpVelocity, GravityFactor and TimeToApex
        private Dictionary<int, WallJumpInformation> _wallJumpsData = new Dictionary<int, WallJumpInformation>();

        //Dashing variables
        private const float _dashCoolDown = 0.25f;
        private bool _isDashPressed = false;
        private bool _requireNewDashPress = false;
        private bool _dashed = false;
        private float _lastDashTime = 0;
        private float _dashDistance = 3.5f;
        private float _dashTime = 0.25f;
        private float _dashSpeed;

        //Gliding variables
        private float _glidingPressedTime = 0f;
        private float _glidingActivationTime = 0.15f; //The time it takes to have the button pressed before gliding

        //Gravity
        private float _desiredGravity; //TODO: I have to look into deleting or not this variable
        private float _jumpingGravityFactor;
        private float _doubleJumpingGravityFactor;
        private float _fallingGravityFactor;
        private float _glidingGravityFactor;
        private float _wallGrapplingGravityFactor;

        //Attack variables
        private const float MAX_ATTACK_DISTANCE = 8f;
        private bool _isAttackPressed = false;
        private bool _requireNewAttackPress = true;
        private bool _isWeaponReady = true;
        private float _attackAngle;
        private Vector2 _aimPosition;

        //Player hit
        private bool _playerHit = false;
        private bool _playerBounceBack = false;
        private bool _playerDead = false;

        //Checkers
        private bool _isGrounded;
        private bool _isGrapplingWall;



        //Getters and Setters
        public AudioManager PlayerAudioManager => _audioManager;
        public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
        public int PlayerLayerID => _playerLayerID;
        public int PlatformLayerID => _platformLayerID;
        public int EnemyLayerID => _enemyLayer;
        public int ProjectileLayerID => _projectileLayer;
        public Rigidbody2D Rb2D => _rb;
        public Animator PlayerAnimator => _animator;
        public PlayerWeapon Weapon => _weapon;


        //HORIZONTAL MOVEMENT GETTERS AND SETTER
        public Vector2 CurrentMovementInput => _currentMovementInput;
        public Vector2 CurrentMovement => _currentMovement;
        public float Speed => _speed;
        public bool IsWalking => _isWalking;
        public float WalkingSpeed => _walkingSpeed;
        public int LastFacingDirection => _lastFacingDirection;
        /////////////////////////////////////////////////////////////////////////////////

        //JUMPING GETTERS AND SETTERS 
        public bool IsJumpPressed => _isJumpPressed;
        public bool Jumped { get => _jumped; set => _jumped = value; }
        public bool RequireNewJumpPress { get => _requireNewJumpPress; set => _requireNewJumpPress = value; }
        public bool IsJumpDownPlatformPressed => _isJumpDownPlatformPressed;
        public bool IsJumpingDownPlatform { get => _isJumpingDownPlatform; set => _isJumpingDownPlatform = value; }
        public bool DoubleJumped { get => _doubleJumped; set => _doubleJumped = value; }
        public float InitialJumpVelocity => _initialJumpVelocity;
        public float InitialDoubleJumpVelocity => _initialDoubleJumpVelocity;
        public bool WallJumped { get => _wallJumped; set => _wallJumped = value; }
        public int MaxWallJumps => _maxWallJumps;
        public int WallJumpsCount { get => _wallJumpsCount; set => _wallJumpsCount = value; }
        public Dictionary<int, WallJumpInformation> WallJumpsData => _wallJumpsData;
        ////////////////////////////////////////////////////////////////////////////////////

        //DASHING GETTERS AND SETTERS
        public bool IsDashPressed => _isDashPressed;
        public bool RequireNewDashPress { get => _requireNewDashPress; set => _requireNewDashPress = value; }
        public bool Dashed { get => _dashed; set => _dashed = value; }
        public float LastDashTime { get => _lastDashTime; set => _lastDashTime = value; }
        public float DashSpeed => _dashSpeed;
        public float DashTime => _dashTime;
        ////////////////////////////////////////////////////////////////////////////////////

        //GLIDING GETTER AND SETTERS
        public float GlidingPressedTime { get => _glidingPressedTime; set => _glidingPressedTime = value; }
        public float GlidingActivationTime => _glidingActivationTime;
        //////////////////////////////////////////////////////////////////////////////////// 

        //GRAVITY GETTER AND SETTERS
        public float JumpingGravityFactor => _jumpingGravityFactor;
        public float DoubleJumpingGravityFactor => _doubleJumpingGravityFactor;
        public float FallingGravityFactor => _fallingGravityFactor;
        public float GlidingGravityFactor => _glidingGravityFactor;
        public float WallGrapplingGravityFactor => _wallGrapplingGravityFactor;
        ///////////////////////////////////////////////////////////////////////////////////// 

        //ATTACK GETTER AND SETTERS
        public bool IsAttackPressed => _isAttackPressed;
        public bool RequireNewAttackPress { get => _requireNewAttackPress; set => _requireNewAttackPress = value; }
        public bool IsWeaponReady { get => _isWeaponReady; set => _isWeaponReady = value; }
        public float AttackAngle => _attackAngle;

        //PLAYERHIT GETTER AND SETTERS
        public float MaxAttackDistance => MAX_ATTACK_DISTANCE;
        public bool PlayerHit { get => _playerHit; set => _playerHit = value; }
        public bool PlayerBounceBack { get => _playerBounceBack; set => _playerBounceBack = value; }
        public bool PlayerDead { get => _playerDead; set => _playerDead = value; }
        //////////////////////////////////////////////////////////////////////////////////////

        //CHECKERS GETTER AND SETTERS
        public bool IsGrounded => _isGrounded;
        public bool IsGrapplingWall { get => _isGrapplingWall; set => _isGrapplingWall = value; }
        //////////////////////////////////////////////////////////////////////////////////////




        private void Awake()
        {
            _audioManager = GetComponent<AudioManager>();

            _playerInput = new PlayerInput();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _lastFacingDirection = 1; //Let's make it always look to the right when spawning
            _weapon = weaponGameObject.GetComponent<PlayerWeapon>();

            GetLayerIDs();

            //Setting up default state
            _factory = new PlayerStateFactory(this);
            _currentState = _factory.GetState(PlayerState.GROUNDED);
            _currentState.EnterState();

            SetUpInputs();

            _walkingSpeed = _speed * 0.7f;
            _dashSpeed = _dashDistance / _dashTime;

            SetUpJumpAndGravityVariables();
        }

        private void OnEnable()
        {
            _playerInput.CharacterControls.Enable();
            GameActions.RetrieveWeapon += OnRetrieveWeapon;
            GameActions.GamePause += OnGamePause;
        }

        private void OnDisable()
        {
            _playerInput.CharacterControls.Disable();
            GameActions.RetrieveWeapon -= OnRetrieveWeapon;
            GameActions.GamePause -= OnGamePause;
        }

        private void HandleSpriteDirection()
        {
            int direction;
            if (_currentMovement.x == 0)
                direction = _lastFacingDirection;
            else
                direction = _currentMovement.x > 0 ? 1 : -1;

            _lastFacingDirection = direction;
            transform.localScale = new Vector3(direction, 1, 1);
        }

        private void HandleGrounded()
        {
            _isGrounded = false;
            Vector2 position = groundChecker.position;
            if (Physics2D.OverlapBox(position, new Vector2(0.45f, 0.1f), 0, groundLayer) ||
                Physics2D.OverlapBox(position, new Vector2(0.45f, 0.1f), 0, platformLayer) &&
                 !_isJumpingDownPlatform)
            {
                _isGrounded = true;
            }
        }

        //Can I handle this logic with collision? It may work and be easier
        private void HandleGrapplingWall()
        {
            Vector2 position = wallChecker.position;
            bool nextToWall = Physics2D.OverlapBox(position, new Vector2(0.1f, 0.4f), 0, groundLayer);

            if (nextToWall && !_isGrounded && _currentMovementInput != Vector2.zero && !_wallJumped)
                _isGrapplingWall = true;
            else
                _isGrapplingWall = false;
        }

        private void Update()
        {
            HandleGrounded();
            HandleGrapplingWall();
            _currentState.UpdateStates();
        }

        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            _currentMovement = new Vector2(_currentMovementInput.x, 0);

            if (_currentMovement.x > 0)
            {
                if (_currentMovement.x < 0.250)
                    _currentMovement.x = 0;
                else if (_currentMovement.x > 0.750)
                    _currentMovement.x = 1;
            }
            else
            {
                if (_currentMovement.x > -0.250)
                    _currentMovement.x = 0;
                else if (_currentMovement.x < -0.750)
                    _currentMovement.x = -1;
            }
            HandleSpriteDirection();
        }


        private void OnJump(InputAction.CallbackContext context)
        {
            _isJumpPressed = context.ReadValueAsButton();
            if (_isJumpPressed)
            {
                _requireNewJumpPress = false;
            }
            _glidingPressedTime = 0f;
        }

        private void OnWalk(InputAction.CallbackContext context)
        {
            _isWalking = !_isWalking;
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            if (Time.time - _lastDashTime >= _dashCoolDown)
            {
                _isDashPressed = context.ReadValueAsButton();
                _requireNewDashPress = false;
            }
        }

        private void OnJumpDownPlatform(InputAction.CallbackContext context)
        {
            _isJumpDownPlatformPressed = context.ReadValueAsButton();
            if (_isJumpDownPlatformPressed)
                _isJumpPressed = false;
        }

        private void OnMouseAim(InputAction.CallbackContext context)
        {
            var pos = context.ReadValue<Vector2>();
            pos = Camera.main.ScreenToWorldPoint(pos);
            pos -= (Vector2)gameObject.transform.position;

            _attackAngle = (float)Math.Atan2(pos.y, pos.x);
        }

        private void OnControllerAim(InputAction.CallbackContext context)
        {
            var pos = context.ReadValue<Vector2>();
            _attackAngle = (float)Math.Atan2(pos.y, pos.x);
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            _isAttackPressed = context.ReadValueAsButton();
            _requireNewAttackPress = false;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == _enemyLayer || col.gameObject.layer == _projectileLayer)
            {
                if (col.gameObject.CompareTag("Snail"))
                {
                    _playerBounceBack = true;
                    _playerHit = true;
                    return;
                }
                _playerDead = true;
                _playerHit = true;
            }
        }

        private void OnRetrieveWeapon()
        {
            _isWeaponReady = true;
            _doubleJumped = false;
            _dashed = false;
            _dashed = false;
            weaponGameObject.SetActive(false);
        }

        private void OnGamePause(bool paused)
        {
            if (paused)
                _playerInput.CharacterControls.Disable();
            else
                _playerInput.CharacterControls.Enable();
        }

        /// <summary>
        /// This initializes the layer_id variables so that they can be used in the
        /// JumpDownPlatform subState
        /// </summary>
        private void GetLayerIDs()
        {
            int layerID = 0;
            int layer = playerLayer.value;
            while (layer > 1)
            {
                layer = layer >> 1;
                layerID++;
            }
            _playerLayerID = layerID;

            layerID = 0;
            layer = platformLayer.value;
            while (layer > 1)
            {
                layer = layer >> 1;
                layerID++;
            }
            _platformLayerID = layerID;
        }

        private void SetUpInputs()
        {
            _playerInput.CharacterControls.Movement.performed += OnMovementInput;
            /*_playerInput.CharacterControls.Movement.started += context => OnMovementInput(context);*/
            _playerInput.CharacterControls.Movement.canceled += OnMovementInput;

            _playerInput.CharacterControls.Jump.started += OnJump;
            _playerInput.CharacterControls.Jump.canceled += OnJump;

            _playerInput.CharacterControls.Walk.started += OnWalk;

            _playerInput.CharacterControls.Dash.started += OnDash;
            _playerInput.CharacterControls.Dash.canceled += OnDash;

            _playerInput.CharacterControls.JumpDownPlatform.started += OnJumpDownPlatform;
            _playerInput.CharacterControls.JumpDownPlatform.canceled += OnJumpDownPlatform;

            _playerInput.CharacterControls.MouseAim.performed += OnMouseAim;
            _playerInput.CharacterControls.ControllerAim.performed += OnControllerAim;

            _playerInput.CharacterControls.Attack.started += OnAttack;
            _playerInput.CharacterControls.Attack.canceled += OnAttack;

        }

        //The jumps' velocity is also calculated inside
        private void SetUpJumpAndGravityVariables()
        {
            float timeToApex = _maxJumpTime / 2; //The time it takes to reach the highest point
            _desiredGravity = -2 * _maxJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex;
            _jumpingGravityFactor = _desiredGravity / _gravity;

            //The falling gravity is higher than the jumping gravity;
            _fallingGravityFactor = _jumpingGravityFactor * 1.6f;

            //Initialization of double jump variables
            timeToApex = _maxDoubleJumpTime / 2;
            _desiredGravity = -2 * _maxDoubleJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialDoubleJumpVelocity = 2 * _maxDoubleJumpHeight / timeToApex;
            _doubleJumpingGravityFactor = _desiredGravity / _gravity;

            //Wall grappling jumps
            SetUpWallJumpVariables();

            //Gliding gravity
            _glidingGravityFactor = _fallingGravityFactor * 0.2f;

            //Wall grappling
            _wallGrapplingGravityFactor = _fallingGravityFactor * 0.15f;
        }

        //All the information is relative to the normal jumpç
        //TODO: redo the calculus so I get a decent Jump
        private void SetUpWallJumpVariables()
        {
            var ratio = 1f / (_maxWallJumps + 1);
            for (int i = 0; i < _maxWallJumps; i++)
            {
                WallJumpInformation info = new WallJumpInformation();
                info.TimeToApex = _maxJumpTime / 2 * (1 - ratio * i);
                var desiredGravity = -2 * _maxJumpHeight * (1 - ratio * i) / (float)Math.Pow(info.TimeToApex, 2);
                info.InitialJumpVelocity = 2 * _maxJumpHeight * (1 - ratio * i) / info.TimeToApex;
                info.GravityFactor = desiredGravity / _gravity;
                _wallJumpsData.Add(i, info);
            }
        }



        public class WallJumpInformation
        {
            public float InitialJumpVelocity;
            public float GravityFactor;
            public float TimeToApex;
        }
    }
}