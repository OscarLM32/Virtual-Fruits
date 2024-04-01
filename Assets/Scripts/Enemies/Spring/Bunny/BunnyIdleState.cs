using UnityEngine;

namespace Enemies.Bunny
{
    public class BunnyIdleState : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _animator.Play("BunnyIdle");
        }
    }
}