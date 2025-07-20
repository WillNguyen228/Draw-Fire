using UnityEngine;
using DialogueEditor;
using UnityEngine.SceneManagement;

public class WizerdTalk : MonoBehaviour
{
    public NPCConversation conversation;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConversationManager.Instance.StartConversation(conversation);
        }
    }

    // THIS will be called from the Dialogue Editor
    public void LoadFightScene()
    {
        SceneManager.LoadScene("Level 3_Fight");
    }
}
