using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionMenu : MonoBehaviour
{
    private const int PAGE_SIZE = 800;

    public Button nextSectionButton;
    public Button previousSectionButton;

    public string maxLevelReached = "0-0";
    private int _currentSection = 0;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = GetComponent<AudioManager>();
        maxLevelReached = SaveLoadSystem.I.GetMaxLevelReached();
        SetUpLevels();
    }

    public void LoadLevel()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene(SaveLoadSystem.I.currentLevel);
    }

    public void GoBackMainMenu()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene(0);
    }

    public void NextSection()
    {
        _audioManager.Play("SectionChange");
        _currentSection++;
        GetComponent<RectTransform>().DOLocalMoveX(-PAGE_SIZE * _currentSection, 0.6f).SetEase(Ease.OutBack);

        previousSectionButton.interactable = true;
        if (_currentSection == 3)
            nextSectionButton.interactable = false;
    }

    public void PreviousSection()
    {
        _audioManager.Play("SectionChange");
        _currentSection--;
        GetComponent<RectTransform>().DOLocalMoveX(-PAGE_SIZE * _currentSection, 0.6f).SetEase(Ease.OutBack);

        nextSectionButton.interactable = true;
        if (_currentSection == 0)
            previousSectionButton.interactable = false;
    }

    private void SetUpLevels()
    {
        LevelSelection[] levels = GetComponentsInChildren<LevelSelection>();
        foreach (var level in levels)
        {
            level.gameObject.GetComponent<Button>().interactable = true;
            if (IsLevelGreaterThanMax(level.level))
                return;
        }
    }

    private bool IsLevelGreaterThanMax(string level)
    {
        var maxLevelReachedValues = maxLevelReached.Split("-");
        var levelValues = level.Split("-");

        if (Int32.Parse(levelValues[0]) > Int32.Parse(maxLevelReachedValues[0]))
            return true;

        if (Int32.Parse(levelValues[1]) > Int32.Parse(maxLevelReachedValues[1]))
            return true;

        return false;
    }
}