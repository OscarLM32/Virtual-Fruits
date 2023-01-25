using UnityEngine;

public class End : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameActions.LevelEndReached();
        _animator.enabled = true;
    }
}
