using UnityEngine;

public class SnailStateMachine : MonoBehaviour
{
    public MonoBehaviour ShellInState;
    public MonoBehaviour ShellOutState;
    [SerializeField] private MonoBehaviour _currentState;
    public LayerMask playerLayer;

    private const float SHELL_IN_ANIMATION_TIME = 0.583f / 3;
    private const float SHELL_OUT_ANIMATION_TIME = 0.583f / 2;

    private string _patrolId;
    private Animator _animator;
    private bool _playerInTrigger = false;

    private Vector2 _lastPosition;
    private Vector2 _velocity = Vector2.zero;
    private float _zRotation;

    public float ShellInAnimationTime => SHELL_IN_ANIMATION_TIME;
    public float ShellOutAnimationTime => SHELL_OUT_ANIMATION_TIME;

    public Animator SnailAnimator => _animator;
    public string PatrolId => _patrolId;
    public bool PlayerIntrigger => _playerInTrigger;


    private void Awake()
    {
        ShellInState.enabled = false;
        _currentState = ShellOutState;
        _currentState.enabled = true;

        //_patrolId = GetComponent<EnemyBasicPatrolling>().patrolId;
        _animator = GetComponent<Animator>();

        _lastPosition = transform.position;
        _zRotation = transform.eulerAngles.z % 360;
    }

    private void Update()
    {
        _velocity = (Vector2)transform.position - _lastPosition;
        _lastPosition = transform.position;
        HandleDirection();
    }

    public void ChangeState(MonoBehaviour newState)
    {
        if (_currentState != null)
            _currentState.enabled = false;
        _currentState = newState;
        _currentState.enabled = true;
    }

    private void HandleDirection()
    {
        if (_velocity == Vector2.zero)
            return;

        bool xPositiveVelocity = _velocity.x > 0 ? true : false;
        bool yPositiveVelocity = _velocity.y > 0 ? true : false;

        switch (_zRotation)
        {
            case 0:
                transform.localScale = xPositiveVelocity ? new Vector3(3, 3, 1) : new Vector3(-3, 3, 1);
                break;
            case 90:
                transform.localScale = yPositiveVelocity ? new Vector3(-3, 3, 1) : new Vector3(3, 3, 1);
                break;
            case 180:
                transform.localScale = xPositiveVelocity ? new Vector3(-3, 3, 1) : new Vector3(3, 3, 1);
                break;
            case 270:
                transform.localScale = yPositiveVelocity ? new Vector3(3, 3, 1) : new Vector3(-3, 3, 1);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Weapon"))
        {
            _playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Player") || col.gameObject.tag.Equals("Weapon"))
            _playerInTrigger = false;
    }
}
