using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimalistAbility_3_2_2 : Ability
{
    public PrimalistAbility_3_2_2(Ability ability) : base(ability)
    {

    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        float power = 0;

        if (_abilityData is PrimalistAbilityData_3_2_2 data)
        {
            float extraPower = Mathf.FloorToInt(heroSlot.Hero.VisibleHeroStats.IntelligenceValue * data.Coef);
            return extraPower;
        }

        return power;
    }
}
