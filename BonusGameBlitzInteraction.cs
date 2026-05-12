using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusGameBlitzInteraction : MonoBehaviour
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
    public BGBEvaderManager evaderScript;
    public BGBTracerManager tracerScript;
    public BGBMatcherManager matcherScript;
    public BGBSliderManager sliderScript;
    public BGBlitzManager blitzScript;

    [Header("FX")]
    public ParticleSystem goopy;

    [Header("Raycast")]
    [SerializeField] float playerReach = 20f;

    public AudioSource bonusGameTheme;
    public AudioSource bgbTheme;
    
    Interactable currentInteractable;
    Interactable disabledInteractable;

    Vector3 savedPlayerPos;
    Quaternion savedPlayerRot;

    BonusTriggerManager matcherBT, evaderBT, tracerBT, sliderBT;


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

        cursorIcon.SetActive(false);
        raycastIcon.SetActive(true);

        if (goopy != null) goopy.Stop();
    }

    void Update()
    {
        CheckInteraction();

        if (Input.GetMouseButtonDown(0))
        {
            TryHandlePanelClick();
        }
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
        blitzScript.scorePanel.SetActive(false);
        blitzScript.bestScorePanel.SetActive(false);

        disabledInteractable = currentInteractable;
        currentInteractable.enabled = false;
    }

    public void CheckInteraction()
    {
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

    public void ExitBonusGameMenu()
    {
        tracerTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        evaderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        matcherTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        sliderTrigger.GetComponent<BonusTriggerManager>().modeSelectPanel.SetActive(false);
        blitzScript.scorePanel.SetActive(true);
        if (blitzScript.hasPlayed == 1)
        {
            blitzScript.bestScorePanel.SetActive(true);
        }
        else
        {
            blitzScript.bestScorePanel.SetActive(false);
        }

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

        if (bgbTheme.isPlaying) bgbTheme.Stop();
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

        if (!matcherScript.normalBeat) matcherTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;
        if (!evaderScript.normalBeat) evaderTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;
        if (!sliderScript.normalBeat) sliderTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;
        if (!tracerScript.normalBeat) tracerTrigger.GetComponent<BonusTriggerManager>().normalButton.enabled = true;

        if (!matcherScript.expertBeat) matcherTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;
        if (!evaderScript.expertBeat) evaderTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;
        if (!sliderScript.expertBeat) sliderTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;
        if (!tracerScript.expertBeat) tracerTrigger.GetComponent<BonusTriggerManager>().expertButton.enabled = true;

        if (!matcherScript.egBeat) matcherTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;
        if (!evaderScript.egBeat) evaderTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;
        if (!sliderScript.egBeat) sliderTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;
        if (!tracerScript.egBeat) tracerTrigger.GetComponent<BonusTriggerManager>().epicButton.enabled = true;

        matcherTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
        sliderTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
        evaderTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
        tracerTrigger.GetComponent<BonusTriggerManager>().exit.enabled = true;
    }
}
