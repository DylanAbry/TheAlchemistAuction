using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CompletionistTracerGame : MonoBehaviour
{
    [Header("Core References")]
    public Camera mainCamera;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameCountdownText;
    

    [Header("Game State")]
    public bool gameActive;
    public bool countdownDone;
    public bool started;
    public int countdownVal = 3;
    public int gameTime = 10;

    [Header("Game Objects")]
    public GameObject loretab;
    public GameObject[] lethals;
    public GameObject[] interns;
    public GameObject[] startMarkers;
    public GameObject[] playMarkers;
    public GameObject[] tabStartMarkers;

    [Header("UI Panels")]
    public GameObject winPanel;
    public GameObject loseOnePanel;
    public GameObject loseTwoPanel;
    public GameObject tracerGameDescription;

    [Header("Shuffle Settings")]
    public List<Transform> goblets;
    public int shuffleCount = 10;
    public float shuffleSpeed = 0.5f;

    private bool isShuffling = false;
    private float startTimer;
    private float gameTimeTrack;

    [Header("Movement Settings")]
    public Vector3 moveOffset;
    public float moveDuration = 1f;

    public CompletionistInteraction compScript;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;
    public AudioSource whistle;
    bool hasPlayedLoseTheme;

    void Start()
    {

        startTimer = 1f;
        gameTimeTrack = 1f;

        loretab.SetActive(false);
        foreach (GameObject lethal in lethals)
        {
            lethal.SetActive(false);
        }

        for (int i = 0; i < interns.Length; i++)
        {
            interns[i].transform.position = startMarkers[i].transform.position;
        }
        foreach (GameObject intern in interns)
        {
            intern.GetComponent<Button>().enabled = false;
        }
        loretab.transform.position = tabStartMarkers[3].transform.position;
        for (int i = 0; i < lethals.Length; i++)
        {
            lethals[i].transform.position = tabStartMarkers[i].transform.position;
        }

        hasPlayedLoseTheme = false;
    }

    void Update()
    {
        if (started)
        {
            countdownText.enabled = true;
            if (countdownVal > 0)
            {
                startTimer -= Time.deltaTime;
                if (startTimer <= 0f)
                {
                    countdownVal--;
                    countdownText.text = countdownVal.ToString();
                    startTimer = 1f;
                }
            }
            else if (started)
            {
                countdownDone = true;
                started = false;
                StartCoroutine(StartGame());
            }
        }

        if (gameActive)
        {
            gameCountdownText.enabled = true;

            if (gameTime > 0)
            {
                gameTimeTrack -= Time.deltaTime;
                if (gameTimeTrack <= 0f)
                {
                    gameTime--;
                    gameCountdownText.text = gameTime.ToString();
                    gameTimeTrack = 1f;
                }
            }
            else
            {
                gameActive = false;
                if (!hasPlayedLoseTheme)
                {
                    bonusLoseTheme.Play();
                    hasPlayedLoseTheme = true;
                }
                
                loseTwoPanel.SetActive(true);
                gameCountdownText.enabled = false;              
            }
        }

        if ((PlayerPrefs.GetInt("NormalTracerComplete", 0) == 1) && (PlayerPrefs.GetInt("ExpertTracerComplete", 0) == 1) && (PlayerPrefs.GetInt("EpicGamerTracerComplete", 0) == 1))
        {
            PlayerPrefs.SetInt("TracerModesComplete", 1);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator StartGame()
    {
        whistle.Play();
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.enabled = false;

        foreach (GameObject intern in interns)
        {
            intern.GetComponent<Button>().enabled = true;
        }
        gameActive = true;
        gameCountdownText.text = gameTime.ToString();
    }

    public void StartShuffle()
    {
        countdownVal = 3;
        countdownText.text = countdownVal.ToString();
        tracerGameDescription.SetActive(false);
        loretab.SetActive(true);
        foreach (GameObject lethal in lethals)
        {
            lethal.SetActive(true);
        }
        if (!isShuffling) StartCoroutine(ShuffleRoutine());
    }

    private IEnumerator ShuffleRoutine()
    {
        isShuffling = true;

        // Move interns from start to play markers
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < interns.Length; i++)
        {
            yield return MoveUIElement(interns[i].GetComponent<RectTransform>(),
                                       startMarkers[i].transform.position,
                                       playMarkers[i].transform.position,
                                       2f);
        }

        // Shuffle goblets
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < shuffleCount; i++)
        {
            int a = Random.Range(0, goblets.Count);
            int b = Random.Range(0, goblets.Count);
            while (b == a) b = Random.Range(0, goblets.Count);

            yield return SwapGoblets(goblets[a], goblets[b], shuffleSpeed);
        }

        yield return new WaitForSeconds(2f);
        started = true;
        isShuffling = false;
    }

    private IEnumerator SwapGoblets(Transform gobletA, Transform gobletB, float duration)
    {
        Vector3 startPosA = gobletA.position;
        Vector3 startPosB = gobletB.position;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            Vector3 arc = Vector3.up * Mathf.Sin(t * Mathf.PI) * 0.5f;

            gobletA.position = Vector3.Lerp(startPosA, startPosB, t) + arc;
            gobletB.position = Vector3.Lerp(startPosB, startPosA, t) + arc;

            yield return null;
        }

        gobletA.position = startPosB;
        gobletB.position = startPosA;
    }

    private IEnumerator MoveUIElement(RectTransform rect, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rect.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }
        rect.position = endPos;
    }

    public void LoretabFound(GameObject clickedGoblet)
    {
        StartCoroutine(HandleFound(clickedGoblet, true));
    }

    public void LethalFound(GameObject clickedGoblet)
    {
        StartCoroutine(HandleFound(clickedGoblet, false));
    }

    private IEnumerator HandleFound(GameObject clickedGoblet, bool isLoretab)
    {
        gameCountdownText.enabled = false;
        foreach (GameObject intern in interns)
        {
            intern.GetComponent<Button>().enabled = false;
        }
        gameActive = false;

        // Animate the rest
        for (int i = 0; i < interns.Length; i++)
        {
            if (interns[i] == clickedGoblet) continue;
            if (i == 0) continue;
            yield return MoveUIElement(interns[i].GetComponent<RectTransform>(),
                                       interns[i].transform.position,
                                       interns[i].transform.position + moveOffset,
                                       moveDuration);
        }

        yield return new WaitForSeconds(0.5f);

        yield return MoveUIElement(interns[0].GetComponent<RectTransform>(),
                                   interns[0].transform.position,
                                   interns[0].transform.position + moveOffset,
                                   moveDuration);




        // Animate the chosen goblet
        yield return MoveUIElement(clickedGoblet.GetComponent<RectTransform>(),
                                   clickedGoblet.transform.position,
                                   clickedGoblet.transform.position + moveOffset,
                                   moveDuration);

        if (isLoretab)
        {
            winPanel.SetActive(true);
            bonusWinTheme.Play();
            if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.Normal)
            {
                PlayerPrefs.SetInt("NormalTracerComplete", 1);
                PlayerPrefs.Save();
            }
            else if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.Expert)
            {
                PlayerPrefs.SetInt("ExpertTracerComplete", 1);
                PlayerPrefs.Save();
            }
            else if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.EpicGamer)
            {
                PlayerPrefs.SetInt("EpicGamerTracerComplete", 1);
                PlayerPrefs.Save();
            }
            else
            {

            }
        }
        else
        {
            bonusLoseTheme.Play();
            loseOnePanel.SetActive(true);
        }
    }

    public void ReturnToAuction()
    {
        winPanel.SetActive(false);
        loseOnePanel.SetActive(false);
        loseTwoPanel.SetActive(false);
        hasPlayedLoseTheme = false;

        if (compScript.bonusGameTheme.isPlaying) compScript.bonusGameTheme.Stop();
        compScript.completionistTheme.Play();

        foreach (GameObject intern in interns)
        {
            intern.GetComponent<Button>().enabled = false;
        }
        loretab.SetActive(false);
        for (int i = 0; i < goblets.Count; i++)
        {
            goblets[i].transform.position = playMarkers[i].transform.position;
        }
        foreach (GameObject lethal in lethals)
        {
            lethal.SetActive(false);
        }
        loretab.transform.position = tabStartMarkers[3].transform.position;
        for (int i = 0; i < lethals.Length; i++)
        {
            lethals[i].transform.position = tabStartMarkers[i].transform.position;
        }
        for (int i = 0; i < interns.Length; i++)
        {
            interns[i].transform.position = startMarkers[i].transform.position;
        }
    }
}
