using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAntiHealAbility : ActivateEnemyAbility
{
    public EnemyAntiHealAbility(Ability ability) : base(ability)
    {
    }

    public override void Activate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is EnemyAntiHealAbilityData abilityData)
        {
            //questParametresSystem.ChangeHealCoef(abilityData.HPCoef);
        }
    }

    public override void DisActivate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is EnemyAntiHealAbilityData abilityData)
        {
            //questParametresSystem.ChangeHealCoef(-abilityData.HPCoef);
        }
    }
}
