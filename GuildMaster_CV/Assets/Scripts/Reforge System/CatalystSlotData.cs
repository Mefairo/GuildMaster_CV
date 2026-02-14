using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reforge System/Catalyst Item")]
public class CatalystSlotData : CraftItemData
{
    [Header("Catalyst Parametres")]
    public int MinCraftValueSpend;
    public int MaxCraftValueSpend;

    public virtual void ApplyCatalystEffect(BlankSlot_UI blankSlot, List<RuneSlot_UI> runeSlots, CatalystSlot_UI catalystSlot,
        ReforgeController reforgeController) { }
}
