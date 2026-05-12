using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public AudioSource selectTheme;
    public AudioSource fleeceFaceOffSound;
    public AudioSource defraudsDomeSound;
    public AudioSource completionistSound;
    bool domeSoundPlayed;
    bool fleeceSoundPlayed;
    bool ccSoundPlayed;
    public float loopStart = 8f;
    public float loopEnd = 50f;
    Interactable currentInteractable;

    public GameObject auctionModePanel;
    public GameObject bossModePanel;
    public GameObject compModePanel;

    public GameObject loadScreen;

    public GameObject bossDoors;
    public GameObject completionistDoors;

    float playerReach = 20f;
    private bool introPlayed;

    // Start is called before the first frame update
    void Start()
    {
        selectTheme.time = 0f;
        selectTheme.Play();
        loadScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            loadScreen.SetActive(true);
            currentInteractable.Interact();
            
            if (bossModePanel.activeInHierarchy)
            {
                SceneManager.LoadScene("AlchemistAuctionFleeceBoss");
            }
            else if (compModePanel.activeInHierarchy)
            {
                SceneManager.LoadScene("CompletionistCave");
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("AlchemistAuctionMainGame");
            }
        }

        if (!introPlayed && selectTheme.time >= loopStart)
        {
            introPlayed = true;
        }

        if (introPlayed && selectTheme.time >= loopEnd)
        {
            selectTheme.time = loopStart;
        }
    }

    public void CheckInteraction()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out hit, playerReach))
        {
            if (hit.collider.tag == "Interactable")
            {
                Interactable newInteractable = hit.collider.GetComponent<Interactable>();


                if (currentInteractable && newInteractable != currentInteractable)
                {
                    currentInteractable.DisableOutline();
                }
                if (newInteractable.enabled)
                {
                    SetNewCurrentInteractable(newInteractable);
                }
                else // If interactable is not enabled
                {
                    DisableCurrentInteractable();
                }
            }
            else
            {
                DisableCurrentInteractable();
            }
        }
    }
    void SetNewCurrentInteractable(Interactable newInteractable)
    {
        if (newInteractable.gameObject.name == "AuctionDoors")
        {
            currentInteractable = newInteractable;
            currentInteractable.EnableOutline();
            if (!domeSoundPlayed)
            {
                defraudsDomeSound.Play();
                domeSoundPlayed = true;
            }
        }
        else if (newInteractable.gameObject.name == "FleeceDoors")
        {
            if (PlayerPrefs.GetInt("UnlockedFleeceFinalFaceOff", 0) != 1)
            {
                return;
            }
            else
            {
                currentInteractable = newInteractable;
                currentInteractable.EnableOutline();
                if (!fleeceSoundPlayed)
                {
                    fleeceFaceOffSound.Play();
                    fleeceSoundPlayed = true;
                }
            }
        }
        else if (newInteractable.gameObject.name == "BonusDoors")
        {
            if (PlayerPrefs.GetInt("UnlockedCompletionistCave", 0) != 1)
            {
                return;
            }
            else
            {
                currentInteractable = newInteractable;
                currentInteractable.EnableOutline();
                if (!ccSoundPlayed)
                {
                    completionistSound.Play();
                    ccSoundPlayed = true;
                }
                currentInteractable = newInteractable;
                currentInteractable.EnableOutline();
            }
        }
        else
        {
            
        }
    }

    void DisableCurrentInteractable()
    {
        if (currentInteractable)
        {
            if (currentInteractable.gameObject.name == "FleeceDoors")
                fleeceSoundPlayed = false;

            if (currentInteractable.gameObject.name == "AuctionDoors")
                domeSoundPlayed = false;

            if (currentInteractable.gameObject.name == "BonusDoors")
                ccSoundPlayed = false;


            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
