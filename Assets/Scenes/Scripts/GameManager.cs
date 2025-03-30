using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 1f;
    public float uiDelay = 1f;  // Delay before showing UI
    public float displayTime = 2f; // Time the UI stays visible before changing scene

    public GameObject completeLevelUI;

    public void CompleteLevel() {
        Debug.Log("LEVEL COMPLETE!");
        //completeLevelUI.SetActive(true);
        Invoke(nameof(ShowCompleteLevelUI), uiDelay);
        Invoke(nameof(LoadNextScene), uiDelay + displayTime);
    }
    void ShowCompleteLevelUI()
    {
        completeLevelUI.SetActive(true);
    }
    public void EndGame() //This function should be activated when the player lose
    {
        gameHasEnded = true;
        Debug.Log("GAME OVER");
        Invoke("Restart", restartDelay);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Restart() {
        //SceneManager.LoadScene("Level01");
        Debug.Log("RESTARTED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
