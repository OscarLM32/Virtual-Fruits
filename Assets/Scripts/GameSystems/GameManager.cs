using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const int MAX_LIVES = 3;
    private const int LVL_SELECT_MENU_IDX = 1;
    private int _currentLives = MAX_LIVES;

    [SerializeField] private Transform _spawnPoint;
    private Transform _player;
    private PlayerInput _playerInput;
    private AudioManager _audioManager;

    public GameObject PauseMenu;
    private bool _gamePaused = false;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _player = GameObject.Find("Player").transform;
        _player.transform.position = new Vector3(_spawnPoint.position.x, _spawnPoint.position.y, 0);

        _playerInput.MenuControls.PauseMenu.started += PauseGame;

        _audioManager = GetComponent<AudioManager>();
    }

    private void Start()
    {
        _audioManager.Play("SpringLevelTheme");
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        _gamePaused = !_gamePaused;
        GameActions.GamePause(_gamePaused);
        PauseMenu.SetActive(_gamePaused);
        Time.timeScale = _gamePaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        _gamePaused = false;
        GameActions.GamePause(_gamePaused);
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ExitLevel()
    {
        SceneManager.LoadScene(LVL_SELECT_MENU_IDX);
    }

    private void PlayerDeath()
    {
        _currentLives--;
        if (_currentLives <= 0)
        {
            SceneManager.LoadScene(LVL_SELECT_MENU_IDX); //Level selection menu
            return;
        }
        _player.position = new Vector3(_spawnPoint.position.x, _spawnPoint.position.y, 0);
    }

    private void CheckpointReached()
    {
        //Maybe we could pass the transform while calling so more than one checkpoint can be placed
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

    private void OnEnable()
    {
        _playerInput.MenuControls.Enable();
        GameActions.CheckpointReached += CheckpointReached;
        GameActions.LevelEndReached += LevelEnd;
        GameActions.PlayerDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        _playerInput.MenuControls.Disable();
        GameActions.CheckpointReached -= CheckpointReached;
        GameActions.LevelEndReached -= LevelEnd;
        GameActions.PlayerDeath -= PlayerDeath;
    }
}
