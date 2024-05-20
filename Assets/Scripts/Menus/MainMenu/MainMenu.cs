using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus.MainMenu
{
    public class MainMenu : MonoBehaviour
    {

        public void OpenMenu(GameObject menu)
        {
            menu.SetActive(true);
        }

        public void CloseMenu(GameObject menu)
        {
            menu.SetActive(false);
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
        }

        public void CloseApp()
        {
            Application.Quit();
        }

    }
}