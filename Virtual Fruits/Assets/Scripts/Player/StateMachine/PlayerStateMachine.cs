using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    private const float GRAVITY = -9.8f; 
    
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public Transform groundChecker;
    public Transform wallChecker;
    public TextMeshProUGUI debugText;

    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;
    
    private PlayerInput _playerInput;
    private Rigidbody2D _rb;
    private Animator _animator;

    //Horizontal movement variables
    private Vector2 _currentMovementInput;
    //TODO: change this variable's name. It's just horizontal movement that stores. Not so representative
    private Vector2 _currentMovement;
    
    private int _lastFacingDirection = 1;


    //TODO: probably rename to "runningSpeed"
    private float _speed = 6f;
    private bool _isWalking = false;
    private float _walkingSpeed;

   
    //Jumping variables
    //TODO: implement a float that takes the time at which the jump is pressed to use it with jumpGraceTime
    private bool _isJumpPressed = false;
    private float _jumpGraceTime = 0.2f; //TODO: apply this grace time
    private bool _jumped = false;
    private bool _requireNewJumpPress = false;
    private float _initialJumpVelocity;
    private float _maxJumpHeight = 2.7f;
    private float _maxJumpTime = 0.7f;

    //Double jumping variables
    //The same input will be used: _isJumpPressed;
    private bool _doubleJumped = false;
    private float _initialDoubleJumpVelocity;
    //I'll make use of this variable in case I decide to make the gravity different for jump and double jump
    private float _maxDoubleJumpHeight = 1.7f;
    private float _maxDoubleJumpTime = 0.5f;
    
    //Dashing variables
    private bool _isDashPressed = false;
    private bool _requireNewDashPress = false; //TODO: apply this logic
    private bool _dashed = false;
    private float _dashDistance = 4f;
    private float _dashTime = 0.35f;
    private float _dashSpeed;
    
    //Gliding variables
    private float _glidingPressedTime = 0f;
    private float _glidingActivationTime = 0.15f; //The time it takes to have the button pressed before gliding
    
    //Gravity
    private float _desiredGravity;     //TODO: I have to look into deleting or not this variable
    private float _jumpingGravityFactor;
    private float _doubleJumpingGravityFactor;
    private float _fallingGravityFactor;
    private float _glidingGravityFactor;
    private float _wallGrapplingGravityFactor;
    
    //Checkers
    private bool _isGrounded;
    private bool _isGrapplingWall;
    
    //Getters and Setters
    public PlayerBaseState CurrentState { get => _currentState; set => _currentState = value; }
    public Rigidbody2D Rb2D => _rb;
    public Animator PlayerAnimator => _animator;

    //HORIZONTAL MOVEMENT GETTERS AND SETTER
    public Vector2 CurrentMovementInput => _currentMovementInput;
    public Vector2 CurrentMovement => _currentMovement;
    public float Speed { get => _speed; set => _speed = value; }
    public bool IsWalking => _isWalking;
    public float WalkingSpeed => _walkingSpeed;
    public int LastFacingDirection => _lastFacingDirection;
    /////////////////////////////////////////////////////////////////////////////////
    
    //JUMPING GETTERS AND SETTERS 
    public bool IsJumpPressed { get => _isJumpPressed; set => _isJumpPressed = value; }
    public bool Jumped { get => _jumped; set => _jumped = value; }
    public bool RequireNewJumpPress { get => _requireNewJumpPress; set => _requireNewJumpPress = value; }
    public bool DoubleJumped { get => _doubleJumped; set => _doubleJumped = value; }
    public float InitialJumpVelocity => _initialJumpVelocity;
    public float InitialDoubleJumpVelocity => _initialDoubleJumpVelocity;
    ////////////////////////////////////////////////////////////////////////////////////
    
    //DASHING GETTERS AND SETTERS
    public bool IsDashPressed => _isDashPressed;
    public bool Dashed {get => _dashed; set => _dashed = value;}
    public float DashSpeed => _dashSpeed;
    public float DashTime => _dashTime;
    ////////////////////////////////////////////////////////////////////////////////////
    
    //GLIDING GETTER AND SETTERS
    public float GlidingPressedTime {get => _glidingPressedTime; set => _glidingPressedTime = value; }
    public float GlidingActivationTime => _glidingActivationTime;
    //////////////////////////////////////////////////////////////////////////////////// 
    
    //GRAVITY GETTER AND SETTERS
    public float JumpingGravityFactor => _jumpingGravityFactor;
    public float DoubleJumpingGravityFactor => _doubleJumpingGravityFactor;
    public float FallingGravityFactor => _fallingGravityFactor;
    public float GlidingGravityFactor => _glidingGravityFactor;
    public float WallGrapplingGravityFactor => _wallGrapplingGravityFactor;
    ///////////////////////////////////////////////////////////////////////////////////// 
    
    public bool IsGrounded{ get => _isGrounded; set => _isGrounded = value;}
    public bool IsGrapplingWall{ get=>_isGrapplingWall; set => _isGrapplingWall = value;}


    private void Awake()
    {
        _playerInput = new PlayerInput();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _lastFacingDirection = 1; //Let's make it always look to the right when spawning
        
        //Setting up state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();
        
        SetUpInputs();

        _walkingSpeed = _speed * 0.7f;
        _dashSpeed = _dashDistance / _dashTime;
        
        SetUpGravityVariables();
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
        IsGrounded = false;
        Vector2 position = groundChecker.position;
        //TODO: change this for a box
        if (Physics2D.OverlapCircle(position, 0.1f, groundLayer) ||
            Physics2D.OverlapCircle(position, 0.1f, platformLayer))
        {
            IsGrounded = true;
        }
    }

    private void HandleGrapplingWall()
    {
        IsGrapplingWall = false;
        Vector2 position = wallChecker.position;
        if (Physics2D.OverlapBox(position, new Vector2(0.1f, 0.4f), 0, groundLayer)
            && !_isGrounded) //The character cannot be considered grappling a wall if grounded, just colliding 
        {
            IsGrapplingWall = true;
        }

    }

    //TODO: Investigate whether it is better for me to use update or fixedupdate  
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
        HandleSpriteDirection();
    }

  
    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
        _glidingPressedTime = 0f;
    }
    
    private void OnWalk(InputAction.CallbackContext context)
    {
        _isWalking = !_isWalking;
        //Debug.Log(_isWalking);
    }
    
    private void OnDash(InputAction.CallbackContext context)
    {
        _isDashPressed = context.ReadValueAsButton();
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
    }
    
    //The jumps' velocity is also calculated inside
    private void SetUpGravityVariables()
    {
        float timeToApex = _maxJumpTime / 2; //The time it takes to reach the highest point
        _desiredGravity = (-2 * _maxJumpHeight) / (float)Math.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        _jumpingGravityFactor = (_desiredGravity / GRAVITY);
        
        //The falling gravity is higher than the jumping gravity;
        _fallingGravityFactor = _jumpingGravityFactor * 1.6f;
        
        //Initialization of double jump variables
        timeToApex = _maxDoubleJumpTime / 2;
        _desiredGravity = (-2 * _maxDoubleJumpHeight) / (float)Math.Pow(timeToApex, 2);
        _initialDoubleJumpVelocity = (2 * _maxDoubleJumpHeight) / timeToApex;
        _doubleJumpingGravityFactor = (_desiredGravity / GRAVITY);
        
        //Gliding gravity
        _glidingGravityFactor = _fallingGravityFactor * 0.2f;
        
        //Wall grappling
        _wallGrapplingGravityFactor = _fallingGravityFactor * 0.15f;
    }
    
    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }
}
