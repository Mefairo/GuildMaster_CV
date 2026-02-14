using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(200)]
public class CraftUIController : MonoBehaviour
{
    [SerializeField] private CraftKeeperDisplay _craftKeeperDisplay;

    private void Awake()
    {
        _craftKeeperDisplay.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //CraftKeeper.OnCraftWindowRequested += DisplayCraftWindow;
        //CraftKeeper.OnCraftWindowClosed += DisplayCraftWindowClose;
    }

    private void OnDisable()
    {
        //CraftKeeper.OnCraftWindowRequested -= DisplayCraftWindow;
        //CraftKeeper.OnCraftWindowClosed -= DisplayCraftWindowClose;
    }

    private void DisplayCraftWindow(CraftSystem craftSystem, PlayerInventoryHolder playerInventory, ShopTypes craftType)
    {
        _craftKeeperDisplay.gameObject.SetActive(true);
        //_craftKeeperDisplay.DisplayCraftWindow(craftSystem, playerInventory, craftType);
    }

    private void DisplayCraftWindowClose()
    {
        _craftKeeperDisplay.gameObject.SetActive(false);
    }
}
