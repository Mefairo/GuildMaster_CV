using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[System.Serializable]
public class CalculattionAbilitiesSystem
{
    public float CheckAbilitiesHero(QuestSlot_UI questSlot, HeroQuestSlot_UI heroSlot, Ability heroAbility)
    {
        float effectivenesPowerAbilities = 0;

        switch (heroAbility.AbilityData.TypeAbility)
        {
            case TypeAbilities.DamageSingleTarget:
                effectivenesPowerAbilities += heroAbility.AbilityDamageSingleTarget(questSlot, heroSlot);
                break;

            case TypeAbilities.AoEDamage:
                effectivenesPowerAbilities += heroAbility.AbilityDamageAoeDamage(questSlot, heroSlot);
                break;

            case TypeAbilities.Aura:
                effectivenesPowerAbilities += heroAbility.AbilityDamageAuraEffect(questSlot, heroSlot);
                break;

            case TypeAbilities.Summon:
                effectivenesPowerAbilities += heroAbility.AbilitySummonDamage(questSlot, heroSlot);
                break;

            case TypeAbilities.Suppression_Aura:
                effectivenesPowerAbilities += heroAbility.AbilityDamageSingleTarget(questSlot, heroSlot);
                break;
        }

        return effectivenesPowerAbilities;
    }
}
