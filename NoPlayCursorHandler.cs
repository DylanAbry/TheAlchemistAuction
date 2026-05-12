using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoPlayCursorHandler : MonoBehaviour
{
    public RawImage cursorImage;
    public RawImage evaderCursorImage;
    public CompletionistEvaderGame evaderScript;

    Scene currentScene;

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
        currentScene = SceneManager.GetActiveScene();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        cursorImage.rectTransform.localScale = Vector3.one * scale;

        if (currentScene.name == "CompletionistCave")
        {
            evaderCursorImage.rectTransform.localScale = Vector3.one * scale;
            cursorImage.gameObject.SetActive(false);
        }
        
        // Set the initial cursor to be the normal one
        currentCursor = cursorImage;
        currentOffset = normalCursorOffset;   
    }

    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "CompletionistCave")
        {
            Debug.Log("ActivateHeart: " + evaderScript.activateHeart);

            if (evaderScript.activateHeart)
            {
                currentCursor = evaderCursorImage;
                evaderCursorImage.gameObject.SetActive(true);
                cursorImage.gameObject.SetActive(false);
                currentOffset = evaderGameCursorOffset;

            }
            else
            {
                currentCursor = cursorImage;
                evaderCursorImage.gameObject.SetActive(false);
                currentOffset = normalCursorOffset;
            }
        }

        Vector2 targetPos = (Vector2)Input.mousePosition + currentOffset;
        currentCursor.rectTransform.position = targetPos;

        if (rotationSpeed != 0f)
        {
            currentCursor.rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (currentScene.name == "CompletionistCave")
        {
            if (!evaderScript.gameActive) return;

            if (other.CompareTag("Obstacle"))
            {
                Debug.Log("Cursor hit: " + other.name);
                Destroy(other.gameObject);
                evaderScript.gameActive = false;
                evaderScript.activateHeart = false;
                evaderScript.losePanel.SetActive(true);
                evaderScript.bonusLoseTheme.Play();
                cursorImage.gameObject.SetActive(true);
                evaderScript.gameCountdown.enabled = false;               
            }
        }
    }
}
