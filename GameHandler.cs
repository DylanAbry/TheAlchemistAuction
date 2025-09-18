using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public class GameHandler : MonoBehaviour
{

    public float timer;
    public IntroManager introScript;
    public PotionManager potionScript;
    public MixResults mixScript;
    public SideButtonManagement buttonScript;
    public int numRounds;
    public int numBonusRounds;
    public int roundTracker;
    public int deFraudTradeThreshold;
    public TextMeshProUGUI roundTrackText;
    public TextMeshProUGUI playerCashText, fleeceCashText;
    public TextMeshProUGUI startingBidText;
    public TextMeshProUGUI highestOfferText;
    public TextMeshProUGUI fleeceFirstBidText;
    public TextMeshProUGUI fleeceBidText;
    public TextMeshProUGUI bidTimer;
    public TextMeshProUGUI soldPrompt;
    public TextMeshProUGUI itemRevealPrompt;
    public TextMeshProUGUI pricePaidPurchase;
    public TextMeshProUGUI potionName;
    public TextMeshProUGUI tierOfPotion;
    public TextMeshProUGUI tradeInButtonText;
    public GameObject spotLight;
    public GameObject itemCover;
    public GameObject potionReturnMarker;


    private GameObject mysteryPotion;
    private GameObject potionToTrade;
    private GameObject deFraudTradingPotion;
    public int moneyBack, numTradedItems;


    public GameObject increaseBidButton;
    public GameObject itemDetailPanel;

    private int playerCash = 1000, fleeceCash = 1000;
    public int startingBidProb;
    public int highestOffer;
    public int fleeceRaiseBidProb;
    public int tradeInsLeft;

    public GameObject[] phalangesGamePanels;  

    public int deFraudPanelIndex;
    public int fleeceFaceIndex;
    public int startingBidPromptProb;
    public int numPlayerPotions, numFleecePotions;

    public int potionIndex;
    public int countdownTime;

    public bool potionForBid;
    public bool playerBidTurn;


    // Trade in variables
    public GameObject confirmTradePanel;
    public GameObject tradeItemMarker;
    public GameObject backToAuctionTradeButton;
    public GameObject tradeIconParent;
    public TextMeshProUGUI confirmTradeItem;
    public TextMeshProUGUI tierTradeIn;
    public TextMeshProUGUI moneyReturned;

    // Pilfer variables
    public GameObject pilferIcon;
    int numPilfers;
    int numFleecePilfers;
    public GameObject confirmPilferPanel;
    public Animator pilferExit;
    public TextMeshProUGUI pilferLifeText;
    public GameObject fleecePilferPanel;
    public GameObject fleecePilferPassIcon;
    public Animator fleecePilfering;
    int fleeceFirstPilferRound, fleeceSecondPilferRound;


    //public Animator countdownAuction;
    public Animator phalangesAnim;
    public Animator potionAnim;
    public Animator itemOfficiallyTraded;
    int tradeIconIndex;
    int fleeceTradeIconIndex;
    public GameObject fleeceTradePanel;
    public TextMeshProUGUI fleeceTradeText;
    int numFleeceTradeIns;

    // Store data for each purchased potion
    public Dictionary<GameObject, ItemInformation> potionsBought = new Dictionary<GameObject, ItemInformation>();

    // Create data containers for the potions that can be traded in
    public GameObject[] tradeInPlacers;
    public GameObject[] tradedInPlacers;
    public GameObject[] tradeInIcons;
    public List<GameObject> itemsToTrade = new List<GameObject>();
    public List<GameObject> fleeceToTrade = new List<GameObject>();

    // Create data containers for the potions on the collections page
    public GameObject[] playerPotionPlacers;
    public GameObject[] fleecePotionPlacers;
    public GameObject[] potionIcons;
    public List<GameObject> playerKeys = new List<GameObject>();
    public List<GameObject> fleeceKeys = new List<GameObject>();

    // Track the potions won in bonus games
    public List<GameObject> bonusPotionsWon = new List<GameObject>();
    public bool fleeceIsMixing;


    // Start is called before the first frame update
    void Start()
    {
        fleeceIsMixing = false;
        roundTracker = 1;
        numPlayerPotions = 0;
        numFleecePotions = 0;
        numBonusRounds = Random.Range(0, 6);
        deFraudTradeThreshold = Random.Range(22, 27) * 10;
        fleeceTradePanel.SetActive(false);
        numRounds = 10 + numBonusRounds;
        numPilfers = 2;
        numFleecePilfers = 2;
        spotLight.SetActive(false);
        fleecePilferPanel.SetActive(false);
        fleecePilferPassIcon.SetActive(false);
        potionForBid = false;
        playerBidTurn = true;
        bidTimer.enabled = false;
        tradeInsLeft = 3;
        numFleeceTradeIns = 0;
        fleeceFirstPilferRound = Random.Range(5, 9);
        fleeceSecondPilferRound = Random.Range(9, numRounds);
        foreach (GameObject panels in phalangesGamePanels)
        {
            panels.SetActive(false);
        }

        phalangesAnim.Play("PhalangesIdle");
    }

    // Update is called once per frame
    void Update()
    {
        roundTrackText.text = string.Format("ROUND " + roundTracker);
        playerCashText.text = string.Format("PLAYER: $" + playerCash);
        fleeceCashText.text = string.Format("DEFRAUD: $" + fleeceCash);
        highestOfferText.text = string.Format("Highest Offer: $" + highestOffer);
        tradeInButtonText.text = string.Format("TRADE IN (" + tradeInsLeft + ")");
        pilferLifeText.text = string.Format("X" + numPilfers);

        if (introScript.auctionStart)
        {
            if (potionForBid)
            {
                bidTimer.enabled = true;

                if (countdownTime > 0)
                {
                    timer -= Time.deltaTime;

                    if (timer <= 0f)
                    {
                        countdownTime--;
                        bidTimer.text = countdownTime.ToString();

                        //countdownAuction.Play("AuctionCountdown");
                        timer = 1f;
                    }
                }
                else
                {
                    bidTimer.enabled = false;
                    //countdownAuction.enabled = false;

                    if (potionForBid)
                    {
                        PotionSold();
                        potionForBid = false;
                    }
                    
                }
            }
        }

        if (numPilfers > 0 && potionScript.deFraudDictionary.Count > 0)
        {
            pilferIcon.SetActive(true);
        }
        else
        {
            pilferIcon.SetActive(false);
        }

        if (tradeInsLeft == 0)
        {
            introScript.tradeInButton.SetActive(false);
        }
    }

    public void AuctionSetup()
    {
        StartCoroutine(BeginTheAuction());
    }

    private IEnumerator BeginTheAuction()
    {
        introScript.introPanels[introScript.introPanels.Length - 1].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        roundTrackText.GetComponent<Animator>().Play("RoundTransition");
        yield return new WaitForSeconds(1.75f);
        phalangesGamePanels[0].SetActive(true);
    }

    public void Spotlight()
    {
        StartCoroutine(SpotlightSetup());
    }

    private IEnumerator SpotlightSetup()
    {
        deFraudPanelIndex = Random.Range(3, 6);
        phalangesGamePanels[0].SetActive(false);
        roundTrackText.GetComponent<Animator>().Play("RoundTrackDefault");
        yield return new WaitForSeconds(0.2f);
        spotLight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        introScript.deFraudPanels[deFraudPanelIndex].SetActive(true);

        fleeceFaceIndex = Random.Range(1, 7);
        introScript.fleeceFaces[12].SetActive(false);
        introScript.fleeceFaces[fleeceFaceIndex].SetActive(true);
    }

    public void FirstItemForBid()
    {
        StartCoroutine(SetupFirstPotion());
    }

    private IEnumerator SetupFirstPotion()
    {
        startingBidProb = Random.Range(0, 99);

        if (startingBidProb < 33)
        {
            highestOffer = 20;
        }
        else if (startingBidProb < 66)
        {
            highestOffer = 40;
        }
        else
        {
            highestOffer = 60;
        }
        introScript.deFraudPanels[deFraudPanelIndex].SetActive(false);
        yield return new WaitForSeconds(0.25f);
        startingBidText.text = string.Format("For this first potion, let's set the bidding price at... $" + highestOffer + ".");
        phalangesGamePanels[1].SetActive(true);

        potionIndex = Random.Range(0, 30);
        mysteryPotion = potionScript.potionList[potionIndex];
        potionScript.potionList[potionIndex].SetActive(true);

        itemCover.SetActive(true);
    }

    public void FleeceFirstBid()
    {
        phalangesGamePanels[1].SetActive(false);
        fleeceFirstBidText.text = string.Format("$" + highestOffer + "!");
        introScript.deFraudPanels[9].SetActive(true);
    }

    private IEnumerator FleeceFollowUpBids()
    {
        yield return new WaitForSeconds(0.5f);
        highestOffer += 20;
        introScript.deFraudPanels[10].SetActive(true);
        fleeceBidText.text = string.Format("$" + highestOffer + "!");
        yield return new WaitForSeconds(0.5f);
        introScript.deFraudPanels[10].SetActive(false);

        if (playerCash >= highestOffer + 20)
        {
            increaseBidButton.SetActive(true);
        }

        
        bidTimer.enabled = false;
        bidTimer.enabled = true;
        playerBidTurn = true;
    }

    public void BiddingWar()
    {
        countdownTime = 4;
        introScript.deFraudPanels[9].SetActive(false);
        phalangesGamePanels[2].SetActive(true);

        if (playerCash >= highestOffer + 20)
        {
            increaseBidButton.SetActive(true);
        }

        playerBidTurn = true;
        potionForBid = true;
    }

    public void IncreaseBid()
    {
        playerBidTurn = false;
        countdownTime = 4;
        highestOffer += 20;
        increaseBidButton.SetActive(false);

        bidTimer.enabled = false;
        bidTimer.enabled = true;
        

        fleeceRaiseBidProb = Random.Range(0, 104);

        if (fleeceCash >= highestOffer + 20)
        {
            if (highestOffer <= 160 || roundTracker == 15)
            {
                StartCoroutine(FleeceFollowUpBids());
            }
            else if (highestOffer <= 200)
            {
                if (fleeceRaiseBidProb < 90)
                {
                    StartCoroutine(FleeceFollowUpBids());
                }
            }
            else if (highestOffer <= 240)
            {
                if (fleeceRaiseBidProb < 75)
                {
                    StartCoroutine(FleeceFollowUpBids());
                }
            }
            else if (highestOffer <= 280)
            {
                if (fleeceRaiseBidProb < 60)
                {
                    StartCoroutine(FleeceFollowUpBids());
                }
            }
            else if (highestOffer <= 320)
            {
                if (fleeceRaiseBidProb < 45)
                {
                    StartCoroutine(FleeceFollowUpBids());
                }
            }
            else if (highestOffer <= 360)
            {
                if (fleeceRaiseBidProb < 30)
                {
                    StartCoroutine(FleeceFollowUpBids());
                }
            }
            else if (highestOffer <= 400)
            {
                if (fleeceRaiseBidProb < 15)
                {
                    StartCoroutine(FleeceFollowUpBids());
                }
            }
            else
            {

            }
        }

    }

    public void PotionSold()
    {
        phalangesGamePanels[2].SetActive(false);
        phalangesGamePanels[3].SetActive(true);
        increaseBidButton.SetActive(false);

        if (!playerBidTurn)
        {
            soldPrompt.text = string.Format("Sold to the player for $" + highestOffer + "! Let's reveal the potion you purchased....");
        }
        else
        {
            soldPrompt.text = string.Format("Sold to Dr. DeFraud for $" + highestOffer + "! Let's reveal the potion you purchased....");
        }
        StartCoroutine(SoldSequence());
    } 

    private IEnumerator SoldSequence()
    {
        if (phalangesAnim.GetCurrentAnimatorStateInfo(0).IsName("PhalangesIdle"))
        {
            phalangesAnim.Play("SoldSlam", 0);
        }

        yield return new WaitForSeconds(0.751f);

        phalangesAnim.Play("PhalangesIdle");

        countdownTime = 3;
        bidTimer.text = countdownTime.ToString();
    }

    public void RevealTheItem()
    {
        itemCover.SetActive(false);
        phalangesGamePanels[3].SetActive(false);
        phalangesGamePanels[7].SetActive(true);
        string potionName = potionScript.itemNames[potionIndex];
        itemRevealPrompt.text = string.Format(potionName + "!");
    }

    public void GiveOutPotion()
    {
        ItemInformation potionDetails = potionScript.potions[mysteryPotion];
        potionDetails.pricePaid = highestOffer;
        int potionIndex = System.Array.IndexOf(potionScript.potionList, mysteryPotion);
        if (!playerBidTurn)
        {
            if (!potionScript.playerDictionary.ContainsKey(mysteryPotion))
            {

                // Update data structures here!!

                potionScript.playerDictionary.Add(mysteryPotion, potionDetails);
                playerKeys.Add(potionIcons[potionIndex]);
                potionIcons[potionIndex].transform.position = playerPotionPlacers[numPlayerPotions].transform.position;
                tradeInIcons[potionIndex].transform.position = tradeInPlacers[numPlayerPotions].transform.position;
                itemsToTrade.Add(tradeInIcons[potionIndex]);
                potionsBought.Add(mysteryPotion, potionDetails);
                numPlayerPotions++;

                if (potionDetails.itemTier < 3)
                {
                    mixScript.mixingIcons[potionIndex].transform.position = mixScript.mixingMarkers[mixScript.mixingIngredients.Count].transform.position;
                    mixScript.mixingIngredients.Add(mysteryPotion);
                    mixScript.potionsToMix.Add(mixScript.mixingIcons[potionIndex]);
                }
            }
            StartCoroutine(PlayerGetsThePotion());
        }
        else
        {
            if (!potionScript.deFraudDictionary.ContainsKey(mysteryPotion))
            {

                // Update data structures here!!

                potionScript.deFraudDictionary.Add(mysteryPotion, potionDetails);
                fleeceKeys.Add(potionIcons[potionIndex]);
                potionIcons[potionIndex].transform.position = fleecePotionPlacers[numFleecePotions].transform.position;
                fleeceToTrade.Add(tradeInIcons[potionIndex]);
                potionsBought.Add(mysteryPotion, potionDetails);
                numFleecePotions++;

                if (potionDetails.itemTier < 3)
                {
                    mixScript.fleeceMixingIngredients.Add(mysteryPotion);
                }
            }
            StartCoroutine(DefraudGetsThePotion());
        }
    }

    private IEnumerator PlayerGetsThePotion()
    {
        phalangesGamePanels[7].SetActive(false);
        playerCash -= highestOffer;
        yield return new WaitForSeconds(0.2f);
        potionAnim.Play("PlayerGetsPotion");
        yield return new WaitForSeconds(0.7f);

        if (roundTracker == fleeceFirstPilferRound)
        {
            if (potionScript.playerDictionary.Count > 0)
            {
                DefraudPilfer();
                yield return new WaitForSeconds(4f);
            }
            else
            {
                fleeceFirstPilferRound++;
                if (fleeceFirstPilferRound == fleeceSecondPilferRound)
                {
                    fleeceSecondPilferRound++;
                }
            }
        }
        else if (roundTracker == fleeceSecondPilferRound)
        {
            if (potionScript.playerDictionary.Count > 0)
            {
                DefraudPilfer();
                yield return new WaitForSeconds(4f);
            }
            else
            {
                fleeceSecondPilferRound++;
            }
        }
        else
        {

        }

        introScript.tradeInButton.SetActive(true);
        introScript.mixButton.SetActive(true);
        introScript.pauseButton.SetActive(true);
        introScript.collectionsButton.SetActive(true);
        introScript.potionsTierButton.SetActive(true);
        phalangesGamePanels[6].SetActive(true);
    }

    private IEnumerator DefraudGetsThePotion()
    {
        phalangesGamePanels[7].SetActive(false);
        fleeceCash -= highestOffer;
        yield return new WaitForSeconds(0.2f);
        potionAnim.Play("DefraudGetsThePotion");
        yield return new WaitForSeconds(0.7f);

        if (roundTracker == fleeceFirstPilferRound)
        {
            if (potionScript.playerDictionary.Count > 0)
            {
                DefraudPilfer();
                yield return new WaitForSeconds(4f);
                if (mixScript.fleeceMixingIngredients.Count >= 2 && !fleeceIsMixing)
                {
                    mixScript.DefraudPotionMix();
                    if (fleeceIsMixing)
                    {
                        yield return new WaitForSeconds(4f);
                    }
                }
            }
            else
            {
                fleeceFirstPilferRound++;
                if (fleeceFirstPilferRound == fleeceSecondPilferRound)
                {
                    fleeceSecondPilferRound++;
                }
            }
        }
        else if (roundTracker == fleeceSecondPilferRound)
        {
            if (potionScript.playerDictionary.Count > 0)
            {
                DefraudPilfer();
                yield return new WaitForSeconds(4f);
                if (mixScript.fleeceMixingIngredients.Count >= 2 && !fleeceIsMixing)
                {
                    mixScript.DefraudPotionMix();
                    if (fleeceIsMixing)
                    {
                        yield return new WaitForSeconds(4f);
                    }
                }
            }
            else
            {
                fleeceSecondPilferRound++;
            }
        }
        else
        {

        }

        if (mixScript.fleeceMixingIngredients.Count >= 2)
        {
            if (!fleeceIsMixing)
            {
                mixScript.DefraudPotionMix();
                if (fleeceIsMixing)
                {
                    yield return new WaitForSeconds(4f);
                }
            }
        }

        if (fleeceCash <= deFraudTradeThreshold)
        {
            DefraudTradeIn();
            if (numFleeceTradeIns > 1)
            {
                deFraudTradeThreshold = Random.Range(17, 23) * 10;
            }
            else if (numFleeceTradeIns > 3)
            {
                deFraudTradeThreshold = Random.Range(13, 18) * 10;
            }
            else
            {
                deFraudTradeThreshold = Random.Range(22, 27) * 10;
            }

            yield return new WaitForSeconds(3f);
            introScript.tradeInButton.SetActive(true);
            introScript.mixButton.SetActive(true);
            introScript.pauseButton.SetActive(true);
            introScript.collectionsButton.SetActive(true);
            introScript.potionsTierButton.SetActive(true);
            phalangesGamePanels[6].SetActive(true);
        }
        else
        {
            introScript.tradeInButton.SetActive(true);
            introScript.mixButton.SetActive(true);
            introScript.pauseButton.SetActive(true);
            introScript.collectionsButton.SetActive(true);
            introScript.potionsTierButton.SetActive(true);
            phalangesGamePanels[6].SetActive(true);
        }
    }

    public void StartNextRound()
    {
        roundTracker++;
        GameObject nextPotion;
        do
        {
            potionIndex = Random.Range(0, 30);
            nextPotion = potionScript.potionList[potionIndex];
        }

        while (potionScript.playerDictionary.ContainsKey(nextPotion) || potionScript.deFraudDictionary.ContainsKey(nextPotion)
            || potionScript.tradeInPotions.ContainsKey(nextPotion));

        nextPotion.SetActive(true);
        mysteryPotion = nextPotion;
        StartCoroutine(NextRoundSetup());
    }

    private IEnumerator NextRoundSetup()
    {
        introScript.tradeInButton.SetActive(false);
        introScript.mixButton.SetActive(false);
        introScript.pauseButton.SetActive(false);
        introScript.collectionsButton.SetActive(false);
        introScript.potionsTierButton.SetActive(false);
        phalangesGamePanels[6].SetActive(false);
        yield return new WaitForSeconds(0.1f);
        roundTrackText.GetComponent<Animator>().Play("RoundTransition");
        yield return new WaitForSeconds(1.75f);

        itemCover.SetActive(true);
        potionAnim.Play("PotionReset");
        startingBidProb = Random.Range(0, 99);

        if (startingBidProb < 33)
        {
            highestOffer = 20;
        }
        else if (startingBidProb < 66)
        {
            highestOffer = 40;
        }
        else
        {
            highestOffer = 60;
        }

        startingBidPromptProb = Random.Range(0, 4);
        if (startingBidPromptProb == 0)
        {
            startingBidText.text = string.Format("Let's start the bidding for this next potion at... $" + highestOffer + ".");
        }
        else if (startingBidPromptProb == 1)
        {
            startingBidText.text = string.Format("We will start the bidding for this potion at... $" + highestOffer + ".");
        }
        else if (startingBidPromptProb == 2)
        {
            startingBidText.text = string.Format("Next up for bid, we have a potion starting at... $" + highestOffer + ".");
        }
        else
        {
            startingBidText.text = string.Format("The price tag for this next potion will begin at... $" + highestOffer + ".");
        }
        phalangesGamePanels[1].SetActive(true);
        roundTrackText.GetComponent<Animator>().Play("RoundTrackDefault");
    }

    public void ShowPotionDetails(GameObject clickedIcon)
    {
        itemDetailPanel.SetActive(true);


        if (potionsBought.TryGetValue(clickedIcon, out ItemInformation potionDetails))
        {
            // Display item details using the values from the weaponDetails object
            potionName.text = $"Name: {potionDetails.itemName}";
            tierOfPotion.text = $"Tier: {potionDetails.itemTier}";
            if (bonusPotionsWon.Contains(clickedIcon))
            {
                pricePaidPurchase.text = $"Price Paid: FREE!";
            }
            else
            {
                pricePaidPurchase.text = $"Price Paid: ${potionDetails.pricePaid}";
            }
        }
        else
        {
            // Log a message if the item is not found in the dictionary
            potionName.text = "Name: Unknown";
            tierOfPotion.text = "Tier: N/A";
            pricePaidPurchase.text = "Price Paid: N/A";
        }
    }

    public void ClosePotionDetails()
    {
        itemDetailPanel.SetActive(false);
    }

    public void TradeInPotion(GameObject clickedPotion)
    {
        confirmTradePanel.SetActive(true);

        // Check if on trade in page and player decides to switch potions to trade

        if (potionToTrade != clickedPotion && potionToTrade != null)
        {
            Debug.Log("The issue is here idiot!");
            Debug.Log(potionToTrade);

            if (!potionScript.tradeInPotions.ContainsKey(potionToTrade))
            {
                Debug.Log("Before: " + potionToTrade);
                int staticIndex = System.Array.IndexOf(potionScript.potionList, potionToTrade);
                GameObject tradeIcon = tradeInIcons[staticIndex];
                int tradePosition = itemsToTrade.IndexOf(tradeIcon);
                Debug.Log(tradePosition + " is the correct index?");
                Vector3 expectedPos = tradeInPlacers[tradePosition].transform.position;

                Debug.Log(tradeInIcons[tradeIconIndex] + " will now be moved back!");
                if (tradeInIcons[tradeIconIndex].transform.position != expectedPos)
                {
                    
                    tradeInIcons[tradeIconIndex].transform.SetParent(tradeIconParent.transform);
                    tradeInIcons[tradeIconIndex].transform.position = expectedPos;
                }
            }
            else
            {
                Debug.Log("Fuck Unity");
            }
        }
        potionToTrade = clickedPotion;
        Debug.Log("After: " + potionToTrade);

        ItemInformation tradedPotionDetails = potionScript.playerDictionary[potionToTrade];

        tradeIconIndex = System.Array.IndexOf(potionScript.potionList, potionToTrade);

        tradeInIcons[tradeIconIndex].transform.SetParent(confirmTradePanel.transform);
        tradeInIcons[tradeIconIndex].transform.position = tradeItemMarker.transform.position;

        backToAuctionTradeButton.SetActive(false);

        if (potionScript.playerDictionary.TryGetValue(clickedPotion, out ItemInformation potionDetails))
        {
            if (bonusPotionsWon.Contains(clickedPotion))
            {
                moneyBack = 50;
            }
            else
            {
                moneyBack = (int)(potionDetails.pricePaid * 0.75);
            }
            confirmTradeItem.text = string.Format("Trade in " + potionDetails.itemName + "?");
            tierTradeIn.text = string.Format("Tier: " + potionDetails.itemTier);
            moneyReturned.text = string.Format("Cash Back: $" + moneyBack);
        }
        else
        {
            // Log a message if the item is not found in the dictionary
            Debug.Log("Item not found in player dictionary!");
            confirmTradeItem.text = "Name: Unknown";
            tierTradeIn.text = "Tier: N/A";
            moneyReturned.text = "Price Paid: N/A";
        }
    }

    public void ConfirmTradeIn()
    {
        //buttonScript.nextItemButton.enabled = true;
        tradeInIcons[tradeIconIndex].transform.SetParent(tradeIconParent.transform);
        StartCoroutine(TradeInTransition());

        if (potionToTrade == null)
        {
            Debug.LogError("potionToTrade is null before trade-in.");
            return;
        }

        ItemInformation tradedPotionDetails = potionScript.playerDictionary[potionToTrade];

        int traderIconIndex = itemsToTrade.FindIndex(item => item.name == tradedPotionDetails.itemName);

        if (traderIconIndex == -1)
        {
            Debug.LogError("Could not find potion " + tradedPotionDetails.itemName + " in itemsToTrade!");
            return;
        }

        Debug.Log("Before removal: " + string.Join(", ", itemsToTrade));

        // Remove from trade list
        itemsToTrade.RemoveAt(traderIconIndex);
        playerKeys.RemoveAt(traderIconIndex);
        if (bonusPotionsWon.Contains(potionToTrade))
        {
            bonusPotionsWon.Remove(potionToTrade);
        }
        potionScript.playerDictionary.Remove(potionToTrade);
        numPlayerPotions--;

        Debug.Log("After removal: " + string.Join(", ", itemsToTrade));



        // Reposition remaining items in the UI
        for (int i = 0; i < itemsToTrade.Count; i++)
        {
            itemsToTrade[i].transform.position = tradeInPlacers[i].transform.position;
            playerKeys[i].transform.position = playerPotionPlacers[i].transform.position;
        }

       


        int tradeInIndex = System.Array.FindIndex(tradeInIcons, icon => icon.name == tradedPotionDetails.itemName);

        if (tradeInIndex == -1)
        {
            Debug.LogError("No matching trade-in icon found for " + tradedPotionDetails.itemName);
            return;
        }

        if (tradedPotionDetails.itemTier < 3)
        {
            mixScript.mixingIngredients.Remove(potionToTrade);
            mixScript.potionsToMix.Remove(mixScript.mixingIcons[tradeInIndex]);
            mixScript.mixingIcons[tradeInIndex].transform.position = potionReturnMarker.transform.position;

            for (int i = 0; i < mixScript.mixingIngredients.Count; i++)
            {
                mixScript.potionsToMix[i].transform.position = mixScript.mixingMarkers[i].transform.position;
            }
        }

        potionIcons[tradeInIndex].SetActive(false);

        if (!potionScript.tradeInPotions.ContainsKey(potionToTrade))
        {
            potionScript.tradeInPotions.Add(potionToTrade, tradedPotionDetails);
            tradeInIcons[tradeInIndex].transform.position = tradedInPlacers[numTradedItems].transform.position;
            tradeInIcons[tradeInIndex].GetComponent<Button>().enabled = false;
            numTradedItems++;
        }

        

        Debug.Log("Removing " + tradedPotionDetails.itemName + " from the active potions!");
        Debug.Log("Removing " + tradedPotionDetails.itemName + " from the player collection list!");

        tradeInsLeft--;
    }

    public void CancelTradeIn()
    {
        tradeInIcons[tradeIconIndex].transform.position = tradeInPlacers[itemsToTrade.IndexOf(tradeInIcons[tradeIconIndex])].transform.position;
        Debug.Log("Index item was returned to: " + itemsToTrade.IndexOf(tradeInIcons[tradeIconIndex]));
        Debug.Log("itemsToTrade: " + string.Join(", ", itemsToTrade));
        tradeInIcons[tradeIconIndex].transform.SetParent(tradeIconParent.transform);
        confirmTradePanel.SetActive(false);
        backToAuctionTradeButton.SetActive(true);
    }

    private IEnumerator TradeInTransition()
    {
        itemOfficiallyTraded.Play("TradeInExit");
        buttonScript.potionsTierButton.SetActive(true);
        buttonScript.tradeInButton.SetActive(true);
        buttonScript.collectionsButton.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        confirmTradePanel.SetActive(false);
        backToAuctionTradeButton.SetActive(true);
        playerCash += moneyBack;
    }

    public void DefraudTradeIn()
    {
        int potionTradeScore;
        int lowestPotionScore = int.MaxValue;
        int lowestTier = int.MaxValue;
        int highestPricePaid = int.MinValue;
        GameObject fleecePotionToTrade = null;

        List<GameObject> deFraudKeys = new List<GameObject>(potionScript.deFraudDictionary.Keys);

        foreach (GameObject potion in deFraudKeys)
        {
            potionTradeScore = 0;
            ItemInformation potionDetails = potionScript.deFraudDictionary[potion];

            /*Conditions that will impact the likelihood that the potion gets traded
             Tier 5 potions are prioritized to stay!! */

            if (fleeceCash + potionDetails.pricePaid >= 60)
            {
                if (potionDetails.itemTier < 5)
                {
                    // Analyze mixing ingredients
                    if (potionDetails.itemTier < 3)
                    {
                        int bestRecipeScore = 0;

                        foreach (var recipe in mixScript.mixingRecipes)
                        {
                            GameObject ingredientA = recipe.Key.Item1;
                            GameObject ingredientB = recipe.Key.Item2;
                            int recipeScore = 0;

                            // First two cases: Dr. DeFraud has a matching mixing ingredient for the current potion
                            if (potion == ingredientA && potionScript.deFraudDictionary.ContainsKey(ingredientB))
                            {
                                recipeScore = (potionDetails.itemTier == 2) ? 6 : 5;
                            }
                            else if (potion == ingredientB && potionScript.deFraudDictionary.ContainsKey(ingredientA))
                            {
                                recipeScore = (potionDetails.itemTier == 2) ? 6 : 5;
                            }

                            // Second two cases: Dr. DeFraud does NOT have a matching mixing ingredient for current potion, but player does, which Fleece can potentially pilfer

                            else if (potion == ingredientA && potionScript.playerDictionary.ContainsKey(ingredientB) && numFleecePilfers > 0)
                            {
                                if (numFleecePilfers == 2)
                                {
                                    recipeScore = (potionDetails.itemTier == 2) ? 5 : 4;
                                }
                                else
                                {
                                    recipeScore = (potionDetails.itemTier == 2) ? 4 : 3;
                                }

                            }
                            else if (potion == ingredientB && potionScript.playerDictionary.ContainsKey(ingredientA) && numFleecePilfers > 0)
                            {
                                if (numFleecePilfers == 2)
                                {
                                    recipeScore = (potionDetails.itemTier == 2) ? 5 : 4;
                                }
                                else
                                {
                                    recipeScore = (potionDetails.itemTier == 2) ? 4 : 3;
                                }
                            }

                            // Third two cases: Dr. DeFraud does NOT have a matching ingredient for current potion, but they can still be purchased....

                            else if (potion == ingredientA && !potionsBought.ContainsKey(ingredientB))
                            {
                                recipeScore = (potionDetails.itemTier == 2) ? 3 : 2;
                            }
                            else if (potion == ingredientB && !potionsBought.ContainsKey(ingredientA))
                            {
                                recipeScore = (potionDetails.itemTier == 2) ? 3 : 2;
                            }

                            bestRecipeScore = Mathf.Max(bestRecipeScore, recipeScore);
                        }

                        potionTradeScore = (bestRecipeScore > 0) ? bestRecipeScore : 1;
                    }

                    // Tier 3 potion evaluation

                    else if (potionDetails.itemTier == 3)
                    {
                        int prob = Random.Range(0, 2);

                        if (prob == 0)
                        {
                            potionTradeScore = 4;
                        }
                        else
                        {
                            potionTradeScore = 3;
                        }
                    }

                    // Tier 4 potion evaluation

                    else
                    {
                        int prob = Random.Range(0, 2);

                        if (prob == 0)
                        {
                            potionTradeScore = 5;
                        }
                        else
                        {
                            potionTradeScore = 4;
                        }
                    }
                }
                else
                {
                    potionTradeScore = 7;
                }
            }
            else
            {

            }
            if (potionTradeScore < lowestPotionScore)
            {
                lowestPotionScore = potionTradeScore;
                lowestTier = potionDetails.itemTier;
                highestPricePaid = potionDetails.pricePaid;
                fleecePotionToTrade = potion;
            }
            else if (potionTradeScore == lowestPotionScore)
            {
                if (potionDetails.itemTier < lowestTier)
                {
                    lowestTier = potionDetails.itemTier;
                    highestPricePaid = potionDetails.pricePaid;
                    fleecePotionToTrade = potion;
                }
                // If tier is also tied, break tie by pricePaid
                else if (potionDetails.itemTier == lowestTier && potionDetails.pricePaid > highestPricePaid)
                {
                    highestPricePaid = potionDetails.pricePaid;
                    fleecePotionToTrade = potion;
                }
            }
        }

        deFraudTradingPotion = fleecePotionToTrade;

        // Update the data structures here!!
        ItemInformation fleeceTradedPotionDetails = potionScript.deFraudDictionary[fleecePotionToTrade];
        fleeceTradeIconIndex = System.Array.IndexOf(potionScript.potionList, deFraudTradingPotion);
        potionScript.deFraudDictionary.Remove(fleecePotionToTrade);
        fleeceKeys.Remove(potionIcons[fleeceTradeIconIndex]);
        fleeceToTrade.Remove(tradeInIcons[fleeceTradeIconIndex]);

        if (fleeceTradedPotionDetails.itemTier < 3)
        {
            mixScript.fleeceMixingIngredients.Remove(fleecePotionToTrade);
        }

        if (!potionScript.tradeInPotions.ContainsKey(deFraudTradingPotion))
        {
            potionScript.tradeInPotions.Add(deFraudTradingPotion, fleeceTradedPotionDetails);
            tradeInIcons[fleeceTradeIconIndex].transform.position = tradedInPlacers[numTradedItems].transform.position;
            potionIcons[fleeceTradeIconIndex].transform.position = potionReturnMarker.transform.position;
            tradeInIcons[fleeceTradeIconIndex].GetComponent<Button>().enabled = false;
            numTradedItems++;
        }

        for (int i = 0; i < potionScript.deFraudDictionary.Count; i++)
        {
            fleeceKeys[i].transform.position = fleecePotionPlacers[i].transform.position;
        }

        if (fleecePotionToTrade == potionScript.potionList[5])
        {
            fleeceTradeText.text = string.Format("I vould like to trade in me Vax!");
        }
        else
        {
            fleeceTradeText.text = string.Format("I vould like to trade in me " + fleeceTradedPotionDetails.itemName + "!");
        }

        StartCoroutine(ProcessDefraudTrade());
        fleeceCash += (int)(fleeceTradedPotionDetails.pricePaid * 0.75);
        numFleeceTradeIns++;
    }

    private IEnumerator ProcessDefraudTrade()
    {
        fleeceTradePanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        fleeceTradePanel.SetActive(false);
    }

    public void PlayerPilfer()
    {
        confirmPilferPanel.SetActive(true);
    }

    public void CancelPilfer()
    {
        confirmPilferPanel.SetActive(false);
    }

    public void ConfirmPlayerPilfer()
    {
        // Execute all data transactions across the player and Fleece's collections
        int potionToPilferFleeceIndex = Random.Range(0, potionScript.deFraudDictionary.Count);
        List<GameObject> fleeceDictionaryList = new List<GameObject>(potionScript.deFraudDictionary.Keys);
        GameObject potionToPilfer = fleeceDictionaryList[potionToPilferFleeceIndex];
        ItemInformation pilferedPotionDetails = potionScript.deFraudDictionary[potionToPilfer];
        potionScript.deFraudDictionary.Remove(potionToPilfer);
        potionScript.playerDictionary.Add(potionToPilfer, pilferedPotionDetails);
        int potionToPilferIndex = System.Array.IndexOf(potionScript.potionList, potionToPilfer);
        itemsToTrade.Add(tradeInIcons[potionToPilferIndex]);
        fleeceToTrade.Remove(tradeInIcons[potionToPilferIndex]);
        fleeceKeys.Remove(potionIcons[potionToPilferIndex]);
        playerKeys.Add(potionIcons[potionToPilferIndex]);

        potionIcons[potionToPilferIndex].transform.position = playerPotionPlacers[playerKeys.Count - 1].transform.position;
        tradeInIcons[potionToPilferIndex].transform.position = tradeInPlacers[playerKeys.Count - 1].transform.position;

        if (pilferedPotionDetails.itemTier < 3)
        {
            mixScript.fleeceMixingIngredients.Remove(potionToPilfer);
            mixScript.mixingIngredients.Add(potionToPilfer);
            mixScript.potionsToMix.Add(mixScript.mixingIcons[potionToPilferIndex]);
            mixScript.mixingIcons[potionToPilferIndex].transform.position = mixScript.mixingMarkers[mixScript.mixingIngredients.Count - 1].transform.position;
        }

        for (int i = 0; i < potionScript.deFraudDictionary.Count; i++)
        {
            fleeceKeys[i].transform.position = fleecePotionPlacers[i].transform.position;
        }

        StartCoroutine(PlayerPilferSequence());

        numPilfers--;
        numFleecePotions--;
        numPlayerPotions++;
    }

    private IEnumerator PlayerPilferSequence()
    {
        confirmPilferPanel.SetActive(false);
        pilferExit.Play("CollectionsExit");
        yield return null;
    }

    public void DefraudPilfer()
    {
        int potionToPilferPlayerIndex = Random.Range(0, potionScript.playerDictionary.Count);
        List<GameObject> playerDictionaryList = new List<GameObject>(potionScript.playerDictionary.Keys);
        GameObject fleecePotionToPilfer = playerDictionaryList[potionToPilferPlayerIndex];
        ItemInformation fleecePilferedPotionDetails = potionScript.playerDictionary[fleecePotionToPilfer];
        potionScript.playerDictionary.Remove(fleecePotionToPilfer);
        potionScript.deFraudDictionary.Add(fleecePotionToPilfer, fleecePilferedPotionDetails);
        int fleecePotionToPilferIndex = System.Array.IndexOf(potionScript.potionList, fleecePotionToPilfer);
        itemsToTrade.Remove(tradeInIcons[fleecePotionToPilferIndex]);
        fleeceToTrade.Add(tradeInIcons[fleecePotionToPilferIndex]);
        fleeceKeys.Add(potionIcons[fleecePotionToPilferIndex]);
        playerKeys.Remove(potionIcons[fleecePotionToPilferIndex]);

        potionIcons[fleecePotionToPilferIndex].transform.position = fleecePotionPlacers[fleeceKeys.Count - 1].transform.position;
        tradeInIcons[fleecePotionToPilferIndex].transform.position = potionReturnMarker.transform.position;

        if (fleecePilferedPotionDetails.itemTier < 3)
        {
            mixScript.mixingIngredients.Remove(fleecePotionToPilfer);
            mixScript.potionsToMix.Remove(mixScript.mixingIcons[fleecePotionToPilferIndex]);
            mixScript.fleeceMixingIngredients.Add(fleecePotionToPilfer);
            mixScript.mixingIcons[fleecePotionToPilferIndex].transform.position = potionReturnMarker.transform.position;

            for (int i = 0; i < mixScript.mixingIngredients.Count; i++)
            {
                mixScript.potionsToMix[i].transform.position = mixScript.mixingMarkers[i].transform.position;
            }
        }

        for (int i = 0; i < potionScript.playerDictionary.Count; i++)
        {
            playerKeys[i].transform.position = playerPotionPlacers[i].transform.position;
            itemsToTrade[i].transform.position = tradeInPlacers[i].transform.position;
        }

        numPlayerPotions--;
        numFleecePotions++;
        numFleecePilfers--;

        StartCoroutine(DefraudPilferSequence());

    }

    private IEnumerator DefraudPilferSequence()
    {
        fleecePilferPanel.SetActive(true);
        fleecePilferPassIcon.SetActive(true);
        fleecePilfering.Play("PilferFleece");
        yield return new WaitForSeconds(4f);
        fleecePilfering.Play("PilferPassReset");
        fleecePilferPassIcon.SetActive(false);
        fleecePilferPanel.SetActive(false);
    }
}
