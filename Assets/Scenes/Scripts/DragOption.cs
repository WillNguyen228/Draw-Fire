using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    public void OnClick(InputAction.CallbackContext content)
    {
        // if (!context.start) {
        //     return;
        // }
        // var rayHit = Physics2D.GetRayIntersection
    }
}
