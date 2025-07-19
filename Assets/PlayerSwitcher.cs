using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public GameObject archerPlayer;
    public GameObject knightPlayer;
    public MagicDrawing magicDrawing; 
    public Animator archerAnimator; 
    public Animator knightAnimator;  
    public static GameObject activePlayer;  // 👈 Make this static

    void Start()
    {
        activePlayer = archerPlayer;
        knightPlayer.SetActive(false);
        // Ensure MagicDrawing starts with correct animator
        magicDrawing.SetActiveAnimator(archerAnimator);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchPlayers();
        }
    }

    public void SwitchPlayers()
    {
        Vector3 currentPosition = activePlayer.transform.position;

        // Disable current player
        activePlayer.SetActive(false);

        // Switch reference
        if (activePlayer == archerPlayer)
        {
            activePlayer = knightPlayer;
            magicDrawing.SetActiveAnimator(knightAnimator);
        }
        else
        {
            activePlayer = archerPlayer;
            magicDrawing.SetActiveAnimator(archerAnimator); 
        }

        // Set new player position and activate
        activePlayer.transform.position = currentPosition;
        activePlayer.SetActive(true);
    }
}
