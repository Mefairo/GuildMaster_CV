using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

[System.Serializable]
public class CalculationAbilitySystem
{
    [SerializeField] private float _abilityPercent;
    [SerializeField] private float _coef1_1;
    [SerializeField] private float _coef1_2;
    [SerializeField] private float _coef2;
    [SerializeField] private float _coef3;
    [Header("Abilities Hero")]
    //[SerializeField] private List<Ability> _abilityHeroList;
    [SerializeField] private List<Ability> _heroAuraAbilities;
    [Header("Heroes Ability Stats")]
    [SerializeField] private List<float> _powerAbilitiesHero = new List<float> { 0, 0, 0, 0 };
    [SerializeField] private List<float> _defenceAbilitiesHero = new List<float> { 0, 0, 0, 0 };
    [Header("Heroes")]
    //[SerializeField] private List<Hero> _localHeroes = new List<Hero>();

    private SendHeroesSlots _sendHeroesSlots;
    private QuestSlot_UI _selectedQuest;

    public event UnityAction OnRemoveAbility;
    public event UnityAction<float, int> OnUpdatePower;
    public event UnityAction<float, int> OnUpdateDefence;

    public List<float> PowerAbilitiesHero => _powerAbilitiesHero;
    public List<float> DefenceAbilitiesHero => _defenceAbilitiesHero;
    public List<Ability> HeroAuraAbilities => _heroAuraAbilities;

    public void Start()
    {
        _powerAbilitiesHero = new List<float> { 0, 0, 0, 0 };
        _defenceAbilitiesHero = new List<float> { 0, 0, 0, 0 };
    }

    public void RemoveLocalHero(HeroQuestSlot_UI heroSlot, SendHeroesSlots heroes, int indexHeroSlot)
    {
        //_sendHeroesSlots = heroes;
        //int suppresAuraAmount = CheckSuppressionAuras(_selectedQuest.Quest.Quest);

        //for (int i = 0; i < heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList.Count; i++)
        //{
        //    if (heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i] != null && 
        //        heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i].AbilityData != null)
        //    {
        //        RemoveAbility(heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i], heroSlot);

        //        if (heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i].AbilityData.TypeAbility == TypeAbilities.Aura &&
        //            heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i].AbilityData.GeneralType == GeneralTypeAbility.Stats)
        //            _heroAuraAbilities.Remove(heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i]);
        //    }

        //    heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList[i] = new Ability();
        //}

        //foreach (Ability auraAbility in _heroAuraAbilities)
        //    auraAbility.ApplySingleAuraEffect(auraAbility, heroSlot, false, suppresAuraAmount);

        //_powerAbilitiesHero[indexHeroSlot] = 0;
        //_defenceAbilitiesHero[indexHeroSlot] = 0;
    }

    public void CheckAbilitiesHero(HeroQuestSlot_UI heroSlot, AbilitySlot_UI abilitySlot_UI, QuestSlot_UI questSlot, SendHeroesSlots heroes)
    {
        //_selectedQuest = questSlot;
        //_sendHeroesSlots = heroes;
        //int indexHero = -1;

        //foreach (HeroQuestSlot_UI activeHero in heroes.Slots)
        //{
        //    if (activeHero != null && activeHero.Hero != null && activeHero.Hero.HeroData != null)
        //    {
        //        if (activeHero.Hero == heroSlot.Hero)
        //        {
        //            indexHero = activeHero.SlotIndex;
        //        }
        //    }
        //}

        //switch (abilitySlot_UI.Ability.AbilityData.GeneralType)
        //{
        //    case GeneralTypeAbility.None:
        //        break;

        //    case GeneralTypeAbility.Attack:
        //        CalculationAttackAbilityHero(heroSlot, abilitySlot_UI.Ability, indexHero);
        //        break;

        //    case GeneralTypeAbility.Stats:
        //        CalculationStatsAbilityHero(heroSlot, abilitySlot_UI.Ability, indexHero);
        //        break;
        //}

        //SetStatsHero(heroSlot, indexHero);
    }

