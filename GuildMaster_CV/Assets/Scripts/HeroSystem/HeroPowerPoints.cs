using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class HeroPowerPoints : PowerPoints
{
    [SerializeField] private float _basePower;
    [SerializeField] private float _equipPower;
    [SerializeField] private float _abilityPower;
    [SerializeField] private float _auraPower;
    [SerializeField] private float _debuffPower;
    [SerializeField] private float _allPower; // »—œŒÀ‹«”≈“—ﬂ ƒÀﬂ –¿—◊≈“¿ ¬ «¿ƒ¿Õ»ﬂ’ QuestParametresSystemFix
    [SerializeField] private float _visibleAllPower; // »—œŒÀ‹«”≈“—ﬂ ƒÀﬂ –¿—◊≈“¿ ¬ Ã≈Õﬁ √»À‹ƒ»» KeeperDisplay

    [SerializeField] private float _baseCoefPower = 1;
    [SerializeField] private float _extraCoefPower;
    [SerializeField] private float _allCoefPower;

    public static UnityAction<float> OnPowerChanged;

    public float BasePower => _basePower;
    public float AllCoefPower => _allCoefPower;
    public float VisibleAllPower => _visibleAllPower;
    public float AllPower
    {
        get => _allPower;
        private set
        {
            _allPower = value;
            OnPowerChanged?.Invoke(value);
        }
    }

    public HeroPowerPoints(float allPower, float allCoefPower) // ƒÀﬂ  Œœ»» √≈–Œﬂ ¬«ﬂ“Œ√Œ  ¬≈—“¿
    {
        _allPower = 0;
        _allCoefPower = 0;

        _allPower = allPower;
        _allCoefPower = allCoefPower;
    }

    public HeroPowerPoints() { }

    #region POWER MULTI CALCULATION
    public void PowerMultiBase(Hero hero)
    {
        hero.VisibleHeroStats.StrengthMulti += hero.HeroData.HeroStats.StrengthMulti;
        hero.VisibleHeroStats.DexterityMulti += hero.HeroData.HeroStats.DexterityMulti;
        hero.VisibleHeroStats.IntelligenceMulti += hero.HeroData.HeroStats.IntelligenceMulti;

        hero.VisibleHeroStats.CritMulti += hero.HeroData.HeroStats.CritMulti;
        hero.VisibleHeroStats.HasteMulti += hero.HeroData.HeroStats.HasteMulti;
        hero.VisibleHeroStats.AccuracyMulti += hero.HeroData.HeroStats.AccuracyMulti;
    }

    public void PowerMultiLevelUp(Hero hero)
    {
        hero.VisibleHeroStats.CritMulti += hero.LevelUpHeroStats.CritMulti;
        hero.VisibleHeroStats.HasteMulti += hero.LevelUpHeroStats.HasteMulti;
        hero.VisibleHeroStats.AccuracyMulti += hero.LevelUpHeroStats.AccuracyMulti;
    }

    public void PowerMultiEquip(Hero hero)
    {
        foreach (EquipSlot equipItemSlot in hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipItemSlot != null && equipItemSlot.BaseItemData != null)
            {
                hero.VisibleHeroStats.StrengthMulti += equipItemSlot.ItemStats[0].StrengthMulti;
                hero.VisibleHeroStats.DexterityMulti += equipItemSlot.ItemStats[0].DexterityMulti;
                hero.VisibleHeroStats.IntelligenceMulti += equipItemSlot.ItemStats[0].IntelligenceMulti;

                hero.VisibleHeroStats.CritMulti += equipItemSlot.ItemStats[0].CritMulti;
                hero.VisibleHeroStats.HasteMulti += equipItemSlot.ItemStats[0].HasteMulti;
                hero.VisibleHeroStats.AccuracyMulti += equipItemSlot.ItemStats[0].AccuracyMulti;
            }
        }
    }

    public void PowerMultiAbility(Hero hero)
    {
        foreach (Ability heroAbility in hero.ActiveBuffs)
        {
            if (heroAbility != null && heroAbility.AbilityData != null)
            {
                hero.VisibleHeroStats.StrengthMulti += heroAbility.Stats[0].StrengthMulti;
                hero.VisibleHeroStats.DexterityMulti += heroAbility.Stats[0].DexterityMulti;
                hero.VisibleHeroStats.IntelligenceMulti += heroAbility.Stats[0].IntelligenceMulti;

                hero.VisibleHeroStats.CritMulti += heroAbility.Stats[0].CritMulti;
                hero.VisibleHeroStats.HasteMulti += heroAbility.Stats[0].HasteMulti;
                hero.VisibleHeroStats.AccuracyMulti += heroAbility.Stats[0].AccuracyMulti;
            }
        }
    }

    public void PowerMultiAura(Hero hero, Quest quest, List<Ability> heroAuras)
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
                    hero.VisibleHeroStats.StrengthMulti += heroAura.Stats[0].StrengthMulti;
                    hero.VisibleHeroStats.DexterityMulti += heroAura.Stats[0].DexterityMulti;
                    hero.VisibleHeroStats.IntelligenceMulti += heroAura.Stats[0].IntelligenceMulti;

                    hero.VisibleHeroStats.CritMulti += heroAura.Stats[0].CritMulti;
                    hero.VisibleHeroStats.HasteMulti += heroAura.Stats[0].HasteMulti;
                    hero.VisibleHeroStats.AccuracyMulti += heroAura.Stats[0].AccuracyMulti;
                }

                else
                {
                    hero.VisibleHeroStats.StrengthMulti += (heroAura.Stats[0].StrengthMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.DexterityMulti += (heroAura.Stats[0].DexterityMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.IntelligenceMulti += (heroAura.Stats[0].IntelligenceMulti / (amountSuppresionAura + 1));

                    hero.VisibleHeroStats.CritMulti += (heroAura.Stats[0].CritMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.HasteMulti += (heroAura.Stats[0].HasteMulti / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.AccuracyMulti += (heroAura.Stats[0].AccuracyMulti / (amountSuppresionAura + 1));
                }
            }
        }
    }

    public void TalentPower(Hero hero, QuestParametresSystemFix questSystem)
    {
        if (questSystem.GuildTalentSystem.PowerUpStats.IsTakenTalent)
        {
            hero.VisibleHeroStats.CritMulti += questSystem.GuildTalentSystem.PowerUpStats.PowerStatValue;
            hero.VisibleHeroStats.HasteMulti += questSystem.GuildTalentSystem.PowerUpStats.PowerStatValue;
            hero.VisibleHeroStats.AccuracyMulti += questSystem.GuildTalentSystem.PowerUpStats.PowerStatValue;
        }
    }
    #endregion




    #region POWER VALUES CALCULATION
    public float CalculateBasePowerValue(Hero hero)
    {
        _basePower = 0;

        hero.VisibleHeroStats.StrengthValue += hero.HeroData.HeroStats.StrengthValue;
        hero.VisibleHeroStats.DexterityValue += hero.HeroData.HeroStats.DexterityValue;
        hero.VisibleHeroStats.IntelligenceValue += hero.HeroData.HeroStats.IntelligenceValue;

        hero.VisibleHeroStats.CritValue += hero.HeroData.HeroStats.CritValue;
        hero.VisibleHeroStats.HasteValue += hero.HeroData.HeroStats.HasteValue;
        hero.VisibleHeroStats.AccuracyValue += hero.HeroData.HeroStats.AccuracyValue;

        int powerStrength = hero.VisibleHeroStats.StrengthMulti * hero.VisibleHeroStats.StrengthValue;
        int powerDexterity = hero.VisibleHeroStats.DexterityMulti * hero.VisibleHeroStats.DexterityValue;
        int powerIntelligence = hero.VisibleHeroStats.IntelligenceMulti * hero.VisibleHeroStats.IntelligenceValue;

        int powerCrit = hero.VisibleHeroStats.CritMulti * hero.VisibleHeroStats.CritValue;
        int powerHaste = hero.VisibleHeroStats.HasteMulti * hero.VisibleHeroStats.HasteValue;
        int powerAccuracy = hero.VisibleHeroStats.AccuracyMulti * hero.VisibleHeroStats.AccuracyValue;

        _basePower += powerStrength + powerDexterity + powerIntelligence + powerCrit + powerHaste + powerAccuracy;

        return _basePower;
    }

    public void EquipPowerValue(Hero hero)
    {
        _equipPower = 0;

        foreach (EquipSlot equipItemSlot in hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipItemSlot != null && equipItemSlot.BaseItemData != null)
            {
                int powerStrength = hero.VisibleHeroStats.StrengthMulti * equipItemSlot.ItemStats[0].StrengthValue;
                int powerDexterity = hero.VisibleHeroStats.DexterityMulti * equipItemSlot.ItemStats[0].DexterityValue;
                int powerIntelligence = hero.VisibleHeroStats.IntelligenceMulti * equipItemSlot.ItemStats[0].IntelligenceValue;

                int powerCrit = hero.VisibleHeroStats.CritMulti * equipItemSlot.ItemStats[0].CritValue;
                int powerHaste = hero.VisibleHeroStats.HasteMulti * equipItemSlot.ItemStats[0].HasteValue;
                int powerAccuracy = hero.VisibleHeroStats.AccuracyMulti * equipItemSlot.ItemStats[0].AccuracyValue;

                #region EQUIP STATS VALUE
                hero.VisibleHeroStats.StrengthValue += equipItemSlot.ItemStats[0].StrengthValue;
                hero.VisibleHeroStats.DexterityValue += equipItemSlot.ItemStats[0].DexterityValue;
                hero.VisibleHeroStats.IntelligenceValue += equipItemSlot.ItemStats[0].IntelligenceValue;

                hero.VisibleHeroStats.CritValue += equipItemSlot.ItemStats[0].CritValue;
                hero.VisibleHeroStats.HasteValue += equipItemSlot.ItemStats[0].HasteValue;
                hero.VisibleHeroStats.AccuracyValue += equipItemSlot.ItemStats[0].AccuracyValue;
                #endregion

                _equipPower += powerStrength + powerDexterity + powerIntelligence + powerCrit + powerHaste + powerAccuracy;
            }
        }
    }

    public void AbilityPowerValue(Hero hero)
    {
        _abilityPower = 0;

        foreach (Ability heroAbility in hero.ActiveBuffs)
        {
            if (heroAbility.Stats.Count != 0)
            {
                int powerStrength = hero.VisibleHeroStats.StrengthMulti * heroAbility.Stats[0].StrengthValue;
                int powerDexterity = hero.VisibleHeroStats.DexterityMulti * heroAbility.Stats[0].DexterityValue;
                int powerIntelligence = hero.VisibleHeroStats.IntelligenceMulti * heroAbility.Stats[0].IntelligenceValue;

                int powerCrit = hero.VisibleHeroStats.CritMulti * heroAbility.Stats[0].CritValue;
                int powerHaste = hero.VisibleHeroStats.HasteMulti * heroAbility.Stats[0].HasteValue;
                int powerAccuracy = hero.VisibleHeroStats.AccuracyMulti * heroAbility.Stats[0].AccuracyValue;

                #region ABILITY STATS VALUE
                hero.VisibleHeroStats.StrengthValue += heroAbility.Stats[0].StrengthValue;
                hero.VisibleHeroStats.DexterityValue += heroAbility.Stats[0].DexterityValue;
                hero.VisibleHeroStats.IntelligenceValue += heroAbility.Stats[0].IntelligenceValue;

                hero.VisibleHeroStats.CritValue += heroAbility.Stats[0].CritValue;
                hero.VisibleHeroStats.HasteValue += heroAbility.Stats[0].HasteValue;
                hero.VisibleHeroStats.AccuracyValue += heroAbility.Stats[0].AccuracyValue;
                #endregion

                _abilityPower += powerStrength + powerDexterity + powerIntelligence + powerCrit + powerHaste + powerAccuracy;
            }
        }
    }

    public void AuraPowerValue(Hero hero, Quest quest, List<Ability> heroAuras)
    {
        _auraPower = 0;

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
                int powerStrength = 0;
                int powerDexterity = 0;
                int powerIntelligence = 0;

                int powerCrit = 0;
                int powerHaste = 0;
                int powerAccuracy = 0;

                if (heroAura.AbilityData.MasterAura)
                {
                    powerStrength = hero.VisibleHeroStats.StrengthMulti * heroAura.Stats[0].StrengthValue;
                    powerDexterity = hero.VisibleHeroStats.DexterityMulti * heroAura.Stats[0].DexterityValue;
                    powerIntelligence = hero.VisibleHeroStats.IntelligenceMulti * heroAura.Stats[0].IntelligenceValue;

                    powerCrit = hero.VisibleHeroStats.CritMulti * heroAura.Stats[0].CritValue;
                    powerHaste = hero.VisibleHeroStats.HasteMulti * heroAura.Stats[0].HasteValue;
                    powerAccuracy = hero.VisibleHeroStats.AccuracyMulti * heroAura.Stats[0].AccuracyValue;

                    #region AURA STATS VALUE
                    hero.VisibleHeroStats.StrengthValue += heroAura.Stats[0].StrengthValue;
                    hero.VisibleHeroStats.DexterityValue += heroAura.Stats[0].DexterityValue;
                    hero.VisibleHeroStats.IntelligenceValue += heroAura.Stats[0].IntelligenceValue;

                    hero.VisibleHeroStats.CritValue += heroAura.Stats[0].CritValue;
                    hero.VisibleHeroStats.HasteValue += heroAura.Stats[0].HasteValue;
                    hero.VisibleHeroStats.AccuracyValue += heroAura.Stats[0].AccuracyValue;
                    #endregion
                }

                else
                {
                    powerStrength = hero.VisibleHeroStats.StrengthMulti * (heroAura.Stats[0].StrengthValue / (amountSuppresionAura + 1));
                    powerDexterity = hero.VisibleHeroStats.DexterityMulti * (heroAura.Stats[0].DexterityValue / (amountSuppresionAura + 1));
                    powerIntelligence = hero.VisibleHeroStats.IntelligenceMulti * (heroAura.Stats[0].IntelligenceValue / (amountSuppresionAura + 1));

                    powerCrit = hero.VisibleHeroStats.CritMulti * (heroAura.Stats[0].CritValue / (amountSuppresionAura + 1));
                    powerHaste = hero.VisibleHeroStats.HasteMulti * (heroAura.Stats[0].HasteValue / (amountSuppresionAura + 1));
                    powerAccuracy = hero.VisibleHeroStats.AccuracyMulti * (heroAura.Stats[0].AccuracyValue / (amountSuppresionAura + 1));

                    #region AURA STATS VALUE
                    hero.VisibleHeroStats.StrengthValue += (heroAura.Stats[0].StrengthValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.DexterityValue += (heroAura.Stats[0].DexterityValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.IntelligenceValue += (heroAura.Stats[0].IntelligenceValue / (amountSuppresionAura + 1));

                    hero.VisibleHeroStats.CritValue += (heroAura.Stats[0].CritValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.HasteValue += (heroAura.Stats[0].HasteValue / (amountSuppresionAura + 1));
                    hero.VisibleHeroStats.AccuracyValue += (heroAura.Stats[0].AccuracyValue / (amountSuppresionAura + 1));
                    #endregion
                }

                _auraPower += powerStrength + powerDexterity + powerIntelligence + powerCrit + powerHaste + powerAccuracy;
            }
        }
    }

    public void DebuffPowerValue(Hero hero, Quest quest)
    {
        _debuffPower = 0;

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
                                    case StatType.StrengthValue:
                                        _debuffPower += hero.VisibleHeroStats.StrengthMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.StrengthValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.DexterityValue:
                                        _debuffPower += hero.VisibleHeroStats.DexterityMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.DexterityValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.IntelligenceValue:
                                        _debuffPower += hero.VisibleHeroStats.IntelligenceMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.IntelligenceValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.CritValue:
                                        _debuffPower += hero.VisibleHeroStats.CritMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.CritValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.HasteValue:
                                        _debuffPower += hero.VisibleHeroStats.HasteMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.HasteValue -= debuffStat.ValueStat;
                                        break;

                                    case StatType.AccuracyValue:
                                        _debuffPower += hero.VisibleHeroStats.AccuracyMulti * debuffStat.ValueStat;
                                        hero.VisibleHeroStats.AccuracyValue -= debuffStat.ValueStat;
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


    public float SumPower(Hero hero, QuestParametresSystemFix questSystem)
    {
        ResetVisibleStats(hero);

        PowerMultiBase(hero);
        PowerMultiEquip(hero);
        PowerMultiLevelUp(hero);

        TalentPower(hero, questSystem);

        CalculateBasePowerValue(hero);
        EquipPowerValue(hero);

        _visibleAllPower = _basePower + _equipPower;
        return _visibleAllPower;
    }

    public float SumPower(Hero hero, Quest quest, QuestParametresSystemFix questSystem, HeroQuestSlot_UI heroSlot)
    {
        _allCoefPower = _baseCoefPower + _extraCoefPower;

        ResetVisibleStats(hero);

        PowerMultiBase(hero);
        PowerMultiEquip(hero);
        PowerMultiAbility(hero);
        PowerMultiAura(hero, quest, questSystem.HeroAuras);
        PowerMultiLevelUp(hero);

        TalentPower(hero, questSystem);

        CalculateBasePowerValue(hero);
        EquipPowerValue(hero);
        AbilityPowerValue(hero);
        AuraPowerValue(hero, quest, questSystem.HeroAuras);
        DebuffPowerValue(hero, quest);

        _allPower = _basePower + _equipPower + _abilityPower + _auraPower - _debuffPower;
        return _allPower * _allCoefPower;
    }

    public void ChangeCoefPower(float coef)
    {
        _extraCoefPower += coef;
    }

    public void ResetValues(Hero hero)
    {
        _baseCoefPower = 1;
        _extraCoefPower = 0;
        _allCoefPower = 0;
    }

    private void ResetVisibleStats(Hero hero)
    {
        #region BASE STATS MULTI
        hero.VisibleHeroStats.StrengthMulti = 0;
        hero.VisibleHeroStats.DexterityMulti = 0;
        hero.VisibleHeroStats.IntelligenceMulti = 0;

        hero.VisibleHeroStats.CritMulti = 0;
        hero.VisibleHeroStats.HasteMulti = 0;
        hero.VisibleHeroStats.AccuracyMulti = 0;
        #endregion

        #region BASE STATS VALUE
        hero.VisibleHeroStats.StrengthValue = 0;
        hero.VisibleHeroStats.DexterityValue = 0;
        hero.VisibleHeroStats.IntelligenceValue = 0;

        hero.VisibleHeroStats.CritValue = 0;
        hero.VisibleHeroStats.HasteValue = 0;
        hero.VisibleHeroStats.AccuracyValue = 0;
        #endregion
    }
}
