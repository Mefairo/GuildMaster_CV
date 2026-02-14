using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability System/Hero Abilities/General Abilities/Stat Multi Scale")]
public class StatScaleAbilityData : AbilityData
{
    [Header("UNIQUE ABILITY SETTINGS")]
    public float Coef;
    public List<StatParametres> StatScale = new List<StatParametres>();
}


[System.Serializable]
public class StatParametres
{
    public StatType StatType;
}
