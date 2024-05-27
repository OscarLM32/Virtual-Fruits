using DynamicDifficulty;
using EditorSystems.Logger;
using Enemies;
using Extensions;
using Level;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static Action LevelStart;

    [SerializeField]private SOSelectedLevelData _soLevelData;
    [SerializeField]private HUDManager _hudManager;

    private const int MAX_LIVES = 3;
    private const int LVL_SELECT_MENU_IDX = 1;
    private int _currentLives = MAX_LIVES;

    [SerializeField]private Transform _spawnPoint;
    [SerializeField]private Transform _player;

    private PlayerInput _playerInput;
    private AudioManager _audioManager;

    public GameObject PauseMenu;
    private bool _gamePaused = false;

    private float _timeElapsed = 0f;


    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.MenuControls.PauseMenu.started += PauseGame;

        _audioManager = GetComponent<AudioManager>();
    }

    private void OnEnable()
    {
        _playerInput.MenuControls.Enable();
        GameActions.OnCheckPointReached += CheckpointReached;
        GameActions.OnLevelCompleted += LevelEnd;
        GameActions.OnPlayerDeath += PlayerDeath;

        LevelDifficultyOrchestrator.OnLevelDifficultySet += StartLevel;
    }

    private void OnDisable()
    {
        _playerInput.MenuControls.Disable();
        GameActions.OnCheckPointReached -= CheckpointReached;
        GameActions.OnLevelCompleted -= LevelEnd;
        GameActions.OnPlayerDeath -= PlayerDeath;

        LevelDifficultyOrchestrator.OnLevelDifficultySet -= StartLevel;
    }

    private void Start()
    {
        LoadLevel();
        
        var levelDifficultyOrchestrator = FindObjectOfType<LevelDifficultyOrchestrator>();
        if(levelDifficultyOrchestrator == null)
        {
            EditorLogger.Log(LoggingSystem.GAME_MANAGER, "The level loaded seems to have no Level Difficulty Orchestrator");
            StartLevel();
        }
        //_audioManager.Play("SpringLevelTheme");
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
    }

    private void LoadLevel()
    {
        var level = _soLevelData.levelData.reference.LoadAssetSync<GameObject>();
        Instantiate(level);
    }

    private void StartLevel()
    {
        _spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        _player.transform.position = _spawnPoint.position;

        LevelStart?.Invoke();
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
        DynamicDifficultyManager.I.SaveData();
        SceneManager.LoadScene(LVL_SELECT_MENU_IDX);
    }

    private void PlayerDeath(EnemyType? enemyType)
    {
        StartCoroutine(PlayerDeathCoroutine());
    }

    private IEnumerator PlayerDeathCoroutine()
    {
        _hudManager.UpdateHP(-1f/MAX_LIVES);
        yield return new WaitForSeconds(1.5f);
        _currentLives--;
        if (_currentLives <= 0)
        {
            ExitLevel();
        }
        _player.position = new Vector3(_spawnPoint.position.x, _spawnPoint.position.y, 0);
    }

    private void CheckpointReached(Vector2 position)
    {
        _spawnPoint.position = position + new Vector2(0, 0.5f);
    }

    private void LevelEnd()
    {
        var averageTime = _soLevelData.levelData.completionTimes[DynamicDifficultyManager.I.GenericDifficulty];
        DynamicDifficultyManager.I.OnLevelCompleted(_timeElapsed, averageTime);

        StartCoroutine(LevelEndCoroutine());
    }

    private IEnumerator LevelEndCoroutine()
    {
        yield return new WaitForSeconds(2f);
        ExitLevel();
    }
}
