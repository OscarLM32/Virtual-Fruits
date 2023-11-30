using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Enemies
{
    public class SnailInShell : Enemy
    {
        private static class SnailInShellAnimations
        {
            public static readonly string SHELL_IDLE = "ShellIdle";
            public static readonly string SHELL_OUT = "ShellOut";
            public static readonly string SHELL_HIT = "ShellHit";
        }

        private SnailStateMachine Context;
        private float _hitTime = 1;
        private float _timeElapsed = 0;
        private bool _playerCollided = false;

        private void Awake()
        {
            Context = GetComponent<SnailStateMachine>();
        }

        private void Update()
        {
            if (_playerCollided)
                HandleHit();
            else if (!Context.PlayerIntrigger)
                StartCoroutine(Exit());
        }

        protected override void OnLevelStart()
        {
            
        }

        private void HandleHit()
        {
            if (_timeElapsed > _hitTime)
            {
                _timeElapsed = 0;
                Context.SnailAnimator.Play(SnailInShellAnimations.SHELL_IDLE);
                _playerCollided = false;
                return;
            }
            _timeElapsed += Time.deltaTime;
            Context.SnailAnimator.Play(SnailInShellAnimations.SHELL_HIT);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag.Equals("Player"))
            {
                _playerCollided = true;
            }
        }

        private IEnumerator Exit()
        {
            Context.SnailAnimator.Play(SnailInShellAnimations.SHELL_OUT);
            yield return new WaitForSeconds(Context.ShellOutAnimationTime);
            DOTween.Play(Context.PatrolId);
            Context.ChangeState(Context.ShellOutState);
        }
    }
}