using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompletionistEvaderGame : MonoBehaviour
{
    public ObstacleSpawner obstacleScript;
    public Camera mainCamera;
    public GameObject evaderGameDescription;

    public bool gameActive;
    public bool started;
    public bool countdownDone;
    public bool activateHeart;

    private float startTimer;
    private float gameTimeTrack;

    public GameObject winPanel;
    public GameObject losePanel;
    

    public TextMeshProUGUI countdown;
    public TextMeshProUGUI gameCountdown;

    public int countdownValue;
    public int gameTime;

    public CompletionistInteraction compScript;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;
    public AudioSource whistle;
    bool hasWinThemePlayed;

    public NoPlayCursorHandler cursorScript;

    // Start is called before the first frame update
    void Start()
    {
        obstacleScript.enabled = false;
        activateHeart = false;
        startTimer = 1f;
        gameTimeTrack = 1f;
        gameCountdown.enabled = false;
        hasWinThemePlayed = false;
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
                if (!hasWinThemePlayed)
                {
                    cursorScript.cursorImage.gameObject.SetActive(true);
                    bonusWinTheme.Play();
                    hasWinThemePlayed = true;
                }
                winPanel.SetActive(true);
                if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.Normal)
                {
                    PlayerPrefs.SetInt("NormalEvaderComplete", 1);
                    PlayerPrefs.Save();
                }
                else if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.Expert)
                {
                    PlayerPrefs.SetInt("ExpertEvaderComplete", 1);
                    PlayerPrefs.Save();
                }
                else if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.EpicGamer)
                {
                    PlayerPrefs.SetInt("EpicGamerEvaderComplete", 1);
                    PlayerPrefs.Save();
                }
                else
                {

                }
                activateHeart = false;
            }
        }
        else
        {
            obstacleScript.ClearObstacles();
            obstacleScript.enabled = false;
        }

        if ((PlayerPrefs.GetInt("NormalEvaderComplete", 0) == 1) && (PlayerPrefs.GetInt("ExpertEvaderComplete", 0) == 1) && (PlayerPrefs.GetInt("EpicGamerEvaderComplete", 0) == 1))
        {
            PlayerPrefs.SetInt("EvaderModesComplete", 1);
            PlayerPrefs.Save();
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
        gameCountdown.text = gameTime.ToString();
    }

    public void ReturnToAuction()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        hasWinThemePlayed = false;

        if (compScript.bonusGameTheme.isPlaying) compScript.bonusGameTheme.Stop();
        compScript.completionistTheme.Play();
    }
}
