using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Collections.Unicode;

public class ReforgeGiverTest : MonoBehaviour
{
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private List<BlankSlotData> _blankList = new List<BlankSlotData>();
    [SerializeField] private List<RuneSlotData> _runeList = new List<RuneSlotData>();
    [SerializeField] private List<MainCatalyst> _catalystList = new List<MainCatalyst>();
    [SerializeField] private List<int> _amountItems = new List<int>();


    private void Awake()
    {
        _buttonSelf?.onClick.AddListener(AddReforgeItems);
    }

    private void AddReforgeItems()
    {
        for (int i = 0; i < _amountItems[0]; i++)
        {
            BlankSlot blank = new BlankSlot(_blankList[0], 1);
            //_playerInventoryHolder.PrimaryInventorySystem.AddToInventory(blank.BlankSlotData, 1);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(blank, 1);
        }

        for (int i = 0; i < _amountItems[1]; i++)
        {
            RuneSlot rune1 = new RuneSlot(_runeList[0], 1);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(rune1, 1);

            RuneSlot rune2 = new RuneSlot(_runeList[1], 1);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(rune2, 1);

            RuneSlot rune3 = new RuneSlot(_runeList[2], 1);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(rune3, 1);
        }

        for (int i = 0; i < _amountItems[2]; i++)
        {
            CatalystSlot catalyst1 = new CatalystSlot(_catalystList[0]);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(catalyst1, 1);

            CatalystSlot catalyst2 = new CatalystSlot(_catalystList[1]);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(catalyst2, 1);

            CatalystSlot catalyst3 = new CatalystSlot(_catalystList[2]);
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(catalyst3, 1);
        }
    }
}
