using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuctionWinSetup : MonoBehaviour
{
    public GameObject returnButton;
    public Animator transition;

    void Start()
    {
        returnButton.SetActive(false);
        StartCoroutine(ReturnAppear());
        PlayerPrefs.SetInt("UnlockedFleeceFinalFaceOff", 1);
        PlayerPrefs.Save();

        foreach (Transform potion in transform)
        {
            if (ProcessResults.activePotionNames.Contains(potion.gameObject.name))
            {
                potion.gameObject.SetActive(true);
            }
            else
            {
                potion.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator ReturnAppear()
    {
        yield return new WaitForSeconds(5f);
        returnButton.SetActive(true);
    }

    public void ReturnToTitleScreen()
    {
        returnButton.SetActive(false);
        StartCoroutine(ReturnProcess());
    }
    private IEnumerator ReturnProcess()
    {
        transition.Play("WinAucFadeOut");
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene("FleeceTrollingScene");
    }
}
