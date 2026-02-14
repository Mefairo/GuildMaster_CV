using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Recruit System/Recruit List")]
public class RecruitList : ScriptableObject
{
    [SerializeField] private List<RecruitHero> _recruitHeroes;

    public List<RecruitHero> RecruitHeroes => _recruitHeroes;
}

[System.Serializable]
public struct RecruitHero
{
    public HeroData HeroData;
    public int HeroLevel;

    //public Hero Hero;
}

