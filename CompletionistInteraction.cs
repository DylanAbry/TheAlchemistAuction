using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.Video;

public class CompletionistInteraction : MonoBehaviour
{
    [Header("Core refs")]
    public PlayerMovement movementScript;
    public PauseManagerAuction pauseScript;
    public GameObject player;

    [Header("UI")]
    public GameObject cursorIcon;
    public GameObject raycastIcon;

    [Header("Bonus triggers")]
    public GameObject matcherTrigger, evaderTrigger, tracerTrigger, sliderTrigger;
    public GameObject matcherPos, evaderPos, tracerPos, sliderPos;

    [Header("Bonus games")]
    public ObstacleSpawner obstacleScript;
    public CompletionistEvaderGame evaderScript;
    public CompletionistTracerGame tracerScript;
    public CompletionistMatcherGame matcherScript;
    public CompletionistSliderGame sliderScript;

    [Header("One Percent Present")]
    public GameObject oppDescription, mysteryBag, present, flipOff, oppExit;
    public int presentProb;
    

    [Header("Secret word")]
    public GameObject wordInteract;
    public GameObject wordInput;
    public TMP_InputField inputField;
    public bool wordCorrect;
    public GameObject[] wordHints;
    public TextMeshProUGUI timesWonReport;

    [Header("FX")]
    public ParticleSystem goopy;

    [Header("Chess")]
    public Camera playerCam;
    public Camera chessCam;
    public GameObject chessIntroPanel;

    [Header("TV")]
    public CompletionistTVPlayer tvScript;
    public GameObject channelOptions;

    [Header("Raycast")]
    [SerializeField] float playerReach = 20f;

    public GameObject chessRulesPanel;
    public GameObject pawnsPanel;
    public GameObject movesPanel;

    public AudioSource completionistTheme;
    public AudioSource bonusGameTheme;
    public AudioSource chessTheme;
    public AudioSource oppTheme;
    public AudioSource bookPage;
    public AudioSource oppWinTheme;
    public AudioSource oppLoseTheme;
    public AudioSource correctly;
    public AudioSource incorrectly;

    Interactable currentInteractable;
    Interactable disabledInteractable;

    Vector3 savedPlayerPos;
    Quaternion savedPlayerRot;
    bool inChessMode = false;

    BonusTriggerManager matcherBT, evaderBT, tracerBT, sliderBT;

    public GameObject tracerMedal, matcherMedal, sliderMedal, evaderMedal;

    public enum BonusGameDifficulty
    {
        Normal,
        Expert,
        EpicGamer
    }

    public static BonusGameDifficulty difficultyLevel;

    void Start()
    {
        // Cache trigger managers (saves tons of GetComponent calls)
        matcherBT = matcherTrigger.GetComponent<BonusTriggerManager>();
        evaderBT = evaderTrigger.GetComponent<BonusTriggerManager>();
        tracerBT = tracerTrigger.GetComponent<BonusTriggerManager>();
        sliderBT = sliderTrigger.GetComponent<BonusTriggerManager>();

        inputField.text = "";
        wordInput.SetActive(false);
        inputField.onEndEdit.AddListener(ReadStringInput);

        cursorIcon.SetActive(false);
        raycastIcon.SetActive(true);

        if (goopy != null) goopy.Stop();

        oppDescription.SetActive(false);
        flipOff.SetActive(false);
        mysteryBag.SetActive(false);
        present.SetActive(false);
        oppExit.SetActive(false);

        chessIntroPanel.SetActive(false);

        wordCorrect = false;
        channelOptions.SetActive(false);

        tracerMedal.SetActive(false);
        evaderMedal.SetActive(false);
        matcherMedal.SetActive(false);
        sliderMedal.SetActive(false);
        

        if (chessCam != null) chessCam.gameObject.SetActive(false);
        if (ChessManager.Instance != null) ChessManager.Instance.enabled = true;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("SecretWordCracked=" + PlayerPrefs.GetInt("SecretWordCracked", 0));
            Debug.Log("NormalSliderComplete=" + PlayerPrefs.GetInt("NormalSliderComplete", 0));
            Debug.Log("ExpertSliderComplete=" + PlayerPrefs.GetInt("ExpertSliderComplete", 0));
            Debug.Log("EpicGamerSliderComplete=" + PlayerPrefs.GetInt("EpicGamerSliderComplete", 0));
            Debug.Log("SliderModesComplete=" + PlayerPrefs.GetInt("SliderModesComplete", 0));
        }

