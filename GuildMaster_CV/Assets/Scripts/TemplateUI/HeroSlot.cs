using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroSlot
{
    [SerializeField] private Hero _hero;

    public Hero Hero => _hero;

    public void ClearInfo()
    {
        _hero = null;
    }

    public void AssignHero(HeroData hero, int level)
    {
        //_hero = new Hero(hero, level);
    }

    public bool IsEmpty()
    {
        return _hero == null;
    }
}
