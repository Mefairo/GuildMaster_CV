using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ManaDrain : ActivateEnemyAbility
{
    public ManaDrain(Ability ability) : base(ability)
    {
        //_manaCost = manaDrainAmount;
    }

    public override void Activate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is ManaDrainData abilityData)
        {
            //questParametresSystem.ChangeManaCoef(abilityData.ManaCoef);
        }
    }

    public override void DisActivate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is ManaDrainData abilityData)
        {
            //questParametresSystem.ChangeManaCoef(0);
        }
    }
}
