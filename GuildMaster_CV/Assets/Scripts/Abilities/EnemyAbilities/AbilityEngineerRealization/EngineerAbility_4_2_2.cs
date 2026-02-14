using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineerAbility_4_2_2 : ActivateHeroAbility
{
    public EngineerAbility_4_2_2(Ability ability) : base(ability)
    {
    }

    public override float ActivateAttackAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots)
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateStatsAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots, bool apply)
    {
        if (_ability.AbilityData is EngineerAbilityData_4_2_2 abilityData)
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
