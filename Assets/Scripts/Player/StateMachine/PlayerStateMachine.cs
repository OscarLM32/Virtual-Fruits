using System;
using UnityEngine;
using UnityEngine.InputSystem;
using EditorSystems.Logger;
using DynamicDifficulty;

namespace Player.StateMachine
{
    [RequireComponent(typeof(AudioManager), typeof(Animator), typeof(Rigidbody2D))]
    public class PlayerStateMachine : MonoBehaviour
    {
        private const int _enemyLayer = 3;
        private const int _projectileLayer = 9;

        public LayerMask playerLayer;
        public LayerMask groundLayer;
        public LayerMask platformLayer;

        public Transform groundChecker;
        public Transform wallChecker;

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
        private Vector2 _currentHorizontalMovement;
        private int _lastFacingDirection = 1;
        private bool _isWalking = false;

        //Jumping variables
        private bool _isJumpPressed = false;
        private bool _jumped = false;
        private bool _requireNewJumpPress = false;

        //Double jumping variables: The same input will be used: _isJumpPressed;
        private bool _doubleJumped = false;

        private bool _isJumpDownPlatformPressed = false;
        private bool _isJumpingDownPlatform = false;

        //Wall jumping variables: Uses _isJumpPressed as input
        private bool _wallJumping = false;
        private int _maxWallJumps = 3;
        private int _wallJumpsCount = 0;
        private float _currentWallJumpTimeToApex;

        //Dashing variables
        private const float _dashCoolDown = 0.25f;
        private float _lastDashTime = 0;
        private bool _isDashPressed = false;
        private bool _requireNewDashPress = false;
        private bool _dashed = false;

        //Gravity
        private float _jumpingGravityFactor;
        private float _fallingGravityFactor;
        private float _glidingGravityFactor;
        private float _wallGrapplingGravityFactor;

        //Attack variables
        private const float _maxAttackDistance = 8f;
        private bool _isAttackPressed = false;
        private bool _requireNewAttackPress = true;
        private bool _isWeaponReady = true;
        private float _attackAngle;
        private bool _attackAvailable = false;

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
        public Vector2 CurrentMovement => _currentHorizontalMovement;
        public bool IsWalking => _isWalking;
        public int LastFacingDirection => _lastFacingDirection;
        /////////////////////////////////////////////////////////////////////////////////

        //JUMPING GETTERS AND SETTERS 
        public bool IsJumpPressed => _isJumpPressed;
        public bool Jumped { get => _jumped; set => _jumped = value; }
        public bool RequireNewJumpPress { get => _requireNewJumpPress; set => _requireNewJumpPress = value; }
        public bool IsJumpDownPlatformPressed => _isJumpDownPlatformPressed;
        public bool IsJumpingDownPlatform { get => _isJumpingDownPlatform; set => _isJumpingDownPlatform = value; }
        public bool DoubleJumped { get => _doubleJumped; set => _doubleJumped = value; }
        public bool WallJumped { get => _wallJumping; set => _wallJumping = value; }
        public int MaxWallJumps {get => _maxWallJumps; set => _maxWallJumps = value; }
        public int WallJumpsCount { get => _wallJumpsCount; set => _wallJumpsCount = value; }
        public float CurrentWallJumpTimeToApex { get => _currentWallJumpTimeToApex; set => _currentWallJumpTimeToApex = value; }
        ////////////////////////////////////////////////////////////////////////////////////

        //DASHING GETTERS AND SETTERS
        public bool IsDashPressed => _isDashPressed;
        public bool RequireNewDashPress { get => _requireNewDashPress; set => _requireNewDashPress = value; }
        public bool Dashed { get => _dashed; set => _dashed = value; }
        public float LastDashTime { get => _lastDashTime; set => _lastDashTime = value; }
        ////////////////////////////////////////////////////////////////////////////////////

        //GRAVITY GETTER AND SETTERS
        public float JumpingGravityFactor { get => _jumpingGravityFactor; 
            set
            {
                _jumpingGravityFactor = value;
                SetUpGravityVariables();
            }
        }
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
        public float MaxAttackDistance => _maxAttackDistance;
        public bool PlayerHit { get => _playerHit; set => _playerHit = value; }
        public bool PlayerBounceBack { get => _playerBounceBack; set => _playerBounceBack = value; }
        public bool PlayerDead { get => _playerDead; set => _playerDead = value; }
        //////////////////////////////////////////////////////////////////////////////////////

        //CHECKERS GETTER AND SETTERS
        public bool IsGrounded => _isGrounded;
        public bool IsGrapplingWall { get => _isGrapplingWall; set => _isGrapplingWall = value; }
        //////////////////////////////////////////////////////////////////////////////////////

        #region UNITY FUNCTIONS

        private void Awake()
        {
            _audioManager = GetComponent<AudioManager>();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerInput = new PlayerInput();
            if (weaponGameObject != null)
            {
                _weapon = weaponGameObject.GetComponent<PlayerWeapon>();
                _attackAvailable = true;
            }

            GetLayerIDs();
            SetUpInputs();
        }

        private void Start()
        {
            var difficulty = DynamicDifficultyManager.I.genericDifficulty;
            EditorLogger.Log(LoggingSystem.PLAYER, $"The difficulty has been set to {difficulty}");
            _factory = new PlayerStateFactory(this, difficulty);

            //Setting up default state
            _currentState = _factory.GetState(PlayerState.GROUNDED);
            _currentState.EnterState();
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
        private void Update()
        {
            HandleGrounded();
            HandleGrapplingWall();
            _currentState.UpdateStates();
        }

        #endregion

        private void HandleSpriteDirection()
        {
            int direction;
            if (_currentHorizontalMovement.x == 0)
                direction = _lastFacingDirection;
            else
                direction = _currentHorizontalMovement.x > 0 ? 1 : -1;

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

            if (nextToWall && !_isGrounded && _currentMovementInput != Vector2.zero && !_wallJumping)
                _isGrapplingWall = true;
            else
                _isGrapplingWall = false;
        }


        private void OnMovementInput(InputAction.CallbackContext context)
        {
            _currentMovementInput = context.ReadValue<Vector2>();
            _currentHorizontalMovement = new Vector2(_currentMovementInput.x, 0);

            if (_currentHorizontalMovement.x > 0)
            {
                if (_currentHorizontalMovement.x < 0.250)
                    _currentHorizontalMovement.x = 0;
                else if (_currentHorizontalMovement.x > 0.750)
                    _currentHorizontalMovement.x = 1;
            }
            else
            {
                if (_currentHorizontalMovement.x > -0.250)
                    _currentHorizontalMovement.x = 0;
                else if (_currentHorizontalMovement.x < -0.750)
                    _currentHorizontalMovement.x = -1;
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
            if (!_attackAvailable) return;

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

        private void SetUpGravityVariables()
        {
            //The falling gravity is higher than the jumping gravity;
            _fallingGravityFactor = _jumpingGravityFactor * 1.6f;

            //Gliding gravity
            _glidingGravityFactor = _fallingGravityFactor * 0.2f;

            //Wall grappling
            _wallGrapplingGravityFactor = _fallingGravityFactor * 0.15f;
        }
    }
}