using UnityEngine;
using UnityEngine.SceneManagement;

public class New_Menu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
