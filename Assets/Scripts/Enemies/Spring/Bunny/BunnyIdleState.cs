using UnityEngine;

namespace Enemies.Bunny
{
    public class BunnyIdleState : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _animator.Play("BunnyIdle");
            _rb.velocity = Vector2.zero;
        }
    }
}