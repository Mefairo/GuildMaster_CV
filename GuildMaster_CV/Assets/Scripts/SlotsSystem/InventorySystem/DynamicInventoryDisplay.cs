using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI slotPrefab;


    public void RefreshDynamicInventory(InventorySystem invToDisplay, int offset)
    {
        ClearSlots();
        _inventorySystem = invToDisplay;
        if (_inventorySystem != null)
        {
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        AssignSlot(invToDisplay, offset);
    }

    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        ClearSlots();

        _slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null)
            return;

        for (int i = offset; i < invToDisplay.InventorySize; i++)
        {
            InventorySlot_UI uiSlot = Instantiate(slotPrefab, transform);
            _slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    public void TabClicked(CheckTypeForTabs tab)
    {
        ClearSlots();

        if (tab.EquipType == EquipType.Helmet)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Metal_Helmet || slot.ItemData.ItemType == ItemType.Leather_Helmet ||
                slot.ItemData.ItemType == ItemType.Clothes_Helmet)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.EquipType == EquipType.Neck)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Amulet)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.EquipType == EquipType.Chest)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Metal_Chest || slot.ItemData.ItemType == ItemType.Leather_Chest ||
                slot.ItemData.ItemType == ItemType.Clothes_Chest)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.EquipType == EquipType.Gloves)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Metal_Gloves || slot.ItemData.ItemType == ItemType.Leather_Gloves ||
                slot.ItemData.ItemType == ItemType.Clothes_Gloves)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.EquipType == EquipType.Ring)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Ring)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.EquipType == EquipType.Pants)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Metal_Pants || slot.ItemData.ItemType == ItemType.Leather_Pants ||
                slot.ItemData.ItemType == ItemType.Clothes_Pants)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.EquipType == EquipType.Boots)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Metal_Boots || slot.ItemData.ItemType == ItemType.Leather_Boots ||
                    slot.ItemData.ItemType == ItemType.Clothes_Boots)
                        CreateItemSlot(slot);
            }
        }

        else if (tab.ItemPrimaryType == ItemPrimaryType.Blank)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (slot.ItemData.ItemType == ItemType.Blank_Amulet || slot.ItemData.ItemType == ItemType.Blank_Boots ||
                    slot.ItemData.ItemType == ItemType.Blank_Chest || slot.ItemData.ItemType == ItemType.Blank_Gloves
                    || slot.ItemData.ItemType == ItemType.Blank_Helmet || slot.ItemData.ItemType == ItemType.Blank_Pants
                    || slot.ItemData.ItemType == ItemType.Blank_Ring)
                        CreateItemSlot(slot);
            }
        }

        //else if (tab.EquipType == EquipType.OneHandWeapon)
        //{
        //    foreach (InventorySlot slot in _inventorySystem.InventorySlots)
        //    {
        //        if (slot.ItemData.ItemType == ItemType.OneHand_Sword || slot.ItemData.ItemType == ItemType.OneHand_Hammer ||
        //        slot.ItemData.ItemType == ItemType.Dagger)
        //            CreateItemSlot(slot);
        //    }
        //}


        //else if (tab.EquipType == EquipType.OffHandWeapon)
        //{
        //    foreach (InventorySlot slot in _inventorySystem.InventorySlots)
        //    {
        //        if (slot.ItemData.ItemType == ItemType.Foliant || slot.ItemData.ItemType == ItemType.Shield)
        //            CreateItemSlot(slot);
        //    }
        //}

        //else if (tab.EquipType == EquipType.TwoHandWeapon)
        //{
        //    foreach (InventorySlot slot in _inventorySystem.InventorySlots)
        //    {
        //        if (slot.ItemData.ItemType == ItemType.TwoHand_Sword || slot.ItemData.ItemType == ItemType.TwoHand_Hammer ||
        //            slot.ItemData.ItemType == ItemType.Bow || slot.ItemData.ItemType == ItemType.CrossBow || slot.ItemData.ItemType == ItemType.Staff)
        //            CreateItemSlot(slot);
        //    }
        //}

        else if (tab.ItemType == ItemType.All)
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
                if (slot.ItemData != null)
                    CreateItemSlot(slot);
        }

        else
        {
            foreach (InventorySlot slot in _inventorySystem.InventorySlots)
            {
                if (slot.ItemData != null)
                    if (tab.ItemType == slot.ItemData.ItemType)
                        CreateItemSlot(slot);
            }
        }
    }

    private void CreateItemSlot(InventorySlot slot)
    {
        InventorySlot_UI uiSlot = Instantiate(slotPrefab, transform);
        _slotDictionary.Add(uiSlot, slot);
        uiSlot.Init(slot);
        uiSlot.UpdateUISlot();
    }

    private void ClearSlots()
    {
        foreach (var item in transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }

        if (_slotDictionary != null)
        {
            _slotDictionary.Clear();
        }
    }



    private void OnDisable()
    {
        _inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }


}
