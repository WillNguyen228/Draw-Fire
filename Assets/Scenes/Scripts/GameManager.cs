using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;

    public float restartDelay = 1f;

    public GameObject completeLevelUI;

    public void CompleteLevel() {
        Debug.Log("LEVEL COMPLETE!");
        completeLevelUI.SetActive(true);
    }
    public void EndGame() //This function should be activated when the player lose
    {
        gameHasEnded = true;
        Debug.Log("GAME OVER");
        Invoke("Restart", restartDelay);
    }

    void Restart() {
        //SceneManager.LoadScene("Level01");
        Debug.Log("RESTARTED");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
