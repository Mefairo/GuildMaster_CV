using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendHeroesSlots : MonoBehaviour
{
    [SerializeField] private List<HeroQuestSlot_UI> _slots;
    [SerializeField] private int _amountActiveHeroes;

    private HeroQuestSlot_UI _selectedSlot;

    public List<HeroQuestSlot_UI> Slots => _slots;
    public int AmountActiveHeroes => _amountActiveHeroes;
    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponentInParent<MainQuestKeeperDisplay>();

        for (int i = 0; i < _slots.Count; i++)
        {
            _slots[i].SetSlotIndex(i);
        }
    }

    private void OnEnable()
    {
        MainQuestKeeperDisplay.OnTakeQuest += ClearAmountActiveSlots;
    }

    private void OnDisable()
    {
        MainQuestKeeperDisplay.OnTakeQuest -= ClearAmountActiveSlots;
    }

    private void ClearChooseSlot(HeroQuestSlot_UI sendhero)
    {
        foreach (HeroQuestSlot_UI slot in _slots)
        {
            slot.ClearChooseSlot();
        }
    }

    public void SelectSlot(HeroQuestSlot_UI slot)
    {
        ClearChooseSlot(null);
        _selectedSlot = slot;
        //slot.HighlightSlot(true);
    }

    public void AddHero()
    {
        _amountActiveHeroes++;
    }

    public void RemoveHero()
    {
        _amountActiveHeroes--;
    }

    public void ClearAmountActiveSlots()
    {
        _amountActiveHeroes = 0;
    }
}
