using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIImageScroller : MonoBehaviour
{
    public RectTransform[] images;
    public float speed = 100f;
    public bool moveLeft = true;

    float imageWidth;
    float totalSpacingWidth;

    float leftBound;
    float rightBound;
    float offset = 50f;

    void Start()
    {
        float spacing = 370f; 
        float width = images[0].rect.width;
        totalSpacingWidth = spacing + width;

        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        float canvasWidth = canvasRect.rect.width;

        // Wrap just outside the screen by HALF an image
        leftBound = -canvasWidth / 2f - (imageWidth / 2f) - offset;
        rightBound = canvasWidth / 2f + (imageWidth / 2f) + offset;

        for (int i = 0; i < images.Length; i++)
        {
            images[i].anchoredPosition = new Vector2(i * totalSpacingWidth, 0);
        }
    }

    void Update()
    {
        float dir = moveLeft ? -1 : 1;
        float move = speed * Time.deltaTime * dir;

        foreach (RectTransform img in images)
        {
            img.anchoredPosition += new Vector2(move, 0);

            // Moving LEFT
            if (moveLeft && img.anchoredPosition.x <= leftBound)
            {
                img.anchoredPosition += new Vector2(totalSpacingWidth * images.Length, 0);
            }

            // Moving RIGHT
            if (!moveLeft && img.anchoredPosition.x >= rightBound)
            {
                img.anchoredPosition -= new Vector2(totalSpacingWidth * images.Length, 0);
            }
        }
    }

    float GetMaxX()
    {
        float max = images[0].anchoredPosition.x;
        foreach (var img in images)
            if (img.anchoredPosition.x > max) max = img.anchoredPosition.x;
        return max;
    }

    float GetMinX()
    {
        float min = images[0].anchoredPosition.x;
        foreach (var img in images)
            if (img.anchoredPosition.x < min) min = img.anchoredPosition.x;
        return min;
    }

}
