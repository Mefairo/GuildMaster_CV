using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffOtherHeroesAbilityRealization : Ability
{
    public BuffOtherHeroesAbilityRealization(Ability ability) : base(ability)
    {
    }

    public override void ApplyBuffEffect_FIX(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroesSlots)
    {
        base.ApplyBuffEffect_FIX(heroSlot, heroesSlots);

        if (_abilityData is BuffOtherHeroesAbilityData data)
        {
            if (data.Direction == SlotDirection.Behind)
                BehindBuffApply(heroSlot, heroesSlots, data);

            else if (data.Direction == SlotDirection.Front)
                FrontBuffApply(heroSlot, heroesSlots, data);
        }
    }

    private void BehindBuffApply(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroesSlots, BuffOtherHeroesAbilityData data)
    {
        // Проверяем, есть ли вообще слоты для баффа
        if (heroSlot.SlotIndex >= heroesSlots.Slots.Count - 1)
            return;

        // Вычисляем сколько слотов реально можно обработать (чтобы не выйти за границы массива)
        int slotsToBuff = Mathf.Min(data.AmountHeroesForBuff, heroesSlots.Slots.Count - heroSlot.SlotIndex - 1);

        for (int i = 1; i <= slotsToBuff; i++)
        {
            int targetIndex = heroSlot.SlotIndex + i;
            var targetSlot = heroesSlots.Slots[targetIndex];

            // Проверяем все необходимые условия
            if (targetSlot != null && targetSlot.Hero != null && targetSlot.Hero.HeroData != null)
            {
                // Применяем бафф
                targetSlot.Hero.ActiveBuffs.Add(this);
                Debug.Log($"Бафф применен к герою в слоте {targetIndex}");
            }
        }
    }

    private void FrontBuffApply(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroesSlots, BuffOtherHeroesAbilityData data)
    {
        // Проверяем, есть ли слоты перед текущим
        if (heroSlot.SlotIndex <= 0)
            return;

        // Вычисляем количество слотов для баффа (не выходя за границы)
        int slotsToBuff = Mathf.Min(data.AmountHeroesForBuff, heroSlot.SlotIndex);

        for (int i = 1; i <= slotsToBuff; i++)
        {
            int targetIndex = heroSlot.SlotIndex - i; // Идем назад по индексам
            var targetSlot = heroesSlots.Slots[targetIndex];

            // Полная проверка валидности слота
            if (targetSlot != null &&
                targetSlot.Hero != null &&
                targetSlot.Hero.HeroData != null)
            {
                // Применяем бафф
                targetSlot.Hero.ActiveBuffs.Add(this);
                Debug.Log($"Бафф применен к предыдущему герою в слоте {targetIndex}");
            }
        }
    }
}