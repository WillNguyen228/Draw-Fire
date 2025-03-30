using UnityEngine;

public class OnPressed : MonoBehaviour
{
    public void Pressed() {
        Debug.Log("THE BUTTON IS PRESSED");
        FindObjectOfType<GameManager>().CompleteLevel();
    }
}
