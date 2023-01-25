using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private const int MAX_LIVES = 3;
    private const int LVL_SELECT_MENU_IDX = 1;
    private int _currentLives = MAX_LIVES;

    [SerializeField] private Transform _spawnPoint; 

    private void OnEnable()
    {
        GameActions.CheckpointReached += Checkpoint;
        GameActions.LevelEndReached += LevelEnd;
        GameActions.PlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        GameActions.CheckpointReached -= Checkpoint;
        GameActions.LevelEndReached -= LevelEnd;
        GameActions.PlayerDeath -= PlayerDeath;
    }

    private void PlayerDeath(Transform player)
    {
        if (_currentLives <= 0)
        {
            SceneManager.LoadScene(LVL_SELECT_MENU_IDX); //Level selection menu
            return;
        }
        StartCoroutine(PlayerRespawn(player));
    }

    private IEnumerator PlayerRespawn(Transform player)
    {
        yield return new WaitForSeconds(2f);
        player.position = _spawnPoint.position;
    }
    
    
    private void Checkpoint()
    {
        Transform checkpoint = GameObject.FindWithTag("Checkpoint").transform;
        _spawnPoint = checkpoint;
    }

    private void LevelEnd()
    {
        StartCoroutine(LevelEndCoroutine());
    }

    private IEnumerator LevelEndCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(LVL_SELECT_MENU_IDX);
    }
}
