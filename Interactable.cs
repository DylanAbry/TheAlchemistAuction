using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    Outline outline;

    public UnityEvent onInteraction;

    public GameObject modePanel;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        modePanel.SetActive(false);
        DisableOutline();
    }

    public void Interact()
    {
        onInteraction.Invoke();


    }

    public void DisableOutline()
    {
        outline.enabled = false;
        modePanel.SetActive(false);
    }
    public void EnableOutline()
    {
        outline.enabled = true;
        modePanel.SetActive(true);
    }
}
