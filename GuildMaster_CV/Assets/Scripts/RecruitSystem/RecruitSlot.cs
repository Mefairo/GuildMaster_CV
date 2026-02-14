using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecruitSlot
{
    [SerializeField] private HeroData _heroData;
    [SerializeField] private int _heroLevel;

    public HeroData HeroData => _heroData;
    public int HeroLevel => _heroLevel;

    public void ClearInfo()
    {
        _heroData = null;
        _heroLevel = 0;
    }

    public void AssignHero(HeroData heroData, int level)
    {
        _heroData = heroData;
        _heroLevel = level;
    }

    public bool IsEmpty()
    {
        return _heroData == null;
    }
}
