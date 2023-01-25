using System;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelection;

    public static int selectedLevel = 0;


    public void Play()
    {
        //SceneManager.LoadScene("LevelSelection");
        SceneManager.LoadScene("LevelFinalPractice");
    }

    public void LevelSelectionEnter()
    {
        mainMenu.SetActive(false);
        levelSelection.SetActive(true); 
    }

    public void LevelSelectionExit()
    {
        levelSelection.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
