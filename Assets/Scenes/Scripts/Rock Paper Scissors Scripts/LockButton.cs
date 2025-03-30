using UnityEngine;
using UnityEngine.UI;

public class LockButton : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void LockPlayer() {
        GetComponent<Button>().interactable = false;        
    }

    public void UnlockPlayer() {
        GetComponent<Button>().interactable = true;        
    }
}
