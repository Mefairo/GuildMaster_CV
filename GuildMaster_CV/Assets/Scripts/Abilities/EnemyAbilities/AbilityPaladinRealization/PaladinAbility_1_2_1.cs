using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAbility_1_2_1 : Ability
{

    public PaladinAbility_1_2_1(Ability ability) : base(ability)
    {
    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        if (_abilityData is PaladinAbilityData_1_2_1 data)
        {
            float currentHealth = Mathf.FloorToInt(heroSlot.Hero.VisibleHeroStats.Health / data.HPMulti);
            return currentHealth;
        }

        else
            return 0;
    }
}
