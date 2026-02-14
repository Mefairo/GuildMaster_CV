using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reforge System/Catalyst Item/Main")]
public class MainCatalyst : CatalystSlotData
{
    public override void ApplyCatalystEffect(BlankSlot_UI blank, List<RuneSlot_UI> runes, CatalystSlot_UI catalyst, ReforgeController reforgeController)
    {
        //blank.BlankSlot.ApplyReforgeEffect(runes);
        blank.BlankSlot.ClearNeutralParametres();

        int amountEffects = 0;
        List<int> runeAmountEffects = new List<int>();
        for (int i = 0; i < runes.Count; i++)
            runeAmountEffects.Add(0);

        for (int i = 0; i < runes.Count; i++)
        {
            if (runes[i].RuneSlot != null && runes[i].RuneSlot.RuneSlotData != null)
            {
                runeAmountEffects[i] = runes[i].RuneSlot.RuneSlotData.RuneSlotStats.Count + runes[i].RuneSlot.RuneSlotData.RuneSlotRes.Count +
                    runes[i].RuneSlot.RuneSlotData.DebuffLists.Count;

                amountEffects += runeAmountEffects[i];
            }
        }

        for (int i = 0; i < blank.BlankSlot.PropertyAmount; i++)
        {
            int randomEffect = Random.Range(1, amountEffects + 1);

            for (int j = 0; j < runeAmountEffects.Count; j++)
            {
                if (runes[j].RuneSlot != null && runes[j].RuneSlot.RuneSlotData != null)
                {
                    if (randomEffect <= runeAmountEffects[j])
                    {
                        if (randomEffect <= runes[j].RuneSlot.RuneSlotData.RuneSlotStats.Count)
                        {
                            Debug.Log(runes[j].RuneSlot.RuneSlotData.RuneSlotStats[randomEffect - 1].StatType);
                            int statValue = blank.BlankSlot.SetRandomValues(runes[j].RuneSlot.RuneSlotData.RuneSlotStats[randomEffect - 1].MinValueStat,
                                runes[j].RuneSlot.RuneSlotData.RuneSlotStats[randomEffect - 1].MaxValueStat + 1);

                            blank.BlankSlot.SetStatType(runes[j].RuneSlot.RuneSlotData.RuneSlotStats[randomEffect - 1].StatType, statValue, blank.BlankSlot.NeutralStats);
                            break;
                        }

                        else
                            randomEffect -= runes[j].RuneSlot.RuneSlotData.RuneSlotStats.Count;

                        if (randomEffect <= runes[j].RuneSlot.RuneSlotData.RuneSlotRes.Count)
                        {
                            Debug.Log(runes[j].RuneSlot.RuneSlotData.RuneSlotRes[randomEffect - 1].ResType);
                            int resValue = blank.BlankSlot.SetRandomValues(runes[j].RuneSlot.RuneSlotData.RuneSlotRes[randomEffect - 1].MinValueRes,
                                runes[j].RuneSlot.RuneSlotData.RuneSlotRes[randomEffect - 1].MaxValueRes + 1);

                            blank.BlankSlot.SetTypeRes(runes[j].RuneSlot.RuneSlotData.RuneSlotRes[randomEffect - 1].ResType, resValue);
                            break;
                        }

                        else
                            randomEffect -= runes[j].RuneSlot.RuneSlotData.RuneSlotRes.Count;

                        if (randomEffect <= runes[j].RuneSlot.RuneSlotData.DebuffLists.Count)
                        {
                            Debug.Log(runes[j].RuneSlot.RuneSlotData.DebuffLists[randomEffect - 1]);
                            blank.BlankSlot.NeutralDebuffs.Add(runes[j].RuneSlot.RuneSlotData.DebuffLists[randomEffect - 1]);
                            break;
                        }

                        else
                            randomEffect -= runes[j].RuneSlot.RuneSlotData.DebuffLists.Count;
                    }

                    else
                        randomEffect -= runeAmountEffects[j];
                }
            }
        }

        for (int k = 0; k < runes.Count; k++)
        {
            if (runes[k].RuneSlot != null && runes[k].RuneSlot.RuneSlotData != null)
            {
                runes[k].RuneSlot.RemoveFromStack(1);
                runes[k].UpdateUISlot(runes[k].RuneSlot);
            }
        }

        int craftValueSpend = blank.BlankSlot.SetRandomValues(catalyst.CatalystSlot.CatalystSlotData.MinCraftValueSpend,
            catalyst.CatalystSlot.CatalystSlotData.MaxCraftValueSpend + 1);
        blank.BlankSlot.ReduceCraftValue(craftValueSpend);

        catalyst.CatalystSlot.RemoveFromStack(1);
        catalyst.UpdateUISlot(catalyst.CatalystSlot);

        reforgeController.CheckSlots();
    }
}
