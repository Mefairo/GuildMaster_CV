using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect System/New Debuff/StatRes Debuff")]
public class DebuffData : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    [TextArea(4, 4)]
    public string Description;
    public bool IsUnique;
    public bool IsGlobal;
    public List<ListDebuffStats> Stats;
    public List<ResistanceList> Resistance;
    public List<ResistanceList> ResistanceHeroes;
    public DebuffList DebuffList;

    public virtual void ApplyDebuffEffect(Hero hero, Quest quest, QuestParametresSystemFix questSystem) { }
}

[System.Serializable]
public class ListDebuffStats
{
    public List<DebuffStats> Stats;
}


[System.Serializable]
public class DebuffStats
{
    public StatType StatType;
    public int ValueStat;
}
