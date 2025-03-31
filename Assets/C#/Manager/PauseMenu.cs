using System.Collections;
using C_.CharacterController;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace C_.Manager
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance { get; private set; } // Singleton 

        public bool isPaused = false;
        public GameObject pauseMenuUI; 
        public GameObject deadMenuUI; 
        public GameObject powerUpMenuUI; 

        private void Awake()
        {
            // Singleton logic
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject); 
            }

            if (pauseMenuUI)
            {
                pauseMenuUI.SetActive(false);
            }
            if (powerUpMenuUI)
            {
                powerUpMenuUI.SetActive(false);
            }
        }

        private void Update()
        {
            // Toggle 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void ResumeGame()
        {
            if (pauseMenuUI)
            {
                pauseMenuUI.SetActive(false);
            }
            if (powerUpMenuUI)
            {
                powerUpMenuUI.SetActive(false);
            }
            StartCoroutine(ResumeAfterDelay());
        }

        private IEnumerator ResumeAfterDelay()
        {
            pauseMenuUI.SetActive(false);   
            powerUpMenuUI.SetActive(false);  
            yield return new WaitForSecondsRealtime(0.1f); 
            Time.timeScale = 1f;           
            isPaused = false;
        }

        public void PauseGame()
        {
            pauseMenuUI.SetActive(true);  
            Time.timeScale = 0f;           
            isPaused = true;
        }
        
        public void ShowPowerUpMenu()
        {
            powerUpMenuUI.SetActive(true);  
            Time.timeScale = 0f;           
            isPaused = true;
        }

        public void SelectPowerUp(int powerUpIndex)
        {
            switch (powerUpIndex)
            {
                case 0:
                    TopDownMovement.Instance.damage += 5;
                    break;
                case 1:
                    TopDownMovement.Instance.dashDamage += 5;
                    break;
                case 2:
                    TopDownMovement.Instance.dashStartValue += 10;
                    break;
            }
            powerUpMenuUI.SetActive(false); 
            ResumeGame();           
        }

        public void DeadMenu()
        {
            deadMenuUI.SetActive(true);
            Time.timeScale = 0f;           
            isPaused = true;
        }

        public void MainMenu()
        {
            Time.timeScale = 1f; 
            isPaused = false;
            SceneManager.LoadScene("Scenes/MainMenu");
            
        }

        public void RestartGame()
        {
            Time.timeScale = 1f; 
            isPaused = false;
            SceneManager.LoadScene("Scenes/GameSceen");
        }

        // Quit
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
