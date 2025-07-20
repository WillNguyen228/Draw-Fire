using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueEditor;

public class Level1IntroDialogue : MonoBehaviour
{
    public NPCConversation conversation; // Assign your NPCConversation component here

    void Start()
    {
        // Freeze game time
        Time.timeScale = 0f;

        if (conversation != null)
        {
            ConversationManager.Instance.StartConversation(conversation);
            // ConversationManager.OnConversationEnded += OnConversationEnded;
        }
        else
        {
            Debug.LogError("No conversation assigned!");
        }
    }

    public void OnDialogueComplete()
    {
        // Unsubscribe the event to avoid duplicate calls
        // ConversationManager.OnConversationEnded -= OnDialogueComplete;

        // Resume time
        Time.timeScale = 1f;

        // Go to the fight scene
        SceneManager.LoadScene("Level 1_Fight");
    }
}
