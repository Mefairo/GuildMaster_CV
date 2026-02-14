using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HeroEquipmentHolder
{
    [SerializeField] private int _equipSize = 19;
    [SerializeField] private EquipSystem _equipSystem;

    public EquipSystem EquipSystem => _equipSystem;

    public static UnityAction<EquipSystem, int> OnHeroEquipmentRequested;

    public HeroEquipmentHolder()
    {
        _equipSystem = new EquipSystem(_equipSize);
    }

    public HeroEquipmentHolder(EquipSystem copyEquipSystem)
    {
        _equipSystem = new EquipSystem(copyEquipSystem);
    }

    public void ShowEquipment()
    {
        OnHeroEquipmentRequested?.Invoke(_equipSystem, 0);
    }
}
