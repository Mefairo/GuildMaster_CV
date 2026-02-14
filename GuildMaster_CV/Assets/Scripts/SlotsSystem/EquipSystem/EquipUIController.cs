using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipUIController : MonoBehaviour
{
    [SerializeField] private DynamicEquipDisplay _equipDisplay;

    private void OnEnable()
    {
        HeroEquipHolder.OnHeroEquipmentRequested += DisplayEquipment;
    }

    private void OnDisable()
    {
        HeroEquipHolder.OnHeroEquipmentRequested -= DisplayEquipment;
    }

    private void DisplayEquipment(EquipSystem equipSystem, int offset)
    {
        _equipDisplay.gameObject.SetActive(true);
        _equipDisplay.RefreshDynamicInventory(equipSystem, offset);
    }
}
