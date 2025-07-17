using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject levelsPanel;
    public static bool ShouldShowLevelsPanel = false;
    void Start()
    {
        if (ShouldShowLevelsPanel)
        {
            levelsPanel.SetActive(true);
            ShouldShowLevelsPanel = false;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    [ContextMenu("Reset PlayerPrefs")]
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs reset.");
    }
}
