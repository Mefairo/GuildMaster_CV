using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecruitKeeper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected int _amountHeroes;
    [SerializeField] protected float _coefHire;
    [SerializeField] protected RecruitList _recruitList;
    [SerializeField] protected HeroesRecruitSystem _recruitSystem;
    [SerializeField] protected GuildValutes _guildValutes;
    [SerializeField] protected NextDayController _nextDayController;
    [SerializeField] protected QuestParametresSystemFix _questParametresSystemFix;
    [SerializeField] private List<HeroData> _heroDataList;
    [SerializeField] private List<HeroesAmountPlaces> _amountHeroesForLevelGuild = new List<HeroesAmountPlaces>();
    [SerializeField] private List<int> _amountHeroesCurrentGuildLevel = new List<int>();
    [Header("Talent Properties")]
    [SerializeField] protected int _resValueTalent;
    [SerializeField] protected int _powerStatTalent;
    [SerializeField] protected int _defenceStatTalent;
    [SerializeField] protected int _extraStatPoint;
    [SerializeField] protected int _extraHealth;
    [Space]
    [Header("UI")]
    //[SerializeField] protected Button _activeButton;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _namePanel;

    public HeroesRecruitSystem RecruitSystem => _recruitSystem;

    public static UnityAction<HeroesRecruitSystem> OnHeroesWindowRequested;

    private void Start()
    {
        SetNewHeroes();

        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _nextDayController.OnNextDay += SetNewHeroes;
    }

    private void OnDisable()
    {
        _nextDayController.OnNextDay -= SetNewHeroes;
    }

    private void SetNewHeroes()
    {
        //ChangeCoefHire(0);  // 0 - коэффициент для найма по умолчанию
        SetAmountHeroes();

        foreach (Hero hero in _recruitSystem.HeroSlots)
            hero.UnsubEvents();

        _recruitSystem = new HeroesRecruitSystem(_amountHeroes);

        SetNewHeroesSettings();

        //for (int i = 0; i < _amountHeroes; i++)
        //{
        //    int randomIndex = UnityEngine.Random.Range(0, _heroDataList.Count);

        //    _recruitSystem.AddNewRecruit(_heroDataList[randomIndex], _guildValutes.Level, _questParametresSystemFix);
        //}

        //ExtraResForNewHeroesByTalent();
        //ExtraPowerStatsByTalent(_powerStatTalent);
        //ExtraDefenceStatsByTalent(_powerStatTalent);
        //AddExtraStatForNewHeroes(_extraStatPoint);
        //AddExtraHPHeroesByTalent(_extraHealth);
    }

    private void SetNewHeroesSettings()
    {
        int amountHeroesCurrentGuildLevel = 0;
        int amountHeroesOtherLevel = 0;

        switch (_guildValutes.Level)
        {
            case 1:
                amountHeroesCurrentGuildLevel = _amountHeroes;
                break;

            case 2:
                amountHeroesCurrentGuildLevel = 2;
                break;

            case 3:
                amountHeroesCurrentGuildLevel = 1;
                break;

            case 4:
                amountHeroesCurrentGuildLevel = 0;
                break;
        }

        amountHeroesOtherLevel = _amountHeroes - amountHeroesCurrentGuildLevel;

        for (int i = 0; i < amountHeroesCurrentGuildLevel; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _heroDataList.Count);

            _recruitSystem.AddNewRecruit(_heroDataList[randomIndex], _guildValutes.Level, _questParametresSystemFix);
        }

        for (int i = 0; i < amountHeroesOtherLevel; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _heroDataList.Count);
            int randomLevel = UnityEngine.Random.Range(1, _guildValutes.Level + 1);

            _recruitSystem.AddNewRecruit(_heroDataList[randomIndex], randomLevel, _questParametresSystemFix);
        }
    }

    public void SetAmountHeroes()
    {
        int localAmount = Random.Range(_amountHeroesForLevelGuild[_guildValutes.Level - 1].MinAmount,
            _amountHeroesForLevelGuild[_guildValutes.Level - 1].MaxAmount + 1);

        _amountHeroes = (int)(localAmount * _coefHire);
    }

    public void ChangeCoefHire(float coefHire)
    {
        _coefHire += coefHire;

        SetNewHeroes();
    }

    protected virtual void OpenWindow()
    {
        if (!_nextDayController.InTransition)
            OnHeroesWindowRequested?.Invoke(_recruitSystem);
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


[System.Serializable]
public class HeroesAmountPlaces
{
    public int MinAmount;
    public int MaxAmount;
}
