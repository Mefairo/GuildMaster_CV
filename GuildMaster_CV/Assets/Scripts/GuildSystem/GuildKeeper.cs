using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuildKeeper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected RecruitList _recruitList;
    [SerializeField] protected NextDayController _nextDayController;
    [SerializeField] protected HeroesRecruitSystem _recruitSystem;
    [Header("UI")]
    //[SerializeField] protected Button _activeButton;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _namePanel;

    public static UnityAction<HeroesRecruitSystem> OnGuildHeroesWindowRequested;

    public HeroesRecruitSystem RecruitSystem => _recruitSystem;

    private void Awake()
    {
        _recruitSystem = new HeroesRecruitSystem(_recruitList.RecruitHeroes.Count);

        //foreach (RecruitHero hero in _recruitList.RecruitHeroes)
        //{
        //    _recruitSystem.AddNewRecruit(hero.HeroData, hero.HeroLevel);
        //}

        //_activeButton.onClick.AddListener(OpenWindow);
    }

    private void Start()
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    public void OpenWindow()
    {
        if (!_nextDayController.InTransition)
            OnGuildHeroesWindowRequested?.Invoke(_recruitSystem);
    }

    private void OnEnable()
    {
        KeeperDisplay.OnHireNewHero += HireHero;

        _nextDayController.OnNextDay += RestHeroes;
    }
    private void OnDisable()
    {
        KeeperDisplay.OnHireNewHero -= HireHero;

        _nextDayController.OnNextDay += RestHeroes;
    }

    private void HireHero(Hero hero)
    {
        //_recruitSystem.HeroSlots.Add(slot);
        _recruitSystem.HeroSlots.Add(hero);
        hero.WeekTracking = _nextDayController.WeekAmount;
    }

    private void RestHeroes()
    {
        foreach (Hero hero in _recruitSystem.HeroSlots)
        {
            if (!hero.IsRested)
            {
                hero.ChangeRested(true);
            }
        }
    }

    public void ExtraStatPointForLevelByTalent(int statValue)
    {
        //foreach (Hero hero in _recruitSystem.HeroSlots)
        //{
        //    hero.LevelSystem.ChangeStatPoints(statValue * (hero.LevelSystem.Level - 1));
        //    hero.LevelSystem.ChangeExtraStatPoints(statValue);
        //}

        //NotificationSystem.Instance.CheckNotifications();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(true);
        _namePanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenWindow();
    }
}
