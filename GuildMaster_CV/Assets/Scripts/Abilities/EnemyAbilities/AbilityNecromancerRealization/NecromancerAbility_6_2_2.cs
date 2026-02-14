using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerAbility_6_2_2 : Ability
{
    public NecromancerAbility_6_2_2(Ability ability) : base(ability)
    {
    }

    public override List<Resistance> ApplyUniqueBuffRes(HeroQuestSlot_UI heroSlot, Quest quest, SendHeroesSlots heroes, List<Resistance> allRes)
    {
        List<Resistance> localRes = new List<Resistance>();

        if (_abilityData is NecromancerAbilityData_6_2_2 data)
        {
            Resistance parentRes = allRes.Find(r => r.TypeDamage == TypeDamage.Dark);
            Resistance desireRes_1 = allRes.Find(r => r.TypeDamage == TypeDamage.Light);
            Resistance desireRes_2 = allRes.Find(r => r.TypeDamage == TypeDamage.Cold);

            if (parentRes != null && desireRes_1 != null && desireRes_2 != null)
            {
                Resistance newRes_1 = new Resistance(desireRes_1.TypeDamage, (int)(parentRes.ValueResistance / data.Coef));
                Resistance newRes_2 = new Resistance(desireRes_2.TypeDamage, (int)(parentRes.ValueResistance / data.Coef));

                localRes.Add(newRes_1);
                localRes.Add(newRes_2);
            }
        }

        return localRes;
    }
}
