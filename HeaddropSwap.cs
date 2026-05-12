using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaddropSwap : MonoBehaviour
{

    public RectTransform canvasRect;
    public GameObject firstImage;
    public GameObject secondImage;

    public AudioSource sound;

    public float speed = 300f;
    public float speedAfterSwap = 600f;

    private bool swapped = false;
    private RectTransform rect;
    void Awake()
    {
        if (canvasRect == null)
        {
            Canvas c = FindObjectOfType<Canvas>();
            if (c != null)
                canvasRect = c.GetComponent<RectTransform>();
        }
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();

        // Spawn just above the top
        float spawnX = Random.Range(-canvasRect.rect.width / 2f + 50f, canvasRect.rect.width / 2f - 50f);
        float spawnY = canvasRect.rect.height / 2f + 150f;
        rect.anchoredPosition = new Vector2(spawnX, spawnY);

        firstImage.SetActive(true);
        secondImage.SetActive(false);
    }

    void Update()
    {
        float currentSpeed = swapped ? speedAfterSwap : speed;
        rect.anchoredPosition += Vector2.down * currentSpeed * Time.deltaTime;

        float halfway = 0f; // halfway = y = 0 (center of screen)
        if (!swapped && rect.anchoredPosition.y <= halfway)
        {
            firstImage.SetActive(false);
            secondImage.SetActive(true);
            swapped = true;
            sound.Play();
        }

        // Clean up below bottom
        if (rect.anchoredPosition.y < -canvasRect.rect.height / 2f - 200f)
            Destroy(gameObject);
    }
}
