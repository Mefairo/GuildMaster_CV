using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] private DynamicInventoryDisplay _inventoryDisplay;

    private void Awake()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //ShopKeeper.OnShopWindowRequested += DisplayShopWindow;
        ShopKeeperDisplay.OnShopWindowClosed += DisplayShopWindowClose;
        //ChestInventory.OnChestWindowClosed += DisplayInventoryClose;
    }

    private void OnDisable()
    {
        //ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;
        ShopKeeperDisplay.OnShopWindowClosed -= DisplayShopWindowClose;
        //ChestInventory.OnChestWindowClosed -= DisplayInventoryClose;
    }


    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory, ShopTypes shopType)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        //_shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory, shopType);
    }

    private void DisplayShopWindowClose()
    {
        _shopKeeperDisplay.gameObject.SetActive(false);
    }

    private void DisplayInventoryClose()
    {
        _inventoryDisplay.gameObject.SetActive(false);
    }
}
