using System;
using TMPro;
using UnityEngine;

namespace Enemies.Bunny
{
    public class BunnyJumpState : MonoBehaviour
    {
        private const float _defaultMaxJumpUpHeightDifference = 2f;
        private const float _defaultMaxJumpDownHeightDifference = 1.75f;
        private const float _maxJumpUpTime = 1f;
        private const float _maxJumpDownTime = 0.75f;
        private float _maxJumpHeight = 0f;
        private float _initialJumpVelocity = 0f;

        private float _desiredGravityFactor = 1;

        private const float _maxHorizontalMovementDuration = 0.75f;
        private float _horizontalSpeed = 0f;

        private Rigidbody2D _rb;
        private Animator _animator;
        [SerializeField]private Collider2D _attackTrigger;

        private Vector2 jumpTo;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            //Play jump animation
            _rb.gravityScale = _desiredGravityFactor;
            _rb.velocity = new Vector2(_horizontalSpeed, _initialJumpVelocity);
            _attackTrigger.enabled = false;
        }

        private void OnDisable()
        {
            _attackTrigger.enabled = true;
        }

        private void Update()
        {
            HandleHorizontalMovementLimit();
            HandleAnimation();
        }

        public void SetUpJump(Vector2 jumpTo)
        {
            this.jumpTo = jumpTo;
            var jumpFrom = transform.position;
            bool isJumpingUp = jumpFrom.y < jumpTo.y;

            _maxJumpHeight = isJumpingUp ? jumpTo.y - jumpFrom.y + _defaultMaxJumpUpHeightDifference : _defaultMaxJumpDownHeightDifference;

            var jumpTime = isJumpingUp ? _maxJumpUpTime : _maxJumpDownTime;
            var timeToApex = jumpTime / 2;
            var _desiredGravity = -2 * _maxJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex;
            _desiredGravityFactor = _desiredGravity / Physics2D.gravity.y;

            var horizontalDiff = jumpTo.x - jumpFrom.x;
            _horizontalSpeed = horizontalDiff / _maxHorizontalMovementDuration;
        }

        //TODO: this is bound to be deleted. It created a very fake effect. Need to find a way to properly calculate the jumps
        private void HandleHorizontalMovementLimit()
        {
            var isPassedPoint = _rb.velocity.x > 0 ? transform.position.x > jumpTo.x : transform.position.x < jumpTo.x;
            if(isPassedPoint)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }

        private void HandleAnimation()
        {
            if(_rb.velocity.y >= 0)
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