using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeroInventory : InventoryHolder
{
    [SerializeField] private Button _activeButton;

    public static UnityAction<InventorySystem, int> OnHeroInventoryDisplayRequested;
    public static UnityAction OnInventoryWindowClosed;

    protected override void Awake()
    {
        base.Awake();

        _activeButton.onClick.AddListener(ShowInventory);
    }

    private void Start()
    {
        var chestSaveData = new InventorySaveData(_primaryInventorySystem, transform.position, transform.rotation);
    }

    private void ShowInventory()
    {
        OnHeroInventoryDisplayRequested?.Invoke(_primaryInventorySystem, 0);
    }
}
