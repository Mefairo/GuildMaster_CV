using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AbilityTree : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private GameObject _primaryContent;
    [SerializeField] private AbilityTreeSlot_UI _primaryAbility;
    [SerializeField] private AbilityTreeSlot_UI _abilitySlotPrefab;
    [SerializeField] private Button _pickButton;
    [SerializeField] private AbilityTreeLinesDrawer _linesDrawer;
    [SerializeField] private KeeperDisplay _guildKeeperDisplay;
    [Header("UI Text Ability")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _manaCost;
    [SerializeField] private TextMeshProUGUI _powerStatsNameText;
    [SerializeField] private TextMeshProUGUI _powerStatsValueText;
    [SerializeField] private TextMeshProUGUI _defenceStatsNameText;
    [SerializeField] private TextMeshProUGUI _defenceStatsValueText;
    [SerializeField] private TextMeshProUGUI _requiredStatsNameText;
    [SerializeField] private TextMeshProUGUI _requiredStatsValueText;
    [SerializeField] private TextMeshProUGUI _availablePointsText;

    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private TextHighlighter _textHighlighter = new TextHighlighter();
    private QuestResistanceSystem _questResistanceSystem;
    [SerializeField] private AbilityTreeSlot_UI _selectedAbilitySlot;
    private HeroSlot_UI _selectedHeroSlot;

    public AbilityTreeSlot_UI AbilitySlotPrefab => _abilitySlotPrefab;
    public GameObject PrimaryContent => _primaryContent;
    public HeroSlot_UI SelectedHeroSlot => _selectedHeroSlot;
    public AbilityTreeSlot_UI SelectedAbilitySlot => _selectedAbilitySlot;
    public AbilityTreeSlot_UI PrimaryAbility => _primaryAbility;
    public AbilityTreeLinesDrawer LinesDrawer => _linesDrawer;

    public static UnityAction OnAbilityTreeRequested;

    private void Awake()
    {
        _questResistanceSystem = GetComponent<QuestResistanceSystem>();
        _panel.SetActive(false);

        _pickButton?.onClick.AddListener(PickAbility);
    }

    private void Start()
    {
        ClearInfo();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            ClosePanel();
    }

    public void OpenWindowAbilityTree(AbilitySlot_UI abilitySlot, HeroSlot_UI heroSlot)
    {
        OnAbilityTreeRequested?.Invoke();

        ClearInfo();
        _panel.SetActive(true);

        _selectedHeroSlot = heroSlot;

        _primaryAbility.Init(abilitySlot.Ability.MainTreeAbility);
        _primaryAbility.PickNewAbility();
        CreateNewAbilities_1();

        // Обновляем линии
        _linesDrawer.DrawLinesForPrimarySlot();
        _linesDrawer.DrawLinesForChildrens();

        _primaryAbility.PickIcon.color = Color.yellow;
        _availablePointsText.text = $"Доступно Очков: {_selectedHeroSlot.Hero.LevelSystem.AbilityPoints}";
    }

    private void CreateNewAbilities_1()
    {
        foreach (Ability newTreeAbility in _primaryAbility.Ability.AbilityData.StudyAbilities)
        {
            Ability newAbility = new Ability(newTreeAbility);
            AbilityTreeSlot_UI newAbilityTreeSlot = Instantiate(_abilitySlotPrefab, _primaryContent.transform);
            newAbilityTreeSlot.Init(newAbility);

            _primaryAbility.ChildAbilitiesSlots.Add(newAbilityTreeSlot);
        }
    }

    public void SelectAbilitySlot(AbilityTreeSlot_UI abilitySlot)
    {
        if (_selectedAbilitySlot != null)
        {
            _selectedAbilitySlot.SelectIcon.gameObject.SetActive(false);
        }

        _selectedAbilitySlot = abilitySlot;

        UpdateUIAbility(abilitySlot.Ability);
    }

    private void UpdateUIAbility(Ability ability)
    {
        _nameText.text = ability.AbilityData.Name;
        _descriptionText.text = _textHighlighter.ChangeColorText(ability.AbilityData.AbilityTreeDescription);
        //_descriptionText.text = ability.AbilityData.AbilityTreeDescription;
        _manaCost.text = $"Manacost: {ability.ManaCost}";
        _availablePointsText.text = $"Доступно Очков: {_selectedHeroSlot.Hero.LevelSystem.AbilityPoints}";

        if (ability.AbilityData.Stats.Count != 0)
        {
            (string, string, string, string) statsText = _statDisplayManager.SetAllStatsText(ability.AbilityData.Stats[0], false);

            _powerStatsNameText.text = statsText.Item1;
            _powerStatsValueText.text = statsText.Item2;

            _defenceStatsNameText.text = statsText.Item3;
            _defenceStatsValueText.text = statsText.Item4;
        }

        UpdateResistance(ability);
        UpdateDebuffs(ability);
        UpdateRequiredStats(ability);
    }

    private void UpdateRequiredStats(Ability ability)
    {
        if (ability.AbilityData.RequiredStats != null)
        {
            (string, string) statsText = _statDisplayManager.SetRequiredStatsValueText(ability.AbilityData.RequiredStats);

            _requiredStatsNameText.text = statsText.Item1;
            _requiredStatsValueText.text = statsText.Item2;
        }
    }

    private void UpdateResistance(Ability ability)
    {
        List<Resistance> resistances = new List<Resistance>();

        if (ability.ResistancesForAttack.Count != 0)
            resistances = ability.ResistancesForAttack;

        else if (ability.ResistancesForDefence.Count != 0)
            resistances = ability.ResistancesForDefence;

        _questResistanceSystem.ShowResistance(resistances);
    }

    private void UpdateDebuffs(Ability ability)
    {
        _questResistanceSystem.ShowDebuffs(ability);
    }

    private void PickAbility()
    {
        bool otherSlotIsPicked = CheckPickedAbilitties();
        bool parentSlotPicked = CheckedParentAbilities();

        if (!otherSlotIsPicked)
        {
            Debug.Log("no pick 1");
            return;
        }

        if (!parentSlotPicked)
        {
            Debug.Log("no pick 2");
            return;
        }

        if (_selectedHeroSlot.Hero.LevelSystem.AbilityPoints == 0)
            return;

        bool isCanPickAbility = _statDisplayManager.CheckStats(_selectedHeroSlot.Hero.VisibleHeroStats, _selectedAbilitySlot.Ability.AbilityData.RequiredStats);

        if (isCanPickAbility)
        {
            Debug.Log("pick");
            Ability matchingAbility = _selectedHeroSlot.Hero.ListAbilities.FirstOrDefault(a => a.MainTreeAbility.AbilityData
            == _selectedAbilitySlot.Ability.MainTreeAbility.AbilityData);

            int index = _selectedHeroSlot.Hero.ListAbilities.IndexOf(matchingAbility);

            for (int i = 0; i < _selectedHeroSlot.Hero.AbilityHolder.HeroAbilitySystem.AbilityList.Count; i++)
            {
                _selectedHeroSlot.Hero.AbilityHolder.HeroAbilitySystem.RemoveAbility(i);
            }

            _selectedHeroSlot.Hero.ListAbilities[index] = _selectedAbilitySlot.Ability;

            _selectedAbilitySlot.PickIcon.color = Color.yellow;
            _selectedAbilitySlot.DrawLineImage.color = Color.yellow;

            _selectedHeroSlot.Hero.LevelSystem.SpendAbilityPoints();
            UpdateAbilityPointsInfo();

            _selectedAbilitySlot.PickNewAbility();

            _guildKeeperDisplay.ShowHeroAbilities(_selectedHeroSlot);
        }

        else
            Debug.Log("no pick");
    }

    private void ClearInfo()
    {
        _nameText.text = "";
        _descriptionText.text = "";

        _manaCost.text = "";

        _powerStatsNameText.text = "";
        _powerStatsValueText.text = "";

        _defenceStatsNameText.text = "";
        _defenceStatsValueText.text = "";

        _requiredStatsNameText.text = "";
        _requiredStatsValueText.text = "";

        _availablePointsText.text = "Доступно Очков: ";

        _questResistanceSystem.ClearIcons();

        ClearSlots();
    }

    private void UpdateAbilityPointsInfo()
    {
        _availablePointsText.text = $"Доступно Очков: {_selectedHeroSlot.Hero.LevelSystem.AbilityPoints}";
    }

    private void ClearSlots()
    {
        foreach (Transform child in _primaryContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private bool CheckPickedAbilitties()
    {
        bool canPick = true;

        foreach (AbilityTreeSlot_UI slot in _selectedAbilitySlot.ParentAbilitySlot.ChildAbilitiesSlots)
        {
            if (slot.PickAbility)
            {
                Debug.Log("false");
                canPick = false;
                break;
            }

            else
            {
                Debug.Log("true");
                canPick = true;
            }
        }

        return canPick;
    }

    private bool CheckedParentAbilities()
    {
        bool canPick = true;

        if (_selectedAbilitySlot.ParentAbilitySlot != null)
        {
            if (_selectedAbilitySlot.ParentAbilitySlot.PickAbility)
            {
                canPick = true;
            }

            else
            {
                canPick = false;
            }
        }

        return canPick;
    }

    public void ClosePanel()
    {
        _linesDrawer.ClearLines();
        ClearSlots();
    }
}
