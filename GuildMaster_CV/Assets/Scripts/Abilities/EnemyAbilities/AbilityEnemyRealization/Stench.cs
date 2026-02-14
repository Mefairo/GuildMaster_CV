using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stench : ActivateEnemyAbility
{
    public Stench(Ability ability) : base(ability)
    {
        //_manaCost = manaDrainAmount;
    }

    public override void Activate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is StenchData abilityData)
        {
            //questParametresSystem.ChangeHealCoef(abilityData.HPCoef);
        }
    }

    public override void DisActivate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is StenchData abilityData)
        {
            //questParametresSystem.ChangeHealCoef(-abilityData.HPCoef);
        }
    }
}
