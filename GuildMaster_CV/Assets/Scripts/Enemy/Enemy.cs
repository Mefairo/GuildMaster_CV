using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[System.Serializable]
public class Enemy
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private List<Resistance> _resistances;
    [SerializeField] private List<Debuff> _debuffImmun;
    [SerializeField] private List<Ability> _listAbilities;
    [SerializeField] private List<BuffList> _buffList = new List<BuffList>();
    [SerializeField] private bool _requireAoeDamage;
    [SerializeField] private List<Ability> _activeBuffs;
    [Header("Systems")]
    [SerializeField] private EnemyResistanceSystem _enemyResistanceSystem;

    public EnemyData EnemyData => _enemyData;
    public List<Resistance> Resistances => _resistances;
    public List<Debuff> DebuffImmun => _debuffImmun;
    public List<Ability> ListAbilities => _listAbilities;
    public List<BuffList> BuffList => _buffList;
    public bool RequireAoeDamage => _requireAoeDamage;
    public List<Ability> ActiveBuffs => _activeBuffs;
    public EnemyResistanceSystem EnemyResistanceSystem => _enemyResistanceSystem;

    public Enemy(EnemyData enemyData)
    {
        _enemyData = enemyData;
        _requireAoeDamage = _enemyData.RequireAoeDamage;

        _resistances = DeepCopyResistances(enemyData.Resistances);
        _debuffImmun = DeepCopyDebuffs(enemyData.DebuffImmun);
        _listAbilities = DeepCopyAbilities(enemyData.Abilities);

        _activeBuffs = new List<Ability>();

        _enemyResistanceSystem = new EnemyResistanceSystem();
    }

    private List<Ability> DeepCopyAbilities(List<Ability> original)
    {
        List<Ability> copy = new List<Ability>();
        foreach (Ability ability in original)
        {
            Ability newAbility = new Ability(ability);
            copy.Add(newAbility);
        }
        return copy;
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

    private List<Debuff> DeepCopyDebuffs(List<Debuff> original)
    {
        List<Debuff> copy = new List<Debuff>();
        foreach (Debuff debuff in original)
        {
            Debuff newDebuff = new Debuff(debuff);
            copy.Add(newDebuff);
        }
        return copy;
    }
}