        // While in chess mode, you probably don't want normal interact scanning
        if (!inChessMode)
            CheckInteraction();

        if (wordInput.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            CloseWordInput();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryHandlePanelClick();
        }

        if (PlayerPrefs.GetInt("SecretWordCracked", 0) == 1)
        {
            foreach (GameObject hint in wordHints)
            {
                hint.SetActive(false);
            }

            if (PlayerPrefs.GetInt("TracerModesComplete", 0) == 1)
            {
                tracerMedal.SetActive(true);
            }

            if (PlayerPrefs.GetInt("EvaderModesComplete", 0) == 1)
            {
                evaderMedal.SetActive(true);
            }

            if (PlayerPrefs.GetInt("MatcherModesComplete", 0) == 1)
            {
                matcherMedal.SetActive(true);
            }

            if (PlayerPrefs.GetInt("SliderModesComplete", 0) == 1)
            {
                sliderMedal.SetActive(true);
            }
        }

        timesWonReport.text = string.Format("Times Won: " + PlayerPrefs.GetInt("OPPTimesWon", 0));

    }

    void TryHandlePanelClick()
    {
        if (currentInteractable == null) return;
        if (currentInteractable.modePanel == null) return;
        if (!currentInteractable.modePanel.activeInHierarchy) return;

        string panelName = currentInteractable.modePanel.gameObject.name;

        switch (panelName)
        {
            case "TracerPreviewPanel":
                tracerBT.modeSelectPanel.SetActive(true);
                break;

            case "EvaderPreviewPanel":
                evaderBT.modeSelectPanel.SetActive(true);
                break;

            case "SliderPreviewPanel":
                sliderBT.modeSelectPanel.SetActive(true);
                break;

            case "MatcherPreviewPanel":
                matcherBT.modeSelectPanel.SetActive(true);
                break;

            case "OnePercentPresentPreviewPanel":
                oppDescription.SetActive(true);
                break;

            case "ChessPreviewPanel":
                EnterChessMode();
                break;
            case "ChessRulesPreviewPanel":
                chessRulesPanel.SetActive(true);
                bookPage.Play();
                break;

            case "SecretWordPreviewPanel":
                wordInput.SetActive(true);
                inputField.text = "";
                inputField.ActivateInputField();
                break;

            case "ChangeChannelOffer":
                channelOptions.SetActive(true);
                break;

            default:
                return;
        }

        // General “entered a UI interaction” behavior
        pauseScript.interacting = true;
        currentInteractable.modePanel.SetActive(false);

        raycastIcon.SetActive(false);
        cursorIcon.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        movementScript.enabled = false;

        disabledInteractable = currentInteractable;
        currentInteractable.enabled = false;
    }

    public void CheckInteraction()
    {
        if ((PlayerPrefs.GetInt("SecretWordCracked", 0) == 1) && wordInteract != null)
        {
            Interactable wordInt = wordInteract.GetComponent<Interactable>();
            if (wordInt != null) wordInt.enabled = false;
        }

        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        bool bonusAllowed = matcherBT.spotDetected || tracerBT.spotDetected || evaderBT.spotDetected || sliderBT.spotDetected;

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            // Bonus interactables
            if (bonusAllowed && hit.collider.CompareTag("Interactable"))
            {
                TrySetInteractable(hit.collider.GetComponent<Interactable>());
                return;
            }

            // Non-bonus interactables
            if (hit.collider.CompareTag("NonBonusInteract"))
            {
                TrySetInteractable(hit.collider.GetComponent<Interactable>());
                return;
            }
        }

        DisableCurrentInteractable();
    }

    void TrySetInteractable(Interactable newInteractable)
    {
        if (newInteractable == null || !newInteractable.enabled)
        {
            DisableCurrentInteractable();
            return;
        }

        if (currentInteractable != null && newInteractable != currentInteractable)
            currentInteractable.DisableOutline();

        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
    }

    void DisableCurrentInteractable()
    {
        if (currentInteractable != null)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }

    public void EnablePlayerInteractions()
    {
        raycastIcon.SetActive(true);
        movementScript.enabled = true;
        cursorIcon.SetActive(false);

        if (disabledInteractable != null)
            disabledInteractable.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseScript.interacting = false;
    }

    // -------------------- Secret Word --------------------

    public void ReadStringInput(string s)
    {
        string input = s.Trim().ToUpper();
        wordCorrect = (input == "PNEUMONOULTRAMICROSCOPICSILICOVOLCANOCONIOSIS");
        if (wordCorrect)
        {
            correctly.Play();
            PlayerPrefs.SetInt("SecretWordCracked", 1);
            PlayerPrefs.Save();
        }
        else
        {
            incorrectly.Play();
        }
        CloseWordInput();
    }

    void CloseWordInput()
    {
        wordInput.SetActive(false);
        inputField.text = "";
        inputField.DeactivateInputField();

        EnablePlayerInteractions();
    }

    // -------------------- One Percent Present --------------------

    public void PlayOPP() => StartCoroutine(LoadOPP());

    IEnumerator LoadOPP()
    {
        oppDescription.SetActive(false);
        completionistTheme.Stop();
        tvScript.player.Pause();
        oppTheme.Play();
        if (goopy != null) goopy.Play();

        yield return new WaitForSeconds(0.6f);
        mysteryBag.SetActive(true);
    }

    public void RevealGift()
    {
        mysteryBag.SetActive(false);
        oppExit.SetActive(true);
        oppTheme.Stop();

        presentProb = Random.Range(0, 100);
        
        if (presentProb == 69)
        {
            present.SetActive(true);
            oppWinTheme.Play();
            PlayerPrefs.SetInt("OPPTimesWon", PlayerPrefs.GetInt("OPPTimesWon", 0) + 1);
            PlayerPrefs.Save();
        }
        else
        {
            oppLoseTheme.Play();
            flipOff.SetActive(true);
        }
        
    }

    public void ExitOPP()
    {
        completionistTheme.Play();
        tvScript.player.Play();
        oppExit.SetActive(false);
        present.SetActive(false);
        flipOff.SetActive(false);
        EnablePlayerInteractions();
    }

    // -------------------- Chess --------------------

    void EnterChessMode()
    {
        if (inChessMode) return;
        StartCoroutine(EnterChessRoutine());
    }

    IEnumerator EnterChessRoutine()
    {
        if (goopy != null) goopy.Play();
        yield return new WaitForSeconds(0.5f);

        savedPlayerPos = player.transform.position;
        savedPlayerRot = player.transform.rotation;

        // Stop FPS control
        movementScript.enabled = false;

        // Switch cameras
        if (playerCam != null) playerCam.gameObject.SetActive(false);
        if (chessCam != null) chessCam.gameObject.SetActive(true);

        // Show chess intro (Begin button should call ChessManager.BeginGame())
        chessIntroPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        

        inChessMode = true;
        completionistTheme.Stop();
        tvScript.player.Pause();
        chessTheme.Play();
    }

    // Hook this to a UI button like “Exit Chess” or call it from ChessManager.EndGame()
    public void ExitChessMode()
    {
        if (!inChessMode) return;

        // Restore player
        player.transform.position = savedPlayerPos;
        player.transform.rotation = savedPlayerRot;

        // Switch cameras
        if (chessCam != null) chessCam.gameObject.SetActive(false);
        if (playerCam != null) playerCam.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        

        inChessMode = false;
        chessTheme.Stop();
        tvScript.player.Play();

        completionistTheme.Play();

        EnablePlayerInteractions();
    }

    public void LoadPawnPage()
    {
        movesPanel.SetActive(false);
        pawnsPanel.SetActive(true);
        bookPage.Play();
    }
    public void LoadMovesPage()
    {
        movesPanel.SetActive(true);
        pawnsPanel.SetActive(false);
        bookPage.Play();
    }
    public void CloseRules()
    {
        chessRulesPanel.SetActive(false);
        bookPage.Play();
        EnablePlayerInteractions();
    }

    public void SelectMovie(VideoClip movie)
    {
        tvScript.selectedClip = movie;
        tvScript.player.Stop();
        tvScript.player.clip = tvScript.selectedClip;
        tvScript.player.Play();
        channelOptions.SetActive(false);
        EnablePlayerInteractions();
    }

    public void CancelMovieSelection()
    {
        channelOptions.SetActive(false);
        EnablePlayerInteractions();
    }

    public void ExitBonusGameMenu()
    {
        tracerTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        evaderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        matcherTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        sliderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);

        EnablePlayerInteractions();
    }

    public void NormalDifficulty()
    {
        obstacleScript.headdropInterval = 2f;
        obstacleScript.splitInterval = 4f;
        obstacleScript.sideSlideInterval = 5f;

        obstacleScript.headdropChance = 1f;
        obstacleScript.splitChance = 0.7f;
        obstacleScript.sideSlideChance = 0.5f;

        evaderScript.gameTime = 60;
        sliderScript.gameTime = 180;

        matcherScript.gameTime = 45;
        matcherScript.failedAttempts = 8;

        tracerScript.gameTime = 10;
        tracerScript.shuffleSpeed = 0.5f;
        tracerScript.shuffleCount = Random.Range(12, 17);
        difficultyLevel = BonusGameDifficulty.Normal;

        StartCoroutine(LoadBonusDescription());
    }

    public void ExpertDifficulty()
    {
        obstacleScript.headdropInterval = 1f;
        obstacleScript.splitInterval = 2f;
        obstacleScript.sideSlideInterval = 2.5f;

        obstacleScript.headdropChance = 1f;
        obstacleScript.splitChance = 0.7f;
        obstacleScript.sideSlideChance = 0.5f;

        evaderScript.gameTime = 90;
        sliderScript.gameTime = 120;

        matcherScript.gameTime = 35;
        matcherScript.failedAttempts = 6;

        tracerScript.gameTime = 8;
        tracerScript.shuffleSpeed = 0.25f;
        tracerScript.shuffleCount = Random.Range(12, 17);
        difficultyLevel = BonusGameDifficulty.Expert;

        StartCoroutine(LoadBonusDescription());
    }

    public void EpicGamerDifficulty()
    {
        obstacleScript.headdropInterval = 1f;
        obstacleScript.splitInterval = 2f;
        obstacleScript.sideSlideInterval = 2.5f;

        obstacleScript.headdropChance = 1f;
        obstacleScript.splitChance = 0.7f;
        obstacleScript.sideSlideChance = 0.5f;

        evaderScript.gameTime = 120;
        sliderScript.gameTime = 60;

        matcherScript.gameTime = 25;
        matcherScript.failedAttempts = 4;

        tracerScript.gameTime = 5;
        tracerScript.shuffleSpeed = 0.15f;
        tracerScript.shuffleCount = Random.Range(20, 26);
        difficultyLevel = BonusGameDifficulty.EpicGamer;

        StartCoroutine(LoadBonusDescription());
    }

    private IEnumerator LoadBonusDescription()
    {
        matcherTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = false;
        evaderTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = false;
        sliderTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = false;
        tracerTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = false;

        matcherTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = false;
        evaderTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = false;
        sliderTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = false;
        tracerTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = false;

        matcherTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = false;
        evaderTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = false;
        sliderTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = false;
        tracerTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = false;

        matcherTrigger.GetComponent<BonusTriggerManager>().exit.enabled = false;
        sliderTrigger.GetComponent<BonusTriggerManager>().exit.enabled = false;
        evaderTrigger.GetComponent<BonusTriggerManager>().exit.enabled = false;
        tracerTrigger.GetComponent<BonusTriggerManager>().exit.enabled = false;

        goopy.Play();

        yield return new WaitForSeconds(0.6f);

        if (completionistTheme.isPlaying) completionistTheme.Stop();
        bonusGameTheme.Play();

        if (matcherTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.activeInHierarchy)
        {
            
            matcherScript.matchingGameDescription.SetActive(true);
            player.transform.position = matcherPos.transform.position;
            player.transform.rotation = matcherPos.transform.rotation;
            matcherTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        }
        else if (evaderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.activeInHierarchy)
        {
            evaderScript.evaderGameDescription.SetActive(true);
            player.transform.position = evaderPos.transform.position;
            player.transform.rotation = evaderPos.transform.rotation;
            evaderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        }
        else if (sliderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.activeInHierarchy)
        {
            sliderScript.slidingGameDescription.SetActive(true);
            player.transform.position = sliderPos.transform.position;
            player.transform.rotation = sliderPos.transform.rotation;
            sliderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        }
        else
        {
            tracerScript.tracerGameDescription.SetActive(true);
            player.transform.position = tracerPos.transform.position;
            player.transform.rotation = tracerPos.transform.rotation;
            tracerTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        }

        matcherTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;
        evaderTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;
        sliderTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;
        tracerTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;

        matcherTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;
        evaderTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;
        sliderTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;
        tracerTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;

        matcherTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;
        evaderTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;
        sliderTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;
        tracerTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;

        matcherTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
        sliderTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
        evaderTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
        tracerTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
    }
}