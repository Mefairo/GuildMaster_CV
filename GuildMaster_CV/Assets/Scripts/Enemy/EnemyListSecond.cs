using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyListSecond 
{
   [SerializeField] private List<EnemyData> _enemyData;

    public List<EnemyData> EnemyDatas => _enemyData;
}
