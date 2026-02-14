using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaladinAbility_2_2_1 : Ability
{
    public PaladinAbility_2_2_1(Ability ability) : base(ability)
    {
    }

    public override float GetDynamicPower(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes)
    {
        if (_abilityData is PaladinAbilityData_2_2_1 data)
        {
            int enemyCheck = 0;

            foreach (Enemy enemy in quest.EnemiesList)
            {
                if (enemy.EnemyData.EnemyType == EnemyType.Undead || enemy.EnemyData.EnemyType == EnemyType.Demon)
                    enemyCheck++;
            }

            float extraPower = Mathf.FloorToInt((heroSlot.Hero.HeroPowerPoints.AllPower * enemyCheck * data.CoefPower) / 100);
            return extraPower;

        }

        else
            return 0;
    }
}
