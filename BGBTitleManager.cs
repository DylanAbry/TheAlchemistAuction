using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BGBTitleManager : MonoBehaviour
{
    public Animator cameraAnim;
    public Animator desEffect;
    public Animator defraudAppear;
    public Animator transition;
    public Animator creditsPage;

    public GameObject fleeceGhostPanel;
    public GameObject desBeam;
    public GameObject cursorImage;
    public GameObject titlePanel;

    public GameObject[] defraudFaces;

    public AudioSource introTheme;
    public AudioSource mainIntroTheme;
    public AudioSource defraudLine;
    public AudioSource destructionatronCharge;
    public AudioSource destructionatronBeam;

    public float loopStart = 12f;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        foreach (GameObject face in defraudFaces)
        {
            face.SetActive(false);
        }

        defraudFaces[0].SetActive(true);
        desBeam.SetActive(false);
        cursorImage.SetActive(false);
        StartCoroutine(TitleScreen());
    }

    void Update()
    {
        if (mainIntroTheme.time >= mainIntroTheme.clip.length - 0.05f)
        {
            mainIntroTheme.time = loopStart;
        }
    }

    private IEnumerator TitleScreen()
    {
        transition.Play("TrollTransitionFadeIn");
        yield return new WaitForSeconds(1.75f);
        desBeam.SetActive(true);
        destructionatronCharge.Play();
        yield return new WaitForSeconds(0.8f);
        defraudAppear.Play("GhostDefraudAppears");
        yield return new WaitForSeconds(1f);
        defraudFaces[0].SetActive(false);
        defraudFaces[1].SetActive(true);
        defraudLine.Play();
        yield return new WaitForSeconds(2.2f);
        defraudFaces[1].SetActive(false);
        defraudFaces[2].SetActive(true);
        yield return new WaitForSeconds(0.25f);
        if (destructionatronCharge.isPlaying) destructionatronCharge.Stop();
        destructionatronBeam.Play();
        desEffect.Play("ExplodePanelFade");
        yield return new WaitForSeconds(0.15f);
        introTheme.Play();
        fleeceGhostPanel.SetActive(false);
        cursorImage.SetActive(true);
        cameraAnim.Play("TitleScreenLookaround");
        yield return new WaitForSeconds(14.8f);
        mainIntroTheme.Play();
    }
    public void PlayGame()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        titlePanel.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        transition.Play("TrollTransitionFadeOut");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("BonusGameBlitz");
    }

    public void LoadCredits()
    {
        titlePanel.SetActive(false);
        creditsPage.Play("CreditsAppear");
    }

    public void CloseCredits()
    {
        StartCoroutine(CloseCreditsSequence());
    }

    private IEnumerator CloseCreditsSequence()
    {
        creditsPage.Play("CreditsExit");
        yield return new WaitForSeconds(0.33f);
        titlePanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
