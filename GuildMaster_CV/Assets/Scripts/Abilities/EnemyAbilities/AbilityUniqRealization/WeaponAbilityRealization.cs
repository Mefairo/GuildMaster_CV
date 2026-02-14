using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponAbilityRealization : Ability
{
    public WeaponAbilityRealization(Ability ability) : base(ability)
    {

    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        float power = 0;

        if (_abilityData is WeaponAbilityData data)
        {
            if (heroSlot.Hero.EquipHolder.EquipSystem.Slots[7] != null && heroSlot.Hero.EquipHolder.EquipSystem.Slots[7].BaseItemData != null)
            {
                if (data.RequiredWeaponTypes.Contains(heroSlot.Hero.EquipHolder.EquipSystem.Slots[7].EquipItemData.ItemType))
                {
                    float extraPower = Mathf.FloorToInt((heroSlot.Hero.HeroPowerPoints.AllPower * data.Coef) / 100);
                    return extraPower;
                }
            }
        }

        return power;
    }
}
