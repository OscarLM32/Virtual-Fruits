using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionMenu : MonoBehaviour
{
    public int maxLevel = 1;
    private string _scenePath = "Scenes/";
    private void Start()
    {
        maxLevel = SaveLoadSystem.I.GetMaxLevelReached();
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(SaveLoadSystem.I.currentLevel);
    }
}
