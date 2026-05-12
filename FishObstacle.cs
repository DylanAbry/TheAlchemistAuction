using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishObstacle : MonoBehaviour
{
    public RectTransform canvasRect;

    public GameObject fullFish;
    public GameObject leftHalf;
    public GameObject rightHalf;

    public float moveSpeed = 700f;
    public float driftSpeed = 500f;
    public float splitDelay = 0.5f;
    public float wobbleAmount = 50f; // pixels
    public float wobbleSpeed = 10f;   // oscillations/sec
    public AudioSource pop;
    private RectTransform rect;
    private bool movingToCenter = true;
    private bool splitting = false;
    private bool fromLeft;
    private float startTime;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        fullFish.SetActive(true);
        leftHalf.SetActive(false);
        rightHalf.SetActive(false);
        startTime = Time.time;
    }

    public void Init(RectTransform canvas, bool spawnFromLeft, float rotationZ)
    {
        canvasRect = canvas;
        fromLeft = spawnFromLeft;

        // Apply random rotation
        rect.localRotation = Quaternion.Euler(0, 0, rotationZ);

        // Flip if from right
        Vector3 scale = transform.localScale;
        scale.x = spawnFromLeft ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void Update()
    {
        float elapsed = Time.time - startTime;
        float wobble = Mathf.Sin(elapsed * wobbleSpeed) * wobbleAmount;

        if (movingToCenter)
        {
            // Move horizontally toward center
            Vector2 target = Vector2.zero; // center of screen
            rect.anchoredPosition = Vector2.MoveTowards(
                rect.anchoredPosition,
                target,
                moveSpeed * Time.deltaTime
            );

            // Apply vertical wobble WHILE swimming in
            rect.anchoredPosition += Vector2.up * wobble * Time.deltaTime;

            if (Vector2.Distance(rect.anchoredPosition, target) < 10f && !splitting)
            {
                StartCoroutine(SplitFish());
            }
        }
        else if (splitting)
        {
            // Drift halves apart with wobble
            RectTransform leftRect = leftHalf.GetComponent<RectTransform>();
            RectTransform rightRect = rightHalf.GetComponent<RectTransform>();

            leftRect.anchoredPosition += (Vector2.left * driftSpeed + Vector2.up * wobble) * Time.deltaTime;
            rightRect.anchoredPosition += (Vector2.right * driftSpeed - Vector2.up * wobble) * Time.deltaTime;

            // Cleanup if far outside
            if (Mathf.Abs(leftRect.anchoredPosition.x) > canvasRect.rect.width / 2f + 400f)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator SplitFish()
    {
        movingToCenter = false;
        yield return new WaitForSeconds(splitDelay);

        fullFish.SetActive(false);
        leftHalf.SetActive(true);
        rightHalf.SetActive(true);
        pop.Play();
        splitting = true;
        startTime = Time.time; // reset wobble timer for halves
    }
}
