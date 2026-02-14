using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroAbilitySlot 
{
    [SerializeField] private Ability _ability;

    public Ability Ability => _ability;

    public HeroAbilitySlot (Ability ability)
    {
        _ability = ability;
    }

    public HeroAbilitySlot() { }

    private void Clear()
    {
        _ability = null;
    }


}
