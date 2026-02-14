using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Affix System/Affix Data")]
public class AffixData : ScriptableObject
{
    public int ID = -1;
    public string Name;
    [TextArea(4, 4)]
    public string Description;
    public Sprite Icon;
    public List<Resistance> Resistances;
    public AffixList ActiveDebuffAffix;
    //public List<SlotData> NeutralizeItems;
    public bool RequireAoeDamage = false;
    public bool ConsiderCountAffix = true;
    public bool RequiredForAllHeroesNeutralize = true;
}
