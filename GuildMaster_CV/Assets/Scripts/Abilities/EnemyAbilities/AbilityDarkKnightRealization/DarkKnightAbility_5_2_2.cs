using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkKnightAbility_5_2_2 : Ability
{
    public DarkKnightAbility_5_2_2(Ability ability) : base(ability)
    {
    }

    public override void ApplyBuffEffect_FIX(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroesSlots)
    {
        base.ApplyBuffEffect_FIX(heroSlot, heroesSlots);

        if (heroSlot.SlotIndex == heroesSlots.Slots.Count - 1)
            return;

        else
        {
            if (heroesSlots.Slots[heroSlot.SlotIndex + 1] != null && heroesSlots.Slots[heroSlot.SlotIndex + 1].Hero != null &&
                 heroesSlots.Slots[heroSlot.SlotIndex + 1].Hero.HeroData != null)
            {
                heroesSlots.Slots[heroSlot.SlotIndex + 1].Hero.ActiveBuffs.Add(this);
            }
        }
    }
}
