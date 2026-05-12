using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFall : MonoBehaviour
{
    public float speed = 300f;
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition -= Vector2.up * speed * Time.deltaTime;

        // destroy once off screen
        if (rect.anchoredPosition.y < -Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
