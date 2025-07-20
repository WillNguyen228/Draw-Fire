using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;

public class EnermySpawning : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;
    public float spawnInterval = 5f;
    public int enemiesPerSpawn = 2;
    public int totalEnemiesToSpawn = 10;

    private float timer;
    private int enemiesSpawned = 0;
    private int enemiesAlive = 0;

    [Header("Win Menu")]
    public WinMenu winMenu;

    [Header("Final Dialogue")]
    public NPCConversation finalConversation;
    private bool finalDialoguePlayed = false;

    void Update()
    {
        if (enemiesSpawned < totalEnemiesToSpawn)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnEnemies();
                timer = 0f;
            }
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemiesPerSpawn && enemiesSpawned < totalEnemiesToSpawn; i++)
        {
            Vector2 spawnPos = GetRandomEdgePositionOutsideCamera();

            if (enemyPrefabs.Length > 0)
            {
                int index = Random.Range(0, enemyPrefabs.Length);
                GameObject enemyToSpawn = enemyPrefabs[index];
                GameObject enemyInstance = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);

                // Register death callback
                Goblin enemyScript = enemyInstance.GetComponent<Goblin>();
                if (enemyScript != null)
                {
                    enemyScript.spawner = this;
                }

                enemiesSpawned++;
                enemiesAlive++;
            }
        }
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0 && enemiesSpawned >= totalEnemiesToSpawn)
        {
            Debug.Log("All enemies defeated. Playing final dialogue.");
            finalDialoguePlayed = true;

            if (finalConversation != null)
            {
                GameManager.IsGamePaused = true;
                Debug.Log("Pausing the game?: " + GameManager.IsGamePaused);
                Debug.Log("Triggering dialogue: " + finalConversation);
                ConversationManager.Instance.StartConversation(finalConversation);
            }
            else
            {
                ShowWinMenu();
            }
        }
    }
    public void OnFinalConversationEnded()
    {
        GameManager.IsGamePaused = false;
        Debug.Log("Pausing the game?: " + GameManager.IsGamePaused);
        Debug.Log("Final dialogue finished. Showing win menu.");
        Debug.Log("Level won. Unlocking more one more level");
        UnlockNewLevel();
        Debug.Log("Trigger victory.");
        winMenu.Setup();
    }

    void ShowWinMenu()
    {
        UnlockNewLevel();
        winMenu.Setup();
    }

    void UnlockNewLevel()
    {
        // When a round is won add one more index to unlocked rounds
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
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
