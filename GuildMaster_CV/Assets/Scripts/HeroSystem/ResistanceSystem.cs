using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResistanceSystem
{
    [SerializeField] private List<Resistance> _baseRes;
    [SerializeField] private List<Resistance> _equipRes;
    [SerializeField] private List<Resistance> _abilityRes;
    [SerializeField] private List<Resistance> _auraRes;
    [SerializeField] private List<Resistance> _debuffRes;
    [SerializeField] private List<Resistance> _talentRes;
    [SerializeField] private List<Resistance> _uniqueBuffRes;
    [SerializeField] private List<Resistance> _allRes; // »—œŒÀ‹«”≈“—ﬂ ƒÀﬂ –¿—◊≈“¿ ¬ «¿ƒ¿Õ»ﬂ’ QuestParametresSystemFix
    [SerializeField] private List<Resistance> _visibleAllRes; // »—œŒÀ‹«”≈“—ﬂ ƒÀﬂ –¿—◊≈“¿ ¬ Ã≈Õﬁ √»À‹ƒ»» KeeperDisplay

    public List<Resistance> AllRes => _allRes;
    public List<Resistance> VisibleAllRes => _visibleAllRes;

    public ResistanceSystem(List<Resistance> allRes) // ƒÀﬂ  Œœ»» √≈–Œﬂ ¬«ﬂ“Œ√Œ  ¬≈—“¿
    {
        _allRes = new List<Resistance>();
        _allRes.Clear();

        foreach (Resistance res in allRes)
        {
            Resistance newRes = new Resistance(res);

            _allRes.Add(newRes);
        }
    }

    public ResistanceSystem() { }

    private void TalentRes(Hero hero, QuestParametresSystemFix questSystem)
    {
        _talentRes = new List<Resistance>();
        _talentRes = DeepCopyResistances(hero.HeroData.Resistance);

        if (questSystem.GuildTalentSystem.ResUpStats.IsTakenTalent)
        {
            foreach (Resistance res in _talentRes)
            {
                res.ValueResistance += hero.LevelSystem.Level * questSystem.GuildTalentSystem.ResUpStats.ResValue;
            }
        }
    }

    public List<Resistance> CalculateBaseRes(Hero hero)
    {
        _baseRes = new List<Resistance>();

        _baseRes = DeepCopyResistances(hero.HeroData.Resistance);

        return _baseRes;
    }

    public void EquipRes(Hero hero)
    {
        _equipRes = new List<Resistance>();

        foreach (EquipSlot equipItemSlot in hero.EquipHolder.EquipSystem.Slots)
        {
            if (equipItemSlot != null && equipItemSlot.BaseItemData != null)
            {
                if (equipItemSlot.BaseItemData is EquipItemData equipData && equipData.EquipType == EquipType.Trinket &&
                    equipData.ItemSecondaryType == ItemSecondaryType.Potion)
                {
                    foreach (Resistance equipRes in equipItemSlot.ItemDefRes[0].Resistances)
                    {
                        Resistance currentRes = _equipRes.Find(r => r.TypeDamage == equipRes.TypeDamage);

                        if (currentRes != null)
                            currentRes.ValueResistance += equipRes.ValueResistance * equipItemSlot.StackSize;

                        else
                            _equipRes.Add(new Resistance { TypeDamage = equipRes.TypeDamage, ValueResistance = equipRes.ValueResistance * equipItemSlot.StackSize });
                    }
                }

                else
                {
                    foreach (Resistance equipRes in equipItemSlot.ItemDefRes[0].Resistances)
                    {
                        Resistance currentRes = _equipRes.Find(r => r.TypeDamage == equipRes.TypeDamage);

                        if (currentRes != null)
                            currentRes.ValueResistance += equipRes.ValueResistance;

                        else
                            _equipRes.Add(new Resistance { TypeDamage = equipRes.TypeDamage, ValueResistance = equipRes.ValueResistance });
                    }
                }
            }
        }
    }

    public void AbilityRes(Hero hero)
    {
        _abilityRes = new List<Resistance>();

        foreach (Ability heroAbility in hero.ActiveBuffs)
        {
            foreach (Resistance abilRes in heroAbility.ResistancesForDefence)
            {
                Resistance currentRes = _abilityRes.Find(r => r.TypeDamage == abilRes.TypeDamage);

                if (currentRes != null)
                    currentRes.ValueResistance += abilRes.ValueResistance;

                else
                    _abilityRes.Add(new Resistance { TypeDamage = abilRes.TypeDamage, ValueResistance = abilRes.ValueResistance });
            }
        }
    }

    public void AuraRes(Hero hero, Quest quest, List<Ability> heroAuras)
    {
        _auraRes = new List<Resistance>();

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
                    foreach (Resistance abilRes in heroAura.ResistancesForDefence)
                    {
                        Resistance currentRes = _auraRes.Find(r => r.TypeDamage == abilRes.TypeDamage);

                        if (currentRes != null)
                            currentRes.ValueResistance += abilRes.ValueResistance;

                        else
                            _auraRes.Add(new Resistance { TypeDamage = abilRes.TypeDamage, ValueResistance = abilRes.ValueResistance });
                    }
                }

                else
                {
                    foreach (Resistance abilRes in heroAura.ResistancesForDefence)
                    {
                        Resistance currentRes = _auraRes.Find(r => r.TypeDamage == abilRes.TypeDamage);

                        if (currentRes != null)
                            currentRes.ValueResistance += (abilRes.ValueResistance / (amountSuppresionAura + 1));

                        else
                            _auraRes.Add(new Resistance { TypeDamage = abilRes.TypeDamage, ValueResistance = (abilRes.ValueResistance / (amountSuppresionAura + 1)) });
                    }
                }
            }
        }
    }

    public void DebuffRes(Hero hero, Quest quest)
    {
        _debuffRes = new List<Resistance>();

        foreach (Enemy enemy in quest.EnemiesList)
        {
            foreach (Ability enemyAbility in enemy.ListAbilities)
            {
                foreach (Debuff enemyDebuff in enemyAbility.DebuffsForAttack)
                {
                    if (!hero.BuffSystem.AllNeutralizeDebuffs.Contains(enemyDebuff.DebuffData.DebuffList))
                    {
                        if (enemyDebuff.DebuffData.Resistance.Count != 0)
                        {
                            foreach (Resistance debuffRes in enemyDebuff.DebuffData.Resistance[quest.Level - 1].Resistances)
                            {
                                Resistance currentRes = _debuffRes.Find(r => r.TypeDamage == debuffRes.TypeDamage);

                                if (currentRes != null)
                                    currentRes.ValueResistance += debuffRes.ValueResistance;

                                else
                                    _debuffRes.Add(new Resistance { TypeDamage = debuffRes.TypeDamage, ValueResistance = debuffRes.ValueResistance });
                            }
                        }
                    }
                }
            }
        }
    }

    public void UniqueBuffRes(Hero hero, Quest quest, HeroQuestSlot_UI heroSlot, SendHeroesSlots heroes, List<Resistance> allRes)
    {
        _uniqueBuffRes = new List<Resistance>();

        foreach (Ability heroAbility in hero.ActiveUniqueBuffs)
        {
            List<Resistance> localList = heroAbility.ApplyUniqueBuffRes(heroSlot, quest, heroes, allRes);

            foreach (Resistance resistance in localList)
            {
                _uniqueBuffRes.Add(resistance);
            }
        }
    }

    public List<Resistance> SumRes(Hero hero, QuestParametresSystemFix questSystem)
    {
        //ClearLists();
        _visibleAllRes = new List<Resistance>();

        CalculateBaseRes(hero);
        EquipRes(hero);
        TalentRes(hero, questSystem);

        SumRes(_visibleAllRes, _baseRes);
        SumRes(_visibleAllRes, _equipRes);
        SumRes(_visibleAllRes, _talentRes);

        return _visibleAllRes;
    }

    public List<Resistance> SumRes(Hero hero, Quest quest, QuestParametresSystemFix questSystem, HeroQuestSlot_UI heroSlot)
    {
        //ClearLists();
        _allRes = new List<Resistance>();

        CalculateBaseRes(hero);
        EquipRes(hero);
        AbilityRes(hero);
        AuraRes(hero, quest, questSystem.HeroAuras);
        DebuffRes(hero, quest);
        TalentRes(hero, questSystem);

        SumRes(_allRes, _baseRes);
        SumRes(_allRes, _equipRes);
        SumRes(_allRes, _abilityRes);
        SumRes(_allRes, _auraRes);
        SumRes(_allRes, _talentRes);
        SubtractRes(_allRes, _debuffRes);

        UniqueBuffRes(hero, quest, heroSlot, questSystem.MainQuestKeeperDisplay.HeroesSlots, _allRes);
        SumRes(_allRes, _uniqueBuffRes);

        return _allRes;
    }

    private void SumRes(List<Resistance> primaryList, List<Resistance> copyList)
    {
        if (copyList.Count != 0)
        {
            foreach (Resistance copyRes in copyList)
            {
                Resistance currentRes = primaryList.Find(r => r.TypeDamage == copyRes.TypeDamage);

                if (currentRes != null)
                    currentRes.ValueResistance += copyRes.ValueResistance;

                else
                    primaryList.Add(new Resistance { TypeDamage = copyRes.TypeDamage, ValueResistance = copyRes.ValueResistance });
            }
        }
    }

    private void SubtractRes(List<Resistance> primaryList, List<Resistance> copyList)
    {
        if (copyList.Count != 0)
        {
            foreach (Resistance copyRes in copyList)
            {
                Resistance currentRes = primaryList.Find(r => r.TypeDamage == copyRes.TypeDamage);

                if (currentRes != null)
                    currentRes.ValueResistance -= copyRes.ValueResistance;

                else
                    primaryList.Add(new Resistance { TypeDamage = copyRes.TypeDamage, ValueResistance = -copyRes.ValueResistance });
            }
        }
    }

    private List<Resistance> DeepCopyResistances(List<Resistance> original)
    {
        List<Resistance> copy = new List<Resistance>();
        foreach (Resistance resistance in original)
        {
            Resistance newResistance = new Resistance(resistance);
            copy.Add(newResistance);
        }
        return copy;
    }

    private void ClearLists()
    {
        _baseRes.Clear();
        _equipRes.Clear();
        _abilityRes.Clear();
        _auraRes.Clear();
        _debuffRes.Clear();
        _talentRes.Clear();
        _uniqueBuffRes.Clear();
        _allRes.Clear();
        _visibleAllRes.Clear();
    }
}
