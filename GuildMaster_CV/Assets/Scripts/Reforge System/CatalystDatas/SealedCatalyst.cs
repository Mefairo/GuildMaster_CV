using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reforge System/Catalyst Item/Sealed")]
public class SealedCatalyst : MainCatalyst
{
    public int SealedAmount = 1;

    public override void ApplyCatalystEffect(BlankSlot_UI blankSlot, List<RuneSlot_UI> runeSlots, CatalystSlot_UI catalyst, ReforgeController reforgeController)
    {
        base.ApplyCatalystEffect(blankSlot, runeSlots, catalyst, reforgeController);

        var activeNeutralStats = GetNonZeroStats(blankSlot.BlankSlot.NeutralStats);
        int activeNeutralEffects = activeNeutralStats.Count + blankSlot.BlankSlot.NeutralRes.Count + blankSlot.BlankSlot.NeutralDebuffs.Count;

        for (int i = 0; i < SealedAmount; i++)
        {
            int randomEffectIndex = UnityEngine.Random.Range(0, activeNeutralEffects);

            if (activeNeutralEffects == 0)
            {
                Debug.LogWarning("Нет доступных эффектов для переноса!");
                return;
            }

            if (randomEffectIndex <= activeNeutralStats.Count - 1)
            {
                var selectedStat = activeNeutralStats[randomEffectIndex];
                MoveStatToSealed(blankSlot.BlankSlot, selectedStat);
                blankSlot.BlankSlot.ReducePropertyAmount(1);
                continue;
            }

            else
                randomEffectIndex -= activeNeutralStats.Count;

            if (randomEffectIndex <= blankSlot.BlankSlot.NeutralRes.Count - 1)
            {
                blankSlot.BlankSlot.SealedRes.Add(blankSlot.BlankSlot.NeutralRes[randomEffectIndex]);
                blankSlot.BlankSlot.NeutralRes.RemoveAt(randomEffectIndex);
                blankSlot.BlankSlot.ReducePropertyAmount(1);
                continue;
            }

            else
                randomEffectIndex -= blankSlot.BlankSlot.NeutralRes.Count;

            if (randomEffectIndex <= blankSlot.BlankSlot.NeutralDebuffs.Count - 1)
            {
                blankSlot.BlankSlot.SealedDebuffs.Add(blankSlot.BlankSlot.NeutralDebuffs[randomEffectIndex]);
                blankSlot.BlankSlot.NeutralDebuffs.RemoveAt(randomEffectIndex);
                blankSlot.BlankSlot.ReducePropertyAmount(1);
                continue;
            }
        }

        reforgeController.CheckSlots();
    }

    // Метод для переноса стата
    private void MoveStatToSealed(BlankSlot blankSlot, (int index, string name, int value) stat)
    {
        // Создаем копию NeutralStats чтобы не менять оригинал во время итерации
        HeroStats neutralStats = new HeroStats(blankSlot.NeutralStats);

        // Обнуляем выбранный стат в NeutralStats
        switch (stat.index)
        {
            case 0: neutralStats.StrengthValue = 0; break;
            case 1: neutralStats.DexterityValue = 0; break;
            case 2: neutralStats.IntelligenceValue = 0; break;

            case 3: neutralStats.EnduranceValue = 0; break;
            case 4: neutralStats.FlexibilityValue = 0; break;
            case 5: neutralStats.SanityValue = 0; break;

            case 6: neutralStats.CritValue = 0; break;
            case 7: neutralStats.HasteValue = 0; break;
            case 8: neutralStats.AccuracyValue = 0; break;

            case 9: neutralStats.DurabilityValue = 0; break;
            case 10: neutralStats.AdaptabilityValue = 0; break;
            case 11: neutralStats.SuppressionValue = 0; break;
        }

        // Добавляем в SealedStats
        HeroStats sealedStats = new HeroStats(blankSlot.SealedStats);
        switch (stat.index)
        {
            case 0: sealedStats.StrengthValue += stat.value; break;
            case 1: sealedStats.DexterityValue += stat.value; break;
            case 2: sealedStats.IntelligenceValue += stat.value; break;

            case 3: sealedStats.EnduranceValue += stat.value; break;
            case 4: sealedStats.FlexibilityValue += stat.value; break;
            case 5: sealedStats.SanityValue += stat.value; break;

            case 6: sealedStats.CritValue += stat.value; break;
            case 7: sealedStats.HasteValue += stat.value; break;
            case 8: sealedStats.AccuracyValue += stat.value; break;

            case 9: sealedStats.DurabilityValue += stat.value; break;
            case 10: sealedStats.AdaptabilityValue += stat.value; break;
            case 11: sealedStats.SuppressionValue += stat.value; break;
        }

        blankSlot.SetNeutalStats(neutralStats);
        blankSlot.SetSealedStats(sealedStats);
    }

