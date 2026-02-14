using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopSlot : Slot
{
    [SerializeField] private InventorySlot _assignedInvSlot;

    public InventorySlot AssignedInvSlot => _assignedInvSlot;

    public ShopSlot()
    {
        ClearSlot();
    }

    //public ShopSlot(InventorySlot invSlot)
    //{
    //    ClearSlot();

    //    var newSlot = SetTypeSlot(invSlot);
    //    invSlot.AssignItem(newSlot);
    //}

    //public override void AssignItem(InventorySlot invSlot)
    //{
    //    Debug.Log("AssignItem 001");
    //    if (invSlot.BaseItemData == null)
    //        Debug.Log("inv null 001");
    //    _assignedInvSlot = invSlot;
    //}

    //private InventorySlot SetTypeSlot(InventorySlot invSlot)
    //{
    //    InventorySlot newSlot;

    //    if (invSlot is BlankSlot blankSlot)
    //        newSlot = new BlankSlot(blankSlot, blankSlot.BlankTier);

    //    else if (invSlot is RuneSlot rune)
    //        newSlot = new RuneSlot(rune, rune.Tier);

    //    else if (invSlot is CatalystSlot catalyst)
    //        newSlot = new CatalystSlot(catalyst, -1);

    //    else if (invSlot is EquipSlot equip)
    //        newSlot = new EquipSlot(equip, -1);

    //    else
    //        newSlot = new InventorySlot(invSlot.ItemData, invSlot.StackSize);
    //    if (invSlot.BaseItemData == null)
    //        Debug.Log("basedata null");
    //    newSlot.UpdateInventorySlot(invSlot.BaseItemData, invSlot.StackSize);
    //    return newSlot;
    //}
}
