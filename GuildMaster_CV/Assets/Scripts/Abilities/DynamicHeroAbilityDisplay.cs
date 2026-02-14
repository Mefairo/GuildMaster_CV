using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicHeroAbilityDisplay : MonoBehaviour
{
    [SerializeField] private List<HeroAbilitySlot_UI> _abilitySlots;
    [SerializeField] private KeeperDisplay _keeperDisplay;
    [SerializeField] private MainQuestKeeperDisplay _mainQuestKeeperDisplay;
    [SerializeField] private QuestParametresSystemFix _questParametresSystemFix;

    private HeroAbilitySystem _heroAbilitySystem;
    private Dictionary<HeroAbilitySlot_UI, HeroAbilitySlot> _abilityDictionary;
    private HeroAbilitySlot_UI _selectedSlot;

    public List<HeroAbilitySlot_UI> AbilitySlots => _abilitySlots;

    private void OnEnable()
    {
        //if (_mainQuestKeeperDisplay == null || _questParametresSystemFix == null)
        //    return;

        if (_questParametresSystemFix.GuildTalentSystem.ExtraAbilitySlot.IsTakenTalent)
            _abilitySlots[2].UnlockSlot();

        else
            _abilitySlots[2].LockSlot();

        if (_mainQuestKeeperDisplay.SelectedHero != null && _mainQuestKeeperDisplay.SelectedHero.Hero != null)
            RefreshDynamicAbilities(_mainQuestKeeperDisplay.SelectedHero.Hero.AbilityHolder.HeroAbilitySystem);

        HeroAbilitySlot_UI.OnDeleteAbility += RefreshDynamicAbilities;
    }

    private void OnDisable()
    {
        //if (_mainQuestKeeperDisplay == null || _questParametresSystemFix == null)
        //    return;

        HeroAbilitySlot_UI.OnDeleteAbility -= RefreshDynamicAbilities;
    }

    private void RefreshDynamicAbilities()
    {
        RefreshDynamicAbilities(_mainQuestKeeperDisplay.SelectedHero.Hero.AbilityHolder.HeroAbilitySystem);
    }

    public void RefreshDynamicAbilities(HeroAbilitySystem system)
    {
        ClearSlots();

        _heroAbilitySystem = system;

        if (_mainQuestKeeperDisplay.SelectedHero != null && _mainQuestKeeperDisplay.SelectedHero.Hero != null &&
            _mainQuestKeeperDisplay.SelectedHero.Hero.HeroData != null)
        {
            if (_mainQuestKeeperDisplay.SelectedHero.Hero.BuffSystem.SilenceDebuff)
            {
                if (_questParametresSystemFix.GuildTalentSystem.ExtraAbilitySlot.IsTakenTalent)
                {
                    _abilitySlots[1].UnlockSlot();
                    _abilitySlots[2].LockSlot();

                    _abilitySlots[2].DeleteSlot();
                }

                else
                {
                    _abilitySlots[1].LockSlot();

                    _abilitySlots[1].DeleteSlot();
                }
            }

            else
            {
                if (_questParametresSystemFix.GuildTalentSystem.ExtraAbilitySlot.IsTakenTalent)
                {
                    _abilitySlots[1].UnlockSlot();
                    _abilitySlots[2].UnlockSlot();
                }

                else
                {
                    _abilitySlots[1].UnlockSlot();
                    _abilitySlots[2].LockSlot();

                    _abilitySlots[2].DeleteSlot();
                }
            }
        }

        AssignAbilitySlot(system);
    }

    private void AssignAbilitySlot(HeroAbilitySystem system)
    {
        ClearSlots();

        _abilityDictionary = new Dictionary<HeroAbilitySlot_UI, HeroAbilitySlot>();

        if (_heroAbilitySystem == null)
            return;

        for (int i = 0; i < system.AbilityList.Count; i++)
        {
            _abilitySlots[i].Init(system.AbilityList[i]);
        }
    }

    private void ClearSlots()
    {
        foreach (HeroAbilitySlot_UI slot in _abilitySlots)
        {
            slot.ClearChooseSlot();
        }

        if (_abilityDictionary != null)
        {
            _abilityDictionary.Clear();
        }
    }
    private void ClearChooseSlot(HeroAbilitySlot_UI abil)
    {
        foreach (HeroAbilitySlot_UI slot in _abilitySlots)
        {
            slot.ClearChooseSlot();
        }
    }

    public void SelectSlot(HeroAbilitySlot_UI selectSlot)
    {
        foreach (HeroAbilitySlot_UI slot in _abilitySlots)
        {
            slot.SetSlotClicked(false);
            slot.ChangeColorFrame(Color.black);
            slot.SetDefaultIcon();
        }

        _selectedSlot = selectSlot;

        selectSlot.SetSlotClicked(true);
        selectSlot.ChangeColorFrame(Color.green);
    }
}
