using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private InventoryDisplay _playerInventory;
    [SerializeField] private Image _inventoryImage;
    [SerializeField] private Button _inventoryButton;

    public static UnityAction OnPlayerInventoryChanged;

    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

    private void Start()
    {
        //SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
        _inventoryButton?.onClick.AddListener(OpenInventory);
    }

    //protected override void LoadInventory(SaveData data)
    //{
    //    // Проверяет сохранение данных инвентаря для этого конкретного сундука, и если он существует, то загружает их
    //    if (data.playerInventory.InvSystem != null)
    //    {
    //        this.primaryInventorySystem = data.playerInventory.InvSystem;
    //        OnPlayerInventoryChanged?.Invoke();
    //    }
    //}

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.B))
        //    OnPlayerInventoryDisplayRequested?.Invoke(_primaryInventorySystem, 0);

        if (_inventoryImage.gameObject.activeSelf == false && Input.GetKeyDown(KeyCode.B))
        {
            OpenInventory();
        }

        else if (_inventoryImage.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.B))
        {
            //_playerInventory.gameObject.SetActive(false);
            _inventoryImage.gameObject.SetActive(false);
            InfoPanelUI.Instance.HideInfo();
        }

        if (_inventoryImage.gameObject.activeSelf == false && Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }

        else if (_inventoryImage.gameObject.activeSelf == true && Input.GetKeyDown(KeyCode.I))
        {
            //_playerInventory.gameObject.SetActive(false);
            _inventoryImage.gameObject.SetActive(false);
            InfoPanelUI.Instance.HideInfo();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(_primaryInventorySystem.InventorySlots[0].GetType().Name);
            Debug.Log(_primaryInventorySystem.InventorySlots[6].GetType().Name);

            if (_primaryInventorySystem.InventorySlots[0].BaseItemData == null)
                Debug.Log("000");

            if (_primaryInventorySystem.InventorySlots[6].BaseItemData == null)
                Debug.Log("001");

            if (_primaryInventorySystem.InventorySlots[0] is CatalystSlot catalystSlot)
                Debug.Log(catalystSlot.Index);
        }
    }

    public bool AddToInventory(SlotData data, int amount)
    {
        var newSlot = new InventorySlot(data, 1);
        var newSlotToAdd = SetTypeNewItem(data);

        if (_primaryInventorySystem.AddToInventory(newSlotToAdd, amount))
        {
            return true;
        }

        return false;
    }

    private InventorySlot SetTypeNewItem(SlotData slotData)
    {
        InventorySlot newSlot;

        if (slotData is BlankSlotData blankData)
            newSlot = new BlankSlot(blankData, 1);

        else if (slotData is RuneSlotData runeData)
            newSlot = new RuneSlot(runeData, runeData.Tier);

        else if (slotData is CatalystSlotData catalystData)
            newSlot = new CatalystSlot(catalystData);

        else if (slotData is EquipItemData equipData)
            newSlot = new EquipSlot(equipData);

        else
            newSlot = new InventorySlot(slotData, 1);

        return newSlot;
    }

    private void OpenInventory()
    {
        _inventoryImage.gameObject.SetActive(true);
        OnPlayerInventoryDisplayRequested?.Invoke(_primaryInventorySystem, 0);
    }

    //public bool AddToInventory(ItemPrefabData data, int amount)
    //{
    //    if (primaryInventorySystem.AddToInventory(data, amount))
    //    {
    //        return true;
    //    }

    //    return false;
    //}
}
