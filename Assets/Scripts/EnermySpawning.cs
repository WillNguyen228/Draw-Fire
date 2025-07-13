using UnityEngine;

public class EnermySpawning : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public int enemiesPerSpawn = 2;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnTwoEnemies();
            timer = 0f;
        }
    }

    void SpawnTwoEnemies()
    {
        for (int i = 0; i < enemiesPerSpawn; i++)
        {
            Vector2 spawnPos = GetRandomEdgePositionOutsideCamera();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector2 GetRandomEdgePositionOutsideCamera()
    {
        Camera cam = Camera.main;

        // Get camera world bounds
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float left = cam.transform.position.x - camWidth / 2f;
        float right = cam.transform.position.x + camWidth / 2f;
        float top = cam.transform.position.y + camHeight / 2f;
        float bottom = cam.transform.position.y - camHeight / 2f;

        float padding = 1.0f; // how far outside the screen they spawn

        int edge = Random.Range(0, 4); // 0 = top, 1 = bottom, 2 = left, 3 = right
        float x = 0f, y = 0f;

        switch (edge)
        {
            case 0: // Top
                x = Random.Range(left, right);
                y = top + padding;
                break;
            case 1: // Bottom
                x = Random.Range(left, right);
                y = bottom - padding;
                break;
            case 2: // Left
                x = left - padding;
                y = Random.Range(bottom, top);
                break;
            case 3: // Right
                x = right + padding;
                y = Random.Range(bottom, top);
                break;
        }

        return new Vector2(x, y);
    }
}
