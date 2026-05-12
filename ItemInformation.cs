using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInformation : MonoBehaviour
{
    public int itemTier;
    public int pricePaid;
    public string itemName;

    public ItemInformation(int tier, string name)
    {
        itemTier = tier;
        pricePaid = 0;
        itemName = name;
    }
}
