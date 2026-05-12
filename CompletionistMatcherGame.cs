using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class CompletionistMatcherGame : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject matchingGameDescription;
    public GameObject[] matchCardIcons;
    public GameObject[] matchCardBacks;
    public GameObject[] chanceHearts;

    public List<int> locsUsed = new List<int>();
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI gameCountdown;
    
    public int countdownValue;
    public int cardSel;
    public int failedAttempts;
    public int correctMatches;
    public int gameTime;

    public GameObject winPanel;
    public GameObject loseOnePanel;
    public GameObject loseTwoPanel;

    public bool gameActive;
    public bool started;
    public bool countdownDone;

    GameObject firstCard;
    GameObject secondCard;

    private float startTimer;
    private float gameTimeTrack;

    public List<GameObject> foundPairs = new List<GameObject>();

    public CompletionistInteraction compScript;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;
    public AudioSource correctly;
    public AudioSource incorrectly;
    public AudioSource whistle;
    bool hasPlayedLoseTheme;

    // Start is called before the first frame update
    void Start()
    {
        locsUsed.Clear();

        foreach (GameObject card in matchCardIcons)
        {
            card.SetActive(false);
        }
        foreach (GameObject card in matchCardBacks)
        {
            card.SetActive(false);
        }

        countdownValue = 3;
        failedAttempts = 8;
        correctMatches = 0;
        countdown.text = countdownValue.ToString();
        gameActive = false;
        started = false;
        countdownDone = false;
        cardSel = 0;
        startTimer = 1f;
        gameTimeTrack = 1f;
        countdown.enabled = false;
        gameCountdown.enabled = false;

        foreach (GameObject heart in chanceHearts)
        {
            heart.SetActive(false);
        }

        hasPlayedLoseTheme = false;

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
                if (!hasPlayedLoseTheme)
                {
                    bonusLoseTheme.Play();
                    hasPlayedLoseTheme = true;
                }
                loseTwoPanel.SetActive(true);
            }
        }

        if ((PlayerPrefs.GetInt("NormalMatcherComplete", 0) == 1) && (PlayerPrefs.GetInt("ExpertMatcherComplete", 0) == 1) && (PlayerPrefs.GetInt("EpicGamerMatcherComplete", 0) == 1))
        {
            PlayerPrefs.SetInt("MatcherModesComplete", 1);
            PlayerPrefs.Save();
        }
    }

    public void SetupMatcher()
    {
        matchingGameDescription.SetActive(false);
        countdownValue = 3;
        countdown.text = countdownValue.ToString();

        foreach (GameObject cards in matchCardBacks)
        {
            cards.SetActive(true);
        }
        foreach (GameObject cards in matchCardIcons)
        {
            cards.GetComponent<Button>().enabled = false;
        }

        for (int i = 0; i < matchCardIcons.Length; i++)
        {
            int loc = 0;
            while (locsUsed.Contains(loc))
            {
                loc = Random.Range(0, matchCardIcons.Length);
            }
            matchCardIcons[i].transform.position = matchCardBacks[loc].transform.position;
            locsUsed.Add(loc);

        }
        started = true;
    }

    private IEnumerator StartGame()
    {
        whistle.Play();
        countdown.text = string.Format("GO!");
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < failedAttempts; i++)
        {
            chanceHearts[i].SetActive(true);
        }
        countdown.enabled = false;
        gameActive = true;
        
        startTimer = 1f;
        gameTimeTrack = 1f;
        gameCountdown.text = gameTime.ToString();
        foreach (GameObject cards in matchCardIcons)
        {
            cards.SetActive(true);
            cards.GetComponent<RawImage>().color = new Color(0f, 0f, 0f, 0f);
            cards.GetComponent<Button>().enabled = true;
        }
    }

    public void PickACard(MatchPairs cardBundle)
    {
        if (cardSel == 0)
        {
            cardBundle.clickedCard.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
            firstCard = cardBundle.clickedCard;
            cardBundle.clickedCard.GetComponent<Button>().enabled = false;
            cardSel++;
        }
        else
        {
            cardBundle.clickedCard.GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);

            foreach (GameObject card in matchCardIcons)
            {
                card.GetComponent<Button>().enabled = false;
            }

            if (firstCard == cardBundle.matchingCard)
            {
                foundPairs.Add(firstCard);
                foundPairs.Add(cardBundle.clickedCard);
                correctly.Play();
                StartCoroutine(CorrectMatch());
            }
            else
            {
                incorrectly.Play();
                StartCoroutine(IncorrectMatch());
            }
        }
    }

    private IEnumerator CorrectMatch()
    {
        correctMatches++;
        yield return new WaitForSeconds(0.5f);

        if (correctMatches != 8)
        {
            foreach (GameObject card in matchCardIcons)
            {
                if (!foundPairs.Contains(card))
                {
                    card.GetComponent<Button>().enabled = true;
                }
            }
        }
        else
        {
            gameActive = false;
            winPanel.SetActive(true);
            bonusWinTheme.Play();
            if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.Normal)
            {
                PlayerPrefs.SetInt("NormalMatcherComplete", 1);
                PlayerPrefs.Save();
            }
            else if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.Expert)
            {
                PlayerPrefs.SetInt("ExpertMatcherComplete", 1);
                PlayerPrefs.Save();
            }
            else if (CompletionistInteraction.difficultyLevel == CompletionistInteraction.BonusGameDifficulty.EpicGamer)
            {
                PlayerPrefs.SetInt("EpicGamerMatcherComplete", 1);
                PlayerPrefs.Save();
            }
            else
            {

            }
            gameCountdown.enabled = false;
        }
        cardSel = 0;
    }
    private IEnumerator IncorrectMatch()
    {
        chanceHearts[failedAttempts - 1].SetActive(false);
        failedAttempts--;
        yield return new WaitForSeconds(0.5f);

        if (failedAttempts != 0)
        {
            foreach (GameObject card in matchCardIcons)
            {
                if (!foundPairs.Contains(card))
                {
                    card.GetComponent<RawImage>().color = new Color(0f, 0f, 0f, 0f);
                    card.GetComponent<Button>().enabled = true;
                }
            }
        }

        else
        {
            gameActive = false;
            bonusLoseTheme.Play();
            loseOnePanel.SetActive(true);
            gameCountdown.enabled = false;
        }
        cardSel = 0;
    }

    public void ReturnToAuction()
    {
        foreach (GameObject heart in chanceHearts)
        {
            heart.SetActive(false);
        }
        winPanel.SetActive(false);
        loseOnePanel.SetActive(false);
        loseTwoPanel.SetActive(false);

        if (compScript.bonusGameTheme.isPlaying) compScript.bonusGameTheme.Stop();
        compScript.completionistTheme.Play();

        foreach (GameObject card in matchCardBacks)
        {
            card.SetActive(false);
        }
        foreach (GameObject card in matchCardIcons)
        {
            card.SetActive(false);
        }

        foundPairs.Clear();
        locsUsed.Clear();
        failedAttempts = 8;
        correctMatches = 0;
    }
}
