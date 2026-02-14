using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using TMPro;

public class QuestKeeperController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private QuestEnemySlot_UI _questEnemyPrefab;
    [SerializeField] private AffixSlot_UI _affixSlotPrefab;
    [SerializeField] private AbilitySlot_UI _abilitySlotPrefab;
    [Header("Data Info")]
    [SerializeField] private GameObject _enemyInfo;
    [SerializeField] private TextMeshProUGUI _nameEnemy;
    [SerializeField] private GameObject _enemySlots;
    [SerializeField] private GameObject _affixesSlots;
    [Header("Content Lists")]
    [SerializeField] private GameObject _enemyList;
    [SerializeField] private GameObject _affixesList;
    [SerializeField] private GameObject _abilityEnemyList;
    [Header("Preview")]
    [SerializeField] private TextMeshProUGUI _nameAbility;
    [SerializeField] private TextMeshProUGUI _primaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _secondaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _descriptionAbility;

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }

    public event UnityAction OnClickOtherSlot;

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponent<MainQuestKeeperDisplay>();
    }

    private void OnEnable()
    {
        MainQuestKeeperDisplay.OnQuestClick += ShowQuestInfo;

        ClearEnemySlots();
        ClearAffixesSlots();
    }

    private void OnDisable()
    {
        MainQuestKeeperDisplay.OnQuestClick -= ShowQuestInfo;

        ClearEnemySlots();
        ClearAffixesSlots();
    }

    private void ShowQuestInfo()
    {
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;

        _enemyInfo.gameObject.SetActive(true);

        ClearEnemySlots();
        ClearAffixesSlots();
        ClearEnemyAbilitiesSlots();

        _enemyInfo.SetActive(true);
        _enemySlots.SetActive(true);
        _affixesSlots.SetActive(true);

        OnClickOtherSlot?.Invoke();

        foreach (Enemy enemy in questSlot.Quest.Quest.EnemiesList)
        {
            CreateEnemySlot(enemy);
        }

        foreach (Affix affix in questSlot.Quest.Quest.AffixesList)
        {
            CreateAffixesSlot(affix);
        }
    }

    public void ClearAllSlots()
    {
        ClearEnemySlots();
        ClearAffixesSlots();
        ClearEnemyAbilitiesSlots();
    }

    private void ClearEnemySlots()
    {
        foreach (Transform enemy in _enemyList.transform.Cast<Transform>())
        {
            Destroy(enemy.gameObject);
        }
    }

    private void ClearAffixesSlots()
    {
        foreach (Transform enemy in _affixesList.transform.Cast<Transform>())
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
        if (_primaryTypeAbility != null) _primaryTypeAbility.text = "";
        if (_secondaryTypeAbility != null) _secondaryTypeAbility.text = "";
        _descriptionAbility.text = "";

        _nameEnemy.text = "";
    }

    private void CreateEnemySlot(Enemy enemy)
    {
        QuestEnemySlot_UI enemySlot = Instantiate(_questEnemyPrefab, _enemyList.transform);
        enemySlot.Init(enemy);
    }

    private void CreateAffixesSlot(Affix affix)
    {
        AffixSlot_UI affixSlot = Instantiate(_affixSlotPrefab, _affixesList.transform);
        affixSlot.Init(affix);
    }

    public void UpdateEnemyInfo(QuestEnemySlot_UI enemySlot)
    {
        ClearEnemyAbilitiesSlots();

        _nameEnemy.text = enemySlot.Enemy.EnemyData.Name;

        foreach (var ability in enemySlot.Enemy.ListAbilities)
            CreateEnemyAbilitySlot(ability);
    }

    private void CreateEnemyAbilitySlot(Ability ability)
    {
        AbilitySlot_UI abilitySlot = Instantiate(_abilitySlotPrefab, _abilityEnemyList.transform);
        abilitySlot.Init(ability);
    }

    public void UpdateUIEntityInfo(AbilitySlot_UI abilitySlot)
    {
        //ClearAbilitiesSlots();

        _nameAbility.text = abilitySlot.Ability.AbilityData.Name;
        _descriptionAbility.text = abilitySlot.Ability.AbilityData.Description;

        switch (abilitySlot.Ability.AbilityData.GeneralType)
        {
            case GeneralTypeAbility.Attack:
                _primaryTypeAbility.text = $"Primary Type Ability:  Attack";
                break;

            case GeneralTypeAbility.Stats:
                _primaryTypeAbility.text = $"Primary Type Ability:  Amplify";
                break;
        }

        switch (abilitySlot.Ability.AbilityData.TypeAbility)
        {
            case TypeAbilities.DamageSingleTarget:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Single Target";
                break;

            case TypeAbilities.AoEDamage:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  AoE";
                break;

            case TypeAbilities.Buff:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Buff";
                break;

            case TypeAbilities.Aura:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Aura";
                break;

            case TypeAbilities.Summon:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Summon";
                break;

            case TypeAbilities.Debuff:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Debuff";
                break;

            case TypeAbilities.Metamorphosis:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Metamorphosis";
                break;

            case TypeAbilities.Suppression_Aura:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Suppression Aura";
                break;
        }
    }

    public void UpdateUIEntityInfo(AffixSlot_UI affixSlot)
    {
        //ClearAbilitiesSlots();

        _nameAbility.text = affixSlot.Affix.AffixData.Name;
        _descriptionAbility.text = affixSlot.Affix.AffixData.Description;

        _primaryTypeAbility.text = "";
        _secondaryTypeAbility.text = "";
    }
}
