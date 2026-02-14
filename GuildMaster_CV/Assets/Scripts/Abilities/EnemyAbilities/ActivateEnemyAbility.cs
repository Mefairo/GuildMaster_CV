using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ActivateEnemyAbility 
{
    protected Ability _ability;

    public float ManaCost => _ability.ManaCost;

    public ActivateEnemyAbility(Ability ability)
    {
        _ability = ability;
    }

    public abstract void Activate(Enemy enemy, List<Hero> heroes);

    public abstract void DisActivate(Enemy enemy, List<Hero> heroes);
}
