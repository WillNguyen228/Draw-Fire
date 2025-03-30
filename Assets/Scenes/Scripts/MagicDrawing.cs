using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDrawing : MonoBehaviour
{
    public GameObject linePrefab;  // Assign a LineRenderer prefab
    private LineRenderer currentLine;
    private List<Vector3> points = new List<Vector3>();
    public List<enemy> enemies;  // Reference to multiple enemy scripts
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start drawing
        {
            CreateNewLine();
        }
        if (Input.GetMouseButton(0)) // Continue drawing
        {
            Vector3 mousePos = GetMouseWorldPosition();
            if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], mousePos) > 0.1f)
            {
                points.Add(mousePos);
                currentLine.positionCount = points.Count;
                currentLine.SetPosition(points.Count - 1, mousePos);
            }
        }
        if (Input.GetMouseButtonUp(0)) // Detect line direction when finished
        {
            Color newColor = ChangeLineColor();
            // Loop through all enemies and remove matching lines
            foreach (enemy currentEnemy in enemies)
            {
                if (currentEnemy != null)  // Ensure the enemy is not destroyed
                {
                    currentEnemy.RemoveMatchingLine(newColor);  // Check & remove matching enemy line
                }  
            }

            // Check if the boss is involved in the process
            boss bossEnemy = FindObjectOfType<boss>();  // Find the boss (adjust as needed for your setup)
            if (bossEnemy != null)
            {
                Debug.Log("Boss found, removing matching line...");
                bossEnemy.RemoveMatchingLine(newColor);  // Apply logic specific to boss
            }
            else
            {
                Debug.LogWarning("Boss not found!");
            }
        }
    }

    void CreateNewLine()
    {
        GameObject newLine = Instantiate(linePrefab);
        currentLine = newLine.GetComponent<LineRenderer>();
        points.Clear();
        StartCoroutine(FadeAndDestroy(newLine, 2f));  // Start fade-out coroutine
    }

    Color ChangeLineColor()
    {
        if (currentLine == null || points.Count < 2) return Color.white; // Not enough points to determine direction

        float deltaX = Mathf.Abs(points[points.Count - 1].x - points[0].x);
        float deltaY = Mathf.Abs(points[points.Count - 1].y - points[0].y);

        // Find the highest and lowest points in the drawing
        float highestY = points[0].y;
        float lowestY = points[0].y;
        int highestIndex = 0, lowestIndex = 0;

        for (int i = 1; i < points.Count; i++)
        {
            if (points[i].y > highestY)
            {
                highestY = points[i].y;
                highestIndex = i;
            }
            if (points[i].y < lowestY)
            {
                lowestY = points[i].y;
                lowestIndex = i;
            }
        }

        // Define a threshold to avoid false peak/dip detections
        float heightThreshold = Mathf.Abs(points[points.Count - 1].y - points[0].y) * 0.5f; // 50% of start-end difference

        Color newColor;

        if (DetectHeartShape()) // Detect a heart shape: two peaks and a dip in the middle
        {
            newColor = Color.magenta; // Heart -> Magenta 
        }
        // Check if it forms a peak ( ∧ ) or a dip ( ∨ )
        else if (highestY - points[0].y > heightThreshold && highestY - points[points.Count - 1].y > heightThreshold) 
        {
            newColor = Color.yellow;  // Peak ( ^ ) -> Yellow
        }
        else if (points[0].y - lowestY > heightThreshold && points[points.Count - 1].y - lowestY > heightThreshold) 
        {
            newColor = Color.green;  // Dip ( v ) -> Green
        }
        else 
        {
            // Default to horizontal/vertical check
            // More horizontal, make blue
            // More vertical, make red
            newColor = (deltaX > deltaY) ? Color.blue : Color.red;
        }

        // Apply the color
        currentLine.startColor = newColor;
        currentLine.endColor = newColor;

        return newColor;
    }

    bool DetectHeartShape()
    {
        // Find first and second peaks ( ∧ ∧ )
        List<int> peakIndices = new List<int>();
        List<int> dipIndices = new List<int>();

        for (int i = 1; i < points.Count - 1; i++)
        {
            if (points[i].y > points[i - 1].y && points[i].y > points[i + 1].y)
            {
                peakIndices.Add(i); // Peak condition: higher than both neighbors
            }
            if (points[i].y < points[i - 1].y && points[i].y < points[i + 1].y)
            {
                dipIndices.Add(i);  // Dip condition: lower than both neighbors
            }
        }

        // We need exactly **two peaks** and **one dip** in the middle for a heart shape
        if (peakIndices.Count == 2 && dipIndices.Count == 1)
        {
            int firstPeak = peakIndices[0];
            int secondPeak = peakIndices[1];
            int dip = dipIndices[0];

            // Loosen the symmetry condition: allow a small offset between the peaks
            bool peaksAreBalanced = Mathf.Abs(points[firstPeak].x - points[secondPeak].x) < 30.0f;  // Loosen symmetry check

            // Allow the dip to be within a certain range between the peaks
            bool dipIsNearCenter = Mathf.Abs((points[firstPeak].x + points[secondPeak].x) / 2 - points[dip].x) < 20.0f; // X-axis distance tolerance

            // Allow the dip to have some curvature at the bottom of the heart shape
            bool dipHasSufficientDepth = Mathf.Abs(points[firstPeak].y - points[dip].y) > 0.3f && Mathf.Abs(points[secondPeak].y - points[dip].y) > 0.3f;

            return peaksAreBalanced || dipIsNearCenter || dipHasSufficientDepth;
        }

        return false;
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // Set distance from the camera
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    IEnumerator FadeAndDestroy(GameObject line, float fadeTime)
    {
        LineRenderer lr = line.GetComponent<LineRenderer>();
        float timer = 0;

        while (timer < fadeTime)
        {
            // Interpolate the alpha value from fully visible (1) to fully transparent (0)
            float alpha = Mathf.Lerp(1, 0, timer / fadeTime);

            // Apply the fading effect by changing the alpha of the line's color
            Color fadedColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.startColor = fadedColor;
            lr.endColor = fadedColor;

            // Increment the timer based on the frame's time
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(line);
    }
}
