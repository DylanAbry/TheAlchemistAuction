using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using TMPro;

public class FleeceBossIntro : MonoBehaviour
{
    public Camera mainCam;
    public DefraudBossBattleManager battleScript;
    private int jumpscareDelay;
    public Animator jumpscare;
    public Animator shuttleIdle;
    public Animator fleeceBattlePos;

    public PlayerMovement movementScript;
    public AudioSource jumpscareSound;
    public AudioSource officeSuspense;
    public AudioSource officeAmbiance;
    public AudioSource introTheme;
    public AudioSource shuttleSound;
    public GameObject shuttle;
    public GameObject playerRPGMarker;
    public GameObject shuttleMarker;
    public GameObject cursorImage;
    public GameObject gamePlayButton;

    public GameObject drEffect;

    public float speed = 5f;

    public bool battleStageSet, jumpscareHappened, startFight;
    private const float resetDistance = 6700f;

    public GameObject jumpScareIcon;
    public GameObject[] deFraudFaces;
    public GameObject[] deFraudIntroPanels;

    private int currentFleecePanel;

    public static bool blockPauseInput = false;

    // Start is called before the first frame update
    void Start()
    {
        jumpscareDelay = Random.Range(5, 25);
        battleStageSet = false;
        jumpscareHappened = false;
        startFight = false;
        drEffect.SetActive(false);
        cursorImage.SetActive(false);
        currentFleecePanel = 0;

        foreach (GameObject panel in deFraudIntroPanels)
        {
            panel.SetActive(false);
        }
        for (int i = 0; i < deFraudFaces.Length; i++)
        {
            deFraudFaces[i].SetActive(false);
        }
    }

    void Update()
    {
        if (battleStageSet)
        {
            shuttle.transform.position += Vector3.forward * speed * Time.deltaTime;

            // Prevent floating-point precision issues
            if (shuttle.transform.position.magnitude > resetDistance)
            {
                shuttle.transform.position = new Vector3(0, 0, 3000);
            }
        }
        if (jumpscareHappened)
        {
            gamePlayButton.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }
        }
        else
        {
            gamePlayButton.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !jumpscareHappened)
        {
            if (officeSuspense.isPlaying) officeSuspense.Stop();
            if (officeAmbiance.isPlaying) officeAmbiance.Stop();
            jumpscareHappened = true;
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            blockPauseInput = true;

            StartCoroutine(JumpscareSequence());
            
        }
    }

    private IEnumerator JumpscareSequence()
    {
        yield return new WaitForSeconds(jumpscareDelay);
        jumpscare.Play("FleeceJumpscare");
        jumpscareSound.Play();
        gameObject.GetComponent<Collider>().isTrigger = false;
        movementScript.enabled = false;
        movementScript.ReleaseCursor();

        if (shuttleIdle.GetCurrentAnimatorStateInfo(0).IsName("ShuttleIdle"))
        {
            shuttleIdle.enabled = false;
        }
        drEffect.SetActive(true);
        drEffect.GetComponent<Animator>().Play("DrDeFraudEntranceEffect");

        yield return new WaitForSeconds(0.45f);

        mainCam.transform.SetParent(shuttle.transform);
        shuttleSound.Play();

        battleStageSet = true;

        shuttle.transform.position = shuttleMarker.transform.position;
        shuttle.transform.rotation = shuttleMarker.transform.rotation;

        mainCam.transform.position = playerRPGMarker.transform.position;
        mainCam.transform.rotation = playerRPGMarker.transform.rotation;

        yield return new WaitForSeconds(2f);
        introTheme.Play();
        jumpScareIcon.SetActive(false);
        deFraudFaces[5].SetActive(true);
        yield return new WaitForSeconds(1.5f);
        deFraudFaces[5].SetActive(false);
        deFraudFaces[1].SetActive(true);
        deFraudIntroPanels[0].SetActive(true);
        movementScript.ReleaseCursor();
        cursorImage.SetActive(true);

    }

    public void NextFleeceBossIntroPanel()
    {
        if (currentFleecePanel < deFraudIntroPanels.Length - 1)
        {
            StartCoroutine(BossTransitionPanel());
        }
        else
        {
            StartCoroutine(PrepareFleeceFaceOff());
        }
        
    }

    private IEnumerator BossTransitionPanel()
    {
        deFraudIntroPanels[currentFleecePanel].SetActive(false);
        if (currentFleecePanel == 0)
        {
            deFraudFaces[1].SetActive(false);
            deFraudFaces[5].SetActive(true);
            yield return new WaitForSeconds(0.75f);
            deFraudFaces[11].SetActive(true);
            deFraudFaces[5].SetActive(false);
        }
        if (currentFleecePanel == 1)
        {
            yield return new WaitForSeconds(1.2f);
            deFraudFaces[11].SetActive(false);
            deFraudFaces[10].SetActive(true);
        }
        if (currentFleecePanel == 2)
        {
            yield return new WaitForSeconds(0.75f);
            deFraudFaces[10].SetActive(false);
            deFraudFaces[7].SetActive(true);
        }
        if (currentFleecePanel == 3)
        {
            deFraudFaces[7].SetActive(false);
            deFraudFaces[14].SetActive(true);
            yield return new WaitForSeconds(0.75f);
            deFraudFaces[14].SetActive(false);
            deFraudFaces[1].SetActive(true);

        }
        if (currentFleecePanel == 4)
        {
            deFraudFaces[1].SetActive(false);
            deFraudFaces[5].SetActive(true);
            yield return new WaitForSeconds(1.5f);
            deFraudFaces[5].SetActive(false);
            deFraudFaces[10].SetActive(true);
        }
        if (currentFleecePanel == 5)
        {
            yield return new WaitForSeconds(0.75f);
            deFraudFaces[10].SetActive(false);
            deFraudFaces[8].SetActive(true);
        }
        if (currentFleecePanel == 6)
        {
            deFraudFaces[8].SetActive(false);
            deFraudFaces[13].SetActive(true);
            yield return new WaitForSeconds(0.75f);
            deFraudFaces[13].SetActive(false);
            deFraudFaces[0].SetActive(true);
        }
        if (currentFleecePanel == 7)
        {
            deFraudFaces[0].SetActive(false);
            deFraudFaces[6].SetActive(true);
            yield return new WaitForSeconds(0.75f);
            deFraudFaces[6].SetActive(false);
            deFraudFaces[3].SetActive(true);
        }
        if (currentFleecePanel == 8)
        {
            deFraudFaces[3].SetActive(false);
            deFraudFaces[11].SetActive(true);
            yield return new WaitForSeconds(2f);
            deFraudFaces[11].SetActive(false);
            deFraudFaces[1].SetActive(true);
        }
        
        currentFleecePanel++;
        deFraudIntroPanels[currentFleecePanel].SetActive(true);
    }

    private IEnumerator PrepareFleeceFaceOff()
    {
        deFraudIntroPanels[currentFleecePanel].SetActive(false);
        deFraudFaces[1].SetActive(false);
        deFraudFaces[5].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        fleeceBattlePos.Play("PrepareFleeceForBattle");
        if (introTheme.isPlaying) introTheme.Stop();
        battleScript.phaseOneTheme.Play();
        yield return new WaitForSeconds(4f);
        battleScript.SetupFirstPlayerTurn();
    }
    public void ClosePauseMenu()
    {
        battleScript.pauseMenu.SetActive(false);
        if (!jumpscareHappened)
        {
            Cursor.lockState = CursorLockMode.Locked;
            cursorImage.SetActive(false);
            return;
        }

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
}
