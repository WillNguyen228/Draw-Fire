using UnityEngine;
using UnityEngine.UI;
// using static fireScene;


public class matchDrag : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 origin;
    private bool dragging;
    // public fireScene Scene;
    public GameObject images;
    
    void Start() {
        origin = transform.position;
        dragging = false;
        images.SetActive(false);
    }

    private Vector3 offset;
   
    private void OnMouseDown() {
        dragging = true;
    }


    void Update() {
        if (dragging) {
            Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            
            transform.position = new Vector3(mouse.x, mouse.y, transform.position.z);
        }
    }
    private void OnMouseUp() {
        dragging = false;
        transform.position = origin;
        images.SetActive(true);
        FindObjectOfType<GameManager>().EndGame();
    }
}
