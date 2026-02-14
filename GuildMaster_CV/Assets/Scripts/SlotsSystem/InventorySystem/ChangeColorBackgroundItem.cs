using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorBackgroundItem
{
    public void SetBackgroundItem(InventorySlot invSlot, Image backImage)
    {
        if (invSlot.BaseItemData.IconBackground != null)
        {
            if (invSlot is BlankSlot blank)
                SetBlankBack(blank, backImage);

            else if (invSlot is EquipSlot equip)
                SetEquipBack(equip, backImage);

            else if (invSlot is InventorySlot inventorySlot)
                SetInvSlotBack(inventorySlot, backImage);
        }

        else
            backImage.gameObject.SetActive(false);
    }

    private void SetBlankBack(BlankSlot slot, Image backImage)
    {
        backImage.gameObject.SetActive(true);
        backImage.sprite = slot.BlankSlotData.IconBackground;

        backImage.color = slot.ColorBack;
    }

    private void SetEquipBack(EquipSlot slot, Image backImage)
    {
        backImage.gameObject.SetActive(true);
        backImage.sprite = slot.EquipItemData.IconBackground;

        backImage.color = ChangeColor(slot.EquipItemData.Tier);
    }

    private void SetInvSlotBack(InventorySlot slot, Image backImage)
    {
        backImage.gameObject.SetActive(false);
    }

    public Color ChangeColor(int tier)
    {
        Color color = Color.white;

        switch (tier)
        {
            case 0:
                color = Color.clear;
                break;

            case 1:
                color = Color.white;
                break;

            case 2:
                color = Color.green;
                break;

            case 3:
                color = Color.blue;
                break;

            case 4:
                color = Color.yellow;
                break;

            case 5:
                color = Color.red;
                break;
        }

        return color;
    }
}
