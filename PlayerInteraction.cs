using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteraction : MonoBehaviour
{
    public AudioSource selectTheme;
    public float loopStart = 8f;
    public float loopEnd = 50f;
    Interactable currentInteractable;

    public GameObject auctionModePanel;
    public GameObject bossModePanel;

    float playerReach = 20f;
    private bool introPlayed;

    // Start is called before the first frame update
    void Start()
    {
        selectTheme.time = 0f;
        selectTheme.Play();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteraction();
        if (Input.GetMouseButtonDown(0) && currentInteractable != null)
        {
            currentInteractable.Interact();

            if (bossModePanel.activeInHierarchy)
            {
                SceneManager.LoadScene("AlchemistAuctionFleeceBoss");
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
            selectTheme.Play();
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
        currentInteractable = newInteractable;
        currentInteractable.EnableOutline();
        Debug.Log("Interactable set!");
    }

    void DisableCurrentInteractable()
    {
        if (currentInteractable)
        {
            currentInteractable.DisableOutline();
            currentInteractable = null;
        }
    }
}
