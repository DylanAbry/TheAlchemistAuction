using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusTriggerManager : MonoBehaviour
{
    public bool spotDetected;
    public GameObject modeSelectPanel;
    public Interactable interObject;
    public Button normalButton;
    public Button expertButton;
    public Button epicButton;
    public Button exit;

    // Start is called before the first frame update
    void Start()
    {
        spotDetected = false;
        modeSelectPanel.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spotDetected = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            spotDetected = false;
        }
        interObject.DisableOutline();
    }
}
