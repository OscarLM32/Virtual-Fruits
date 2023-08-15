using UnityEngine;

public class End : MonoBehaviour
{
    private Animator _animator;
    private AudioManager _audioManager;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        _audioManager = GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GetComponent<Collider2D>().enabled = false;
        GameActions.LevelEndReached();
        _animator.enabled = true;
        _audioManager.Play("VictoryTheme");
    }
}
