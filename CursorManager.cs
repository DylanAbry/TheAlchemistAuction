using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public RawImage cursorImage;
    public RawImage evaderCursorImage;
    public DodgerGameHandler evaderScript;
    public GameHandler gameScript;
    public ProcessResults resultsScript;

    public RectTransform cursor;
    public Canvas canvas;
    public Rect cursorRect;

    public float scale = 2.5f;
    public float rotationSpeed = 0f;

    public Vector2 normalCursorOffset = new Vector2(2f, -22f);
    public Vector2 evaderGameCursorOffset = new Vector2(0f, 0f);

    private Vector2 currentOffset;
    private RawImage currentCursor;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        cursorImage.rectTransform.localScale = Vector3.one * scale;
        evaderCursorImage.rectTransform.localScale = Vector3.one * scale;

        // Set the initial cursor to be the normal one
        currentCursor = cursorImage;
        currentOffset = normalCursorOffset;
        cursorImage.gameObject.SetActive(true);
        evaderCursorImage.gameObject.SetActive(false);
    }

    void Update()
    {
        // Use the active cursor and move it based on the current offset
        Vector2 targetPos = (Vector2)Input.mousePosition + currentOffset;
        currentCursor.rectTransform.position = targetPos;

        // Handle rotation (optional)
        if (rotationSpeed != 0f)
        {
            currentCursor.rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }

        if (evaderScript.activateHeart)
        {
            currentCursor = evaderCursorImage;
            evaderCursorImage.gameObject.SetActive(true);
            cursorImage.gameObject.SetActive(false);
            currentOffset = evaderGameCursorOffset;
        }
        else
        {
            // Revert back to the normal cursor and set its offset
            currentCursor = cursorImage;
            cursorImage.gameObject.SetActive(true);
            evaderCursorImage.gameObject.SetActive(false);
            currentOffset = normalCursorOffset;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!evaderScript.gameActive) return;

        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Cursor hit: " + other.name);
            Destroy(other.gameObject);
            evaderScript.gameActive = false;
            evaderScript.activateHeart = false;
            if (!resultsScript.suddenDeath)
            {
                evaderScript.losePanel.SetActive(true);
            }
            else
            {
                evaderScript.suddenDeathLosePanel.SetActive(true);
            }
            evaderScript.gameCountdown.enabled = false;
            evaderScript.bonusLoseTheme.Play();
            gameScript.bonusGameOutcome = 2;
        }
    }
}
