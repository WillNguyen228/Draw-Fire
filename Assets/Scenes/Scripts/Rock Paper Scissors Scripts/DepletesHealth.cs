using UnityEngine;
using UnityEngine.TerrainTools;
using UnityEngine.SceneManagement;

public class DepletesHealth : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    RectTransform rt;

    public GameObject completeLevelUI;
    void Start() {
        rt = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void ReduceHealth(int intensity) {
        Vector3 new_result = rt.localScale = new Vector3(rt.localScale.x - 0.1f*intensity, rt.localScale.y, rt.localScale.z);
        if (new_result.x < 0) {
            rt.localScale = rt.localScale = new Vector3(0, rt.localScale.y, rt.localScale.z);
            Debug.Log(transform.parent.name);
            if (transform.parent.name == "barbackgroundmosquito") {
                Invoke(nameof(ShowCompleteLevelUI), 1f);
                Invoke(nameof(LoadNextScene), 3f);
            } else {
                Invoke(nameof(Restart), 1f);
            }
        }
    }

    void ShowCompleteLevelUI()
    {
        completeLevelUI.SetActive(true);
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
