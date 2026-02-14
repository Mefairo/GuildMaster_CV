using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoulWithering : ActivateEnemyAbility
{
    public SoulWithering(Ability ability) : base(ability)
    {
        //_manaCost = manaDrainAmount;
    }

    public override void Activate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is SoulWitheringData abilityData)
        {
            //questParametresSystem.ChangeHealCoef(abilityData.HPCoef);
            //questParametresSystem.ChangeManaCoef(abilityData.ManaCoef);
        }
    }

    public override void DisActivate(Enemy enemy, List<Hero> heroes)
    {
        if (_ability.AbilityData is SoulWitheringData abilityData)
        {
            //questParametresSystem.ChangeHealCoef(-abilityData.HPCoef);
            //questParametresSystem.ChangeManaCoef(0);
        }
    }
}
