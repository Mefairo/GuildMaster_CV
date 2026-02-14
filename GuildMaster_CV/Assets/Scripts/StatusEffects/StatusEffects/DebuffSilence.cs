using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect System/New Debuff/Silence Debuff")]
public class DebuffSilence : DebuffData
{

    public override void ApplyDebuffEffect(Hero hero, Quest quest, QuestParametresSystemFix questSystem)
    {
        hero.BuffSystem.ChangeSilenceDebuff(hero, questSystem);
    }
}
