using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RewardItems
{
    [SerializeField] private SlotData _rewardItem;
    [SerializeField] private int _minAmountItems;
    [SerializeField] private int _maxAmountItems;
    [SerializeField] private int _amountItems;
    [SerializeField] private int _chanceDrop;

    public SlotData RewardItem => _rewardItem;
    public int MinAmountItems => _minAmountItems;
    public int MaxAmountItems => _maxAmountItems;
    public int AmountItems => _amountItems;
    public int ChanceDrop => _chanceDrop;

    public RewardItems(SlotData rewardItem, int minAmount, int maxAmount)
    {
        _rewardItem = rewardItem;
        _minAmountItems = minAmount;
        _maxAmountItems = maxAmount;
        _amountItems = Random.Range(minAmount, maxAmount);
    }

    public RewardItems(SlotData rewardItem, int amount)
    {
        _rewardItem = rewardItem;
        _amountItems = amount;
    }

    public RewardItems(SlotData rewardItem)
    {
        if(rewardItem is BlankSlotData blankData)
        {
            _rewardItem = blankData;

        }
    }
}
