using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerAbility_1_2_2 : ActivateHeroAbility
{
    public EngineerAbility_1_2_2(Ability ability) : base(ability)
    {
    }

    public override float ActivateAttackAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots)
    {
        float power = 0;

        //if (_ability.AbilityData is EngineerAbilityData_1_2_2 abilityData)
        //{
        //    float hasteBonusPower = heroSlot.Hero.HeroStats.AccuracyMulti * (heroSlot.Hero.PowerPoints * abilityData.AccuracyCoef / 100); // 100% - это проценты

        //    power += (heroSlot.Hero.PowerPoints + hasteBonusPower) * ability.AbilityDamageSingleTarget(questSlot, heroSlot) / 100;   // 100% - это проценты
        //    power = Mathf.RoundToInt(power);
        //}

        return power;
    }

    public override void ActivateStatsAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots, bool apply)
    {
     
    }

    public override void DisActivateAttackAbility(Hero hero, Ability ability, Quest quest, List<HeroQuestSlot_UI> heroesSlots)
    {
        throw new System.NotImplementedException();
    }
}
