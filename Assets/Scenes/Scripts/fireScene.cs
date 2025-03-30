using UnityEngine;

public class fireScene : MonoBehaviour
{
    public GameObject Scene;

    public void fire() {
        Scene.SetActive(true);
    }
}
