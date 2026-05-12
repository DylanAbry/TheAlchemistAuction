using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLoserManager : MonoBehaviour
{
    public GameObject earth;
    public GameObject[] defraudFaces;
    public GameObject[] defraudPanels;
    public GameObject gameOverPanel;
    public GameObject exitButton;
    public Animator transition;
    public AudioSource gulp;

    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject f in defraudFaces)
        {
            f.SetActive(false);
        }
        foreach (GameObject p in defraudPanels)
        {
            p.SetActive(false);
        }

        defraudFaces[0].SetActive(true);
        exitButton.SetActive(false);
        gameOverPanel.SetActive(false);
        StartCoroutine(DefraudEatsWorld());
    }

    private IEnumerator DefraudEatsWorld()
    {
        transition.Play("BossLoseFadeOut");
        yield return new WaitForSeconds(4f);
        if (earth.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("EarthIdle"))
        {
            earth.GetComponent<Animator>().Play("EarthConsumed");
        }
        defraudFaces[0].SetActive(false);
        defraudFaces[1].SetActive(true);
        yield return new WaitForSeconds(2.5f);
        defraudFaces[1].SetActive(false);
        defraudFaces[2].SetActive(true);
        earth.SetActive(false);
        defraudPanels[0].SetActive(true);
        gulp.Play();
        yield return new WaitForSeconds(1f);
        defraudPanels[0].SetActive(false);
        yield return new WaitForSeconds(2f);
        defraudFaces[1].SetActive(true);
        defraudFaces[2].SetActive(false);
        defraudPanels[1].SetActive(true);
        yield return new WaitForSeconds(4f);
        defraudPanels[1].SetActive(false);
        defraudFaces[1].SetActive(false);
        defraudFaces[3].SetActive(true);
        yield return new WaitForSeconds(1f);
        gameOverPanel.SetActive(true);
        exitButton.SetActive(true);
    }
    public void ExitToTitle()
    {
        StartCoroutine(ExitSequence());
    }

    private IEnumerator ExitSequence()
    {
        exitButton.SetActive(false);
        transition.Play("BossLoseFadeIn");
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("FleeceTrollingScene");
    }
}
