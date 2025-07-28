using UnityEngine;

public class PlayerSwitcher : MonoBehaviour
{
    public GameObject archerPlayer;
    public GameObject knightPlayer;
    public MagicDrawing magicDrawing; 
    public Animator archerAnimator; 
    public Animator knightAnimator;  
    public static GameObject activePlayer;  // 👈 Make this static
    private Player archerScript;
    private Player knightScript;

    void Start()
    {
        activePlayer = archerPlayer;
        archerPlayer.SetActive(true);
        knightPlayer.SetActive(false);
        // Ensure MagicDrawing starts with correct animator
        magicDrawing.SetActiveAnimator(archerAnimator);

        // Cache Player scripts
        archerScript = archerPlayer.GetComponent<Player>();
        knightScript = knightPlayer.GetComponent<Player>();

        archerScript.SetActiveState(true);
        knightScript.SetActiveState(false);
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
            archerScript.SetActiveState(false);
            knightScript.SetActiveState(true);
        }
        else
        {
            activePlayer = archerPlayer;
            magicDrawing.SetActiveAnimator(archerAnimator);
            knightScript.SetActiveState(false);
            archerScript.SetActiveState(true);
        }

        // Set new player position and activate
        activePlayer.transform.position = currentPosition;
        activePlayer.SetActive(true);
    }
}
