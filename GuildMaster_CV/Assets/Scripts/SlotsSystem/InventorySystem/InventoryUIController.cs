using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay _inventoryPanel;
    [SerializeField] private DynamicInventoryDisplay _playerBackpackPanel;

    private void OnEnable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested += DisplayPlayerInventory;
        HeroInventory.OnHeroInventoryDisplayRequested += DisplayInventory;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDinamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnPlayerInventoryDisplayRequested -= DisplayPlayerInventory;
        HeroInventory.OnHeroInventoryDisplayRequested -= DisplayInventory;
    }

    private void DisplayInventory(InventorySystem invToDisplay, int offset)
    {
        _inventoryPanel.gameObject.SetActive(true);
        _inventoryPanel.RefreshDynamicInventory(invToDisplay, offset);
    }

    private void DisplayPlayerInventory(InventorySystem invToDisplay, int offset)
    {
        _playerBackpackPanel.gameObject.SetActive(true);
        _playerBackpackPanel.RefreshDynamicInventory(invToDisplay, offset);
    }
}
