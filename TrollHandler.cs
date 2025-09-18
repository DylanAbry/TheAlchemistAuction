using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class TrollHandler : MonoBehaviour
{
    public Animator fleeceDeFraud;
    public Animator transitionPanel;
    public GameObject[] deFraudPics;
    public GameObject[] fleeceQuotes;

    private int quoteIndex;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            deFraudPics[i].SetActive(false);
        }
        for (int i = 0; i < fleeceQuotes.Length; i++)
        {
            fleeceQuotes[i].SetActive(false);
        }

        quoteIndex = Random.Range(0, fleeceQuotes.Length);

        StartCoroutine(TrollSequence());
    }

    private IEnumerator TrollSequence()
    {
        yield return new WaitForSeconds(0.2f);
        transitionPanel.Play("TrollTransitionFadeIn");
        yield return new WaitForSeconds(1.5f);
        fleeceDeFraud.Play("FleeceTrollEnter");
        yield return new WaitForSeconds(1f);
        deFraudPics[2].SetActive(false);
        deFraudPics[1].SetActive(true);
        fleeceQuotes[quoteIndex].SetActive(true);
        yield return new WaitForSeconds(5f);
        fleeceQuotes[quoteIndex].SetActive(false);
        deFraudPics[1].SetActive(false);
        deFraudPics[0].SetActive(true);
        fleeceDeFraud.Play("FleeceTrollExit");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TitleScreen");
    }

}
