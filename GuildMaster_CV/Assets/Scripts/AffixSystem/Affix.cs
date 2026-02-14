using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Affix 
{
    [SerializeField] private AffixData _affixData;
    [SerializeField] private bool _requireAoeDamage;

    public AffixData AffixData => _affixData;

    public bool RequireAoeDamage => _requireAoeDamage;

    public Affix(AffixData affixData)
    {
        _affixData = affixData;
        if (_affixData == null)
            Debug.Log("null 11");
        _requireAoeDamage = _affixData.RequireAoeDamage;

        //_listAbilities = DeepCopyAbilities(affixData.Abilities);
    }

    //private List<DebuffList> DeepCopyDebuffList(List<DebuffList> original)
    //{
    //    List<DebuffList> copy = new List<DebuffList>();
    //    foreach (DebuffList ability in original)
    //    {
    //        DebuffList newAbility = new DebuffList(ability);
    //        copy.Add(newAbility);
    //    }
    //    return copy;
    //}
}
