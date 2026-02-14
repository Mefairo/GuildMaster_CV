using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HeroAbilityHolder
{
    [SerializeField] private HeroAbilitySystem _heroAbilitySystem;

    public HeroAbilitySystem HeroAbilitySystem => _heroAbilitySystem;

    public static UnityAction<HeroAbilitySystem> OnHeroAbilityRequested;

    public HeroAbilityHolder()
    {
        _heroAbilitySystem = new HeroAbilitySystem(3);
    }

    public HeroAbilityHolder(HeroAbilitySystem abilitySystem)
    {
        _heroAbilitySystem = new HeroAbilitySystem(abilitySystem);
    }

    public void ShowAbilities()
    {
        OnHeroAbilityRequested?.Invoke(_heroAbilitySystem);
    }
}
