using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleManager : MonoBehaviour
{
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

    int startTime = 3;

    // Start is called before the first frame update
    void Start()
    {
        creditsButton.SetActive(false);
        exitButton.SetActive(false);
        playButton.SetActive(false);
        drDefraud.SetActive(false);
        StartCoroutine(TitleScreenCountdown());
    }

    private IEnumerator TitleScreenCountdown()
    {
        yield return new WaitForSeconds(0.5f);
        int timeLeft = startTime;

        while (timeLeft > 0)
        {
            countdown.text = timeLeft.ToString();
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }
        countdown.enabled = false;
        explodePanel.SetActive(true);
        mainCamera.transform.position = titleMarker.transform.position;
        mainCamera.transform.rotation = titleMarker.transform.rotation;
        yield return new WaitForSeconds(2f);
        cameraAnim.Play("TitleScreenLookaround");
        panelExploder.Play("ExplodePanelFade");
        creditsButton.SetActive(true);
        exitButton.SetActive(true);
        playButton.SetActive(true);
        drDefraud.SetActive(true);

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

}