    private (List<int>, List<int>) CheckNullStats(HeroStats stats)
    {
        List<int> statsListIndexex = new List<int>();
        List<int> statsListValue = new List<int>();

        #region CHECK_STATS 
        if (stats.StrengthValue > 0)
        {
            statsListIndexex.Add(0);
            statsListValue.Add(stats.StrengthValue);
        }

        if (stats.DexterityValue > 0)
        {
            statsListIndexex.Add(1);
            statsListValue.Add(stats.DexterityValue);
        }

        if (stats.IntelligenceValue > 0)
        {
            statsListIndexex.Add(2);
            statsListValue.Add(stats.IntelligenceValue);
        }

        if (stats.EnduranceValue > 0)
        {
            statsListIndexex.Add(3);
            statsListValue.Add(stats.EnduranceValue);
        }

        if (stats.FlexibilityValue > 0)
        {
            statsListIndexex.Add(4);
            statsListValue.Add(stats.FlexibilityValue);
        }

        if (stats.SanityValue > 0)
        {
            statsListIndexex.Add(5);
            statsListValue.Add(stats.SanityValue);
        }

        if (stats.CritValue > 0)
        {
            statsListIndexex.Add(6);
            statsListValue.Add(stats.CritValue);
        }

        if (stats.HasteValue > 0)
        {
            statsListIndexex.Add(7);
            statsListValue.Add(stats.HasteValue);
        }

        if (stats.AccuracyValue > 0)
        {
            statsListIndexex.Add(8);
            statsListValue.Add(stats.AccuracyValue);
        }

        if (stats.DurabilityValue > 0)
        {
            statsListIndexex.Add(9);
            statsListValue.Add(stats.DurabilityValue);
        }

        if (stats.AdaptabilityValue > 0)
        {
            statsListIndexex.Add(10);
            statsListValue.Add(stats.AdaptabilityValue);
        }

        if (stats.SuppressionValue > 0)
        {
            statsListIndexex.Add(11);
            statsListValue.Add(stats.SuppressionValue);
        }
        #endregion

        return (statsListIndexex, statsListValue);
    }

    public List<(int index, string name, int value)> GetNonZeroStats(HeroStats stats)
    {
        var result = new List<(int, string, int)>();

        var statsToCheck = new (string name, int value)[]
        {
        ("Strength", stats.StrengthValue),
        ("Dexterity", stats.DexterityValue),
        ("Intelligence", stats.IntelligenceValue),
        ("Endurance", stats.EnduranceValue),
        ("Flexibility", stats.FlexibilityValue),
        ("Sanity", stats.SanityValue),
        ("Crit", stats.CritValue),
        ("Haste", stats.HasteValue),
        ("Accuracy", stats.AccuracyValue),
        ("Durability", stats.DurabilityValue),
        ("Adaptability", stats.AdaptabilityValue),
        ("Suppression", stats.SuppressionValue)
        };

        for (int i = 0; i < statsToCheck.Length; i++)
        {
            if (statsToCheck[i].value > 0)
            {
                result.Add((i, statsToCheck[i].name, statsToCheck[i].value));
            }
        }

        return result;
    }
}