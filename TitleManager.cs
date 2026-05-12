using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
    public AudioSource countdownVoice;
    public AudioSource shuttleEngine;
    public AudioSource laser;
    public AudioSource dangerLurking;
    public AudioSource mainTheme;
    public float loopStartTime = 18f;

    public Animator transition;
    public GameObject mainCamera;
    public GameObject playButton;
    public GameObject exitButton;
    public GameObject creditsButton;
    public TextMeshProUGUI countdown;
    public GameObject explodePanel;
    public GameObject titleMarker;
    public GameObject drDefraud;
    public Animator panelExploder;
    public Animator cameraAnim;

    public GameObject title;
    public GameObject creditsPanel;
    public GameObject[] musicCreditPages;

    public GameObject cursorImage;

    int startTime = 3;
    int musicIndex;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject panel in musicCreditPages)
        {
            panel.SetActive(false);
        }
        musicCreditPages[0].SetActive(true);
        creditsButton.SetActive(false);
        exitButton.SetActive(false);
        playButton.SetActive(false);
        drDefraud.SetActive(false);
        cursorImage.SetActive(false);
        title.SetActive(false);
        musicIndex = 0;
        StartCoroutine(TitleScreenCountdown());
        
    }

    void Update()
    {
        if (mainTheme.time >= mainTheme.clip.length - 0.05f)
        {
            mainTheme.time = loopStartTime;
        }
    }

    private IEnumerator TitleScreenCountdown()
    {
        yield return new WaitForSeconds(0.5f);
        int timeLeft = startTime;
        countdownVoice.Play();

        while (timeLeft > 0)
        {
            countdown.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        countdown.enabled = false;
        laser.Play();
        explodePanel.SetActive(true);
        if (shuttleEngine.isPlaying)
        {
            shuttleEngine.Stop();
        }
        mainCamera.transform.position = titleMarker.transform.position;
        mainCamera.transform.rotation = titleMarker.transform.rotation;
        yield return new WaitForSeconds(2f);
        dangerLurking.Play();
        cursorImage.SetActive(true);
        title.SetActive(true);
        cameraAnim.Play("TitleScreenLookaround");
        panelExploder.Play("ExplodePanelFade");
        creditsButton.SetActive(true);
        exitButton.SetActive(true);
        playButton.SetActive(true);
        drDefraud.SetActive(true);
        yield return new WaitForSeconds(15.75f);
        mainTheme.Play();

    }

    public void LoadGame()
    {
        StartCoroutine(GameLoadSequence());
    }

    private IEnumerator GameLoadSequence()
    {
        creditsButton.SetActive(false);
        exitButton.SetActive(false);
        playButton.SetActive(false);
        transition.Play("TitleTransition");
        yield return new WaitForSeconds(3.75f);
        SceneManager.LoadScene("AlchemistAuctionSelectGame");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenCredits()
    {
        StartCoroutine(CreditsOpenSequence());
    }

    private IEnumerator CreditsOpenSequence()
    {
        playButton.SetActive(false);
        exitButton.SetActive(false);
        creditsButton.SetActive(false);
        title.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        creditsPanel.GetComponent<Animator>().Play("CreditsAppear");
    }

    public void CloseCredits()
    {
        StartCoroutine(CreditsCloseSequence());
    }

    private IEnumerator CreditsCloseSequence()
    {
        creditsPanel.GetComponent<Animator>().Play("CreditsExit");
        yield return new WaitForSeconds(0.33f);
        playButton.SetActive(true);
        exitButton.SetActive(true);
        creditsButton.SetActive(true);
        title.SetActive(true);
    }

    public void NextMusicPanel()
    {
        musicCreditPages[musicIndex].SetActive(false);
        
        if (musicIndex == musicCreditPages.Length - 1)
        {
            musicIndex = 0;
            musicCreditPages[musicIndex].SetActive(true);
        }
        else
        {
            musicIndex++;
            musicCreditPages[musicIndex].SetActive(true);
        }
    }
    public void PreviousMusicPanel()
    {
        musicCreditPages[musicIndex].SetActive(false);

        if (musicIndex == 0)
        {
            musicIndex = 2;
            musicCreditPages[musicIndex].SetActive(true);
        }
        else
        {
            musicIndex--;
            musicCreditPages[musicIndex].SetActive(true);
        }
    }

}
