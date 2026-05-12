using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ProcessResults : MonoBehaviour
{
    public GameHandler gameScript;

    public static List<string> activePotionNames = new List<string>();

    public bool suddenDeath;
    public GameObject suddenDeathLight;
    public GameObject chooseFatePanel;

    public Animator transition;

    public int luckyNumber;
    public GameObject[] numberButtons;

    public TextMeshProUGUI winningNumberText;
    public TextMeshProUGUI losingNumberText;

    public AudioSource suddenDeathAlarm;
    public AudioSource suddenDeathTheme;
    public float suddenDeathLoopStart = 7f;
    public float suddenDeathLoopEnd = 53f;
    public bool suddenDeathIntroPlayed;

    public AudioSource bonusWinTheme;
    public AudioSource bonusLoseTheme;

    // Start is called before the first frame update
    void Start()
    {
        suddenDeath = false;
        suddenDeathLight.SetActive(false);
        chooseFatePanel.SetActive(false);

        foreach (GameObject button in numberButtons)
        {
            button.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!suddenDeathIntroPlayed && suddenDeathTheme.time >= suddenDeathLoopStart)
        {
            suddenDeathIntroPlayed = true;
            Debug.Log("Switched Intro Played variable!");
        }

        if (suddenDeathIntroPlayed && suddenDeathTheme.time >= suddenDeathLoopEnd)
        {
            suddenDeathTheme.timeSamples =
                (int)(suddenDeathLoopStart * suddenDeathTheme.clip.frequency);

            if (!suddenDeathTheme.isPlaying)
            {
                suddenDeathTheme.Play();
            }
        }
    }

    public void ViewAuctionResults()
    {
        gameScript.phalangesGamePanels[12].SetActive(false);
        gameScript.phalangesGamePanels[13].SetActive(false);
        StartCoroutine(TallyResults());
    }

    private IEnumerator ThePlayerWins()
    {
        yield return new WaitForSeconds(2.5f);
        transition.Play("AucTransFade");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("AuctionWinScene");
    }

    private IEnumerator ThePlayerLosesHaha()
    {
        yield return new WaitForSeconds(2.5f);
        transition.Play("AucTransFade");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("AuctionLoseScene");
    }

    private IEnumerator SuddenDeath()
    {
        yield return new WaitForSeconds(2.5f);
        suddenDeath = true;
        suddenDeathLight.SetActive(true);
        suddenDeathAlarm.Play();
        gameScript.playerCashText.enabled = false;
        gameScript.fleeceCashText.enabled = false;
        suddenDeathLight.GetComponent<Animator>().Play("SuddenDeathLighting");
        yield return new WaitForSeconds(5f);
        suddenDeathTheme.Play();
        gameScript.phalangesGamePanels[14].SetActive(true);
    }

    public void ShowSDInstructions()
    {
        StartCoroutine(SuddenDeathInstructions());
    }

    private IEnumerator SuddenDeathInstructions()
    {
        gameScript.phalangesGamePanels[14].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameScript.phalangesGamePanels[15].SetActive(true);
    }

    public void ChooseFateAppear()
    {
        StartCoroutine(ChooseYourFate());
    }

    private IEnumerator ChooseYourFate()
    {
        gameScript.phalangesGamePanels[15].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameScript.phalangesGamePanels[16].SetActive(true);
        chooseFatePanel.SetActive(true);
    }

    public void NumberGuessSelected()
    {
        gameScript.phalangesGamePanels[16].SetActive(false);
        chooseFatePanel.SetActive(false);
        StartCoroutine(NumberSelectionProcess());
    }

    private IEnumerator NumberSelectionProcess()
    {
        yield return new WaitForSeconds(2f);
        luckyNumber = Random.Range(1, 4);
        gameScript.phalangesGamePanels[17].SetActive(true);
        yield return new WaitForSeconds(2f);
        foreach (GameObject button in numberButtons)
        {
            button.GetComponent<Button>().enabled = false;
            button.SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        foreach (GameObject button in numberButtons)
        {
            button.GetComponent<Button>().enabled = true;
        }
    }

    public void Lotto(GameObject clickedButton)
    {
        GameObject winningNumber = numberButtons[luckyNumber - 1];
        if (clickedButton == winningNumber)
        {
            StartCoroutine(CorrectNumber());
        }
        else
        {
            StartCoroutine(IncorrectNumber());
        }

        foreach (GameObject button in numberButtons)
        {
            button.SetActive(false);
        }

    }

    private IEnumerator CorrectNumber()
    {
        gameScript.phalangesGamePanels[17].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameScript.phalangesGamePanels[18].SetActive(true);
        yield return new WaitForSeconds(4f);
        gameScript.phalangesGamePanels[18].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        bonusWinTheme.Play();
        winningNumberText.text = string.Format(luckyNumber + "! You took the risk and it paid off! Congratulations, you are the official winner of the Alchemist Auction!!");
        gameScript.phalangesGamePanels[19].SetActive(true);
        yield return new WaitForSeconds(3f);
        gameScript.phalangesGamePanels[19].SetActive(false);
        transition.Play("AucTransFade");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("AuctionWinScene");

    }
    private IEnumerator IncorrectNumber()
    {
        gameScript.phalangesGamePanels[17].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        gameScript.phalangesGamePanels[18].SetActive(true);
        yield return new WaitForSeconds(4f);
        gameScript.phalangesGamePanels[18].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        bonusLoseTheme.Play();
        losingNumberText.text = string.Format(luckyNumber + "! Sorry, but your guess was incorrect! Dr. Defraud is the official winner of the Alchemist Auction!!");
        gameScript.phalangesGamePanels[20].SetActive(true);
        yield return new WaitForSeconds(3f);
        gameScript.phalangesGamePanels[20].SetActive(false);
        transition.Play("AucTransFade");
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("AuctionLoseScene");
    }

    public void BonusGameSelected()
    {
        gameScript.phalangesGamePanels[16].SetActive(false);
        chooseFatePanel.SetActive(false);
        gameScript.BonusGameAccept();
    }


    private IEnumerator TallyResults()
    {
        gameScript.cashHolder.SetActive(false);
        if (gameScript.playerScore > gameScript.defraudScore)
        {
            StartCoroutine(ThePlayerWins());
        }
        else if (gameScript.defraudScore > gameScript.playerScore)
        {
            StartCoroutine(ThePlayerLosesHaha());
        }
        else
        {
            StartCoroutine(SuddenDeath());
        }
        yield return null;
    }

    public void SuddenDeathEnding(GameObject clickedButton)
    {
        clickedButton.SetActive(false);
        StartCoroutine(DetermineSuddenDeathOutcome());
    }

    private IEnumerator DetermineSuddenDeathOutcome()
    {
        if (gameScript.bonusGameOutcome == 1)
        {
            yield return new WaitForSeconds(1f);
            transition.Play("AucTransFade");
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("AuctionWinScene");
        }
        else
        {
            yield return new WaitForSeconds(1f);
            transition.Play("AucTransFade");
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("AuctionLoseScene");
        }
    }
}
