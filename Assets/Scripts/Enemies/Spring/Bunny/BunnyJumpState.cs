using System;
using UnityEngine;

namespace Enemies.Bunny
{
    public class BunnyJumpState : MonoBehaviour
    {
        private const float _defaultMaxJumpUpHeightDifference = 2f;
        private const float _defaultMaxJumpDownHeightDifference = 0.2f;
        private const float _maxJumpUpTime = 1f;
        private const float _maxJumpDownTime = 0.75f;
        private float _maxJumpHeight = 0f;
        private float _initialJumpVelocity = 0f;

        private float _desiredGravityFactor = 1;

        private const float _maxHorizontalMovementDuration = 0.75f;
        private float _horizontalSpeed = 0f;

        private Rigidbody2D _rb;

        private Vector2 jumpTo;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            //Play jump animation
            Debug.Log($"OnEnable: {_desiredGravityFactor} | {new Vector2(_horizontalSpeed, _initialJumpVelocity)}");
            _rb.gravityScale = _desiredGravityFactor;
            _rb.velocity = new Vector2(_horizontalSpeed, _initialJumpVelocity);
        }

        private void Update()
        {
            //HandleHorizontalMovementLimit();
        }

        public void SetUpJump(Vector2 jumpTo)
        {
            this.jumpTo = jumpTo;
            var jumpFrom = transform.position;
            bool isJumpingUp = jumpFrom.y < jumpTo.y;
            _maxJumpHeight = isJumpingUp ? jumpTo.y + _defaultMaxJumpUpHeightDifference : jumpFrom.y + _defaultMaxJumpDownHeightDifference ;
            Debug.Log(_maxJumpHeight);

            var jumpTime = isJumpingUp ? _maxJumpUpTime : _maxJumpDownTime;
            var timeToApex = jumpTime / 2;
            var _desiredGravity = -2 * _maxJumpHeight / (float)Math.Pow(timeToApex, 2);
            _initialJumpVelocity = 2 * _maxJumpHeight / timeToApex;
            _desiredGravityFactor = _desiredGravity / Physics2D.gravity.y;

            var horizontalDiff = jumpTo.x - jumpFrom.x;
            _horizontalSpeed = horizontalDiff / _maxHorizontalMovementDuration;
        }

        private void HandleHorizontalMovementLimit()
        {
            var distance= jumpTo.x - transform.position.x;
            if(distance <= 0)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
            }
        }
    }
}