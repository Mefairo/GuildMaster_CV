using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDataList 
{
    [SerializeField] private RegionQuest _regionQuest;
    [SerializeField] private List<EnemyListSecond> _enemyListSecond;

    public List<EnemyListSecond> EnemyListSecond => _enemyListSecond;
    public RegionQuest RegionQuest => _regionQuest;
}
