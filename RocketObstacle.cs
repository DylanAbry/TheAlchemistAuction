using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketObstacle : MonoBehaviour
{
    public RectTransform canvasRect;
    public float speed = 350f;

    private RectTransform rect;
    private bool fromLeft;

    void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (canvasRect == null)
        {
            Canvas c = FindObjectOfType<Canvas>();
            if (c != null)
                canvasRect = c.GetComponent<RectTransform>();
        }
    }

    // Init is called by the spawner
    public void Init(bool spawnFromLeft, RectTransform canvas)
    {
        fromLeft = spawnFromLeft;
        canvasRect = canvas;

        // Flip if coming from right
        if (!fromLeft)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // make sure flipped correctly
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // ensure correct facing left->right
            transform.localScale = scale;
        }
    }

    void Update()
    {
        Vector2 dir = fromLeft ? Vector2.right : Vector2.left;
        rect.anchoredPosition += dir * speed * Time.deltaTime;

        // Cleanup after fully leaving screen
        if (Mathf.Abs(rect.anchoredPosition.x) > canvasRect.rect.width / 2f + 600f)
            Destroy(gameObject);
    }
}
