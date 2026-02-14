using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;
using System;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public Image BackgroundSprite;
    public TextMeshProUGUI ItemCount;
    public InventorySlot AssignedInventorySlot;
    public BlankSlot BlankTest;

    private ChangeColorBackgroundItem _changeColorBack = new ChangeColorBackgroundItem();

    //[SerializeField] private int _dropOffset;
    //[SerializeField] private Transform _pointDrop;


    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;

        BackgroundSprite.color = Color.clear;
        BackgroundSprite.preserveAspect = true;

        ItemCount.text = "";
    }

    public void UpdateMouseSlot(InventorySlot invSlot)
    {
        //AssignedInventorySlot.AssignItem(invSlot);

        //UpdateMouseSlot();

        if (invSlot == null)
        {
            ClearSlot();
            return;
        }

        if (invSlot is BlankSlot blank)
        {
            AssignedInventorySlot = new BlankSlot(blank, blank.Index);
        }

        else if(invSlot is RuneSlot rune)
            AssignedInventorySlot = new RuneSlot(rune, rune.Index);

        else if (invSlot is CatalystSlot catalyst)
            AssignedInventorySlot = new CatalystSlot(catalyst, catalyst.Index);

        else if (invSlot is EquipSlot equip)
            AssignedInventorySlot = new EquipSlot(equip, equip.Index);

        else if (invSlot is InventorySlot inventorySlot)
            AssignedInventorySlot = new InventorySlot(inventorySlot.BaseItemData, inventorySlot.StackSize);

        AssignedInventorySlot.UpdateInventorySlot(invSlot.BaseItemData, invSlot.StackSize);
        //if (invSlot is BlankSlot blank)
        //{
        //    Debug.Log("assing blank mouse");
        //    AssignedInventorySlot = new BlankSlot(blank, blank.Index);
        //    //AssignedInventorySlot = clone;
        //    AssignedInventorySlot.AssignItem(AssignedInventorySlot);
        //}

        //else
        //{
        //    Debug.Log("assing inv mouse");
        //    AssignedInventorySlot = new InventorySlot(invSlot.BaseItemData, invSlot.StackSize);
        //    //AssignedInventorySlot.AssignItem(invSlot);
        //}

        if (AssignedInventorySlot == null)
            Debug.Log("mouse null 1");

        if (AssignedInventorySlot.BaseItemData == null)
            Debug.Log("mouse null 2");

        if (AssignedInventorySlot.ItemData == null)
            Debug.Log("mouse null 3");

        UpdateMouseSlot();
    }

    public InventorySlot SetMouseSlotEquip(InventorySlot equipSlot, EquipSlot slotToCompare)
    {
        InventorySlot newSlot;

        if (slotToCompare is BlankSlot blank)
            newSlot = new BlankSlot(blank, blank.Index);

        else if (slotToCompare is EquipSlot equip)
            newSlot = new EquipSlot(equip, equip.Index);

        else
            newSlot = new InventorySlot();

        return newSlot;
    }

    public void UpdateMouseSlot()
    {
        ItemSprite.sprite = AssignedInventorySlot.BaseItemData.Icon;
        ItemSprite.color = Color.white;

        _changeColorBack.SetBackgroundItem(AssignedInventorySlot, BackgroundSprite);

        //if (AssignedInventorySlot.ItemData.IconBackground != null)
        //    ChangeBackgroundColor();

        //else
        //    BackgroundSprite.color = BackgroundSprite.color.WithAlpha(0);

        ItemCount.text = AssignedInventorySlot.StackSize.ToString();

    }

    private void Update()
    {
        DropItem();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Тип объекта под мышью: " + this.AssignedInventorySlot.GetType().Name);
            if (AssignedInventorySlot.BaseItemData == null)
                Debug.Log("mouse base null");

            if (AssignedInventorySlot is BlankSlot blank)
                Debug.Log(blank.CraftValue.ToString());

            if (AssignedInventorySlot is RuneSlot rune)
                Debug.Log(rune.RuneSlotData.DisplayName);

            if (AssignedInventorySlot is CatalystSlot cata)
                Debug.Log(cata.CatalystSlotData.DisplayName);
        }
    }

    private void DropItem()
    {
        if (AssignedInventorySlot.ItemData != null)
        {
            transform.position = Mouse.current.position.ReadValue();

            if (Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                return;
            }
        }
    }

    public void ClearSlot()
    {
        AssignedInventorySlot.ClearSlot();

        ItemCount.text = "";

        ItemSprite.sprite = null;
        ItemSprite.color = Color.clear;

        BackgroundSprite.sprite = null;
        BackgroundSprite.color = Color.clear;
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
