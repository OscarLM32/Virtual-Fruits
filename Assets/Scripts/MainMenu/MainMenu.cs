using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject MainMenuOptions;
    public TextMeshProUGUI Score;

    private AudioManager _audioManager;
    private void Start()
    {
        _audioManager = GetComponent<AudioManager>();
        SetScore();
    }

    public void OpenMenu(GameObject menu)
    {
        _audioManager.Play("Button");
        MainMenuOptions.SetActive(false);
        menu.SetActive(true);
    }

    public void CloseMenu(GameObject menu)
    {
        _audioManager.Play("Button");
        menu.SetActive(false);
        MainMenuOptions.SetActive(true);
    }

    public void Play()
    {
        _audioManager.Play("Button");
        SceneManager.LoadScene(1);
    }

    public void CloseApp()
    {
        Application.Quit();
    }

    private void SetScore()
    {
        Score.text = SaveLoadSystem.I.GetTotalPickedFruits().ToString();
    }

}
