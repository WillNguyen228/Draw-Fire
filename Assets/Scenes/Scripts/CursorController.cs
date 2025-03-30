using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Texture2D cursor;
    public Texture2D cursorClick;

    private CursorControls controls;

    private void Awake()
    {
        controls = new CursorControls();
        ChangeCursor(cursor);
        // Cursor.lockState = CursorLockMode.Confined;

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        controls.Mouse.Click.started += _ => StartedClick();
        controls.Mouse.Click.performed += _ => EndedClick();
    }

    private void StartedClick(){
        ChangeCursor(cursorClick);
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void EndedClick(){
        ChangeCursor(cursor);
    }
    private void ChangeCursor(Texture2D cursorType)
    {
        Vector2 hotspot = new Vector2(cursorType.width / 2, cursorType.height / 2);
        Cursor.SetCursor(cursorType, hotspot, CursorMode.ForceSoftware);
    }

    public void DisableCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Reset to default
        Cursor.visible = true;
    }
}
