using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject GameOverMenu;
    [SerializeField] private GameObject pauseButton;
    public void Setup()
    {
        Debug.Log("Turning on Game Over Screen");
        GameOverMenu.SetActive(true);

        // Make sure the pause menu is hidden
        if (pauseButton != null)
        {
            pauseButton.SetActive(false);
        }

        // Freeze game
        Time.timeScale = 0f;
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("New Menu");
    }
}
