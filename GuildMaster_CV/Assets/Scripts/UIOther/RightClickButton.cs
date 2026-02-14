using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RightClickButton : MonoBehaviour, IPointerClickHandler
{
    private InventorySlot_UI _invSlot_UI;

    private void Awake()
    {
        _invSlot_UI = GetComponentInParent<InventorySlot_UI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_invSlot_UI is BlankSlot_UI blankSlot_UI)
            {
                if (blankSlot_UI.BlankSlot.BlankSlotData != null)
                    blankSlot_UI.OnUISlotRightClick();
            }

            else if (_invSlot_UI is RuneSlot_UI runeSlot_UI)
            {
                if (runeSlot_UI.RuneSlot.RuneSlotData != null)
                    runeSlot_UI.OnUISlotRightClick();
            }

            else if (_invSlot_UI is CatalystSlot_UI catalystSlot_UI)
            {
                if (catalystSlot_UI.CatalystSlot.CatalystSlotData != null)
                    catalystSlot_UI.OnUISlotRightClick();
            }

            else if (_invSlot_UI is EquipSlot_UI equipSlot_UI)
            {
                if (equipSlot_UI.AssignedEquipSlot.EquipItemData != null || equipSlot_UI.AssignedEquipSlot.BaseItemData != null)
                    equipSlot_UI.OnUISlotRightClick();
            }

            else
            {
                if (_invSlot_UI.AssignedInventorySlot.BaseItemData != null)
                    _invSlot_UI.OnUISlotRightClick();
            }
        }
    }

    private void OnRightClick()
    {
        // Например: использовать предмет, выбросить его и т. д.
        Debug.Log("Действие по правому клику!");
    }
}