    private void CalculationAttackAbilityHero(HeroQuestSlot_UI heroSlot, Ability ability, int indexHero)
    {
        if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        {
            _coef1_1 = 0;

            //if (ability.AbilityData.UniqAbility)
            //{
            //    _powerAbilitiesHero[indexHero] += ability.ActivateHeroAttackAbility(heroSlot, ability, _selectedQuest, _sendHeroesSlots.Slots);
            //}

            //else
            {
                switch (ability.AbilityData.TypeAbility)
                {
                    case TypeAbilities.None:
                        Debug.LogWarning("NOne hero ability");
                        break;

                    case TypeAbilities.DamageSingleTarget:
                        _coef1_1 += ability.AbilityDamageSingleTarget(_selectedQuest, heroSlot);
                        break;

                    case TypeAbilities.AoEDamage:
                        _coef1_1 += ability.AbilityDamageAoeDamage(_selectedQuest, heroSlot);
                        break;

                    case TypeAbilities.Summon:
                        _coef1_1 += ability.AbilitySummonDamage(_selectedQuest, heroSlot);
                        break;

                    case TypeAbilities.Aura:

                        //int suppresAuraAmount = CheckSuppressionAuras(_selectedQuest.Quest.Quest);
                        //int suppresAuraAmount = 0;

                        //if (ability.AbilityData.MasterAura)
                        //    _coef1_1 += ability.AbilityDamageAuraEffect(_selectedQuest, 0, heroSlot);

                        //else
                            //_coef1_1 += ability.AbilityDamageAuraEffect(_selectedQuest, suppresAuraAmount, heroSlot);

                        break;
                }

            }
            //_powerAbilitiesHero[indexHero] += Mathf.FloorToInt(heroSlot.Hero.PowerPoints * _coef1_1 / 100);
        }


        OnUpdatePower?.Invoke(_powerAbilitiesHero[indexHero], indexHero);
    }

    private void CalculationStatsAbilityHero(HeroQuestSlot_UI heroSlot, Ability ability, int indexHero)
    {
        //if (ability.AbilityData.UniqAbility)
        //    ability.ActivateHeroStatsAbility(heroSlot, ability, _selectedQuest, _sendHeroesSlots.Slots, true);

        //else
        //{
        //    switch (ability.AbilityData.TypeAbility)
        //    {
        //        case TypeAbilities.None:
        //            Debug.LogWarning("NOne hero ability");
        //            break;

        //        case TypeAbilities.Aura:

        //            //int suppresAuraAmount = CheckSuppressionAuras(_selectedQuest.Quest.Quest);
        //            int suppresAuraAmount = 0;

        //            _heroAuraAbilities.Add(ability);

        //            if (ability.AbilityData.MasterAura)
        //                ability.AbilityBuffAuraEffect(ability, _selectedQuest, heroSlot, true, _sendHeroesSlots, 0);

        //            else
        //                ability.AbilityBuffAuraEffect(ability, _selectedQuest, heroSlot, true, _sendHeroesSlots, suppresAuraAmount);

        //            break;

        //        case TypeAbilities.Buff:
        //            ability.AbilityBuffEffect(ability, _selectedQuest, heroSlot, true);
        //            break;
        //    }
        //}

        //// Теперь пересчитываем защиту героя
        //if (heroSlot != null)
        //{
        //    _defenceAbilitiesHero[indexHero] = heroSlot.Hero.DefencePoints;
        //}

        //// Обновляем атаку и защиту
        //RecalculateAttackAbilities(heroSlot, indexHero);
        //OnUpdateDefence?.Invoke(_defenceAbilitiesHero[indexHero], indexHero);
    }

    public void RecalculateAttackAbilities(SendHeroesSlots heroes, QuestSlot_UI questSlot)
    {
        //_selectedQuest = questSlot;

        //foreach (HeroQuestSlot_UI heroSlot in heroes.Slots)
        //{
        //    if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        //        RecalculateAttackAbilities(heroSlot, heroSlot.SlotIndex);
        //}
    }

    private void RecalculateAttackAbilities(HeroQuestSlot_UI heroSlot, int indexHero)
    {
        // Обнуляем текущую силу героя
        //_coef1_1 = 0;
        //_powerAbilitiesHero[indexHero] = 0;

        //// Перебираем все способности героя
        //foreach (Ability ability in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
        //{
        //    if (ability.AbilityData != null)
        //    {
        //        if (ability.AbilityData.GeneralType == GeneralTypeAbility.Attack)
        //        {
        //            if (ability.AbilityData.UniqAbility)
        //            {
        //                _powerAbilitiesHero[indexHero] += ability.ActivateHeroAttackAbility(heroSlot, ability, _selectedQuest, _sendHeroesSlots.Slots);
        //            }

        //            else
        //            {
        //                switch (ability.AbilityData.TypeAbility)
        //                {
        //                    case TypeAbilities.None:
        //                        Debug.LogWarning("NOne hero ability");
        //                        break;

        //                    case TypeAbilities.DamageSingleTarget:
        //                        _coef1_1 += ability.AbilityDamageSingleTarget(_selectedQuest, heroSlot);
        //                        break;

        //                    case TypeAbilities.AoEDamage:
        //                        _coef1_1 += ability.AbilityDamageAoeDamage(_selectedQuest, heroSlot);
        //                        break;

        //                    case TypeAbilities.Aura:

        //                        int suppresAuraAmount = CheckSuppressionAuras(_selectedQuest.Quest.Quest);

        //                        if (ability.AbilityData.MasterAura)
        //                            _coef1_1 += ability.AbilityDamageAuraEffect(_selectedQuest, 0, heroSlot);

        //                        else
        //                            _coef1_1 += ability.AbilityDamageAuraEffect(_selectedQuest, suppresAuraAmount, heroSlot);
        //                        break;

        //                    case TypeAbilities.Summon:
        //                        _coef1_1 += ability.AbilitySummonDamage(_selectedQuest, heroSlot);
        //                        break;
        //                }
        //            }
        //        }

        //        // Прибавляем значение силы от этой абилки
        //    }
        //}
        //_powerAbilitiesHero[indexHero] += Mathf.FloorToInt(heroSlot.Hero.PowerPoints * _coef1_1 / 100);

        //// Вызываем событие обновления силы
        //SetStatsHero(heroSlot, indexHero);
        //OnUpdatePower?.Invoke(_powerAbilitiesHero[indexHero], indexHero);
    }

