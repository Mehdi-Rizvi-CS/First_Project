using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        // Listens for the Escape key to toggle the pause state
if (UnityEngine.InputSystem.Keyboard.current != null && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Hide the menu
        Time.timeScale = 1f;          // Resume game time
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Show the menu
        Time.timeScale = 0f;          // Freeze game time
        GameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        // Crucial: Reset time scale to normal before leaving the scene, 
        // otherwise your Main Menu might be frozen.
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        // Based on "image_7ad692.jpg", your menu scene appears to be named "Menu".
        SceneManager.LoadScene("Main Menu"); 
    }
}