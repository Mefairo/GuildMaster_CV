using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Reflection;
using System.Text;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class KeeperDisplay : MonoBehaviour
{
    [Header("Others GameObjects")]
    [SerializeField] protected QuestParametresSystemFix _questParametresSystemFix;
    [Header("Prefabs")]
    [SerializeField] protected HeroSlot_UI _slotHeroPrefab;
    [SerializeField] private AbilitySlot_UI _abilitySlotPrefab;
    [Header("Content Lists")]
    [SerializeField] protected GameObject _listHeroes;
    [SerializeField] private GameObject _abilityList;
    [SerializeField] protected GuildValutes _guildValutes;
    [Header("Inventory")]
    [SerializeField] protected DynamicEquipDisplay _equipDisplay;
    [SerializeField] protected GuildKeeper _guildKeeper;
    [Header("Info")]
    [SerializeField] protected TextMeshProUGUI _itemPreviewDescription;
    [SerializeField] protected TextMeshProUGUI _statPoints;
    [SerializeField] protected TextMeshProUGUI _abilityPoints;
    [SerializeField] protected TextMeshProUGUI _powerStatsName;
    [SerializeField] protected TextMeshProUGUI _powerStatsMulti;
    [SerializeField] protected TextMeshProUGUI _powerStatsValue;
    [SerializeField] protected TextMeshProUGUI _defenceStatsName;
    [SerializeField] protected TextMeshProUGUI _defenceStatsMulti;
    [SerializeField] protected TextMeshProUGUI _defenceStatsValue;
    [SerializeField] protected TextMeshProUGUI _powerPoints;
    [SerializeField] protected TextMeshProUGUI _defencePoints;
    [SerializeField] protected TextMeshProUGUI _dailyPayHeroText;
    [Header("Resistance")]
    [SerializeField] private ResImage_UI[] _resImages;
    [Header("Abilities")]
    [SerializeField] private TextMeshProUGUI _nameAbility;
    [SerializeField] private TextMeshProUGUI _primaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _secondaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _descriptionAbility;
    [SerializeField] protected DynamicHeroAbilityDisplay _heroAbilityDisplay;
    [SerializeField] protected HeroAbilitiesInfo _heroAbilityInfo;
    [SerializeField] protected Button _abilityTreeButton;
    [SerializeField] protected AbilityTree _abilityTree;
    [Header("Datas")]
    [SerializeField] protected GameObject _inventoryData;
    [SerializeField] protected GameObject _infoData;
    [SerializeField] protected GameObject _resistanceData;
    [SerializeField] protected GameObject _abilitiesData;
    [Space]
    [SerializeField] protected TextMeshProUGUI _heroPlaces;
    [SerializeField] protected List<HeroSlot_UI> _heroesList = new List<HeroSlot_UI>();

    protected HeroesRecruitSystem _recruitSystem;
    protected HeroSlot_UI _selectedHero;
    protected AbilitySlot_UI _selectedAbility;
    protected StatDisplayManager _statDisplayManager = new StatDisplayManager();
    protected TextHighlighter _textHighlighter = new TextHighlighter();

    public QuestParametresSystemFix QuestParametresSystemFix => _questParametresSystemFix;
    public HeroSlot_UI SelectedHero => _selectedHero;
    public AbilitySlot_UI SelectedAbility => _selectedAbility;
    public GuildKeeper GuildKeeper => _guildKeeper;
    public List<HeroSlot_UI> HeroesList => _heroesList;

    public static UnityAction<Hero> OnHireNewHero;

    protected virtual void Awake()
    {
        _abilityTreeButton?.onClick.AddListener(OpenAbilityTree);
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (this.gameObject.activeSelf)
                this.gameObject.SetActive(false);
        }
    }

    protected virtual void OnEnable()
    {
        HeroEquipmentHolder.OnHeroEquipmentRequested += ShowHeroInventory;
        HeroAbilityHolder.OnHeroAbilityRequested += ShowHeroAbilities;
        StatsUpSystem.OnOverStatPoints += DisplayHeroes;
    }

    protected virtual void OnDisable()
    {
        HeroEquipmentHolder.OnHeroEquipmentRequested -= ShowHeroInventory;
        HeroAbilityHolder.OnHeroAbilityRequested -= ShowHeroAbilities;
        StatsUpSystem.OnOverStatPoints -= DisplayHeroes;

        _selectedHero = null;
    }

    public void DisplayHeroWindow(HeroesRecruitSystem recruitSystem)
    {
        _recruitSystem = recruitSystem;

        RefreshDisplay();
        RefreshInfo();
        DisplayHeroes();
    }

    protected virtual void RefreshDisplay()
    {
        ClearSlots();

        _heroPlaces.text = $"Places: {_guildValutes.CurrentHeroPlaces}/{_guildValutes.MaxHeroPlaces}";
    }

    protected void ClearSlots()
    {
        ClearHeroesSlots();
        ClearAbilitiesSlots();

        _heroesList.Clear();
    }

    protected void CreateHeroSlot(Hero hero)
    {
        HeroSlot_UI heroSlot = Instantiate(_slotHeroPrefab, _listHeroes.transform);
        heroSlot.Init(hero);

        _heroesList.Add(heroSlot);
    }

    private void CreateAbilitySlot(Ability ability)
    {
        AbilitySlot_UI abilitySlot = Instantiate(_abilitySlotPrefab, _abilityList.transform);
        abilitySlot.Init(ability);
    }

    protected void DisplayHeroes()
    {
        RefreshDisplay();

        foreach (Hero hero in _recruitSystem.HeroSlots)
        {
            if (hero.HeroData == null)
                continue;

            CreateHeroSlot(hero);
        }
    }

    protected void RefreshInfo()
    {
        _inventoryData.gameObject.SetActive(false);
        _infoData.gameObject.SetActive(false);
        _resistanceData.gameObject.SetActive(false);
        _abilitiesData.gameObject.SetActive(false);
    }

    public virtual void UpdateHeroPreview(HeroSlot_UI hero)
    {
        _heroAbilityInfo.ClearAllIcons();

        _inventoryData.gameObject.SetActive(true);
        _infoData.gameObject.SetActive(true);
        _resistanceData.gameObject.SetActive(true);
        _abilitiesData.gameObject.SetActive(false);

        ShowHeroInventory(hero);
        UpdateUIInfo(hero.Hero);
        ShowHeroResistance(hero);

        //if (!_inventoryData.activeSelf && !_infoData.activeSelf && !_resistanceData.activeSelf && !_abilitiesData.activeSelf)
        //{
        //    _inventoryData.gameObject.SetActive(true);
        //    ShowHeroInventory(hero);
        //    ShowHeroInfo(hero);
        //    ShowHeroResistance(hero);
        //}

        //else if (_inventoryData.activeSelf)
        //    ShowHeroInventory(hero);

        //else if (_infoData.activeSelf)
        //    ShowHeroInfo(hero);

        //else if (_resistanceData.activeSelf)
        //    ShowHeroResistance(hero);

        //if (_abilitiesData.activeSelf)
        //    ShowHeroAbilities(hero);
    }

    public void UpdateAbilityInfo(AbilitySlot_UI abilitySlot_UI)
    {
        _nameAbility.text = abilitySlot_UI.Ability.AbilityData.Name;
        _descriptionAbility.text = _textHighlighter.ChangeColorText(abilitySlot_UI.Ability.AbilityData.Description);
        //_descriptionAbility.text = abilitySlot_UI.Ability.AbilityData.Description;

        switch (abilitySlot_UI.Ability.AbilityData.GeneralType)
        {
            case GeneralTypeAbility.Attack:
                _primaryTypeAbility.text = $"Primary Type Ability:  Attack";
                break;

            case GeneralTypeAbility.Stats:
                _primaryTypeAbility.text = $"Primary Type Ability:  Amplify";
                break;
        }

        switch (abilitySlot_UI.Ability.AbilityData.TypeAbility)
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

        HeroSlot_UI hero = _heroesList.FirstOrDefault(i => i == _selectedHero);

        if (!hero.Hero.IsSentOnQuest)
        {
            bool abilityExists = hero.Hero.AbilityHolder.HeroAbilitySystem.AbilityList.Any(ability => ability == abilitySlot_UI.Ability);

            for (int i = 0; i < _heroAbilityDisplay.AbilitySlots.Count; i++)
            {
                if (_heroAbilityDisplay.AbilitySlots[i].IsSlotClicked == true && !abilityExists)
                {
                    hero.Hero.AbilityHolder.HeroAbilitySystem.AddAbility(abilitySlot_UI.Ability, i);
                }
            }

            hero.Hero.AbilityHolder.ShowAbilities();
        }
    }

    protected void ShowHeroInventory(HeroSlot_UI hero)
    {
        hero.Hero.EquipHolder.ShowEquipment();

        //_inventoryName.text = $"INVENTORY {hero.Slot.Hero.HeroData.Name}";
    }

    protected void ShowHeroInventory(EquipSystem equipSystem, int offset)
    {
        if (equipSystem == null)
            Debug.Log("equipSystem null");
        _equipDisplay.RefreshDynamicInventory(equipSystem, offset);

        //_inventoryName.text = $"INVENTORY {_selectedHero.Slot.Hero.HeroData.Name}";
    }

    public void ShowHeroAbilities(HeroSlot_UI hero)
    {
        ClearAbilitiesSlots();

        foreach (var ability in hero.Hero.ListAbilities)
            CreateAbilitySlot(ability);

        hero.Hero.AbilityHolder.ShowAbilities();
    }

    protected void ShowHeroAbilities(HeroAbilitySystem abilitySystem)
    {
        //_heroAbilityDisplay.RefreshDynamicAbilities(abilitySystem);
    }

    public void UpdateUIInfo(Hero hero)
    {
        ClearInfo();

        _powerPoints.text = $"Power points: {Mathf.FloorToInt(hero.HeroPowerPoints.SumPower(hero, _questParametresSystemFix))}";
        _defencePoints.text = $"Defence Points: {Mathf.FloorToInt(hero.HeroDefencePoints.SumDefence(hero, _questParametresSystemFix))}";

        _dailyPayHeroText.text = $"Daily Pay: {hero.DailyPay}";
        _statPoints.text = $"Stats Points: {hero.LevelSystem.StatPoints}";
        _abilityPoints.text = $"Ability Points: {hero.LevelSystem.AbilityPoints}";

        (string, string, string, string) multiText = _statDisplayManager.SetStatsMultiText(hero.VisibleHeroStats);
        (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(hero.VisibleHeroStats, true);

        _powerStatsName.text = statsText.Item1;
        _powerStatsMulti.text = multiText.Item2;
        _powerStatsValue.text = statsText.Item2;

        _defenceStatsName.text = statsText.Item3;
        _defenceStatsMulti.text = multiText.Item4;
        _defenceStatsValue.text = statsText.Item4;

        UpdateResistance(hero);

        _selectedHero.UpdateUISlot(hero);
    }

    private void UpdateResistance(Hero hero)
    {
        hero.ResistanceSystem.SumRes(hero, _questParametresSystemFix);

        for (int i = 0; i < _resImages.Length; i++)
        {
            _resImages[i].SetTypeDamage(hero.ResistanceSystem.VisibleAllRes[i].TypeDamage);
            _resImages[i].Percent.text = $"{hero.ResistanceSystem.VisibleAllRes[i].ValueResistance.ToString()}%";
        }
    }

    protected void ShowHeroResistance(HeroSlot_UI hero)
    {
        List<Resistance> resistances = hero.Hero.Resistances;

        StringBuilder resistNames = new StringBuilder();
        StringBuilder resistValues = new StringBuilder();

        foreach (var resist in resistances)
        {
            resistNames.AppendLine(resist.TypeDamage.ToString());
            resistValues.AppendLine(resist.ValueResistance.ToString());
        }
    }

    public virtual void SelectHero(HeroSlot_UI heroSlot)
    {
        _selectedHero = heroSlot;

        UpdateResistance();
    }

    public void SelectAbility(AbilitySlot_UI ability)
    {
        _selectedAbility = ability;
    }

    public void TabClicked(CheckTypeForTabs tab)
    {
        RefreshInfo();

        if (_selectedHero == null)
            return;

        else
        {
            switch (tab.HeroTabs)
            {
                case HeroTabs.All:
                    _inventoryData.gameObject.SetActive(true);
                    _infoData.gameObject.SetActive(true);
                    _resistanceData.gameObject.SetActive(true);

                    ShowHeroInventory(_selectedHero);
                    UpdateUIInfo(_selectedHero.Hero);
                    ShowHeroResistance(_selectedHero);

                    break;

                case HeroTabs.Inventory:
                    _inventoryData.gameObject.SetActive(true);
                    ShowHeroInventory(_selectedHero);
                    break;

                case HeroTabs.Info:
                    _infoData.gameObject.SetActive(true);
                    UpdateUIInfo(_selectedHero.Hero);
                    break;

                case HeroTabs.Resistance:
                    _resistanceData.gameObject.SetActive(true);
                    ShowHeroResistance(_selectedHero);
                    break;

                case HeroTabs.Abilities:
                    _abilitiesData.gameObject.SetActive(true);
                    ShowHeroAbilities(_selectedHero);
                    break;
            }
        }
    }

    private void ClearHeroesSlots()
    {
        foreach (Transform hero in _listHeroes.transform.Cast<Transform>())
        {
            Destroy(hero.gameObject);
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
        _primaryTypeAbility.text = "";
        _secondaryTypeAbility.text = "";
    }

    private void ClearInfo()
    {
        _nameAbility.text = "";
        _descriptionAbility.text = "";
        _primaryTypeAbility.text = "";
        _secondaryTypeAbility.text = "";

        _powerPoints.text = "";
        _defencePoints.text = "";

        _dailyPayHeroText.text = "";
        _statPoints.text = "";
        _abilityPoints.text = "";

        _powerStatsName.text = "";
        _powerStatsMulti.text = "";
        _powerStatsValue.text = "";

        _defenceStatsName.text = "";
        _defenceStatsMulti.text = "";
        _defenceStatsValue.text = "";
    }

    private void UpdateResistance()
    {
        for (int i = 0; i < _resImages.Length; i++)
        {
            _resImages[i].SetTypeDamage(_selectedHero.Hero.Resistances[i].TypeDamage);
            _resImages[i].Percent.text = $"{_selectedHero.Hero.Resistances[i].ValueResistance.ToString()}%";
        }
    }

    private void OpenAbilityTree()
    {
        if (_selectedHero != null && _selectedHero.Hero != null && _selectedAbility != null)
            _abilityTree.OpenWindowAbilityTree(_selectedAbility, _selectedHero);
    }
}
