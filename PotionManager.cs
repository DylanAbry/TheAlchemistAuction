using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{

    public Dictionary<GameObject, ItemInformation> playerDictionary = new Dictionary<GameObject, ItemInformation>();
    public Dictionary<GameObject, ItemInformation> deFraudDictionary = new Dictionary<GameObject, ItemInformation>();
    public Dictionary<GameObject, ItemInformation> potions = new Dictionary<GameObject, ItemInformation>();
    public Dictionary<GameObject, ItemInformation> tradeInPotions = new Dictionary<GameObject, ItemInformation>();

    public GameObject[] potionList;
    public string[] itemNames;

    int tier = 1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 30; i++)
        {
            string name = itemNames[i - 1];
            potions[potionList[i - 1]] = new ItemInformation(tier, name);
            potionList[i - 1].SetActive(false);

            if (i % 6 == 0)
            {
                tier++;
            }

        }

        tier = 3;

        for (int i = 31; i <= potionList.Length; i++)
        {
            string name = itemNames[i - 1];
            potions[potionList[i - 1]] = new ItemInformation(tier, name);
            potionList[i - 1].SetActive(false);

            if (i % 6 == 0)
            {
                tier++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
