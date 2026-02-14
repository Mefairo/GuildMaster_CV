using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect System/New Debuff/Light Debuff")]
public class DebuffFear : DebuffData
{
    public float LightCoef;

    public override void ApplyDebuffEffect(Hero hero, Quest quest, QuestParametresSystemFix questSystem)
    {
        questSystem.ChangeLightCoef(LightCoef);
    }
}
