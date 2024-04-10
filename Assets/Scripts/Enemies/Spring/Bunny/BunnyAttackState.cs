
using System;
using UnityEngine;

namespace Enemies.Bunny
{
    public class BunnyAttackState : MonoBehaviour
    {
        public Action OnAttackFinished;

        [SerializeField]private float _maxJumpHeight = 3;
        [SerializeField]private float _maxJumpTime = 1;

        private float _initialJumpVelocity = 9.8f;
        private float _desiredGravityFactor = 1;

        private float _horizontalSpeed = 0;

        private Vector2 _attackTo;

        private Rigidbody2D _rb;
        private Animator _animator;
        [SerializeField]private BoxCollider2D _attackTrigger;
        [SerializeField]private Transform _groundChecker;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _attackTrigger.enabled = false;
            _rb.gravityScale = _desiredGravityFactor;
            _rb.velocity = new Vector2(_horizontalSpeed, _initialJumpVelocity);
        }

        private void Update()
        {
            HandleGrounded();
            HandleJumpAnimation();
        }

        public void SetUpAttack(Vector2 attackTo)
        {
            _attackTo = attackTo;
            CalculateHorizontalVelocity();
        }

        public void SetDynamicValues(float maxJumpTime, float maxAttackRange)
        {
            _maxJumpTime = maxJumpTime;

            var timeToApex = _maxJumpTime / 2;
            var _desiredGravity = -2 * _maxJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex;
            _desiredGravityFactor = _desiredGravity / Physics2D.gravity.y;

            _attackTrigger.size = new Vector2(maxAttackRange, _attackTrigger.size.y);
        }

        private void CalculateHorizontalVelocity()
        {
            float distance = Vector2.Distance(transform.position, _attackTo);
            _horizontalSpeed = distance / _maxJumpTime;

            if(transform.position.x > _attackTo.x)
            {
                _horizontalSpeed = -_horizontalSpeed;
            }
        }

        private void HandleGrounded()
        {
            //I don't want to check for grounded when the enemy is going up. This way we can avoid the enemy being considered
            //grounded right at the start of the attack
            if (_rb.velocity.y > 0) return;

            Vector2 position = _groundChecker.position;
            if (Physics2D.OverlapBox(position, new Vector2(0.45f, 0.1f), 0, LayerMask.GetMask("Ground")) ||
                Physics2D.OverlapBox(position, new Vector2(0.45f, 0.1f), 0, LayerMask.GetMask("Platform")))
            {
                _attackTrigger.enabled = true;
                OnAttackFinished?.Invoke();
            }
        }

        private void HandleJumpAnimation()
        {
            if (_rb.velocity.y >= 0)
            {
                _animator.Play("BunnyJump");
            }
            else
            {
                _animator.Play("BunnyFall");
            }
        }
    }
}