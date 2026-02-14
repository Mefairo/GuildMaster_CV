using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.Events;

public class QuestKeeperDisplay : MonoBehaviour
{
    [SerializeField] private QuestSlot_UI _questPrefab;
    [SerializeField] private QuestEnemySlot_UI _questEnemyPrefab;
    [SerializeField] private AbilitySlot_UI _abilitySlotPrefab;
    [SerializeField] private GameObject _questList;
    [SerializeField] private GameObject _enemyList;
    [SerializeField] private GameObject _abilityList;
    [Header("Preview")]
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _power;
    [SerializeField] private TextMeshProUGUI _defence;
    [SerializeField] private TextMeshProUGUI _enemies;
    [SerializeField] private TextMeshProUGUI _nameAbility;
    [SerializeField] private TextMeshProUGUI _descriptionAbility;
    [Header("Datas")]
    [SerializeField] private GameObject _infoData;
    [SerializeField] private GameObject _enemyInfoData;
    [Header("Button Tabs")]
    [SerializeField] private Button _infoButton;
    [SerializeField] private Button _enemyInfoButton;
    [Header("Other")]
    [SerializeField] private QuestResistanceSystem _questResistanceSystem;
    [SerializeField] private Transform _imageEnemyContainer;

    private QuestBoardSystem _boardSystem;
    private QuestSlot_UI _selectedQuest;
    private QuestEnemySlot_UI _selectedEnemy;
    private AbilitySlot_UI _selectedAbility;

    public event UnityAction OnClickOtherSlot;

    private void Awake()
    {
        _infoButton.onClick.AddListener(ShowInfo);
        _enemyInfoButton.onClick.AddListener(ShowEnemyInfo);
    }


    public void DisplayQuestBoard(QuestBoardSystem boardSystem)
    {
        _boardSystem = boardSystem;

        RefreshDisplay();
        RefreshInfo();
        DisplayQuestBoard();
    }

    private void RefreshDisplay()
    {
        ClearSlots();
    }

    private void ClearSlots()
    {
        ClearQuestSlots();
        ClearEnemySlots();
        ClearAbilitiesSlots();
    }

    private void CreateQuestSlot(QuestSlot quest)
    {
        //QuestSlot_UI questSlot = Instantiate(_questPrefab, _questList.transform);
        //questSlot.Init(quest);
    }

    private void CreateEnemySlot(Enemy enemy)
    {
        QuestEnemySlot_UI enemySlot = Instantiate(_questEnemyPrefab, _enemyList.transform);
        enemySlot.Init(enemy);
    }

    private void CreateAbilitySlot(Ability ability)
    {
        AbilitySlot_UI abilitySlot = Instantiate(_abilitySlotPrefab, _abilityList.transform);
        abilitySlot.Init(ability);
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
    }

    public void SelectQuest(QuestSlot_UI questSlot)
    {
        _selectedQuest = questSlot;
    }

    public void SelectEnemy(QuestEnemySlot_UI enemy)
    {
        _selectedEnemy = enemy;

        ShowEnemyInfo();
    }

    public void SelectAbility(AbilitySlot_UI ability)
    {
        _selectedAbility = ability;
    }

    public void UpdateQuestText(QuestSlot_UI questSlot)
    {
        RefreshInfo();
        ClearEnemySlots();
        ClearAbilitiesSlots();

        _infoData.gameObject.SetActive(true);

        _level.text = $"Level: {questSlot.Quest.Quest.Level}";
        _power.text = $"Power: {questSlot.Quest.Quest.Power}";
        _defence.text = $"Defence: {questSlot.Quest.Quest.Defence}";

        _enemies.text = "";

        for (int i = 0; i < questSlot.Quest.Quest.EnemiesList.Count; i++)
        {
            _enemies.text += $"{questSlot.Quest.Quest.EnemiesList[i].EnemyData.Name}\n";
        }

        foreach (var enemy in _selectedQuest.Quest.Quest.EnemiesList)
        {
            CreateEnemySlot(enemy);
        }
    }

    public void UpdateEnemyInfo(QuestEnemySlot_UI enemy)
    {
        ClearAbilitiesSlots();

        foreach (var ability in enemy.Enemy.ListAbilities)
            CreateAbilitySlot(ability);
    }

    public void UpdateAbilityInfo(AbilitySlot_UI ability)
    {
        _nameAbility.text = ability.Ability.AbilityData.Name;
        _descriptionAbility.text = ability.Ability.AbilityData.Description;
    }

    private void RefreshInfo()
    {
        _infoData.gameObject.SetActive(false);
        _enemyInfoData.gameObject.SetActive(false);
    }

    public void UpdateQuestPreview(QuestSlot_UI quest)
    {
        if (!_infoData.activeSelf && !_enemyInfoData.activeSelf)
            ShowInfo();

        if (_infoData.activeSelf && !_enemyInfoData.activeSelf)
            ShowInfo();

        else if (_enemyInfoData.activeSelf)
        {
            ShowEnemyInfo();

            ClearEnemySlots();
            ClearAbilitiesSlots();

            OnClickOtherSlot?.Invoke();
            //_questResistanceSystem.ClearResistanceIcons(_imageEnemyContainer);

            foreach (var enemy in _selectedQuest.Quest.Quest.EnemiesList)
            {
                CreateEnemySlot(enemy);
            }
        }
    }

    private void ShowInfo()
    {
        RefreshInfo();

        UpdateQuestText(_selectedQuest);
    }

    private void ShowEnemyInfo()
    {
        RefreshInfo();

        _enemyInfoData.gameObject.SetActive(true);
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

    private void ClearAbilitiesSlots()
    {
        foreach (Transform ability in _abilityList.transform.Cast<Transform>())
        {
            Destroy(ability.gameObject);
        }

        _nameAbility.text = "";
        _descriptionAbility.text = "";
    }

}
