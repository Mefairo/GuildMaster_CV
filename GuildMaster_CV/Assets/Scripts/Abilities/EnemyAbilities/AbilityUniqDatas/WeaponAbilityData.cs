using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability System/Hero Abilities/General Abilities/Weapon Required")]
public class WeaponAbilityData : AbilityData
{
    [Header("UNIQUE ABILITY SETTINGS")]
    public List<ItemType> RequiredWeaponTypes = new List<ItemType>();
    public float Coef;
}
