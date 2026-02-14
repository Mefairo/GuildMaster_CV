using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem/New ExpeditionData")]
public class ExpeditionData : QuestData
{
    [Header("Days To Disappear")]
    public int MinAmountDaysToDisapper;
    public int MaxAmountDaysToDisapper;
    [Header("Amount Rewards Non-Main Stage")]
    //public int MinAmountStageRewards;
    //public int MaxAmountStageRewards;
    public List<ExpeditionRewardList> StageRewardItems;
    public List<DrawingItemData> DrawingRewards;
}
