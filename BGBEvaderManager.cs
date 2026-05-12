using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BGBEvaderManager : MonoBehaviour
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

    public BonusGameBlitzInteraction interactScript;
    public BGBlitzManager blitzScript;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;
    public AudioSource whistle;
    bool hasWinThemePlayed;

    public bool normalBeat, expertBeat, egBeat;

    public BGBTimer timerScript;
    public GameObject normalSelect, expertSelect, egSelect;

    public AudioSource chaching;

    // Start is called before the first frame update
    void Start()
    {
        obstacleScript.enabled = false;
        activateHeart = false;
        startTimer = 1f;
        gameTimeTrack = 1f;
        gameCountdown.enabled = false;
        hasWinThemePlayed = false;

        normalBeat = false;
        expertBeat = false;
        egBeat = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!blitzScript.gameActive) obstacleScript.ClearObstacles();

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
                    bonusWinTheme.Play();
                    hasWinThemePlayed = true;
                }
                winPanel.SetActive(true);
                if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Normal)
                {
                    blitzScript.score += 120;
                    Color c = normalSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    normalSelect.GetComponent<Image>().color = c;
                    normalSelect.GetComponent<Button>().enabled = false;
                    normalBeat = true;
                    blitzScript.normalsBeat += 1;
                    blitzScript.checks[3].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Expert)
                {
                    blitzScript.score += 250;
                    Color c = expertSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    expertSelect.GetComponent<Image>().color = c;
                    expertSelect.GetComponent<Button>().enabled = false;
                    expertBeat = true;
                    blitzScript.expertsBeat += 1;
                    blitzScript.checks[4].SetActive(true);
                }
                else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.EpicGamer)
                {
                    blitzScript.score += 320;
                    Color c = egSelect.GetComponent<Image>().color;
                    c.a = 0.1f;
                    egSelect.GetComponent<Image>().color = c;
                    egSelect.GetComponent<Button>().enabled = false;
                    egBeat = true;
                    blitzScript.egsBeat += 1;
                    blitzScript.checks[5].SetActive(true);
                }
                else
                {

                }
                if (normalBeat && expertBeat && egBeat)
                {
                    timerScript.timer += 60f;
                }

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
        gameCountdown.text = gameTime.ToString();
    }

    public void ReturnToAuction()
    {
        if (winPanel.activeInHierarchy) chaching.Play();
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        hasWinThemePlayed = false;
        blitzScript.scorePanel.SetActive(true);
        if (blitzScript.hasPlayed == 1)
        {
            blitzScript.bestScorePanel.SetActive(true);
        }
        else
        {
            blitzScript.bestScorePanel.SetActive(false);
        }


        if (interactScript.bonusGameTheme.isPlaying) interactScript.bonusGameTheme.Stop();
        interactScript.bgbTheme.Play();
        if (blitzScript.normalsBeat == 4 && blitzScript.expertsBeat == 4 && blitzScript.egsBeat == 4)
        {
            timerScript.timer = 0f;
        }
    }
}
