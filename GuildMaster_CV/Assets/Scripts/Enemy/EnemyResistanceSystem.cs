using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyResistanceSystem
{
    [SerializeField] private List<Resistance> _baseRes;
    [SerializeField] private List<Resistance> _abilityRes;
    [SerializeField] private List<Resistance> _debuffRes;
    [SerializeField] private List<Resistance> _allRes;

    public List<Resistance> AllRes => _allRes;

    public List<Resistance> CalculateBaseRes(Enemy enemy)
    {
        _baseRes = new List<Resistance>();

        _baseRes = DeepCopyResistances(enemy.EnemyData.Resistances);

        return _baseRes;
    }

    public void AbilityRes(Enemy enemy)
    {
        _abilityRes = new List<Resistance>();

        foreach (Ability enemyAbility in enemy.ActiveBuffs)
        {
            foreach (Resistance abilRes in enemyAbility.ResistancesForDefence)
            {
                Resistance currentRes = _abilityRes.Find(r => r.TypeDamage == abilRes.TypeDamage);

                if (currentRes != null)
                    currentRes.ValueResistance += abilRes.ValueResistance;

                else
                    _abilityRes.Add(new Resistance { TypeDamage = abilRes.TypeDamage, ValueResistance = abilRes.ValueResistance });
            }
        }
    }

    public void DebuffRes(Enemy enemy, SendHeroesSlots heroSlots, Quest quest)
    {
        _debuffRes = new List<Resistance>();

        foreach (HeroQuestSlot_UI heroSlot in heroSlots.Slots)
        {
            if (heroSlot.Hero != null && heroSlot.Hero.HeroData != null)
            {
                foreach (Ability heroAbility in heroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList)
                {
                    if (heroAbility != null && heroAbility.AbilityData != null)
                    {
                        if (heroAbility.AbilityData.GeneralType == GeneralTypeAbility.Attack)
                        {
                            foreach (Debuff heroDebuff in heroAbility.DebuffsForAttack)
                            {
                                if (!enemy.DebuffImmun.Contains(heroDebuff))
                                {
                                    if (heroDebuff.DebuffData.ResistanceHeroes.Count != 0)
                                    {
                                        foreach (Resistance debuffRes in heroDebuff.DebuffData.ResistanceHeroes[heroSlot.Hero.LevelSystem.Level - 1].Resistances)
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
            }
        }
    }

    public List<Resistance> SumRes(Enemy enemy, Quest quest, QuestParametresSystemFix questSystem)
    {
        _allRes = new List<Resistance>();

        CalculateBaseRes(enemy);
        AbilityRes(enemy);
        DebuffRes(enemy, questSystem.MainQuestKeeperDisplay.HeroesSlots, quest);

        SumRes(_allRes, _baseRes);
        SumRes(_allRes, _abilityRes);
        SubtractRes(_allRes, _debuffRes);

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
}
