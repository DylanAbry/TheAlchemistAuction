using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DodgerGameHandler : MonoBehaviour
{
    public ObstacleSpawner obstacleScript;
    public IntroManager introScript;
    public PotionManager potionScript;
    public ProcessResults resultsScript;
    public Camera mainCamera;
    public GameHandler gameScript;
    public GameObject evaderGameDescription;

    public bool gameActive;
    public bool started;
    public bool countdownDone;
    public bool activateHeart;

    private float startTimer;
    private float gameTimeTrack;

    public GameObject winPanel;
    public GameObject losePanel;
    public GameObject suddenDeathWinPanel;
    public GameObject suddenDeathLosePanel;

    public TextMeshProUGUI countdown;
    public TextMeshProUGUI gameCountdown;
    public TextMeshProUGUI phalangesWinText;
    public TextMeshProUGUI phalangesLoseText;

    public int countdownValue;
    public int gameTime;

    public AudioSource bonusLoseTheme;
    public AudioSource bonusWinTheme;
    public AudioSource whistle;
    bool hasPlayedWinTheme;

    // Start is called before the first frame update
    void Start()
    {
        obstacleScript.enabled = false;
        activateHeart = false;
        startTimer = 1f;
        gameTimeTrack = 1f;
        gameCountdown.enabled = false;
        hasPlayedWinTheme = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            countdown.enabled = true;
            if (countdownValue > 0)
            {
                startTimer -= Time.deltaTime;
                if (startTimer <= 0f)
                {
                    countdownValue--;
                    countdown.text = countdownValue.ToString();
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
            gameCountdown.enabled = true;

            if (gameTime > 0)
            {
                gameTimeTrack -= Time.deltaTime;
                if (gameTimeTrack <= 0f)
                {
                    gameTime--;
                    gameCountdown.text = gameTime.ToString();
                    gameTimeTrack = 1f;
                }
            }
            else
            {
                gameActive = false;
                gameCountdown.enabled = false;
                if (!resultsScript.suddenDeath)
                {
                    winPanel.SetActive(true);
                }
                else
                {
                    suddenDeathWinPanel.SetActive(true);
                }

                if (!hasPlayedWinTheme)
                {
                    bonusWinTheme.Play();
                    hasPlayedWinTheme = true;
                }
                
                gameScript.bonusGameOutcome = 1;
                activateHeart = false;
            }
        }
        else
        {
            obstacleScript.ClearObstacles();
            obstacleScript.enabled = false;
        }
    }

    public void SetupEvader()
    {
        evaderGameDescription.SetActive(false);
        activateHeart = true;
        started = true;
        countdownValue = 3;
        startTimer = 1f;
        gameTimeTrack = 1f;
        countdown.text = countdownValue.ToString();
    }

    private IEnumerator StartGame()
    {
        whistle.Play();
        countdown.text = string.Format("GO!");
        yield return new WaitForSeconds(1f);
        countdown.enabled = false;
        gameActive = true;
        obstacleScript.enabled = true;
        if (resultsScript.suddenDeath)
        {
            gameTime = 90;
        }
        else
        {
            gameTime = 60;
        }
        gameCountdown.text = gameTime.ToString();
    }

    public void ReturnToAuction()
    {
        if (gameScript.bonusGameType == 1)
        {
            winPanel.SetActive(false);
            losePanel.SetActive(false);

            StartCoroutine(BonusGameExit(gameScript.stageMarker.transform.position, gameScript.stageMarker.transform.rotation, 3.5f));
        }
    }

    private IEnumerator BonusGameExit(Vector3 targetPosition, Quaternion newRotation, float duration)
    {   
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

            // Smoothly interpolate position & rotation
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPosition, t);
            mainCamera.transform.rotation = Quaternion.Slerp(startRot, newRotation, t);

            yield return null; // Wait until next frame
        }

        // Ensure final position and rotation are exact
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
    }
}
