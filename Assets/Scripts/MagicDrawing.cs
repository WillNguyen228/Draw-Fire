using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicDrawing : MonoBehaviour
{
    // Make it so that they player have to draw a spiral to start the game
    public GameObject linePrefab;  // Assign a LineRenderer prefab
    private LineRenderer currentLine;
    private List<Vector3> points = new List<Vector3>();
    public List<enemy> enemies;  // Reference to multiple enemy scripts
    public Animator archerAnimator;
    public Animator knightAnimator;
    public PlayerSwitcher playerSwitcher;
    private Animator currentAnimator;
    private bool isHoldingBow = false; // New: tracks if bow is being held
    void Start()
    {
        currentAnimator = archerAnimator; // or knightAnimator if that’s your default
    }
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
        // New: If holding bow and click LMB again, shoot
        if (isHoldingBow && Input.GetMouseButtonDown(0))
        {
            currentAnimator.SetTrigger("BowShoot");
            isHoldingBow = false;
        }
    }
    public void SetActiveAnimator(Animator animator)
    {
        currentAnimator = animator;
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
        //if (currentLine == null || points.Count < 2) return Color.white; // Not enough points to determine direction

        float length = CalculateLineLength();
        if (length < 2f)  // Example: ignore short strokes
        {
            Debug.Log("Drawing too short to be valid. Length: " + length);
            return new Color(0.5f, 0.5f, 0.5f); // Gray for invalid shape
        }

        float deltaX = Mathf.Abs(points[points.Count - 1].x - points[0].x);
        float deltaY = Mathf.Abs(points[points.Count - 1].y - points[0].y);

        Color newColor;

        if (DetectHeartShape()) // Detect a heart shape: two peaks and a dip in the middle
        {
            newColor = Color.magenta; // Heart -> Magenta 
        }
        else if (DetectCircleShape())
        {
            newColor = Color.cyan; // Circle -> Cyan
        }
        // Check if it forms a peak ( ∧ ) or a dip ( ∨ )
        else if (HasStrongPeak())
        {
            newColor = Color.yellow;  // Peak ( ^ ) -> Yellow
        }
        else if (HasStrongDip())
        {
            newColor = Color.blue;  // Dip ( v ) -> Blue
            // 🔁 Trigger player switch here
            if (playerSwitcher != null)
            {
                playerSwitcher.SwitchPlayers(); // Switch characters
            }
        }
        else if (IsStraightEnough())
        {
            // Default to horizontal/vertical check
            // More horizontal, make blue
            // More vertical, make red
            newColor = (deltaX > deltaY) ? Color.green : Color.red;
        }
        else
        {
            newColor = new Color(0.5f, 0.5f, 0.5f); // fallback color
        }

        // Apply the color
        currentLine.startColor = newColor;
        currentLine.endColor = newColor;

        // Trigger sword animation if red (vertical swipe)
        if (newColor == Color.red && currentAnimator != null)
        {
            currentAnimator.SetTrigger("Attack");
        }
        //Trigger charge animation if blue
        if (newColor == Color.green && currentAnimator != null)
        {
            currentAnimator.SetTrigger("BowCharge");
            isHoldingBow = true;
        }
        // Trigger AOE attack when circle is drawn
        if (newColor == Color.cyan && currentAnimator != null)
        {
            currentAnimator.SetTrigger("AOEAttack");  // Circle shape triggers AOE
        }
        // Trigger healing action when circle is drawn
        if (newColor == Color.magenta && currentAnimator != null)
        {
            currentAnimator.SetTrigger("Heal");  // Heart shape triggers Heal
        }

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

    bool DetectCircleShape()
    {
        if (points.Count < 10) return false; // Too few points for a circle

        if (Vector3.Distance(points[0], points[points.Count - 1]) > 0.5f)
            return false; // Not a closed shape

        // 1. Calculate center of mass (average point)
        Vector3 center = Vector3.zero;
        foreach (var p in points)
            center += p;
        center /= points.Count;

        // 2. Calculate average radius
        float avgRadius = 0f;
        foreach (var p in points)
            avgRadius += Vector3.Distance(p, center);
        avgRadius /= points.Count;

        // 3. Check how close each point's distance is to the average
        float variance = 0f;
        foreach (var p in points)
        {
            float dist = Vector3.Distance(p, center);
            variance += Mathf.Abs(dist - avgRadius);
        }

        float avgVariance = variance / points.Count;

        // 4. Accept as circle if variance is small (tweak this value)
        return avgVariance < 0.3f * avgRadius;
    }

    float CalculateLineLength()
    {
        float totalLength = 0f;
        for (int i = 1; i < points.Count; i++)
        {
            totalLength += Vector3.Distance(points[i], points[i - 1]);
        }
        return totalLength;
    }

    bool HasStrongPeak()
    {
        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector2 a = points[i - 1] - points[i];
            Vector2 b = points[i + 1] - points[i];

            float angle = Vector2.Angle(a, b);
            if (angle < 90f) // small angle = sharp corner = peak
            {
                if (a.y > 0 && b.y > 0) // both pointing down
                    return true;
            }
        }
        return false;
    }

    bool HasStrongDip()
    {
        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector2 a = points[i - 1] - points[i];
            Vector2 b = points[i + 1] - points[i];

            float angle = Vector2.Angle(a, b);
            if (angle < 90f) // small angle = sharp corner = dip
            {
                if (a.y < 0 && b.y < 0) // both pointing up
                    return true;
            }
        }
        return false;
    }

    bool IsStraightEnough()
    {
        // Calculate direction from start to end
        Vector2 mainDir = points[points.Count - 1] - points[0];
        mainDir.Normalize();

        // Check that most intermediate points follow the main direction closely
        int alignedPoints = 0;
        for (int i = 1; i < points.Count - 1; i++)
        {
            Vector2 dir = points[i] - points[0];
            dir.Normalize();
            float angle = Vector2.Angle(dir, mainDir);
            if (angle < 15f)
                alignedPoints++;
        }

        float ratio = (float)alignedPoints / (points.Count - 2);
        return ratio > 0.8f; // at least 80% of points must align
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
