using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideButtonManagement : MonoBehaviour
{

    public GameObject pauseScreen;

    public GameObject potionsTierButton;
    public GameObject increaseBidButton;
    public GameObject collectionsButton;
    public GameObject tradeInButton;
    public GameObject mixButton;


    public Animator mixScreen;
    public Animator collectionsScreen;
    public Animator tradeInScreen;
    public Animator potionScreen;

    void Start()
    {
        pauseScreen.SetActive(false);
    }
    public void PauseGame()
    {
        pauseScreen.SetActive(true);
    }
    public void ResumeGame()
    {
        pauseScreen.SetActive(false);
    }
    public void CollectionsEnter()
    {
        collectionsScreen.Play("CollectionsEnter");
    }
    public void CollectionsExit()
    {
        collectionsScreen.Play("CollectionsExit");
    }
    public void TradeInEnter()
    {
        tradeInScreen.Play("TradeInEnter");
    }
    public void TradeInExit()
    {
        tradeInScreen.Play("TradeInExit");
    }
    public void MixingEnter()
    {
        mixScreen.Play("MixingEnter");
    }
    public void MixingExit()
    {
        mixScreen.Play("MixingExit");
    }
    public void PotionTiersEnter()
    {
        potionScreen.Play("PotionTiersEnter");
    }
    public void PotionTiersExit()
    {
        potionScreen.Play("PotionTiersExit");
    }
}
