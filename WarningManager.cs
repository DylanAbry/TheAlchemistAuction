using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WarningManager : MonoBehaviour
{
    public Animator warnAnim;
    public GameObject continueButton;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetupWarning());
    }

    private IEnumerator SetupWarning()
    {
        yield return new WaitForSeconds(3f);
        continueButton.SetActive(true);
    }

    public void ContinueToGame()
    {
        StartCoroutine(OpenNextGameScene());
    }

    private IEnumerator OpenNextGameScene()
    {
        continueButton.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        warnAnim.Play("WarnFadeOut");
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("FleeceTrollingScene");
    }
}
