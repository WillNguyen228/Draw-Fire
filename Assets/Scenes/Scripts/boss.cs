using UnityEngine;
using TMPro;  // Import TextMeshPro namespace
using System.Collections;
using UnityEngine.SceneManagement;

public class boss : MonoBehaviour
{
    public enum LineType { Horizontal, Vertical, VShape, CaretShape, HeartShape }
    public GameObject completeDrawingLevelUI; // Reference to your UI that will show the "Level Complete" message
    public float delayBeforeUI = 2f;   // Delay in seconds before showing the "Level Complete" UI
    public LineType[] enemyLineTypes;  // The line type associated with the enemy
    private TextMeshProUGUI[] lineTypeTexts;  // Reference to the UI Text to display the line type above the enemy
    private Camera mainCamera;  // Reference to the main camera
     private Canvas canvas;  // Reference to the canvas

    void Start()
    {
        // Assign random line types to the enemy (you can populate this array as needed)
        enemyLineTypes = new LineType[15];  // Random length between 1 and 3 for the example

        // Populate the enemyLineTypes array with random values
        for (int i = 0; i < enemyLineTypes.Length - 1; i++)
        {
            enemyLineTypes[i] = (LineType)Random.Range(0, 4);  // Assign random line types
        }

        // Append HeartShape at the last index
        enemyLineTypes[enemyLineTypes.Length - 1] = LineType.HeartShape;

        // Get the reference to the main camera
        mainCamera = Camera.main;

        // Find the canvas
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        // Initialize the array of TextMeshProUGUI objects
        lineTypeTexts = new TextMeshProUGUI[enemyLineTypes.Length];

        // Create TextMeshPro objects for each line type in the array
        for (int i = 0; i < enemyLineTypes.Length; i++)
        {
            GameObject lineTextObject = new GameObject("LineTypeText_" + i);
            lineTextObject.transform.SetParent(canvas.transform);

            lineTypeTexts[i] = lineTextObject.AddComponent<TextMeshProUGUI>();

            // Set TMP font asset (make sure to assign a TMP font in the editor)
            lineTypeTexts[i].fontSize = 1;
            lineTypeTexts[i].color = Color.white;
            lineTypeTexts[i].alignment = TextAlignmentOptions.Center;  // Align the text to the center

            // Set the line type text
            UpdateLineTypeText(i);
        }
    }

    void UpdateLineTypeText(int index)
    {
        switch (enemyLineTypes[index])
        {
            case LineType.Horizontal:
                lineTypeTexts[index].text = "<color=blue>-</color>";  // Horizontal line symbol
                break;
            case LineType.Vertical:
                lineTypeTexts[index].text = "<color=red>|</color>";  // Vertical line symbol
                break;
            case LineType.VShape:
                lineTypeTexts[index].text = "<color=green>V</color>";  // V shape symbol
                break;
            case LineType.CaretShape:
                lineTypeTexts[index].text = "<color=yellow>^</color>";  // ^ shape symbol
                break;
            case LineType.HeartShape:
                lineTypeTexts[index].text = "♥"; // Unicode heart symbol
                break;
        }
    }

    void Update()
    {
        // Update the position of the line type text to follow the enemy
        SetLineTextPosition();
    }

    // Function to set the position of the text above the enemy
    void SetLineTextPosition()
    {
        float offset = (lineTypeTexts.Length - 1) * 0.5f;  // Centering offset
        // Loop through each TextMeshPro object and position them above the enemy
        for (int i = 0; i < lineTypeTexts.Length; i++)
        {
            // Convert the world position of the enemy to screen position
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + new Vector3((i - offset) * 1.0f, 2.0f, 0));

            // Convert the screen position to local canvas position
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(lineTypeTexts[i].rectTransform.parent.GetComponent<RectTransform>(), screenPosition, mainCamera, out localPosition);

            // Set the position of the text in the canvas (UI space)
            lineTypeTexts[i].rectTransform.localPosition = localPosition;

            // Adjust font size to better fit the screen size (optional)
            // float screenWidth = Screen.width;
            // lineTypeText.fontSize = Mathf.RoundToInt(screenWidth / 50f);  // Dynamic size based on screen width
        }
    }

    public void RemoveMatchingLine(Color drawnColor)
    {
        LineType matchingType = LineType.Horizontal; // Default value to prevent unassigned error

        // Determine the corresponding line type based on color
        if (drawnColor == Color.blue)
            matchingType = LineType.Horizontal;
        else if (drawnColor == Color.red)
            matchingType = LineType.Vertical;
        else if (drawnColor == Color.green)
            matchingType = LineType.VShape;
        else if (drawnColor == Color.yellow)
            matchingType = LineType.CaretShape;
        else if (drawnColor == Color.magenta) 
            matchingType = LineType.HeartShape;

        // Find and remove the first matching line type
        for (int i = 0; i < enemyLineTypes.Length; i++)
        {
            if (enemyLineTypes[i] == matchingType)
            {
                // Remove the matched line type by shifting elements left
                for (int j = i; j < enemyLineTypes.Length - 1; j++)
                {
                    enemyLineTypes[j] = enemyLineTypes[j + 1];
                }
                
                // Resize the array
                System.Array.Resize(ref enemyLineTypes, enemyLineTypes.Length - 1);
                
                // Update UI
                Destroy(lineTypeTexts[i].gameObject);
                lineTypeTexts[i] = null;
                
                // Shift UI elements
                for (int j = i; j < lineTypeTexts.Length - 1; j++)
                {
                    lineTypeTexts[j] = lineTypeTexts[j + 1];
                }
                System.Array.Resize(ref lineTypeTexts, lineTypeTexts.Length - 1);
                
                break; // Exit after removing the first match
            }
        }
        // If there are no more line types associated with the enemy, destroy the enemy object
        if (enemyLineTypes.Length == 0)
        {
            if (this.CompareTag("Boss"))
            {
                Debug.Log("Boss defeated!");
                // Apply special boss defeat behavior here
            }
            // Start the coroutine to delay showing the "Level Complete" UI and destroy the enemy
            // StartCoroutine(HandleLevelComplete());
            Debug.Log("Destroying boss...");
            Destroy(gameObject);

            // Show the level complete UI and move to the next level
            Debug.Log("Level Complete UI will now appear");

            FindObjectOfType<GameManager>().CompleteLevel();
        }
    }
    
    // Coroutine to wait before showing UI and destroying the enemy
    // private IEnumerator HandleLevelComplete()
    // {
    //     // Destroy the enemy object
    //     Debug.Log("Destroying boss...");
    //     Destroy(gameObject);

    //     // Wait for the specified time before proceeding
    //      Debug.Log($"Waiting for {delayBeforeUI} seconds before showing level complete UI...");
    //     yield return new WaitForSeconds(delayBeforeUI);

    //     // Show the level complete UI and move to the next level
    //     Debug.Log("Level Complete UI will now appear");
    //     completeDrawingLevelUI.SetActive(true);

    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    // }
}
