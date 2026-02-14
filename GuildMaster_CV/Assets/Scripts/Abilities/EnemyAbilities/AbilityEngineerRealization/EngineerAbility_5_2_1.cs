using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerAbility_5_2_1 : ActivateHeroAbility
{
    public EngineerAbility_5_2_1(Ability ability) : base(ability)
    {
    }

    public override float ActivateAttackAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots)
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateStatsAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots, bool apply)
    {
        if (_ability.AbilityData is EngineerAbilityData_5_2_1 abilityData)
        {
            ability.AbilityBuffEffect(ability, questSlot, heroSlot, apply);

            if (heroSlot.SlotIndex == 0)
                return;

            else
                ability.AbilityBuffEffect(ability, questSlot, heroesSlots[heroSlot.SlotIndex - 1], apply);
        }
    }

    public override void DisActivateAttackAbility(Hero hero, Ability ability, Quest quest, List<HeroQuestSlot_UI> heroesSlots)
    {
        throw new System.NotImplementedException();
    }
}
