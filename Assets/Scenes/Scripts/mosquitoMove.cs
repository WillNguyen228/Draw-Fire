using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Rendering;


public class mosquitoMove : MonoBehaviour
{
    private Rigidbody2D rb2d; // Reference to the Rigidbody2D component
    float rand_direction2;
    float rand_direction1;
    bool once = true;
    public float repulsionRadius = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the GameObject
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void FixedUpdate()
    {
        if (once == true)
        {
            // Randomly generate a direction for the mosquito to move in
            rand_direction1 = UnityEngine.Random.Range(-1f, 1f);
            rand_direction2 = UnityEngine.Random.Range(-1f, 1f);
            once = false;
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 buttonPos2D = new Vector2(transform.position.x, transform.position.y);
        Vector2 directionToMouse = mousePos - buttonPos2D;
        // Debug.Log(directionToMouse);
        float distance = Vector2.Distance(transform.position, mousePos);
        // Debug.Log(distance);
        if (distance < repulsionRadius)
        {
            // Move the object in the opposite direction
            // Debug.Log("repulsed");
            rand_direction1 = -directionToMouse[0];
            rand_direction2 = -directionToMouse[1];
        }

        Vector2 moveDirection = new Vector2(rand_direction1, rand_direction2).normalized; // Example direction
        rb2d.linearVelocity = moveDirection * 10f; // Set the velocity of the Rigidbody2D
    }

    private void OnCollisionEnter2D(Collision2D bounce)
    {
        if (bounce.gameObject.tag == "CollideLeft"){
            // Debug.Log("Hit:" + bounce.transform.name);
            rand_direction1 = UnityEngine.Random.Range(0f, 1f);
            rand_direction2 = UnityEngine.Random.Range(-1f, 1f);
        }
        else if (bounce.gameObject.tag == "CollideRight"){
            // Debug.Log("Hit:" + bounce.transform.name);
            rand_direction1 = UnityEngine.Random.Range(-1f, 0f);
            rand_direction2 = UnityEngine.Random.Range(-1f, 1f);
        }
        else if (bounce.gameObject.tag == "CollideRoof"){
            // Debug.Log("Hit:" + bounce.transform.name);
            rand_direction1 = UnityEngine.Random.Range(-1f, 1f);
            rand_direction2 = UnityEngine.Random.Range(-1f, 0f);
        }
        else if (bounce.gameObject.tag == "CollideFloor"){
            // Debug.Log("Hit:" + bounce.transform.name);
            rand_direction1 = UnityEngine.Random.Range(-1f, 1f);
            rand_direction2 = UnityEngine.Random.Range(0f, 1f);
        }
    }
}
