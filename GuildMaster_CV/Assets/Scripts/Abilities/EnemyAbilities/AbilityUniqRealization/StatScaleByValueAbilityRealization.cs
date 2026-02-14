using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScaleByValueAbilityRealization : Ability
{
    public StatScaleByValueAbilityRealization(Ability ability) : base(ability)
    {

    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        float power = 0;

        if (_abilityData is StatScaleByValueAbilityData data)
        {
            foreach (StatParametres stat in data.StatScale)
                power += CalculateStatsMulti(stat, heroSlot, data.Coef);

            return Mathf.FloorToInt(power);
        }

        return power;
    }

    private float CalculateStatsMulti(StatParametres stat, HeroQuestSlot_UI heroSlot, float coef)
    {
        float value = 0;

        switch (stat.StatType)
        {
            // PRIMARY POWER MULTI

            case StatType.StrengthValue:
                value = heroSlot.Hero.VisibleHeroStats.StrengthValue * coef;
                break;

            case StatType.DexterityValue:
                value = heroSlot.Hero.VisibleHeroStats.DexterityValue * coef;
                break;

            case StatType.IntelligenceValue:
                value = heroSlot.Hero.VisibleHeroStats.IntelligenceValue * coef;
                break;

            //  PRIMARY DEFENCE MULTI

            case StatType.EnduranceValue:
                value = heroSlot.Hero.VisibleHeroStats.EnduranceValue * coef;
                break;

            case StatType.FlexibilityValue:
                value = heroSlot.Hero.VisibleHeroStats.FlexibilityValue * coef;
                break;

            case StatType.SanityValue:
                value = heroSlot.Hero.VisibleHeroStats.SanityValue * coef;
                break;

            //  SECONDARY POWER MULTI

            case StatType.CritValue:
                value = heroSlot.Hero.VisibleHeroStats.StrengthValue * coef;
                break;

            case StatType.HasteValue:
                value = heroSlot.Hero.VisibleHeroStats.HasteValue * coef;
                break;

            case StatType.AccuracyValue:
                value = heroSlot.Hero.VisibleHeroStats.AccuracyValue * coef;
                break;

            //  SECONDARY DEFENCE MULTI

            case StatType.DurabilityValue:
                value = heroSlot.Hero.VisibleHeroStats.DurabilityValue * coef;
                break;

            case StatType.AdaptabilityValue:
                value = heroSlot.Hero.VisibleHeroStats.AdaptabilityValue * coef;
                break;

            case StatType.SuppressionValue:
                value = heroSlot.Hero.VisibleHeroStats.SuppressionValue * coef;
                break;
        }

        return value;
    }
}
