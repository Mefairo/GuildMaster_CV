using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HeroDefencePoints : DefencePoints
{
    [SerializeField] private float _baseDefence;
    [SerializeField] private float _equipDefence;
    [SerializeField] private float _abilityDefence;
    [SerializeField] private float _auraDefence;
    [SerializeField] private float _debuffDefence;
    [SerializeField] private float _allDefence; // »—œŒÀ‹«”≈“—ﬂ ƒÀﬂ –¿—◊≈“¿ ¬ «¿ƒ¿Õ»ﬂ’ QuestParametresSystemFix
    [SerializeField] private float _visibleAllDefence; // »—œŒÀ‹«”≈“—ﬂ ƒÀﬂ –¿—◊≈“¿ ¬ Ã≈Õﬁ √»À‹ƒ»» KeeperDisplay

    [SerializeField] private float _baseCoefDefence = 1;
    [SerializeField] private float _extraCoefDefence;
    [SerializeField] private float _allCoefDefence;

    public float AllCoefDefence => _allCoefDefence;
    public float VisibleAllDefence => _visibleAllDefence;

    public static UnityAction<float> OnDefenceChanged;

    public float BaseDefence => _baseDefence;
    public float AllDefence
    {
        get => _allDefence;
        private set
        {
            _allDefence = value;
            OnDefenceChanged?.Invoke(value);
        }
    }

    public HeroDefencePoints(float allDefence, float allCoefDefence) // ƒÀﬂ  Œœ»» √≈–Œﬂ ¬«ﬂ“Œ√Œ  ¬≈—“¿
    {
        _allDefence = 0;
        _allCoefDefence = 0;

        _allDefence = allDefence;
        _allCoefDefence = allCoefDefence;
    }

    public HeroDefencePoints() { }


    #region DEFENCE MULTI CALCULATION
    public void DefenceMultiBase(Hero hero)
    {
        hero.VisibleHeroStats.EnduranceMulti += hero.HeroData.HeroStats.EnduranceMulti;
        hero.VisibleHeroStats.FlexibilityMulti += hero.HeroData.HeroStats.FlexibilityMulti;
        hero.VisibleHeroStats.SanityMulti += hero.HeroData.HeroStats.SanityMulti;

        hero.VisibleHeroStats.DurabilityMulti += hero.HeroData.HeroStats.DurabilityMulti;
        hero.VisibleHeroStats.AdaptabilityMulti += hero.HeroData.HeroStats.AdaptabilityMulti;
        hero.VisibleHeroStats.SuppressionMulti += hero.HeroData.HeroStats.SuppressionMulti;
    }

    public void DefenceMultiLevelUp(Hero hero)
    {
        hero.VisibleHeroStats.DurabilityMulti += hero.LevelUpHeroStats.DurabilityMulti;
        hero.VisibleHeroStats.AdaptabilityMulti += hero.LevelUpHeroStats.AdaptabilityMulti;
        hero.VisibleHeroStats.SuppressionMulti += hero.LevelUpHeroStats.SuppressionMulti;
    }

    public void DefenceMultiEquip(Hero hero)
    {
        foreach (EquipSlot equipItemSlot in hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipItemSlot != null && equipItemSlot.BaseItemData != null)
            {
                hero.VisibleHeroStats.EnduranceMulti += equipItemSlot.ItemStats[0].EnduranceMulti;
                hero.VisibleHeroStats.FlexibilityMulti += equipItemSlot.ItemStats[0].FlexibilityMulti;
                hero.VisibleHeroStats.SanityMulti += equipItemSlot.ItemStats[0].SanityMulti;

                hero.VisibleHeroStats.DurabilityMulti += equipItemSlot.ItemStats[0].DurabilityMulti;
                hero.VisibleHeroStats.AdaptabilityMulti += equipItemSlot.ItemStats[0].AdaptabilityMulti;
                hero.VisibleHeroStats.SuppressionMulti += equipItemSlot.ItemStats[0].SuppressionMulti;
            }
        }
    }

    public void DefenceMultiAbility(Hero hero)
    {
        foreach (Ability heroAbility in hero.ActiveBuffs)
        {
            if (heroAbility != null && heroAbility.AbilityData != null)
            {
                hero.VisibleHeroStats.EnduranceMulti += heroAbility.Stats[0].EnduranceMulti;
                hero.VisibleHeroStats.FlexibilityMulti += heroAbility.Stats[0].FlexibilityMulti;
                hero.VisibleHeroStats.SanityMulti += heroAbility.Stats[0].SanityMulti;

                hero.VisibleHeroStats.DurabilityMulti += heroAbility.Stats[0].DurabilityMulti;
                hero.VisibleHeroStats.AdaptabilityMulti += heroAbility.Stats[0].AdaptabilityMulti;
                hero.VisibleHeroStats.SuppressionMulti += heroAbility.Stats[0].SuppressionMulti;
            }
        }
    }

    public void DefenceMultiAura(Hero hero, Quest quest, List<Ability> heroAuras)
    {
        if (heroAuras.Count != 0)
        {
            int amountSuppresionAura = 0;

            foreach (Enemy enemy in quest.EnemiesList)
            {
                foreach (Ability enemyAbility in enemy.ListAbilities)
                {
                    if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Suppression_Aura)
                        amountSuppresionAura++;
                }
            }

            foreach (Ability heroAura in heroAuras)
            {
                if (heroAura.AbilityData.MasterAura)
                {
                    hero.VisibleHeroStats.EnduranceMulti += heroAura.Stats[0].EnduranceMulti;
                    hero.VisibleHeroStats.FlexibilityMulti += heroAura.Stats[0].FlexibilityMulti;
                    hero.VisibleHeroStats.SanityMulti += heroAura.Stats[0].SanityMulti;

                    hero.VisibleHeroStats.DurabilityMulti += heroAura.Stats[0].DurabilityMulti;
                    hero.VisibleHeroStats.AdaptabilityMulti += heroAura.Stats[0].AdaptabilityMulti;
                    hero.VisibleHeroStats.SuppressionMulti += heroAura.Stats[0].SuppressionMulti;
                }

                else
                {
                    hero.VisibleHeroStats.EnduranceMulti += (heroAura.Stats[0].EnduranceMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.FlexibilityMulti += (heroAura.Stats[0].FlexibilityMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.SanityMulti += (heroAura.Stats[0].SanityMulti / (amountSuppresionAura + 1));

                    hero.VisibleHeroStats.DurabilityMulti += (heroAura.Stats[0].DurabilityMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.AdaptabilityMulti += (heroAura.Stats[0].AdaptabilityMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.SuppressionMulti += (heroAura.Stats[0].SuppressionMulti / (amountSuppresionAura + 1));
                }
            }
        }
    }


    public void TalentDefenceMulti(Hero hero, QuestParametresSystemFix questSystem)
    {
        if(questSystem.GuildTalentSystem.DefenceUpStats.IsTakenTalent)
        {
            hero.VisibleHeroStats.DurabilityMulti += questSystem.GuildTalentSystem.DefenceUpStats.DefenceStatValue;
            hero.VisibleHeroStats.AdaptabilityMulti += questSystem.GuildTalentSystem.DefenceUpStats.DefenceStatValue;
            hero.VisibleHeroStats.SuppressionMulti += questSystem.GuildTalentSystem.DefenceUpStats.DefenceStatValue;
        }

        if (questSystem.GuildTalentSystem.HealthUpStats.IsTakenTalent)
        {
            hero.VisibleHeroStats.Health += questSystem.GuildTalentSystem.HealthUpStats.ExtraHP;
        }
    }
    #endregion


    #region DEFENCE VALUES CALCULATION
    public float CalculateBaseDefenceValue(Hero hero)
    {
        _baseDefence = 0;

        hero.VisibleHeroStats.EnduranceValue += hero.HeroData.HeroStats.EnduranceValue;
        hero.VisibleHeroStats.FlexibilityValue += hero.HeroData.HeroStats.FlexibilityValue;
        hero.VisibleHeroStats.SanityValue += hero.HeroData.HeroStats.SanityValue;

        hero.VisibleHeroStats.DurabilityValue += hero.HeroData.HeroStats.DurabilityValue;
        hero.VisibleHeroStats.AdaptabilityValue += hero.HeroData.HeroStats.AdaptabilityValue;
        hero.VisibleHeroStats.SuppressionValue += hero.HeroData.HeroStats.SuppressionValue;

        hero.VisibleHeroStats.Health += hero.HeroData.HeroStats.Health;

        int defenceEndurance = hero.VisibleHeroStats.EnduranceMulti * hero.VisibleHeroStats.EnduranceValue;
        int defenceFlexibility = hero.VisibleHeroStats.FlexibilityMulti * hero.VisibleHeroStats.FlexibilityValue;
        int defenceSanity = hero.VisibleHeroStats.SanityMulti * hero.VisibleHeroStats.SanityValue;

        int defenceDurability = hero.VisibleHeroStats.DurabilityMulti * hero.VisibleHeroStats.DurabilityValue;
        int defenceAdaptability = hero.VisibleHeroStats.AdaptabilityMulti * hero.VisibleHeroStats.AdaptabilityValue;
        int defenceSuppression = hero.VisibleHeroStats.SuppressionMulti * hero.VisibleHeroStats.SuppressionValue;

        _baseDefence += defenceEndurance + defenceFlexibility + defenceSanity + defenceDurability + defenceAdaptability
            + defenceSuppression + hero.VisibleHeroStats.Health;

        return _baseDefence;
    }

    public void EquipDefenceValue(Hero hero)
    {
        _equipDefence = 0;

        foreach (EquipSlot equipItemSlot in hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipItemSlot != null && equipItemSlot.BaseItemData != null)
            {
                int defenceEndurance = hero.VisibleHeroStats.EnduranceMulti * equipItemSlot.ItemStats[0].EnduranceValue;
                int defenceFlexibility = hero.VisibleHeroStats.FlexibilityMulti * equipItemSlot.ItemStats[0].FlexibilityValue;
                int defenceSanity = hero.VisibleHeroStats.SanityMulti * equipItemSlot.ItemStats[0].SanityValue;

                int defenceDurability = hero.VisibleHeroStats.DurabilityMulti * equipItemSlot.ItemStats[0].DurabilityValue;
                int defenceAdaptability = hero.VisibleHeroStats.AdaptabilityMulti * equipItemSlot.ItemStats[0].AdaptabilityValue;
                int defenceSuppression = hero.VisibleHeroStats.SuppressionMulti * equipItemSlot.ItemStats[0].SuppressionValue;

                #region VISIBLE EQUIP STATS VALUE
                hero.VisibleHeroStats.EnduranceValue += equipItemSlot.ItemStats[0].EnduranceValue;
                hero.VisibleHeroStats.FlexibilityValue += equipItemSlot.ItemStats[0].FlexibilityValue;
                hero.VisibleHeroStats.SanityValue += equipItemSlot.ItemStats[0].SanityValue;

                hero.VisibleHeroStats.DurabilityValue += equipItemSlot.ItemStats[0].DurabilityValue;
                hero.VisibleHeroStats.AdaptabilityValue += equipItemSlot.ItemStats[0].AdaptabilityValue;
                hero.VisibleHeroStats.SuppressionValue += equipItemSlot.ItemStats[0].SuppressionValue;

                hero.VisibleHeroStats.Health += equipItemSlot.ItemStats[0].Health;
                #endregion

                _equipDefence += defenceEndurance + defenceFlexibility + defenceSanity + defenceDurability + defenceAdaptability
            + defenceSuppression + equipItemSlot.ItemStats[0].Health;
            }
        }
    }

    public void AbilityDefenceValue(Hero hero)
    {
        _abilityDefence = 0;

        foreach (Ability heroAbility in hero.ActiveBuffs)
        {
            if (heroAbility != null && heroAbility.AbilityData != null)
            {
                if(heroAbility.Stats.Count != 0)
                {
                    int defenceEndurance = hero.VisibleHeroStats.EnduranceMulti * heroAbility.Stats[0].EnduranceValue;
                    int defenceFlexibility = hero.VisibleHeroStats.FlexibilityMulti * heroAbility.Stats[0].FlexibilityValue;
                    int defenceSanity = hero.VisibleHeroStats.SanityMulti * heroAbility.Stats[0].SanityValue;

                    int defenceDurability = hero.VisibleHeroStats.DurabilityMulti * heroAbility.Stats[0].DurabilityValue;
                    int defenceAdaptability = hero.VisibleHeroStats.AdaptabilityMulti * heroAbility.Stats[0].AdaptabilityValue;
                    int defenceSuppression = hero.VisibleHeroStats.SuppressionMulti * heroAbility.Stats[0].SuppressionValue;

                    #region VISIBLE ABILITY STATS VALUE
                    hero.VisibleHeroStats.EnduranceValue += heroAbility.Stats[0].EnduranceValue;
                    hero.VisibleHeroStats.FlexibilityValue += heroAbility.Stats[0].FlexibilityValue;
                    hero.VisibleHeroStats.SanityValue += heroAbility.Stats[0].SanityValue;

                    hero.VisibleHeroStats.DurabilityValue += heroAbility.Stats[0].DurabilityValue;
                    hero.VisibleHeroStats.AdaptabilityValue += heroAbility.Stats[0].AdaptabilityValue;
                    hero.VisibleHeroStats.SuppressionValue += heroAbility.Stats[0].SuppressionValue;

                    hero.VisibleHeroStats.Health += heroAbility.Stats[0].Health;
                    #endregion

                    _abilityDefence += defenceEndurance + defenceFlexibility + defenceSanity + defenceDurability + defenceAdaptability
                + defenceSuppression + heroAbility.Stats[0].Health;
                }
            }
        }
    }

    public void AuraDefenceValue(Hero hero, Quest quest, List<Ability> heroAuras)
    {
        _auraDefence = 0;

        if (heroAuras.Count != 0)
        {
            int amountSuppresionAura = 0;

            foreach (Enemy enemy in quest.EnemiesList)
            {
                foreach (Ability enemyAbility in enemy.ListAbilities)
                {
                    if (enemyAbility.AbilityData.TypeAbility == TypeAbilities.Suppression_Aura)
                        amountSuppresionAura++;
                }
            }

            foreach (Ability heroAura in heroAuras)
            {
                int defenceEndurance = 0;
                int defenceFlexibility = 0;
                int defenceSanity = 0;

                int defenceDurability = 0;
                int defenceAdaptability = 0;
                int defenceSuppression = 0;

                if (heroAura.AbilityData.MasterAura)
                {
                    defenceEndurance = hero.VisibleHeroStats.EnduranceMulti * heroAura.Stats[0].EnduranceValue;
                    defenceFlexibility = hero.VisibleHeroStats.FlexibilityMulti * heroAura.Stats[0].FlexibilityValue;
                    defenceSanity = hero.VisibleHeroStats.SanityMulti * heroAura.Stats[0].SanityValue;

                    defenceDurability = hero.VisibleHeroStats.DurabilityMulti * heroAura.Stats[0].DurabilityValue;
                    defenceAdaptability = hero.VisibleHeroStats.AdaptabilityMulti * heroAura.Stats[0].AdaptabilityValue;
                    defenceSuppression = hero.VisibleHeroStats.SuppressionMulti * heroAura.Stats[0].SuppressionValue;

                    #region VISIBLE AURA STATS VALUE
                    hero.VisibleHeroStats.EnduranceValue += heroAura.Stats[0].EnduranceValue;
                    hero.VisibleHeroStats.FlexibilityValue += heroAura.Stats[0].FlexibilityValue;
                    hero.VisibleHeroStats.SanityValue += heroAura.Stats[0].SanityValue;

                    hero.VisibleHeroStats.DurabilityValue += heroAura.Stats[0].DurabilityValue;
                    hero.VisibleHeroStats.AdaptabilityValue += heroAura.Stats[0].AdaptabilityValue;
                    hero.VisibleHeroStats.SuppressionValue += heroAura.Stats[0].SuppressionValue;

                    hero.VisibleHeroStats.Health += heroAura.Stats[0].Health;
                    #endregion
                }

                else
                {
                    defenceEndurance = hero.VisibleHeroStats.EnduranceMulti * (heroAura.Stats[0].EnduranceValue / (amountSuppresionAura + 1));
                    defenceFlexibility = hero.VisibleHeroStats.FlexibilityMulti * (heroAura.Stats[0].FlexibilityValue / (amountSuppresionAura + 1));
                    defenceSanity = hero.VisibleHeroStats.SanityMulti * (heroAura.Stats[0].SanityValue / (amountSuppresionAura + 1));

                    defenceDurability = hero.VisibleHeroStats.DurabilityMulti * (heroAura.Stats[0].DurabilityValue / (amountSuppresionAura + 1));
                    defenceAdaptability = hero.VisibleHeroStats.AdaptabilityMulti * (heroAura.Stats[0].AdaptabilityValue / (amountSuppresionAura + 1));
                    defenceSuppression = hero.VisibleHeroStats.SuppressionMulti * (heroAura.Stats[0].SuppressionValue / (amountSuppresionAura + 1));

                    #region VISIBLE AURA STATS VALUE
                    hero.VisibleHeroStats.EnduranceValue += (heroAura.Stats[0].EnduranceValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.FlexibilityValue += (heroAura.Stats[0].FlexibilityValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.SanityValue += (heroAura.Stats[0].SanityValue / (amountSuppresionAura + 1));

                    hero.VisibleHeroStats.DurabilityValue += (heroAura.Stats[0].DurabilityValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.AdaptabilityValue += (heroAura.Stats[0].AdaptabilityValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.SuppressionValue += (heroAura.Stats[0].SuppressionValue / (amountSuppresionAura + 1));

                    hero.VisibleHeroStats.Health += heroAura.Stats[0].Health;
                    #endregion
                }

                _auraDefence += defenceEndurance + defenceFlexibility + defenceSanity + defenceDurability + defenceAdaptability
            + defenceSuppression + heroAura.Stats[0].Health;
            }
        }
    }

    public void DebuffDefence(Hero hero, Quest quest)
    {
        _debuffDefence = 0;

        foreach (Enemy enemy in quest.EnemiesList)
        {
            foreach (Ability enemyAbility in enemy.ListAbilities)
            {
                foreach (Debuff enemyDebuff in enemyAbility.DebuffsForAttack)
                {
                    if (!hero.BuffSystem.AllNeutralizeDebuffs.Contains(enemyDebuff.DebuffData.DebuffList))
                    {
                        if (enemyDebuff.DebuffData.Stats.Count != 0)
                        {
                            foreach (DebuffStats debuffStat in enemyDebuff.DebuffData.Stats[quest.Level - 1].Stats)
                            {
                                switch (debuffStat.StatType)
                                {
                                    case StatType.EnduranceValue:
                                        _debuffDefence += hero.VisibleHeroStats.EnduranceMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.EnduranceValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.FlexibilityValue:
                                        _debuffDefence += hero.VisibleHeroStats.FlexibilityMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.FlexibilityValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.SanityValue:
                                        _debuffDefence += hero.VisibleHeroStats.SanityMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.SanityValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.DurabilityValue:
                                        _debuffDefence += hero.VisibleHeroStats.DurabilityMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.DurabilityValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.AdaptabilityValue:
                                        _debuffDefence += hero.VisibleHeroStats.AdaptabilityMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.AdaptabilityValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.SuppressionValue:
                                        _debuffDefence += hero.VisibleHeroStats.SuppressionMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.SuppressionValue -= debuffStat.ValueStat;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
#endregion



    public float SumDefence(Hero hero, QuestParametresSystemFix questParametresSystem)
    {
        ResetVisibleStats(hero);

        DefenceMultiBase(hero);
        DefenceMultiEquip(hero);
        DefenceMultiLevelUp(hero);

        TalentDefenceMulti(hero, questParametresSystem);

        CalculateBaseDefenceValue(hero);
        EquipDefenceValue(hero);

        _visibleAllDefence = _baseDefence + _equipDefence;
        return _visibleAllDefence;
    }

    public float SumDefence(Hero hero, Quest quest, QuestParametresSystemFix questSystem, HeroQuestSlot_UI heroSlot)
    {
        _allCoefDefence = _baseCoefDefence + _extraCoefDefence;

        ResetVisibleStats(hero);

        DefenceMultiBase(hero);
        DefenceMultiEquip(hero);
        DefenceMultiAbility(hero);
        DefenceMultiAura(hero, quest, questSystem.HeroAuras);
        DefenceMultiLevelUp(hero);

        TalentDefenceMulti(hero, questSystem);

        CalculateBaseDefenceValue(hero);
        EquipDefenceValue(hero);
        AbilityDefenceValue(hero);
        AuraDefenceValue(hero, quest, questSystem.HeroAuras);
        DebuffDefence(hero, quest);

        _allDefence = _baseDefence + _equipDefence + _abilityDefence + _auraDefence - _debuffDefence;
        return _allDefence * _allCoefDefence;
    }

    public void ChangeCoefDefence(float coef)
    {
        _extraCoefDefence += coef;
    }

    public void ResetValues(Hero hero)
    {
        _baseCoefDefence = 1;
        _extraCoefDefence = 0;
        _allCoefDefence = 0;
    }

    private void ResetVisibleStats(Hero hero)
    {
        #region BASE STATS MULTI
        hero.VisibleHeroStats.EnduranceMulti = 0;
        hero.VisibleHeroStats.FlexibilityMulti = 0;
        hero.VisibleHeroStats.SanityMulti = 0;

        hero.VisibleHeroStats.DurabilityMulti = 0;
        hero.VisibleHeroStats.AdaptabilityMulti = 0;
        hero.VisibleHeroStats.SuppressionMulti = 0;
        #endregion

        #region BASE STATS VALUE
        hero.VisibleHeroStats.EnduranceValue = 0;
        hero.VisibleHeroStats.FlexibilityValue = 0;
        hero.VisibleHeroStats.SanityValue = 0;

        hero.VisibleHeroStats.DurabilityValue = 0;
        hero.VisibleHeroStats.AdaptabilityValue = 0;
        hero.VisibleHeroStats.SuppressionValue = 0;

        hero.VisibleHeroStats.Health = 0;
        #endregion
    }
}
