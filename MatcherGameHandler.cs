using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class MatcherGameHandler : MonoBehaviour
{
    public IntroManager introScript;
    public PotionManager potionScript;
    public ProcessResults resultsScript;
    public Camera mainCamera;
    public GameHandler gameScript;
    public GameObject matchingGameDescription;
    public GameObject[] matchCardIcons;
    public GameObject[] matchCardBacks;
    public GameObject[] chanceHearts;

    public List<int> locsUsed = new List<int>();
    public TextMeshProUGUI countdown;
    public TextMeshProUGUI gameCountdown;
    public TextMeshProUGUI phalangesWinText;
    public TextMeshProUGUI phalangesLoseText;
    public int countdownValue;
    public int cardSel;
    public int failedAttempts;
    public int correctMatches;
    public int gameTime;

    public GameObject winPanel;
    public GameObject loseOnePanel;
    public GameObject loseTwoPanel;

    public GameObject suddenDeathWinPanel;
    public GameObject suddenDeathLoseOnePanel;
    public GameObject suddenDeathLoseTwoPanel;


    public bool gameActive;
    public bool started;
    public bool countdownDone;

    GameObject firstCard;
    GameObject secondCard;

    private float startTimer;
    private float gameTimeTrack;

    public List<GameObject> foundPairs = new List<GameObject>();

    public AudioSource bonusLoseTheme;
    public AudioSource bonusWinTheme;
    public AudioSource correctly;
    public AudioSource incorrectly;
    public AudioSource whistle;
    bool hasLoseThemePlayed;

    // Make a list that holds all of the card pairs that are found!!

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

        suddenDeathWinPanel.SetActive(false);
        suddenDeathLoseOnePanel.SetActive(false);
        suddenDeathLoseTwoPanel.SetActive(false);
        hasLoseThemePlayed = false;
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
                    loseTwoPanel.SetActive(true);
                }
                else
                {
                    suddenDeathLoseTwoPanel.SetActive(true);
                }

                if (!hasLoseThemePlayed)
                {
                    bonusLoseTheme.Play();
                    hasLoseThemePlayed = true;
                }
                
                gameScript.bonusGameOutcome = 2;
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
        foreach (GameObject heart in chanceHearts)
        {
            heart.SetActive(true);
        }
        countdown.enabled = false;
        gameActive = true;
        if (resultsScript.suddenDeath)
        {
            gameTime = 35;
            failedAttempts = 6;
            for (int i = 0; i < 6; i++)
            {
                chanceHearts[i].SetActive(true);
            }
        }
        else
        {
            gameTime = 45;
            failedAttempts = 8;
            foreach (GameObject heart in chanceHearts)
            {
                heart.SetActive(true);
            }
        }
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
            gameCountdown.enabled = false;
            gameScript.bonusGameOutcome = 1;
            if (!resultsScript.suddenDeath)
            {
                winPanel.SetActive(true);
            }
            else
            {
                suddenDeathWinPanel.SetActive(true);
            }
            bonusWinTheme.Play();
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
            gameCountdown.enabled = false;
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
        cardSel = 0;
    }

    public void ReturnToAuction()
    {
        if (gameScript.bonusGameType == 3)
        {
            foreach (GameObject heart in chanceHearts)
            {
                heart.SetActive(false);
            }
            winPanel.SetActive(false);
            loseOnePanel.SetActive(false);
            loseTwoPanel.SetActive(false);

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

        hasLoseThemePlayed = false;
    }
}
