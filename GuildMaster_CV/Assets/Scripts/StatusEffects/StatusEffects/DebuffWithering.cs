using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect System/New Debuff/Mana Debuff")]
public class DebuffWithering : DebuffData
{
    public float ManaCoef;

    public override void ApplyDebuffEffect(Hero hero, Quest quest, QuestParametresSystemFix questSystem)
    {
        questSystem.ChangeManaCoef(ManaCoef);
    }
}
