using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class toxinDrag : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 origin;
    private bool dragging;
    
    //Set this in the Inspector
    private BucketCollider bucket;
    
    void Start() {
        origin = transform.position;
        dragging = false;
        bucket = GameObject.Find("Bucket").GetComponent<BucketCollider>();
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
    public void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Collided toxin");
        if (other.tag == "Toxin") {
            bucket.toxic = true;
        }
        
    }
    private void OnMouseUp() {
        dragging = false;
        if (bucket.toxic == true) {
            bucket.changeImage();
            FindObjectOfType<GameManager>().CompleteLevel();
        }
        transform.position = origin;
    }
}
