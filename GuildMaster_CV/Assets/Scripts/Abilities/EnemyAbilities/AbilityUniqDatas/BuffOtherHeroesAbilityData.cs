using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability System/Hero Abilities/General Abilities/Buff Others")]
public class BuffOtherHeroesAbilityData : AbilityData
{
    [Header("UNIQUE ABILITY SETTINGS")]
    public SlotDirection Direction;
    public int AmountHeroesForBuff;
}

public enum SlotDirection
{
    None,
    Front,
    Behind
}
