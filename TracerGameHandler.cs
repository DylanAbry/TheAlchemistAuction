using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TracerGameHandler : MonoBehaviour
{
    [Header("External Scripts")]
    public PotionManager potionScript;
    public GameHandler gameScript;
    public IntroManager introScript;
    public ProcessResults resultsScript;

    [Header("Core References")]
    public Camera mainCamera;
    public TextMeshProUGUI countdownText;        
    public TextMeshProUGUI gameCountdownText;
    public TextMeshProUGUI phalangesWinText;
    public TextMeshProUGUI phalangesLoseText;

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
    public GameObject suddenDeathWinPanel;
    public GameObject suddenDeathLoseOnePanel;
    public GameObject suddenDeathLoseTwoPanel;
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
        if (resultsScript.suddenDeath)
        {
            shuffleSpeed = 0.25f;
        }
        else
        {
            shuffleSpeed = 0.5f;
        }
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
                gameCountdownText.enabled = false;
                if (!hasPlayedLoseTheme)
                {
                    bonusLoseTheme.Play();
                    hasPlayedLoseTheme = true;
                }
                gameScript.bonusGameOutcome = 2;
                if (!resultsScript.suddenDeath)
                {
                    loseTwoPanel.SetActive(true);
                    
                }
                else
                {
                    suddenDeathLoseTwoPanel.SetActive(true);
                }
            }
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
        if (resultsScript.suddenDeath)
        {
            gameTime = 8;
        }
        else
        {
            gameTime = 10;
        }
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
        shuffleCount = Random.Range(12, 17);
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
            if (!resultsScript.suddenDeath)
            {
                winPanel.SetActive(true);
            }
            else
            {
                suddenDeathWinPanel.SetActive(true);
            }
            bonusWinTheme.Play();
            gameScript.bonusGameOutcome = 1;
        }
        else
        {
            if (!resultsScript.suddenDeath)
            {
                loseOnePanel.SetActive(true);
            }
            else
            {
                suddenDeathLoseOnePanel.SetActive(true);
            }
            bonusLoseTheme.Play();
            gameScript.bonusGameOutcome = 2;
        }
    }

    public void ReturnToAuction()
    {
        if (gameScript.bonusGameType == 0)
        {
            winPanel.SetActive(false);
            loseOnePanel.SetActive(false);
            loseTwoPanel.SetActive(false);

            StartCoroutine(BonusGameExit(gameScript.stageMarker.transform.position,
                                         gameScript.stageMarker.transform.rotation,
                                         3.5f));
        }
    }

    private IEnumerator BonusGameExit(Vector3 targetPosition, Quaternion newRotation, float duration)
    {
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
        if (gameScript.bonusGameTheme.isPlaying)
        {
            gameScript.bonusGameTheme.Stop();
            gameScript.mainTheme.Play();

        }
        yield return new WaitForSeconds(0.5f);

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, newRotation, t);
            yield return null;
        }

        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = newRotation;

        gameScript.cashHolder.SetActive(true);
        gameScript.playerCashText.enabled = true;
        gameScript.fleeceCashText.enabled = true;

        yield return new WaitForSeconds(0.2f);

        if (gameScript.bonusGameOutcome == 1)
        {
            introScript.fleeceFaces[0].SetActive(false);
            introScript.fleeceFaces[10].SetActive(true);
            gameScript.defraudAnim.Play("DefraudBonusGameReturn");
            gameScript.itemCover.SetActive(true);
            gameScript.potionAnim.Play("PotionReset");

            phalangesWinText.text = string.Format("A stellar performance of quick wittiness that was! Please take this additional free potion as a prize...");
            gameScript.phalangesGamePanels[4].SetActive(true);
            
        }
        else if (gameScript.bonusGameOutcome == 2)
        {
            introScript.fleeceFaces[0].SetActive(true);
            gameScript.defraudAnim.Play("DefraudBonusGameReturn");
            phalangesLoseText.text = string.Format("What an absolute choke-a-thon that was! Sorry, but as punishment for stinking up the place, you lost your " + potionScript.itemNames[gameScript.potionIndex] + "...");
            gameScript.phalangesGamePanels[5].SetActive(true);
        }
        else
        {

        }

        hasPlayedLoseTheme = false;
    }
}
