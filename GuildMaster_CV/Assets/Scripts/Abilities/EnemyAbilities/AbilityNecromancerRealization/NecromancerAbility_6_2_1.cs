using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAbility_6_2_1 : Ability
{
    public NecromancerAbility_6_2_1(Ability ability) : base(ability)
    {
    }

    public override List<Resistance> ApplyUniqueBuffRes(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes, List<Resistance> allRes)
    {
        List<Resistance> localRes = new List<Resistance>();

        if (_abilityData is NecromancerAbilityData_6_2_1 data)
        {
            Resistance parentRes = allRes.Find(r => r.TypeDamage == TypeDamage.Dark);
            Resistance desireRes = allRes.Find(r => r.TypeDamage == TypeDamage.Light);

            if (parentRes != null && desireRes != null)
            {
                Resistance newRes = new Resistance(desireRes.TypeDamage, (int)(parentRes.ValueResistance / data.Coef));
                localRes.Add(newRes);
            }

        }

        return localRes;
    }
}
