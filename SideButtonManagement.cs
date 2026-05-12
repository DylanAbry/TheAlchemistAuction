using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideButtonManagement : MonoBehaviour
{
    public IntroManager introScript;
    public GameObject pauseScreen;
    public AudioSource pauseTheme;
    public AudioSource mainTheme;

    public GameObject potionsTierButton;
    public GameObject increaseBidButton;
    public GameObject collectionsButton;
    public GameObject tradeInButton;
    public GameObject mixButton;
    public GameObject biddersGuideButton;
    public GameObject biddersGuide;
    public GameObject pauseButton;
    public GameObject phalangesPanelParent;
    public GameObject phalangesNextPanel;

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
        collectionsButton.SetActive(false);
        potionsTierButton.SetActive(false);
        pauseButton.SetActive(false);
        tradeInButton.SetActive(false);
        biddersGuideButton.SetActive(false);
        phalangesPanelParent.SetActive(false);
        mixButton.SetActive(false);
        phalangesNextPanel.SetActive(false);
        if (mainTheme.isPlaying) mainTheme.Pause();
        pauseTheme.Play();
    }
    public void ResumeGame()
    {
        pauseTheme.Stop();
        mainTheme.Play();
        pauseScreen.SetActive(false);
        collectionsButton.SetActive(true);
        potionsTierButton.SetActive(true);
        pauseButton.SetActive(true);
        tradeInButton.SetActive(true);
        biddersGuideButton.SetActive(true);
        phalangesPanelParent.SetActive(true);
        mixButton.SetActive(true);
        phalangesNextPanel.SetActive(true);
    }
    public void CollectionsEnter()
    {
        collectionsScreen.Play("CollectionsEnter");
        collectionsButton.SetActive(false);
        potionsTierButton.SetActive(false);
        pauseButton.SetActive(false);
        tradeInButton.SetActive(false);
        biddersGuideButton.SetActive(false);
        phalangesPanelParent.SetActive(false);
        mixButton.SetActive(false);
        phalangesNextPanel.SetActive(false);
    }
    public void CollectionsExit()
    {
        collectionsScreen.Play("CollectionsExit");
        collectionsButton.SetActive(true);
        if (introScript.auctionStart)
        {
            potionsTierButton.SetActive(true);
            pauseButton.SetActive(true);
            tradeInButton.SetActive(true);
            mixButton.SetActive(true);
            biddersGuideButton.SetActive(true);
            phalangesNextPanel.SetActive(true);
        }
        else
        {
            phalangesPanelParent.SetActive(true);
        }
    }
    public void TradeInEnter()
    {
        tradeInScreen.Play("TradeInEnter");
        collectionsButton.SetActive(false);
        potionsTierButton.SetActive(false);
        pauseButton.SetActive(false);
        tradeInButton.SetActive(false);
        biddersGuideButton.SetActive(false);
        phalangesPanelParent.SetActive(false);
        mixButton.SetActive(false);
        phalangesNextPanel.SetActive(false);
    }
    public void TradeInExit()
    {
        tradeInScreen.Play("TradeInExit");
        tradeInButton.SetActive(true);
        if (introScript.auctionStart)
        {
            potionsTierButton.SetActive(true);
            pauseButton.SetActive(true);
            biddersGuideButton.SetActive(true);
            mixButton.SetActive(true);
            phalangesNextPanel.SetActive(true);
            collectionsButton.SetActive(true);
        }
        else
        {
            phalangesPanelParent.SetActive(true);
        }
    }
    public void MixingEnter()
    {
        mixScreen.Play("MixingEnter");
        collectionsButton.SetActive(false);
        potionsTierButton.SetActive(false);
        pauseButton.SetActive(false);
        tradeInButton.SetActive(false);
        phalangesPanelParent.SetActive(false);
        mixButton.SetActive(false);
        biddersGuideButton.SetActive(false);
        phalangesNextPanel.SetActive(false);
    }
    public void MixingExit()
    {
        mixScreen.Play("MixingExit");
        mixButton.SetActive(true);
        if (introScript.auctionStart)
        {
            potionsTierButton.SetActive(true);
            pauseButton.SetActive(true);
            tradeInButton.SetActive(true);
            biddersGuideButton.SetActive(true);
            phalangesNextPanel.SetActive(true);
            collectionsButton.SetActive(true);
        }
        else
        {
            phalangesPanelParent.SetActive(true);
        }
    }
    public void PotionTiersEnter()
    {
        potionScreen.Play("PotionTiersEnter");
        collectionsButton.SetActive(false);
        potionsTierButton.SetActive(false);
        biddersGuideButton.SetActive(false);
        pauseButton.SetActive(false);
        tradeInButton.SetActive(false);
        phalangesPanelParent.SetActive(false);
        mixButton.SetActive(false);
        phalangesNextPanel.SetActive(false);
    }
    public void PotionTiersExit()
    {
        potionScreen.Play("PotionTiersExit");
        potionsTierButton.SetActive(true);
        if (introScript.auctionStart)
        {
            pauseButton.SetActive(true);
            tradeInButton.SetActive(true);
            phalangesNextPanel.SetActive(true);
            biddersGuideButton.SetActive(true);
            collectionsButton.SetActive(true);
            mixButton.SetActive(true);
        }
        else
        {
            phalangesPanelParent.SetActive(true);
        }
    }

    public void BiddersGuideEnter()
    {
        collectionsButton.SetActive(false);
        potionsTierButton.SetActive(false);
        pauseButton.SetActive(false);
        tradeInButton.SetActive(false);
        phalangesPanelParent.SetActive(false);
        mixButton.SetActive(false);
        phalangesNextPanel.SetActive(false);
        biddersGuideButton.SetActive(false);
        biddersGuide.GetComponent<Animator>().Play("BiddersGuideEnter");
    }
    public void BiddersGuideExit()
    {
        biddersGuide.GetComponent<Animator>().Play("BiddersGuideExit");
        StartCoroutine(BGExitSequence());
    }

    private IEnumerator BGExitSequence()
    {
        yield return new WaitForSeconds(0.2f);
        potionsTierButton.SetActive(true);
        pauseButton.SetActive(true);
        tradeInButton.SetActive(true);
        phalangesNextPanel.SetActive(true);
        collectionsButton.SetActive(true);
        mixButton.SetActive(true);
        biddersGuideButton.SetActive(true);
    }
}
