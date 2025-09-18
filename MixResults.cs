using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MixResults : MonoBehaviour
{
    public Dictionary<(GameObject, GameObject), GameObject> mixingRecipes = new Dictionary<(GameObject, GameObject), GameObject>();

    public PotionManager potionScript;
    public GameHandler gameScript;
    public SideButtonManagement buttonScript;
    public GameObject[] mixingIcons;
    public List<GameObject> potionsToMix = new List<GameObject>();
    public GameObject[] mixingMarkers;
    public List<GameObject> mixingIngredients = new List<GameObject>();
    public List<GameObject> fleeceMixingIngredients = new List<GameObject>();
    public int ingredientSel, firstPotionIndex, secondPotionIndex;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI confirmDescription;
    public TextMeshProUGUI outputDescription;

    private GameObject firstIcon;
    private GameObject secondIcon;

    public GameObject defraudMixPanel;

    private GameObject firstIngredient, secondIngredient, outputPotion;

    public GameObject firstMarker, firstConfirmMarker, secondConfirmMarker, outputConfirmMarker;
    public GameObject confirmMixPanel;
    public GameObject cancelMixButton;
    public GameObject recipePage;
    public GameObject backToAuction;


    void Start()
    {
        ingredientSel = 0;
        cancelMixButton.SetActive(false);
        defraudMixPanel.SetActive(false);
        recipePage.SetActive(false);
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Blood"), GameObject.Find("Canvas/Potions/Urine")), GameObject.Find("Canvas/ReplicaPotions/Treejuvenatum Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Sanitizer"), GameObject.Find("Canvas/Potions/Wax")), GameObject.Find("Canvas/ReplicaPotions/Glueygone Goo Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Gasoline"), GameObject.Find("Canvas/Potions/Blood")), GameObject.Find("Canvas/ReplicaPotions/Mummyrita Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Milk"), GameObject.Find("Canvas/Potions/Wax")), GameObject.Find("Canvas/ReplicaPotions/Elephantitan Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Gasoline"), GameObject.Find("Canvas/Potions/Milk")), GameObject.Find("Canvas/ReplicaPotions/Clayper Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Urine"), GameObject.Find("Canvas/Potions/Sanitizer")), GameObject.Find("Canvas/ReplicaPotions/Ultramarine Mix Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Bigback Brew"), GameObject.Find("Canvas/Potions/Gasoline")), GameObject.Find("Canvas/ReplicaPotions/Vomikaze Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Viperous Veinom"), GameObject.Find("Canvas/Potions/Blood")), GameObject.Find("Canvas/ReplicaPotions/Cardio Cola Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Pixelixir"), GameObject.Find("Canvas/Potions/Urine")), GameObject.Find("Canvas/ReplicaPotions/Uzi Ooze Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Punchsucker"), GameObject.Find("Canvas/Potions/Sanitizer")), GameObject.Find("Canvas/ReplicaPotions/Sonic Skeletonic Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Bamboozla"), GameObject.Find("Canvas/Potions/Wax")), GameObject.Find("Canvas/ReplicaPotions/Tungstenskin Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Barber"), GameObject.Find("Canvas/Potions/Milk")), GameObject.Find("Canvas/ReplicaPotions/Snowceror Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Viperous Veinom"), GameObject.Find("Canvas/Potions/Bamboozla")), GameObject.Find("Canvas/ReplicaPotions/Terrorauma Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Punchsucker"), GameObject.Find("Canvas/Potions/Bigback Brew")), GameObject.Find("Canvas/ReplicaPotions/Ass Steroids Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Barber"), GameObject.Find("Canvas/Potions/Pixelixir")), GameObject.Find("Canvas/ReplicaPotions/Achooboom Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Viperous Veinom"), GameObject.Find("Canvas/Potions/Barber")), GameObject.Find("Canvas/ReplicaPotions/Pyroplasmale Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Bamboozla"), GameObject.Find("Canvas/Potions/Bigback Brew")), GameObject.Find("Canvas/ReplicaPotions/Deepsleepreaper Replica"));
        mixingRecipes.Add((GameObject.Find("Canvas/Potions/Punchsucker"), GameObject.Find("Canvas/Potions/Pixelixir")), GameObject.Find("Canvas/ReplicaPotions/Salivalava Replica"));
    }

    void Update()
    {
        if (descriptionText == null) return;

        if (ingredientSel == 0)
        {
            descriptionText.text = string.Format("Select first ingredient....");
        }
        else
        {
            descriptionText.text = string.Format("Select second ingredient....");
        }
    }

    public void SelectIngredient(IngredientBundle bundle)
    {
        GameObject ingredientIcon = bundle.ingredientIcon;
        GameObject ingredientObject = bundle.ingredientObject;
        GameObject confirmIcon = bundle.confirmIcon;

        if (ingredientSel == 0)
        {
            cancelMixButton.SetActive(true);
            backToAuction.SetActive(false);
            firstIcon = ingredientIcon;
            firstIngredient = ingredientObject;
            firstIcon.transform.position = firstMarker.transform.position;
            confirmIcon.transform.position = firstConfirmMarker.transform.position;
            firstPotionIndex = mixingIngredients.IndexOf(ingredientObject);
            ingredientSel++;

            foreach (GameObject ingredient in mixingIngredients)
            {
                if (ingredient == ingredientObject)
                {
                    GameObject potionObjectIcon = GameObject.Find(ingredient.name + "MixIcon");
                    potionObjectIcon.GetComponent<Button>().enabled = false;
                }

                else
                {
                    GameObject potionObject = ingredient;

                    Debug.Log(potionObject + ", " + firstIngredient);

                    if (mixingRecipes.ContainsKey((firstIngredient, potionObject)) || mixingRecipes.ContainsKey((potionObject, firstIngredient)))
                    {

                        GameObject potionObjectIcon = GameObject.Find(potionObject.name + "MixIcon");

                    }
                    else
                    {
                        GameObject potionObjectIcon = GameObject.Find(potionObject.name + "MixIcon");
                        potionObjectIcon.GetComponent<RawImage>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                        potionObjectIcon.GetComponent<Button>().enabled = false;
                    }
                }
            }
        }
        else
        {
            secondIcon = ingredientIcon;
            secondIngredient = ingredientObject;
            secondPotionIndex = mixingIngredients.IndexOf(ingredientObject);
            confirmIcon.transform.position = secondConfirmMarker.transform.position;

            if (mixingRecipes.TryGetValue((firstIngredient, secondIngredient), out outputPotion) || mixingRecipes.TryGetValue((secondIngredient, firstIngredient), out outputPotion))
            {
                ShowConfirmMixScreen(outputPotion);
            }
        }
    }

    public void ShowConfirmMixScreen(GameObject outputPotion)
    {
        confirmMixPanel.SetActive(true);
        GameObject outputIcon = GameObject.Find(outputPotion.name + " Confirm Icon");
        Debug.Log(outputIcon);
        outputDescription.text = string.Format("Resulting Potion: " + outputPotion.name);
        confirmDescription.text = string.Format("Mix " + firstIngredient.name + " and " + secondIngredient.name + "?");

        outputIcon.transform.position = outputConfirmMarker.transform.position;
    }

    public void CancelMix()
    {
        ingredientSel = 0;
        confirmMixPanel.SetActive(false);
        cancelMixButton.SetActive(false);
        backToAuction.SetActive(true);

        firstIcon.transform.position = mixingMarkers[firstPotionIndex].transform.position;

        for (int i = 0; i < potionsToMix.Count; i++)
        {
            potionsToMix[i].GetComponent<Button>().enabled = true;
            potionsToMix[i].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void MixPotions()
    {
        ItemInformation firstMixerDetails = potionScript.playerDictionary[firstIngredient];
        ItemInformation secondMixerDetails = potionScript.playerDictionary[secondIngredient];
        // Work on mixing the potions next!!!!
        int firstMixIconIndex = System.Array.FindIndex(gameScript.tradeInIcons, icon => icon.name == firstMixerDetails.itemName);
        int secondMixIconIndex = System.Array.FindIndex(gameScript.tradeInIcons, icon => icon.name == secondMixerDetails.itemName);

        firstIcon.transform.position = gameScript.potionReturnMarker.transform.position;
        secondIcon.transform.position = gameScript.potionReturnMarker.transform.position;
        gameScript.potionIcons[firstMixIconIndex].transform.position = gameScript.potionReturnMarker.transform.position;
        gameScript.potionIcons[secondMixIconIndex].transform.position = gameScript.potionReturnMarker.transform.position;
        gameScript.tradeInIcons[firstMixIconIndex].transform.position = gameScript.tradedInPlacers[gameScript.numTradedItems].transform.position;
        gameScript.tradeInIcons[firstMixIconIndex].GetComponent<Button>().enabled = false;
        gameScript.tradeInIcons[secondMixIconIndex].GetComponent<Button>().enabled = false;
        gameScript.numTradedItems++;
        gameScript.tradeInIcons[secondMixIconIndex].transform.position = gameScript.tradedInPlacers[gameScript.numTradedItems].transform.position;
        gameScript.numTradedItems++;

        // Update all of the data structures here: Removals!!

        mixingIngredients.Remove(firstIngredient);
        mixingIngredients.Remove(secondIngredient);
        potionsToMix.Remove(mixingIcons[firstMixIconIndex]);
        potionsToMix.Remove(mixingIcons[secondMixIconIndex]);
        potionScript.playerDictionary.Remove(firstIngredient);
        potionScript.playerDictionary.Remove(secondIngredient);
        gameScript.itemsToTrade.Remove(gameScript.tradeInIcons[firstMixIconIndex]);
        gameScript.itemsToTrade.Remove(gameScript.tradeInIcons[secondMixIconIndex]);
        gameScript.playerKeys.Remove(gameScript.potionIcons[firstMixIconIndex]);
        gameScript.playerKeys.Remove(gameScript.potionIcons[secondMixIconIndex]);
        gameScript.numPlayerPotions -= 2;

        for (int i = 0; i < potionScript.playerDictionary.Count; i++)
        {
            gameScript.itemsToTrade[i].transform.position = gameScript.tradeInPlacers[i].transform.position;
            gameScript.playerKeys[i].transform.position = gameScript.playerPotionPlacers[i].transform.position;
        }

        for (int i = 0; i < mixingIngredients.Count; i++)
        {
            potionsToMix[i].transform.position = mixingMarkers[i].transform.position;
            potionsToMix[i].GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 1f);
            potionsToMix[i].GetComponent<Button>().enabled = true;

        }

        // Additions!!
        ItemInformation outputPotionDetails = potionScript.potions[outputPotion];
        int mixReplicaIconIndex = System.Array.FindIndex(gameScript.tradeInIcons, icon => icon.name == outputPotionDetails.itemName);
        Debug.Log(mixReplicaIconIndex);
        if (outputPotionDetails.itemTier == 3)
        {
            outputPotionDetails.pricePaid = 200;
        }
        else if (outputPotionDetails.itemTier == 4)
        {
            outputPotionDetails.pricePaid = 270;
        }
        else
        {
            outputPotionDetails.pricePaid = 340;
        }

        potionScript.tradeInPotions.Add(firstIngredient, firstMixerDetails);
        potionScript.tradeInPotions.Add(secondIngredient, secondMixerDetails);
        potionScript.playerDictionary.Add(outputPotion, outputPotionDetails);
        gameScript.itemsToTrade.Add(gameScript.tradeInIcons[mixReplicaIconIndex]);
        gameScript.playerKeys.Add(gameScript.potionIcons[mixReplicaIconIndex]);
        gameScript.potionsBought.Add(outputPotion, outputPotionDetails);

        Debug.Log("Index of output potion: " + gameScript.numPlayerPotions);
        gameScript.tradeInIcons[mixReplicaIconIndex].transform.position = gameScript.tradeInPlacers[gameScript.numPlayerPotions].transform.position;
        gameScript.potionIcons[mixReplicaIconIndex].transform.position = gameScript.playerPotionPlacers[gameScript.numPlayerPotions].transform.position;
        gameScript.numPlayerPotions++;

        Debug.Log("Total player potions: " + gameScript.numPlayerPotions);

        for (int i = 0; i < gameScript.itemsToTrade.Count; i++)
        {
            Debug.Log(i + ": " + gameScript.itemsToTrade[i]);
        }

        StartCoroutine(PotionMixingProcess());

    }

    private IEnumerator PotionMixingProcess()
    {
        buttonScript.mixScreen.Play("MixingExit");
        confirmMixPanel.SetActive(false);
        cancelMixButton.SetActive(false);
        backToAuction.SetActive(true);
        ingredientSel = 0;

        yield return new WaitForSeconds(0.5f);
    }

    public void DefraudPotionMix()
    {
        StartCoroutine(DefraudMixSequence());
    }

    private IEnumerator DefraudMixSequence()
    {
        gameScript.fleeceIsMixing = true;

        while (true)
        {
            var cands = BuildFleeceCandidates();
            var plan = SelectDefraudPlan(cands);

            if (plan.Count == 0)
                break;

            foreach (var step in plan)
            {
                ExecuteFleeceMix(step.fleeceIngA, step.fleeceIngB, step.fleeceMixOutput);

                defraudMixPanel.SetActive(true);
                yield return new WaitForSeconds(4f);
                defraudMixPanel.SetActive(false);
                yield return new WaitForSeconds(2f);
            }
        }

        gameScript.fleeceIsMixing = false;
        
    }

    private struct FleeceCandidate {

        public GameObject fleeceIngA, fleeceIngB, fleeceMixOutput;
        public int outTier;
    }

    private List<FleeceCandidate> BuildFleeceCandidates()
    {
        List<FleeceCandidate> candidates = new List<FleeceCandidate>();
        for (int i = 0; i < fleeceMixingIngredients.Count; i++)
        {
            for (int j = i + 1; j < fleeceMixingIngredients.Count; j++)
            {
                var ingOne = fleeceMixingIngredients[i];
                var ingTwo = fleeceMixingIngredients[j];

                if (mixingRecipes.TryGetValue((ingOne, ingTwo), out var outPotion) || mixingRecipes.TryGetValue((ingTwo, ingOne), out outPotion))
                {
                    ItemInformation info = potionScript.potions[outPotion];
                    candidates.Add(new FleeceCandidate
                    {
                        fleeceIngA = ingOne,
                        fleeceIngB = ingTwo,
                        fleeceMixOutput = outPotion,
                        outTier = info.itemTier
                    });
                }
            }
        }
        candidates.Sort((x, y) => y.outTier.CompareTo(x.outTier));
        return candidates;
    }

    private List<FleeceCandidate> SelectDefraudPlan(List<FleeceCandidate> cands)
    {
        var used = new HashSet<GameObject>();
        var plan = new List<FleeceCandidate>();

        foreach (var c in cands)
        {
            if (used.Contains(c.fleeceIngA) || used.Contains(c.fleeceIngB)) continue;
            plan.Add(c);
            used.Add(c.fleeceIngA); 
            used.Add(c.fleeceIngB);
        }
        return plan;
    }

    private void ExecuteFleeceMix(GameObject potionA, GameObject potionB, GameObject outputPotion)
    {
        ItemInformation firstMixerDetails = potionScript.deFraudDictionary[potionA];
        ItemInformation secondMixerDetails = potionScript.deFraudDictionary[potionB];
        ItemInformation outPutDetails = potionScript.potions[outputPotion];
        // Work on mixing the potions next!!!!
        int firstMixIconIndex = System.Array.FindIndex(gameScript.potionIcons, icon => icon.name == firstMixerDetails.itemName);
        Debug.Log(firstMixIconIndex);
        int secondMixIconIndex = System.Array.FindIndex(gameScript.potionIcons, icon => icon.name == secondMixerDetails.itemName);
        Debug.Log(secondMixIconIndex);
        int mixReplicaIconIndex = System.Array.FindIndex(gameScript.potionIcons, icon => icon.name == outPutDetails.itemName);
        Debug.Log(mixReplicaIconIndex);
        // Remove required elements in data structures here!!

        fleeceMixingIngredients.Remove(potionA);
        fleeceMixingIngredients.Remove(potionB);
        potionScript.deFraudDictionary.Remove(potionA);
        potionScript.deFraudDictionary.Remove(potionB);
        gameScript.fleeceKeys.Remove(gameScript.potionIcons[firstMixIconIndex]);
        gameScript.fleeceKeys.Remove(gameScript.potionIcons[secondMixIconIndex]);
        gameScript.fleeceToTrade.Remove(gameScript.tradeInIcons[firstMixIconIndex]);
        gameScript.fleeceToTrade.Remove(gameScript.tradeInIcons[secondMixIconIndex]);
        gameScript.potionIcons[firstMixIconIndex].transform.position = gameScript.potionReturnMarker.transform.position;
        gameScript.potionIcons[secondMixIconIndex].transform.position = gameScript.potionReturnMarker.transform.position;
        gameScript.tradeInIcons[firstMixIconIndex].transform.position = gameScript.tradedInPlacers[gameScript.numTradedItems].transform.position;
        gameScript.tradeInIcons[firstMixIconIndex].GetComponent<Button>().enabled = false;
        gameScript.tradeInIcons[secondMixIconIndex].GetComponent<Button>().enabled = false;
        gameScript.numTradedItems++;
        gameScript.tradeInIcons[secondMixIconIndex].transform.position = gameScript.tradedInPlacers[gameScript.numTradedItems].transform.position;
        gameScript.numTradedItems++;
        gameScript.numFleecePotions -= 2;

        for (int i = 0; i < potionScript.deFraudDictionary.Count; i++)
        {
            gameScript.fleeceKeys[i].transform.position = gameScript.fleecePotionPlacers[i].transform.position;
        }

        // Add new elements here!!

        potionScript.tradeInPotions.Add(potionA, firstMixerDetails);
        potionScript.tradeInPotions.Add(potionB, secondMixerDetails);
        potionScript.deFraudDictionary.Add(outputPotion, outPutDetails);
        gameScript.fleeceToTrade.Add(gameScript.tradeInIcons[mixReplicaIconIndex]);
        gameScript.fleeceKeys.Add(gameScript.potionIcons[mixReplicaIconIndex]);
        gameScript.potionsBought.Add(outputPotion, outPutDetails);

       
        gameScript.potionIcons[mixReplicaIconIndex].transform.position = gameScript.fleecePotionPlacers[gameScript.numFleecePotions].transform.position;
        gameScript.numFleecePotions++;

        if (outPutDetails.itemTier == 3)
        {
            outPutDetails.pricePaid = 200;
        }
        else if (outPutDetails.itemTier == 4)
        {
            outPutDetails.pricePaid = 270;
        }
        else
        {
            outPutDetails.pricePaid = 340;
        }

    }
    public void OpenBrewBook()
    {
        recipePage.SetActive(true);
    }
    public void CloseBrewBook()
    {
        recipePage.SetActive(false);
    }

}

