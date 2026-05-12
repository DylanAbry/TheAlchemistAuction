using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManagerAuction : MonoBehaviour
{
    public GameObject liquotholPanel;
    public GameObject deFraudPanel;
    public GameObject gameplayPanel;
    public GameObject historyPanel;
    public GameObject settingsPanel;
    public GameObject quitPanel;

    public Button liquotholButton;
    public Button deFraudButton;
    public Button quitBack;
    public Button toLevelSelect;
    public Button toTitle;

    public Slider volumeSlider;
    public Slider mouseSensitivitySlider;

    public Animator transitionPanel;
    public PlayerMovement mouseLook;

    public GameObject gameCursorIcon;

    public GameObject pauseMenu;
    public PlayerMovement movementScript;
    public PlayerInteraction interactScript;
    public CompletionistInteraction compInteract;
    public FleeceBossIntro flcScript;
    public BonusGameBlitzInteraction interScript;

    public bool interacting;


    // Start is called before the first frame update
    void Start()
    {

        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        volumeSlider.value = volume;
        mouseSensitivitySlider.value = sensitivity;

        ApplyVolume(volume);
        ApplySensitivity(sensitivity);

        volumeSlider.onValueChanged.AddListener(ApplyVolume);
        mouseSensitivitySlider.onValueChanged.AddListener(ApplySensitivity);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == ("AlchemistAuctionSelectGame")) 
        {
            if (pauseMenu.activeInHierarchy)
            {
                movementScript.enabled = false;
                interactScript.enabled = false;
            }
            else
            {
                movementScript.enabled = true;
                interactScript.enabled = true;
            }          
        }
        else if (SceneManager.GetActiveScene().name == ("AlchemistAuctionFleeceBoss"))
        {
            if (pauseMenu.activeInHierarchy)
            {
                movementScript.enabled = false;
            }
            else
            {
                if (!flcScript.jumpscareHappened)
                {
                    movementScript.enabled = true;
                }
                
            }
        }
        else if (SceneManager.GetActiveScene().name == ("CompletionistCave"))
        {
            if (pauseMenu.activeInHierarchy)
            {
                movementScript.enabled = false;
                compInteract.enabled = false;
            }
            else
            {
                if (!interacting)
                {
                    movementScript.enabled = true;
                    compInteract.enabled = true;
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == ("BonusGameBlitz"))
        {
            if (pauseMenu.activeInHierarchy)
            {
                movementScript.enabled = false;
                interScript.enabled = false;
            }
            else
            {
                if (!interacting)
                {
                    movementScript.enabled = true;
                    interScript.enabled = true;
                }
            }
        }
        else
        {

        }
    }

    public void ShowGamePlayPanel()
    {
        gameplayPanel.SetActive(true);
        //liquotholButton.enabled = false;
        //deFraudButton.enabled = false;
    }

    public void CloseGamePlayPanel()
    {
        gameplayPanel.SetActive(false);
        liquotholButton.enabled = true;
        deFraudButton.enabled = true;
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
        //liquotholButton.enabled = false;
        //deFraudButton.enabled = false;
    }

    public void CloseSettingsPanel()
    {
        settingsPanel.SetActive(false);
        //liquotholButton.enabled = true;
        //deFraudButton.enabled = true;
    }

    public void ShowHistoryPanel()
    {
        historyPanel.SetActive(true);
        //liquotholButton.enabled = false;
        //deFraudButton.enabled = false;
    }

    public void CloseHistoryPanel()
    {
        historyPanel.SetActive(false);
        //liquotholButton.enabled = true;
        //deFraudButton.enabled = true;
    }

    public void ShowQuitPanel()
    {
        quitPanel.SetActive(true);
        //liquotholButton.enabled = false;
        //deFraudButton.enabled = false;
    }

    public void CloseQuitPanel()
    {
        quitPanel.SetActive(false);
        //liquotholButton.enabled = true;
        //deFraudButton.enabled = true;
    }

    public void ShowLiquotholPanel()
    {
        liquotholPanel.SetActive(true);
        liquotholButton.enabled = false;
        deFraudButton.enabled = false;
    }

    public void CloseLiquotholPanel()
    {
        liquotholPanel.SetActive(false);
        liquotholButton.enabled = true;
        deFraudButton.enabled = true;
    }

    public void ShowDefraudPanel()
    {
        deFraudPanel.SetActive(true);
        liquotholButton.enabled = false;
        deFraudButton.enabled = false;
    }

    public void CloseDefraudPanel()
    {
        deFraudPanel.SetActive(false);
        liquotholButton.enabled = true;
        deFraudButton.enabled = true;
    }

    public void ReturnToLevelSelect()
    {
        StartCoroutine(LevelSelectSequence());
    }

    private IEnumerator LevelSelectSequence()
    {
        quitBack.enabled = false;
        toLevelSelect.enabled = false;
        toTitle.enabled = false;
        if (SceneManager.GetActiveScene().name == "AlchemistAuctionSelectGame")
        {
            transitionPanel.Play("LvlSelectFade");
        }
        else if (SceneManager.GetActiveScene().name == "AlchemistAuctionFleeceBoss")
        {
            transitionPanel.Play("BossFadeTransition");
        }
        else if (SceneManager.GetActiveScene().name == "AlchemistAuctionMainGame")
        {
            transitionPanel.Play("AucTransFade");
        }
        else
        {
            transitionPanel.Play("CompletionistTransition");
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("AlchemistAuctionSelectGame");
    }

    public void ReturnToTitle()
    {
        StartCoroutine(TitleSequence());
    }

    private IEnumerator TitleSequence()
    {
        quitBack.enabled = false;

        if (SceneManager.GetActiveScene().name == "BonusGameBlitz")
        {
            Time.timeScale = 1f;
            yield return null;
            transitionPanel.SetTrigger("SetTransition");
            yield return new WaitForSecondsRealtime(5f);
            SceneManager.LoadScene("BonusGameBlitzTitle");
            yield break;
        }
        if (SceneManager.GetActiveScene().name != "AlchemistAuctionSelectGame") toLevelSelect.enabled = false;
        toTitle.enabled = false;
        if (SceneManager.GetActiveScene().name == "AlchemistAuctionSelectGame")
        {
            transitionPanel.Play("LvlSelectFade");
        }
        else if (SceneManager.GetActiveScene().name == "AlchemistAuctionFleeceBoss")
        {
            transitionPanel.Play("BossFadeTransition");
        }
        else if (SceneManager.GetActiveScene().name == "AlchemistAuctionMainGame")
        {
            transitionPanel.Play("AucTransFade");
        }
        else if (SceneManager.GetActiveScene().name == "BonusGameBlitz")
        {
            Time.timeScale = 1f;
            yield return null;
            transitionPanel.SetTrigger("CompletionistTransition");
            yield return new WaitForSecondsRealtime(5f);
            SceneManager.LoadScene("BonusGameBlitzTitle");
            yield break;
        }        
        else
        {

        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("FleeceTrollingScene");
    }

    void ApplyVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void ApplySensitivity(float value)
    {
        // Only apply if this scene actually has camera rotation
        if (mouseLook != null)
        {
            mouseLook.mouseSensitivity = value;
        }

        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }
}
