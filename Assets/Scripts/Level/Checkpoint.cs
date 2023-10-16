using System.Collections;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool _flagOut = false;
    private Animator _animator;
    private float _flagOutLength;

    private static class Animations
    {
        public static readonly string FLAG_OUT = "FlagOut";
        public static readonly string FLAG_IDLE = "FlagIdle";
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
        AnimatorClipInfo[] clipsInfo = _animator.GetCurrentAnimatorClipInfo(0);
        foreach (var info in clipsInfo)
        {
            if (info.clip.name == Animations.FLAG_OUT)
                _flagOutLength = info.clip.length;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!_flagOut)
        {
            _flagOut = true;
            StartCoroutine(FlagAnimation());
        }
        GameActions.CheckpointReached();
    }

    private IEnumerator FlagAnimation()
    {
        _animator.enabled = true;
        _animator.Play(Animations.FLAG_OUT);
        yield return new WaitForSeconds(_flagOutLength);
        _animator.Play(Animations.FLAG_IDLE);
    }
}
