using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public GameObject ActivePlayer { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // optional
    }

    public void SetActivePlayer(GameObject player)
    {
        ActivePlayer = player;
    }
}
