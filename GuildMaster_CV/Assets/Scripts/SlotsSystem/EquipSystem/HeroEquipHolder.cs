using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HeroEquipHolder : MonoBehaviour
{
    [SerializeField] private int _equipSize;
    [SerializeField] private EquipSystem _equipSystem;
    [SerializeField] private Button _activeButton;

    public EquipSystem EquipSystem => _equipSystem;

    public static UnityAction<EquipSystem, int> OnHeroEquipmentRequested;

    private void Awake()
    {
        NewEquip();

        _activeButton.onClick.AddListener(ShowEquipment);
    }

    private void ShowEquipment()
    {
        OnHeroEquipmentRequested?.Invoke(_equipSystem, 0);
    }

    public void NewEquip()
    {
        _equipSystem = new EquipSystem(_equipSize);
    }
}
