using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect System/New Debuff/Power Debuff")]
public class DebuffSapping : DebuffData
{
    public float PowerCoef;

    public override void ApplyDebuffEffect(Hero hero, Quest quest, QuestParametresSystemFix questSystem)
    {
        hero.HeroPowerPoints.ChangeCoefPower(-PowerCoef);
    }
}
