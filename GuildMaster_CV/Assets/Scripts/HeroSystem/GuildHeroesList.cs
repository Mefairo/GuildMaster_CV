using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildHeroesList : MonoBehaviour
{
    [SerializeField] private List<Hero> _heroesList = new List<Hero>();

    private void OnEnable()
    {
        //RecruitKeeperDisplay.OnNewHero += AddNewHero;
    }

    private void OnDisable()
    {
        //RecruitKeeperDisplay.OnNewHero -= AddNewHero;
    }

    private void AddNewHero(Hero hero)
    {
        _heroesList.Add(hero);
    }
}
