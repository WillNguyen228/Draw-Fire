using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static bool GamePaused = false;
    public GameObject pauseMenuUI; // Reference to the pause menu UI GameObject

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
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
        pauseMenuUI.SetActive(false); // Hide the pause menu UI
        Time.timeScale = 1f; // Resume the game by setting time scale back to 1
        GamePaused = false; // Set the game paused state to false
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Show the pause menu UI
        Time.timeScale = 0f; // Pause the game by setting time scale to 0
        GamePaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f; // Resume the game before loading the menu
        Debug.Log("Loading menu..."); // Log message for loading menu
        SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        // Load the main menu scene here (you can use SceneManager.LoadScene("MainMenu") if you have a scene named "MainMenu")
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game..."); // Log message for quitting game
        Application.Quit(); // Quit the application
    }
}
