using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;            // Drag your cursor PNG here in Inspector
    public Vector2 hotspot = Vector2.zero;     // Click point (0,0 = top-left)
    public CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
        DontDestroyOnLoad(gameObject); // Keep across scenes
    }
}
