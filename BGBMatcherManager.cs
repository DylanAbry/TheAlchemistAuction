using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class BGBMatcherManager : MonoBehaviour
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

    public BonusGameBlitzInteraction interactScript;
    public BGBlitzManager blitzScript;
    public BGBTimer timerScript;

    public bool normalBeat, expertBeat, egBeat;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;
    public AudioSource correctly;
    public AudioSource incorrectly;
    public AudioSource whistle;
    bool hasPlayedLoseTheme;

    public GameObject normalSelect, expertSelect, egSelect;

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


        normalBeat = false;
        expertBeat = false;
        egBeat = false;
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
            if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Normal)
            {
                blitzScript.score += 80;
                Color c = normalSelect.GetComponent<Image>().color;
                c.a = 0.1f;
                normalSelect.GetComponent<Image>().color = c;
                normalSelect.GetComponent<Button>().enabled = false;
                normalBeat = true;
                blitzScript.normalsBeat += 1;
                blitzScript.checks[0].SetActive(true);
            }
            else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.Expert)
            {
                blitzScript.score += 220;
                Color c = expertSelect.GetComponent<Image>().color;
                c.a = 0.1f;
                expertSelect.GetComponent<Image>().color = c;
                expertSelect.GetComponent<Button>().enabled = false;
                expertBeat = true;
                blitzScript.expertsBeat += 1;
                blitzScript.checks[1].SetActive(true);
            }
            else if (BonusGameBlitzInteraction.difficultyLevel == BonusGameBlitzInteraction.BonusGameDifficulty.EpicGamer)
            {
                blitzScript.score += 300;
                Color c = egSelect.GetComponent<Image>().color;
                c.a = 0.1f;
                egSelect.GetComponent<Image>().color = c;
                egSelect.GetComponent<Button>().enabled = false;
                egBeat = true;
                blitzScript.egsBeat += 1;
                blitzScript.checks[2].SetActive(true);
            }
            else
            {

            }
            gameCountdown.enabled = false;
            if (normalBeat && expertBeat && egBeat)
            {
                timerScript.timer += 60f;
            }
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
        if (blitzScript.normalsBeat == 4 && blitzScript.expertsBeat == 4 && blitzScript.egsBeat == 4)
        {
            timerScript.timer = 0f;
        }
    }
}
