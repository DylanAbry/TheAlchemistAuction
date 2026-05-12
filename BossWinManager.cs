using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossWinManager : MonoBehaviour
{
    public GameObject exitButton;
    public Animator transition;

    void Start()
    {
        PlayerPrefs.SetInt("UnlockedCompletionistCave", 1);
        PlayerPrefs.Save();
    }
    public void ReturnToTitleScreen()
    {
        StartCoroutine(LeaveSequence());
    }
    private IEnumerator LeaveSequence()
    {
        exitButton.SetActive(false);
        transition.Play("BossWinLeave");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("FleeceTrollingScene");
    }
}
