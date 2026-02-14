using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Reforge System/Catalyst Item/Base")]
public class BaseCatalyst : MainCatalyst
{
    public int AmountOpenBase = 1;

    public override void ApplyCatalystEffect(BlankSlot_UI blankSlot, List<RuneSlot_UI> runeSlots, CatalystSlot_UI catalyst, ReforgeController reforgeController)
    {
        List<int> runeTiers = new List<int>();
        for (int i = 0; i < runeSlots.Count; i++)
        {
            if (runeSlots[i].RuneSlot != null && runeSlots[i].RuneSlot.RuneSlotData != null && runeSlots[i].RuneSlot.Tier > 0)
                runeTiers.Add(runeSlots[i].RuneSlot.Tier);
        }

        int minRuneTier = runeTiers.Min();

        base.ApplyCatalystEffect(blankSlot, runeSlots, catalyst, reforgeController);
        blankSlot.BlankSlot.ClearBaseParametres();

        if (AmountOpenBase == 1)
        {
            int baseStatIndex = Random.Range(0, blankSlot.BlankSlot.BlankSlotData.BaseStats.Count);



            int randomBaseValue = Random.Range(blankSlot.BlankSlot.BlankSlotData.BaseStats[minRuneTier - 1].BaseStats[baseStatIndex].MinValueStat,
                blankSlot.BlankSlot.BlankSlotData.BaseStats[minRuneTier - 1].BaseStats[baseStatIndex].MaxValueStat + 1);

            blankSlot.BlankSlot.SetStatType(blankSlot.BlankSlot.BlankSlotData.BaseStats[minRuneTier - 1].BaseStats[baseStatIndex].StatType,
                randomBaseValue, blankSlot.BlankSlot.BaseStats);
        }

        else if (AmountOpenBase == 2)
        {
            for (int k = 0; k < blankSlot.BlankSlot.BlankSlotData.BaseStats.Count; k++)
            {


                int randomBaseValue = Random.Range(blankSlot.BlankSlot.BlankSlotData.BaseStats[minRuneTier - 1].BaseStats[k].MinValueStat,
                    blankSlot.BlankSlot.BlankSlotData.BaseStats[minRuneTier - 1].BaseStats[k].MaxValueStat + 1);

                blankSlot.BlankSlot.SetStatType(blankSlot.BlankSlot.BlankSlotData.BaseStats[minRuneTier - 1].BaseStats[k].StatType,
        randomBaseValue, blankSlot.BlankSlot.BaseStats);
            }
        }

        reforgeController.CheckSlots();
    }
}
