using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWaterScroll : MonoBehaviour
{
    public float speed = 0.1f;
    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
    }

    void Update()
    {
        Rect uv = rawImage.uvRect;
        float offset = Time.time * speed;

        uv.x = offset;
        uv.y = offset;

        rawImage.uvRect = uv;
    }
}
