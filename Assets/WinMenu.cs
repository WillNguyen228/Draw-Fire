using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject pauseButton;
    public void Setup()
    {
        Debug.Log("Turning on Game Over Screen");
        winMenu.SetActive(true);

        // Make sure the pause menu is hidden
        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        // Freeze game
        Time.timeScale = 0f;
    }

    public void Next()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Levels()
    {
        Time.timeScale = 1f;
        Menu.ShouldShowLevelsPanel = true;
        SceneManager.LoadScene("New Menu");
    }
  
}
