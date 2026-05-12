using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSideMovement : MonoBehaviour
{
    public bool fromLeft;
    RectTransform rect;
    float speed = 200f; // adjust pixels per second

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        float dir = fromLeft ? 1 : -1;
        rect.anchoredPosition += new Vector2(dir * speed * Time.deltaTime, 0);

 
        if (rect.anchoredPosition.x < -100 || rect.anchoredPosition.x > rect.parent.GetComponent<RectTransform>().rect.width + 100)
        {
            gameObject.SetActive(false);
        }           
    }
}
