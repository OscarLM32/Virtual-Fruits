using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public String level;
    private Button _playButton;

    private void Start()
    {
        _playButton = GameObject.Find("Play").GetComponent<Button>();
    }

    public void OnClick()
    {
        SaveLoadSystem.I.currentLevel = level;
        _playButton.interactable = true;
    }
}
