using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatScaleAbilityReaization : Ability
{
    public StatScaleAbilityReaization(Ability ability) : base(ability)
    {

    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        float power = 0;

        if (_abilityData is StatScaleAbilityData data)
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

            case StatType.StrengthMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.StrengthMulti * coef) / 100;
                break;

            case StatType.DexterityMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.DexterityMulti * coef) / 100;
                break;

            case StatType.IntelligenceMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.IntelligenceMulti * coef) / 100;
                break;

            //  PRIMARY DEFENCE MULTI

            case StatType.EnduranceMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.EnduranceMulti * coef) / 100;
                break;

            case StatType.FlexibilityMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.FlexibilityMulti * coef) / 100;
                break;

            case StatType.SanityMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.SanityMulti * coef) / 100;
                break;

            //  SECONDARY POWER MULTI

            case StatType.CritMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.CritMulti * coef) / 100;
                break;

            case StatType.HasteMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.HasteMulti * coef) / 100;
                break;

            case StatType.AccuracyMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.AccuracyMulti * coef) / 100;
                break;

            //  SECONDARY DEFENCE MULTI

            case StatType.DurabilityMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.DurabilityMulti * coef) / 100;
                break;

            case StatType.AdaptabilityMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.AdaptabilityMulti * coef) / 100;
                break;

            case StatType.SuppressionMulti:
                value = (heroSlot.Hero.HeroPowerPoints.AllPower * heroSlot.Hero.VisibleHeroStats.SuppressionMulti * coef) / 100;
                break;
        }

        return value;
    }
}
