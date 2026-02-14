using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect System/New Debuff/Heal Debuff")]
public class DebuffBleed : DebuffData
{
    public float HealCoef;

    public override void ApplyDebuffEffect(Hero hero, Quest quest, QuestParametresSystemFix questSystem)
    {
        questSystem.ChangeHealCoef(HealCoef);
    }
}
