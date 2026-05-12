using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using TMPro;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public PlayerInteraction interactScript;
    public FleeceBossIntro flcIntScript;
    public DefraudBossBattleManager battleScript;
    public CompletionistInteraction compScript;
    public CompletionistTVPlayer tvScript;
    public float mouseSensitivity = 2f;

    //public BGBlitzManager blitzScript;

    private Rigidbody rb;
    private Transform cam;
    private float cameraPitch = 0f;

    bool playerMove;

    public GameObject pauseMenu;
    public GameObject gameCursorIcon;
    public AudioSource pauseTheme;


    void Start()
    {
        playerMove = false;
        StartCoroutine(PlayerMoveAllow());
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        gameCursorIcon.SetActive(false);
    }

    private IEnumerator PlayerMoveAllow()
    {
        yield return new WaitForSeconds(0.75f);
        playerMove = true;
    }

    void Update()
    {
        if (playerMove) HandleMouseLook();

        if (Input.GetButtonDown("Cancel") && !pauseMenu.activeInHierarchy)
        {
            /**if (SceneManager.GetActiveScene().name == "BonusGameBlitz")
            {
                if (!blitzScript.gameActive) return;
                if (blitzScript.bgbMainTheme.isPlaying) blitzScript.bgbMainTheme.Pause();
                Time.timeScale = 0f;
                pauseTheme.Play();
                gameCursorIcon.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);

            }**/





            if (SceneManager.GetActiveScene().name == ("AlchemistAuctionFleeceBoss"))
            {
                if (flcIntScript.jumpscareHappened || FleeceBossIntro.blockPauseInput)
                {
                    return;
                }
                else
                {
                    if (flcIntScript.officeSuspense.isPlaying) flcIntScript.officeSuspense.Stop();
                    if (flcIntScript.officeAmbiance.isPlaying) flcIntScript.officeAmbiance.Stop();
                    Cursor.lockState = CursorLockMode.None;
                    gameCursorIcon.SetActive(true);
                    pauseMenu.SetActive(true);
                    pauseTheme.Play();
                }
            }
            else if (SceneManager.GetActiveScene().name == ("CompletionistCave")) 
            {
                if (compScript.completionistTheme.isPlaying) compScript.completionistTheme.Stop();
                tvScript.player.Pause();
                pauseTheme.Play();
                gameCursorIcon.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
            } 
            else if (SceneManager.GetActiveScene().name == ("AlchemistAuctionSelectGame"))
            {
                if (interactScript.selectTheme.isPlaying) interactScript.selectTheme.Stop();
                pauseTheme.Play();
                gameCursorIcon.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
            } 
            else
            {

            }
        }
    }

    void FixedUpdate()
    {
        if (playerMove) HandleMovement();
    }

    void HandleMouseLook()
    {
        //if (!blitzScript.gameActive) return;
        if (cam == null)
            RefreshCamera();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player (horizontal)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera (vertical)
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);
        cam.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    void HandleMovement()
    {
        //if (!blitzScript.gameActive) return;
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * inputX + transform.forward * inputZ;
        Vector3 targetVelocity = moveDirection * moveSpeed;
        Vector3 currentVelocity = rb.velocity;

        // Preserve current Y velocity (gravity, falling, etc.)
        rb.velocity = new Vector3(targetVelocity.x, currentVelocity.y, targetVelocity.z);
    }

    public void ReleaseCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ReturnToPlaying()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        if (pauseTheme.isPlaying)
        {
            pauseTheme.Stop();
        }
        Cursor.lockState = CursorLockMode.Locked;
        gameCursorIcon.SetActive(false);
        //blitzScript.bgbMainTheme.Play();

        if (SceneManager.GetActiveScene().name == ("CompletionistCave"))
        {
            tvScript.player.Play();
            compScript.completionistTheme.Play();
            Cursor.lockState = CursorLockMode.Locked;
            gameCursorIcon.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == ("AlchemistAuctionSelectGame"))
        {
            interactScript.selectTheme.time = interactScript.loopStart;
            interactScript.selectTheme.Play();
            Cursor.lockState = CursorLockMode.Locked;
            gameCursorIcon.SetActive(false);
        }
        else if (SceneManager.GetActiveScene().name == ("AlchemistAuctionFleeceBoss"))
        {
            if (flcIntScript.jumpscareHappened)
            {
                battleScript.pauseTheme.Stop();
                if (battleScript.phaseOne)
                {
                    battleScript.phaseOneTheme.Play();
                }
                else if (battleScript.phaseTwo)
                {
                    battleScript.phaseTwoTheme.Play();
                }
                else if (battleScript.phaseThree)
                {
                    battleScript.phaseThreeTheme.Play();
                }
                else
                {

                }
            }
            else
            {
                flcIntScript.officeSuspense.Play();
                flcIntScript.officeAmbiance.Play();
                Cursor.lockState = CursorLockMode.Locked;
                gameCursorIcon.SetActive(false);
            }
        }
    }

    public void RefreshCamera()
    {
        if (Camera.main != null)
            cam = Camera.main.transform;
    }
}