    public void RecalculateStatsAbilities(Hero hero)
    {
        //foreach (Ability auraAbility in _heroAuraAbilities)
        //{
        //    int suppresAuraAmount = 0;

        //    foreach (Enemy enemy in _selectedQuest.Quest.Quest.EnemiesList)
        //    {
        //        foreach (Ability enemyAbility in enemy.ListAbilities)
        //        {
        //            if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Suppression_Aura)
        //                suppresAuraAmount++;
        //        }
        //    }

        //    if (auraAbility.AbilityData.MasterAura)
        //        auraAbility.ApplySingleAuraEffect(auraAbility, hero, true, 0);

        //    else
        //        auraAbility.ApplySingleAuraEffect(auraAbility, hero, true, suppresAuraAmount);
        //}

    }

    public void RemoveAbility(Ability ability, HeroQuestSlot_UI heroSlot)
    {
        //int indexHero = -1;

        //foreach (HeroQuestSlot_UI activeHero in _sendHeroesSlots.Slots)
        //{
        //    if (activeHero != null && activeHero.Hero != null && activeHero.Hero.HeroData != null)
        //    {
        //        if (activeHero.Hero == heroSlot.Hero)
        //        {
        //            indexHero = activeHero.SlotIndex;
        //        }
        //    }
        //}

        //if (ability.AbilityData.UniqAbility)
        //    ability.ActivateHeroStatsAbility(heroSlot, ability, _selectedQuest, _sendHeroesSlots.Slots, false);

        //else
        //{
        //    if (ability.AbilityData.TypeAbility == TypeAbilities.Buff)
        //    {
        //        ability.AbilityBuffEffect(ability, _selectedQuest, heroSlot, false);
        //    }

        //    if (ability.AbilityData.TypeAbility == TypeAbilities.Aura)
        //    {
        //        int suppresAuraAmount = CheckSuppressionAuras(_selectedQuest.Quest.Quest);

        //        _heroAuraAbilities.Remove(ability);

        //        if (ability.AbilityData.MasterAura)
        //            ability.AbilityBuffAuraEffect(ability, _selectedQuest, heroSlot, false, _sendHeroesSlots, 0);

        //        else
        //            ability.AbilityBuffAuraEffect(ability, _selectedQuest, heroSlot, false, _sendHeroesSlots, suppresAuraAmount);

        //    }
        //}

        //OnRemoveAbility?.Invoke();
        //RecalculateAttackAbilities(heroSlot, indexHero);
    }

    private void SetStatsHero(HeroQuestSlot_UI heroSlot, int heroIndex)
    {
        if (heroSlot != null && heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
        {
            _defenceAbilitiesHero[heroIndex] = heroSlot.Hero.DefencePoints;
        }

        OnUpdateDefence?.Invoke(_defenceAbilitiesHero[heroIndex], heroIndex);
    }

    private int CheckSuppressionAuras(Quest quest)
    {
        return 0;
        //int suppresAuraAmount = 0;

        //foreach (Enemy enemy in quest.EnemiesList)
        //{
        //    foreach (Ability enemyAbility in enemy.ListAbilities)
        //    {
        //        if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Suppression_Aura)
        //            suppresAuraAmount++;
        //    }
        //}
        //if (suppresAuraAmount == 0)
        //    return 1;

        //else
        //    return (suppresAuraAmount + 1);
    }

    public void ClearAuraList()
    {
        //_heroAuraAbilities.Clear();
    }
}
