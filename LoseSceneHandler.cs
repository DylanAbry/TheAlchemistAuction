using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class LoseSceneHandler : MonoBehaviour
{
    public Animator cameraAnim;
    public Animator uiAnim;
    public PostProcessVolume volume;
    private DepthOfField depth;

    public AudioSource gameOverMusic;
    public AudioSource lightningStrike;
    public AudioSource jumpscareSound;
    public AudioSource defraudLaugh;

    public float startValue = 175f;
    public float endValue = 1f;
    public float duration = 1f;    
    private float timer = 0f;

    private bool startClear, endClear;

    public GameObject finalFleeceImage;
    public GameObject fleecePanel;
    public GameObject exitButton;
    public GameObject bhbbPotion;
    public GameObject gameOverPanel;

    public GameObject[] uiObjects;

    void Start()
    {
        bhbbPotion.SetActive(false);
        StartCoroutine(LosingAnimation());
        volume.profile.TryGetSettings(out depth);
        if (depth != null)
        {
            depth.focalLength.value = 175f;
        }

        startClear = false;
        endClear = false;
        finalFleeceImage.SetActive(false);
        fleecePanel.SetActive(false);
        exitButton.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private IEnumerator LosingAnimation()
    {
        yield return new WaitForSeconds(2f);
        uiAnim.Play("LoserUIAnim");
        cameraAnim.Play("LoserCameraAnim");
        yield return new WaitForSeconds(10f);
        startClear = true;
        yield return new WaitForSeconds(6f);
        jumpscareSound.Play();
        yield return new WaitForSeconds(2.5f);
        lightningStrike.Play();
        yield return new WaitForSeconds(0.1f);
        defraudLaugh.Play();
        yield return new WaitForSeconds(5.9f);
        lightningStrike.Play();
        yield return new WaitForSeconds(16.5f);
        gameOverMusic.Play();
        exitButton.SetActive(true);
        fleecePanel.SetActive(true);
        bhbbPotion.SetActive(true);
        finalFleeceImage.SetActive(true);
        gameOverPanel.SetActive(true);
    }

    void Update()
    {
        if (startClear && !endClear)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            // Interpolate linearly (can use SmoothStep for more cinematic easing)
            depth.focalLength.value = Mathf.Lerp(startValue, endValue, t);
            if (t >= 1f)
            {
                endClear = true;
            }
        }
    }

    public void ReturnToMainScreen()
    {
        StartCoroutine(CloseLoseScene());
    }

    private IEnumerator CloseLoseScene()
    {
        exitButton.SetActive(false);
        foreach (GameObject obj in uiObjects)
        {
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.5f);
        uiAnim.CrossFade("LosingTransitionFadeOut", 0.1f);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("FleeceTrollingScene");

    }
}
