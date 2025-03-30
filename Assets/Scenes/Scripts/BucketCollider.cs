using UnityEngine;
using UnityEngine.UI;

public class BucketCollider : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool toxic;
    public Image m_Image;
    public Sprite m_Sprite;

    public void changeImage() {
        m_Image.sprite = m_Sprite;
    }

    public void Start()
    {
        toxic = false;
        m_Image = GetComponent<Image>();
        // m_Sprite = Resources.Load<Sprite>("Assets/Sprites/wickedBucket");
    }

    // Update is called once per frame
    public void Update()
    {
        // gameObject.GetComponent<BoxCollider2D>().size = new Vector2 (
        //     gameObject.GetComponent<RectTransform>().sizeDelta.x,
        //     gameObject.GetComponent<RectTransform>().sizeDelta.y
        // );
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Toxin") {
            toxic = true;
        }
        
    }
}
