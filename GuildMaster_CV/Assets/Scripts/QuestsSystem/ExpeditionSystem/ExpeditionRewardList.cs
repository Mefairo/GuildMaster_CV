using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpeditionRewardList
{
    [SerializeField] private List<RewardItems> _rewardItems = new List<RewardItems>();

    public List<RewardItems> RewardItems => _rewardItems;

    public ExpeditionRewardList(List<RewardItems> rewardItems)
    {
        _rewardItems = rewardItems;
    }

    public ExpeditionRewardList(ExpeditionRewardList copyList)
    {
        _rewardItems = copyList.RewardItems;
    }
}
