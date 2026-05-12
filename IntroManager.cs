using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;

public class IntroManager : MonoBehaviour
{
    public Animator camera;
    public Animator deFraudAppears;
    public Animator deFraudEntranceEffect;

    public AudioSource womanScream;

    public GameObject[] introPanels;
    public GameObject[] deFraudPanels;
    public GameObject[] fleeceFaces;
    public GameObject potionsTierButton;
    public GameObject increaseBidButton;
    public GameObject tradeInButton;
    public GameObject collectionsButton;
    public GameObject mixButton;
    public GameObject pauseButton;
    public GameObject itemCover;

    public TextMeshProUGUI playerFunds, fleeceFunds;

    public GameObject cashHolder;


    public int currentPanel;
    private int deFraudIntroIndex;

    public bool auctionStart = false;

    private bool fleeceExitRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        pauseButton.SetActive(false);
        mixButton.SetActive(false);
        collectionsButton.SetActive(false);
        tradeInButton.SetActive(false);
        potionsTierButton.SetActive(false);
        increaseBidButton.SetActive(false);
        itemCover.SetActive(false);
        cashHolder.SetActive(false);
        deFraudIntroIndex = Random.Range(0, 3);

        foreach (GameObject face in fleeceFaces)
        {
            face.SetActive(false);
        }
        foreach (GameObject panel in introPanels)
        {
            panel.SetActive(false);
        }

        foreach (GameObject quote in deFraudPanels)
        {
            quote.SetActive(false);
        }

        fleeceFaces[0].SetActive(true);

        playerFunds.enabled = false;
        fleeceFunds.enabled = false;
        currentPanel = 0;
        StartCoroutine(AuctionEntrance());
    }

    private IEnumerator AuctionEntrance()
    {
        yield return new WaitForSeconds(2f);
        camera.Play("CameraFocusOnStage");
        yield return new WaitForSeconds(5f);
        camera.enabled = false;
        introPanels[currentPanel].SetActive(true);
        cashHolder.SetActive(true);
        playerFunds.enabled = true;
        fleeceFunds.enabled = true;
    }

    public void NextPanel()
    {
        StartCoroutine(NextPanelSequence());
    }

    public void SkipRules()
    {
        introPanels[currentPanel].SetActive(false);
        currentPanel = introPanels.Length - 1;
        StartCoroutine(NextPanelSequence());
    }

    public void RepeatRules()
    {
        introPanels[currentPanel].SetActive(false);
        currentPanel = 7;
        StartCoroutine(NextPanelSequence());
    }

    private IEnumerator NextPanelSequence()
    {
        if (currentPanel < introPanels.Length)
        {
            if (currentPanel == 5)
            {
                introPanels[currentPanel].SetActive(false);
                yield return new WaitForSeconds(0.15f);
                deFraudAppears.Play("DeFraudAppears");
                yield return new WaitForSeconds(0.95f);
                deFraudEntranceEffect.Play("DeFraudAuctionEntrance");
                yield return new WaitForSeconds(0.075f);
                womanScream.Play();
                yield return new WaitForSeconds(0.925f);
                fleeceFaces[0].SetActive(false);
                fleeceFaces[6].SetActive(true);
                deFraudPanels[deFraudIntroIndex].SetActive(true);
                yield return new WaitForSeconds(2f);
                fleeceFaces[6].SetActive(false);
                fleeceFaces[5].SetActive(true);
                yield break;
            }
            else if (currentPanel == 18)
            {
                yield return new WaitForSeconds(0.25f);
                introPanels[currentPanel].SetActive(true);
                auctionStart = true;
                yield break;
            }
            else
            {
                introPanels[currentPanel].SetActive(false);
                currentPanel++;
                yield return new WaitForSeconds(0.25f);
            }

            if (currentPanel == introPanels.Length - 1)
            {
                auctionStart = true;
            }
            else
            {
                introPanels[currentPanel].SetActive(true);
  
                if (currentPanel == 10)
                {
                    increaseBidButton.SetActive(true);
                }
                else if (currentPanel == 11)
                {
                    increaseBidButton.SetActive(false);
                    tradeInButton.SetActive(true);
                }
                else if (currentPanel == 12)
                {
                    tradeInButton.SetActive(false);
                    potionsTierButton.SetActive(true);
                }
                else if (currentPanel == 13 || currentPanel == 14)
                {
                    potionsTierButton.SetActive(false);
                    collectionsButton.SetActive(true);
                }
                else if (currentPanel == 15)
                {
                    collectionsButton.SetActive(false);
                    mixButton.SetActive(true);
                }
                else if (currentPanel == 16)
                {
                    mixButton.SetActive(false);
                }
                else
                {

                }
            }
        }
        else
        {
            yield return null;
        }
    }

    public void FleeceTrashTalkPanel()
    {
        if (fleeceExitRunning) return;
        StartCoroutine(FleecePanelExit());
    }

    private IEnumerator FleecePanelExit()
    {
        fleeceExitRunning = true;
        deFraudPanels[deFraudIntroIndex].SetActive(false);
        fleeceFaces[5].SetActive(true);
        fleeceFaces[6].SetActive(false);
        yield return new WaitForSeconds(0.25f);
        currentPanel++;
        introPanels[currentPanel].SetActive(true);
        fleeceExitRunning = false;
    }

}
