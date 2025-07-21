using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class FullscreenBackground : MonoBehaviour
{
    void Update()
    {
        var cam = Camera.main;
        if (cam == null) return;

        var sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == null) return;

        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;
        transform.localScale = new Vector3(screenWidth / spriteSize.x, screenHeight / spriteSize.y, 1f);
        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0f);
    }
}
