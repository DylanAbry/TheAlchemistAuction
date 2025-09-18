using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FleeceBossIntro : MonoBehaviour
{
    public Camera mainCam;
    private int jumpscareDelay;
    public Animator jumpscare;
    public Animator shuttleIdle;

    public PlayerMovement movementScript;
    public AudioSource jumpscareSound;
    public GameObject shuttle;
    public GameObject playerRPGMarker;
    public GameObject shuttleMarker;

    public GameObject drEffect;

    public float speed = 5f;

    private bool battleStageSet;

    // Start is called before the first frame update
    void Start()
    {
        jumpscareDelay = Random.Range(5, 25);
        battleStageSet = false;
        drEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (battleStageSet)
        {
            shuttle.transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
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

        if (shuttleIdle.GetCurrentAnimatorStateInfo(0).IsName("ShuttleIdle"))
        {
            shuttleIdle.enabled = false;
        }
        drEffect.SetActive(true);
        drEffect.GetComponent<Animator>().Play("DrDeFraudEntranceEffect");

        yield return new WaitForSeconds(0.45f);

        mainCam.transform.SetParent(shuttle.transform);

        battleStageSet = true;

        shuttle.transform.position = shuttleMarker.transform.position;
        shuttle.transform.rotation = shuttleMarker.transform.rotation;

        mainCam.transform.position = playerRPGMarker.transform.position;
        mainCam.transform.rotation = playerRPGMarker.transform.rotation;

    }
}
