using UnityEditor.Search;
using UnityEngine;


public class mosquitoGoByeBye : MonoBehaviour
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    mosquitoMove mosquitoMoveScript;

    void Start()
    {
        mosquitoMoveScript = GetComponent<mosquitoMove>(); // Get the mosquitoMove script attached to the GameObject
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Clicked()
    {
        mosquitoMoveScript.enabled = false; // Disable the mosquitoMove script to stop movement
        
    }
}
