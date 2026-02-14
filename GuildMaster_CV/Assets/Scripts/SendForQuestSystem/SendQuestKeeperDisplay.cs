using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.Events;

public class SendQuestKeeperDisplay : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private QuestSlot_UI _questPrefab;
    [SerializeField] private QuestEnemySlot_UI _questEnemyPrefab;
    [SerializeField] private AbilitySlot_UI _abilitySlotPrefab;
    [SerializeField] private HeroForSendSlot_UI _heroQuestPrefab;
    [SerializeField] private HeroSlot_UI _heroPrefab;
    [Header("Content Lists")]
    [SerializeField] private GameObject _questList;
    [SerializeField] private GameObject _enemyList;
    [SerializeField] private GameObject _abilityEnemyList;
    [SerializeField] private GameObject _abilityHeroList;
    [SerializeField] private GameObject _heroesList;
    [Header("Data Info")]
    [SerializeField] private GameObject _enemyInfo;
    [SerializeField] private GameObject _heroInfo;
    [SerializeField] private GameObject _enemySlots;
    [SerializeField] private GameObject _sendHeroInfo;
    [SerializeField] private SendHeroesSlots _heroesSlots;
    [Header("Preview")]
    [SerializeField] private TextMeshProUGUI _nameAbility;
    [SerializeField] private TextMeshProUGUI _descriptionAbility;
    [Header("Other")]
    [SerializeField] private GuildKeeper _guildKeeper;
    [Header("Lists")]
    [SerializeField] private List<HeroForSendSlot_UI> _heroes = new List<HeroForSendSlot_UI>();
    [SerializeField] private List<Hero> _copyHeroesList;

    private QuestBoardSystem _boardSystem;
    private HeroesRecruitSystem _recruitSystem;

    private QuestSlot_UI _selectedQuest;
    private QuestEnemySlot_UI _selectedEnemy;
    private AbilitySlot_UI _selectedAbility;
    private HeroQuestSlot_UI _selectedHero;
    private HeroForSendSlot_UI _selectedSendHero;

    public SendHeroesSlots HeroesSlots => _heroesSlots;

    public event UnityAction OnClickOtherSlot;
    public event UnityAction<HeroQuestSlot_UI> OnClickHeroSlot;

    public void DisplayQuestBoard(QuestBoardSystem boardSystem)
    {
        _boardSystem = boardSystem;

        RefreshDisplay();
        RefreshInfo();
        DisplayQuestBoard();
    }

    private void DisplayQuestBoard()
    {
        RefreshDisplay();

        foreach (QuestSlot quest in _boardSystem.QuestSlots)
        {
            if (quest.Quest.QuestData == null)
                continue;

            CreateQuestSlot(quest);
        }

        _enemySlots.gameObject.SetActive(true);
    }

    private void RefreshDisplay()
    {
        ClearSlots();
    }

    private void ClearSlots()
    {
        ClearQuestSlots();
        ClearEnemySlots();
        ClearEnemyAbilitiesSlots();
        ClearHeroAbilitiesSlots();
        ClearHeroSlots();
    }

    private void ClearQuestSlots()
    {
        foreach (Transform quest in _questList.transform.Cast<Transform>())
        {
            Destroy(quest.gameObject);
        }
    }

    private void ClearEnemySlots()
    {
        foreach (Transform enemy in _enemyList.transform.Cast<Transform>())
        {
            Destroy(enemy.gameObject);
        }
    }

    private void ClearEnemyAbilitiesSlots()
    {
        foreach (Transform ability in _abilityEnemyList.transform.Cast<Transform>())
        {
            Destroy(ability.gameObject);
        }

        _nameAbility.text = "";
        _descriptionAbility.text = "";
    }

    private void ClearHeroAbilitiesSlots()
    {
        foreach (Transform ability in _abilityHeroList.transform.Cast<Transform>())
        {
            Destroy(ability.gameObject);
        }
    }

    private void ClearHeroSlots()
    {
        foreach (Transform hero in _heroesList.transform.Cast<Transform>())
        {
            Destroy(hero.gameObject);
        }
    }

    private void CreateQuestSlot(QuestSlot quest)
    {
        QuestSlot_UI questSlot = Instantiate(_questPrefab, _questList.transform);
        //questSlot.Init(quest);
    }

    private void CreateEnemySlot(Enemy enemy)
    {
        QuestEnemySlot_UI enemySlot = Instantiate(_questEnemyPrefab, _enemyList.transform);
        enemySlot.Init(enemy);
    }

    private void CreateEnemyAbilitySlot(Ability ability)
    {
        AbilitySlot_UI abilitySlot = Instantiate(_abilitySlotPrefab, _abilityEnemyList.transform);
        abilitySlot.Init(ability);
    }

    private void CreateHeroAbilitySlot(Ability ability)
    {
        AbilitySlot_UI abilitySlot = Instantiate(_abilitySlotPrefab, _abilityHeroList.transform);
        abilitySlot.Init(ability);
    }

    private void CreateHeroSlot(Hero hero)
    {
        HeroForSendSlot_UI heroSlot = Instantiate(_heroQuestPrefab, _heroesList.transform);
        heroSlot.Init(hero);

        //_heroes.Add(heroSlot);
    }

    //private void CreateHeroSlot(HeroSlot hero)
    //{
    //    HeroSlot_UI heroSlot = Instantiate(_heroPrefab, _heroesList.transform);
    //    heroSlot.Init(hero);

    //    _heroes.Add(heroSlot);
    //}

    public void SelectQuest(QuestSlot_UI questSlot)
    {
        _selectedQuest = questSlot;
    }

    public void SelectEnemy(QuestEnemySlot_UI enemy)
    {
        _selectedEnemy = enemy;
    }

    public void SelectAbility(AbilitySlot_UI ability)
    {
        _selectedAbility = ability;
    }

    public void SelectGuildHero(HeroForSendSlot_UI sendHero)
    {
        _selectedSendHero = sendHero;
    }

    public void SelectHero(HeroQuestSlot_UI sendHero)
    {
        _selectedHero = sendHero;

        if (sendHero.Hero == null || sendHero.Hero.HeroData == null || sendHero == null)
        {
            OnClickHeroSlot?.Invoke(sendHero);
            ShowHeroesList();
        }
    }

    private void RefreshInfo()
    {
        _enemyInfo.SetActive(false);
        _heroInfo.SetActive(false);
        _enemySlots.SetActive(false);
        _sendHeroInfo.SetActive(false);
    }

    public void UpdateQuestInfo(QuestSlot_UI questSlot)
    {
        RefreshInfo();
        ClearEnemySlots();
        ClearEnemyAbilitiesSlots();

        _enemyInfo.SetActive(true);
        _enemySlots.SetActive(true);

        OnClickOtherSlot?.Invoke();

        foreach (var enemy in _selectedQuest.Quest.Quest.EnemiesList)
        {
            CreateEnemySlot(enemy);
        }
    }

    public void UpdateEnemyInfo(QuestEnemySlot_UI enemySlot)
    {
        ClearEnemyAbilitiesSlots();

        foreach (var ability in enemySlot.Enemy.ListAbilities)
            CreateEnemyAbilitySlot(ability);
    }

    public void UpdateEnemyAbilityInfo(AbilitySlot_UI abilitySlot)
    {
        //ClearAbilitiesSlots();

        _nameAbility.text = abilitySlot.Ability.AbilityData.Name;
        _descriptionAbility.text = abilitySlot.Ability.AbilityData.Description;
    }

    private void ShowHeroesList()
    {
        RefreshInfo();

        _sendHeroInfo.SetActive(true);
        DisplayHeroes();
    }

    private void DisplayHeroes()
    {
        ClearHeroSlots();

        _copyHeroesList = new List<Hero>();

        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            _copyHeroesList.Add(hero);
        }


        foreach (Hero hero in _copyHeroesList)
        {
            if (hero.HeroData == null)
                continue;

            CreateHeroSlot(hero);
        }
    }

    public void TakeHero(HeroForSendSlot_UI hero)
    {
        HeroQuestSlot_UI selectedSlot = _selectedHero;
        if (selectedSlot != null)
        {
            selectedSlot.Init(hero.Hero);
        }

        Hero heroToRemove = _copyHeroesList.FirstOrDefault(h => h.HeroData == hero.Hero.HeroData);
        if (heroToRemove != null)
        {
            _copyHeroesList.Remove(heroToRemove);
            Debug.Log("Hero removed from the list");
        }
        else
        {
            Debug.LogWarning("Hero not found in the list");
        }

        //DisplayHeroes();


        ClearHeroSlots();

        foreach (Hero copyHero in _copyHeroesList)
        {
            if (copyHero.HeroData == null)
                continue;

            CreateHeroSlot(copyHero);
        }
    }

    //[SerializeField] private QuestSlot_UI _questPrefab;
    //[SerializeField] private GameObject _enemySlots_GO;
    //[SerializeField] private GameObject _questList_GO;

    //private QuestBoardSystem _boardSystem;



    //public void DisplayQuestBoard(QuestBoardSystem boardSystem)
    //{
    //    _boardSystem = boardSystem;

    //    //ClearSlots();
    //    //RefreshInfo();
    //    DisplayQuestBoard();
    //}

    //private void DisplayQuestBoard()
    //{
    //    ClearQuestSlots();

    //    foreach (QuestSlot quest in _boardSystem.QuestSlots)
    //    {
    //        if (quest.Quest.QuestData == null)
    //            continue;

    //        CreateQuestSlot(quest);
    //    }

    //    _enemySlots_GO.gameObject.SetActive(true);
    //}

    //private void ClearQuestSlots()
    //{
    //    foreach (Transform quest in _questList_GO.transform.Cast<Transform>())
    //    {
    //        Destroy(quest.gameObject);
    //    }
    //}

    //private void CreateQuestSlot(QuestSlot quest)
    //{
    //    QuestSlot_UI questSlot = Instantiate(_questPrefab, _questList_GO.transform);
    //    questSlot.Init(quest);
    //}
}
