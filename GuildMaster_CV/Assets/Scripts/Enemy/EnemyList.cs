using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField] private List<EnemyDataList> _enemyDataList;

    public List<EnemyDataList> EnemyDataList => _enemyDataList;
}
