using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAbility_1_2_1 : Ability
{
    public NecromancerAbility_1_2_1(Ability ability) : base(ability)
    {
    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        if (_abilityData is NecromancerAbilityData_1_2_1 data)
        {
            bool heroContents = false;

            foreach (HeroQuestSlot_UI activeHeroSlot in heroes.Slots)
            {
                if (activeHeroSlot.Hero != null && activeHeroSlot.Hero.HeroData != null)
                {
                    if (activeHeroSlot.Hero.HeroData == data.HeroData)
                        heroContents = true;
                }
            }

            if (!heroContents)
                return 0;

            else
            {
                float extraPower = Mathf.FloorToInt((heroSlot.Hero.HeroPowerPoints.AllPower * data.PowerCoef) / 100);
                return extraPower;
            }
        }

        else
            return 0;
    }
}
