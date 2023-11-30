using DG.Tweening;
using UnityEngine;

namespace Enemies
{
    public class SnailOutShell : MonoBehaviour
    {
        private static class SnailOutShellAnimations
        {
            public static readonly string IDLE = "SnailIdle";
            public static readonly string WALK = "SnailWalk";
            public static readonly string SHELL_IN = "ShellIn";
        }

        private SnailStateMachine Context;

        private void Awake()
        {
            Context = GetComponent<SnailStateMachine>();
        }

        private void Update()
        {
            Context.SnailAnimator.Play(SnailOutShellAnimations.WALK);
            if (Context.PlayerIntrigger)
                Exit();
        }

        private void Exit()
        {
            DOTween.Pause(Context.PatrolId);
            Context.SnailAnimator.Play(SnailOutShellAnimations.SHELL_IN);
            Context.ChangeState(Context.ShellInState);
        }
    }
}