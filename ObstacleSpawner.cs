using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject[] headdropPrefabs;
    public ProcessResults resultsScript;
    public GameObject splitPrefab;
    public GameObject sideSlidePrefab;

    Scene currentScene;

    [Header("Spawn Intervals (seconds)")]
    public float headdropInterval = 2f;
    public float splitInterval = 4f;
    public float sideSlideInterval = 5f;

    [Header("Spawn Chances (0 to 1)")]
    public float headdropChance = 1f;   // 1 = always spawn when interval is up
    public float splitChance = 0.7f;    // 70% chance each interval
    public float sideSlideChance = 0.5f;

    [Header("References")]
    public RectTransform canvasRect;  // assign your main canvas here

    private float headTimer, splitTimer, slideTimer;

    private List<GameObject> activeObstacles = new List<GameObject>();

    void Update()
    {
        headTimer -= Time.deltaTime;
        splitTimer -= Time.deltaTime;
        slideTimer -= Time.deltaTime;
        currentScene = SceneManager.GetActiveScene();

        if (headTimer <= 0f)
        {
            if (Random.value < headdropChance)
                SpawnHeaddrop();
            headTimer = headdropInterval;
        }

        if (splitTimer <= 0f)
        {
            if (Random.value < splitChance)
                SpawnSplit();
            splitTimer = splitInterval;
        }

        if (slideTimer <= 0f)
        {
            if (Random.value < sideSlideChance)
                SpawnSideSlide();
            slideTimer = sideSlideInterval;
        }

        if (currentScene.name == "AlchemistAuctionMainGame")
        {
            if (resultsScript.suddenDeath)
            {
                headdropInterval = 1f;
                splitInterval = 2f;
                sideSlideInterval = 2.5f;
            }
        }
    }

    void SpawnHeaddrop()
    {
        if (headdropPrefabs.Length == 0) return;

        int index = Random.Range(0, headdropPrefabs.Length);
        GameObject prefab = headdropPrefabs[index];

        GameObject obj = Instantiate(prefab, canvasRect);
        RectTransform rt = obj.GetComponent<RectTransform>();

        // Force consistent spawn position regardless of prefab offsets
        float spawnX = Random.Range(
            -canvasRect.rect.width / 2f + 50f,
             canvasRect.rect.width / 2f - 50f
        );
        float spawnY = canvasRect.rect.height / 2f + 150f;

        rt.anchoredPosition = new Vector2(spawnX, spawnY);
        rt.localScale = Vector3.one; // normalize scale in case prefabs differ
        rt.rotation = Quaternion.identity; // reset any rotation

        activeObstacles.Add(obj);
    }

    void SpawnSplit()
    {
        GameObject obj = Instantiate(splitPrefab, canvasRect);
        RectTransform rt = obj.GetComponent<RectTransform>();

        bool fromLeft = Random.value > 0.5f;
        float spawnX = fromLeft ? -canvasRect.rect.width / 2f - 200f : canvasRect.rect.width / 2f + 200f;
        float spawnY = Random.Range(-canvasRect.rect.height / 2f, canvasRect.rect.height / 2f);

        rt.anchoredPosition = new Vector2(spawnX, spawnY);

        // Random slight tilt for variety
        float randomRotation = Random.Range(-20f, 20f);

        FishObstacle fish = obj.GetComponent<FishObstacle>();
        fish.Init(canvasRect, fromLeft, randomRotation);

        activeObstacles.Add(obj);
    }

    void SpawnSideSlide()
    {
        bool fromLeft = Random.value > 0.5f;
        float spawnX = fromLeft ? -canvasRect.rect.width / 2f - 200f : canvasRect.rect.width / 2f + 200f;
        float minY = -canvasRect.rect.height / 2f + 50f;
        float maxY = 0f; 
        float spawnY = Random.Range(minY, maxY);

        GameObject obj = Instantiate(sideSlidePrefab, canvasRect);
        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(spawnX, spawnY);

        RocketObstacle rocket = obj.GetComponent<RocketObstacle>();
        rocket.Init(fromLeft, canvasRect);

        activeObstacles.Add(obj);
    }
    public void ClearObstacles()
    {
        foreach (GameObject obj in activeObstacles)
        {
            if (obj != null) Destroy(obj);
        }
        activeObstacles.Clear();
    }
}
