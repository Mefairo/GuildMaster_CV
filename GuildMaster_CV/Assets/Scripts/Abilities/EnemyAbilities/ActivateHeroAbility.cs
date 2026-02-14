using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivateHeroAbility 
{
    protected Ability _ability;

    public float ManaCost => _ability.ManaCost;

    public ActivateHeroAbility(Ability ability)
    {
        _ability = ability;
    }

    public abstract float ActivateAttackAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots);

    public abstract void ActivateStatsAbility(HeroQuestSlot_UI heroSlot, Ability ability, QuestSlot_UI questSlot, List<HeroQuestSlot_UI> heroesSlots, bool apply);

    public abstract void DisActivateAttackAbility(Hero hero, Ability ability, Quest quest, List<HeroQuestSlot_UI> heroesSlots);
}
