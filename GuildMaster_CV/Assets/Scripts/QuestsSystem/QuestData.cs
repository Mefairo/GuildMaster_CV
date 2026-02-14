using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "QuestSystem/New QuestData")]
public class QuestData : ScriptableObject
{
    public int Level;
    [Header("Power")]
    public int MinPower;
    public int MaxPower;
    [Header("Defence")]
    public int MinDefence;
    public int MaxDefence;
    [Header("Resistance")]
    public int MinRes;
    public int MaxRes;
    [Header("Amount Enemy")]
    public int MinAmountEnemy;
    public int MaxAmountEnemy;
    [Header("Amount Affixes")]
    public int MinAmountAffixes;
    public int MaxAmountAffixes;
    [Header("Amount Days")]
    public int MinAmountDays;
    public int MaxAmountDays;
    [Header("Reward Guild Reputation")]
    public int MinRewardGuildReputation;
    public int MaxRewardGuildReputation;
    [Header("Reward Gold")]
    public int MinRewardGold;
    public int MaxRewardGold;
    [Header("Reward Hero Experience")]
    public int MinRewardHeroExp;
    public int MaxRewardHeroExp;
    [Header("Extra Reward Items")]
    public List<RewardItems> RewardItems;
}
