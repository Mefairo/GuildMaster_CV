using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder _inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] _slots;

    protected virtual void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    protected virtual void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    private void RefreshStaticDisplay()
    {
        if (_inventoryHolder != null)
        {
            _inventorySystem = _inventoryHolder.PrimaryInventorySystem;
            _inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }
        else
            Debug.LogWarning($"Инвентарь не назначен этому {this.gameObject}");

        AssignSlot(_inventorySystem, 0);
    }

    protected virtual void Start()
    {
        RefreshStaticDisplay();
    }

    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        _slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < _inventoryHolder.Offset; i++)
        {
            _slotDictionary.Add(_slots[i], _inventorySystem.InventorySlots[i]);
            _slots[i].Init(_inventorySystem.InventorySlots[i]);
        }
    }

}
