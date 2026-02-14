using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability System/Hero Abilities/General Abilities/Stat Value Scale")]
public class StatScaleByValueAbilityData : AbilityData
{
    [Header("UNIQUE ABILITY SETTINGS")]
    public float Coef;
    public List<StatParametres> StatScale = new List<StatParametres>();
}