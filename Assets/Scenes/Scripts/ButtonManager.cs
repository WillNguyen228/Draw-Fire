using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour
{
    public List<Button> buttons; // Assign buttons in Inspector
    private HashSet<Button> pressedButtons = new HashSet<Button>(); // Track pressed buttons

    public GameObject completeLevelUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonPressed(button)); // Add listener
        }
    }

     void OnButtonPressed(Button button)
    {
        if (!pressedButtons.Contains(button))
        {
            pressedButtons.Add(button); // Mark button as pressed
        }

        if (pressedButtons.Count == buttons.Count) // Check if all buttons are pressed
        {
            FindObjectOfType<CursorController>().DisableCursor();
            Invoke(nameof(ShowCompleteLevelUI), 1f);
            Invoke(nameof(LoadNextLevel), 3f); // Delay transition for effect
        }
    }

    void ShowCompleteLevelUI()
    {
        completeLevelUI.SetActive(true);
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
