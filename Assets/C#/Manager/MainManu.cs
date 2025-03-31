using System;
using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

namespace C_.Manager
{
    public class MainMenu : MonoBehaviour
    {
        public GameObject mainMenu;
        public GameObject optionsMenu;

        private void Awake()
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }

        public void StartGame()
        {
            if (HighScore_Level.Instance)
            {
                HighScore_Level.Instance.playerScore = 0;
            }
            SceneManager.LoadScene("Scenes/GameSceen");
            Debug.Log("loading");
        }

        public void OpenSettings()
        {
            mainMenu.SetActive(false);
            Debug.Log("Settings Menu Opened");
            optionsMenu.SetActive(true);
        }

        public void CloseSettings()
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }

        public void QuitGame()
        {
            
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}