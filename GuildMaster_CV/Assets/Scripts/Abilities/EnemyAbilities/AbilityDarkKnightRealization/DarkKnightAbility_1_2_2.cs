using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightAbility_1_2_2 : Ability
{
    public DarkKnightAbility_1_2_2(Ability ability) : base(ability)
    {

    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        float power = 0;

        if (_abilityData is DarkKnightAbilityData_1_2_2 data)
        {
            float extraPower = Mathf.FloorToInt((heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.HasteMulti * data.HasteCoef) / 100);
            return extraPower;
        }

        return power;
    }
}
