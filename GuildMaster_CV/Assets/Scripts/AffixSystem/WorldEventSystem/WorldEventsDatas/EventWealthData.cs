using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "World Event System/World Event Data/Wealth")]
public class EventWealthData : WorldEventData
{
    public float CoefBuyCost;
    public float CoefSellCost;
    public float CoefAmountItems;
}
