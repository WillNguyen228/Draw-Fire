using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ChangeImage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public UnityEngine.UI.Image oldImage;
    public Sprite newImage;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ImageChange()
    {
        oldImage.sprite = newImage; // Change the image to the new sprite
    }
}
