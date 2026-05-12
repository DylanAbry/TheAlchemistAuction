using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class BGBlitzManager : MonoBehaviour
{
    public PlayerMovement movementScript;

    public Animator transition;

    public GameObject bgbInstructions;
    public bool gameActive;

    public GameObject gameCursorImage;
    public GameObject raycastImage;

    public GameObject scorePanel;
    public GameObject timerPanel;
    public GameObject bestScorePanel;
    public GameObject loadingScreen;

    public Button playAgain;
    public Button toTitle;

    public int score;
    public int bestScore;
    public int normalsBeat;
    public int expertsBeat;
    public int egsBeat;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI normalsBeatText;
    public TextMeshProUGUI expertsBeatText;
    public TextMeshProUGUI egsBeatText;
    public bool countdownDone;

    public TextMeshProUGUI startGameCountdownText;
    public int countdown;
    public float startTimer;

    public TextMeshProUGUI finalScoreReport;
    public TextMeshProUGUI bestScoreReport;

    public int hasPlayed;

    public GameObject[] checks;

    public AudioSource bgbMainTheme;
    public AudioSource instructionsTheme;
    public AudioSource click;
    public AudioSource goSound;

    public float loopStart = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        hasPlayed = PlayerPrefs.GetInt("HasPlayedBefore", 0);
        bestScore = PlayerPrefs.GetInt("HighScore", 0);

        movementScript.enabled = false;
        bgbInstructions.SetActive(true);
        gameActive = false;
        scorePanel.SetActive(false);
        bestScorePanel.SetActive(false);
        gameCursorImage.SetActive(true);
        loadingScreen.SetActive(false);

        startGameCountdownText.enabled = false;
        countdown = 3;
        startTimer = 1f;
        timerPanel.SetActive(false);

        normalsBeat = 0;
        expertsBeat = 0;
        egsBeat = 0;
        raycastImage.SetActive(false);

        foreach (GameObject check in checks)
        {
            check.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = string.Format("CASH: $" + score);
        bestScoreText.text = string.Format("BEST: $" + bestScore);
        finalScoreReport.text = string.Format("Score: $" + score);
        bestScoreReport.text = string.Format("Best: $" + bestScore);
        normalsBeatText.text = string.Format(normalsBeat + "/4");
        expertsBeatText.text = string.Format(expertsBeat + "/4");
        egsBeatText.text = string.Format(egsBeat + "/4");

        if (instructionsTheme.time >= instructionsTheme.clip.length - 0.05f)
        {
            instructionsTheme.time = loopStart;
        }
    }

    public void CloseInstructions()
    {
        bgbInstructions.GetComponent<Animator>().Play("CloseBGBInstructions");
        gameCursorImage.SetActive(false);
        if (instructionsTheme.isPlaying) instructionsTheme.Stop();
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        yield return new WaitForSeconds(1f);

        startGameCountdownText.enabled = true;

        while (countdown > 0)
        {
            startGameCountdownText.text = countdown.ToString();
            click.Play();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        goSound.Play();
        startGameCountdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        bgbMainTheme.Play();
        startGameCountdownText.enabled = false;
        scorePanel.SetActive(true);
        timerPanel.SetActive(true);
        raycastImage.SetActive(true);
        if (hasPlayed == 1)
        {
            bestScorePanel.SetActive(true);
        }
        else
        {
            bestScorePanel.SetActive(false);
        }
        movementScript.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        gameActive = true;
        score = 0;
    }

    public void PlayAgain()
    {
        StartCoroutine(PlayAgainSequence());
    }

    private IEnumerator PlayAgainSequence()
    {
        Time.timeScale = 1f;
        yield return null;
        playAgain.interactable = false;
        toTitle.interactable = false;
        loadingScreen.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToTitleScreen()
    {
        StartCoroutine(TitleScreenSequence());
    }

    private IEnumerator TitleScreenSequence()
    {
        Time.timeScale = 1f;
        yield return null;
        playAgain.interactable = false;
        toTitle.interactable = false;

        transition.SetTrigger("SetTransition");
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("BonusGameBlitzTitle");
    }

}
