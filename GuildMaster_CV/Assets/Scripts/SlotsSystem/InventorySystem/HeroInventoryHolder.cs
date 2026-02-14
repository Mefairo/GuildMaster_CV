using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeroInventoryHolder : InventoryHolder
{

    public static UnityAction OnPlayerInventoryChanged;

    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested;

    private void Start()
    {
        //SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
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

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        OnPlayerInventoryDisplayRequested?.Invoke(_primaryInventorySystem, 10);
    //    }
    //}

    public bool AddToInventory(SlotData data, int amount)
    {
        if (_primaryInventorySystem.AddToInventory(data, amount))
        {
            return true;
        }

        return false;
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
