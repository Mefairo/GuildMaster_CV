using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Region System/New Region")]
public class RegionData : ScriptableObject
{
    public RegionQuest RegionType;
    public string RegionName;
    [TextArea(10, 10)]
    public string RegionDescription;
    public List<EnemyListSecond> EnemyDataListq;
    public List<RewardItemsRegion> RewardRegion;
    public List<RewardItemsRegion> RewardRunes;
    public List<RewardItemsRegion> RewardCatalysts;
    public List<BlankRewards> RewardBlankss;
}


[System.Serializable]
public class BlankRewards
{
    public BlankSlotData BlankSlotData;
    public int MinAmount;
    public int MaxAmount;
    public int ChanceDrop;
}
