using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.VisualScripting;

public class ExpeditionalCompany : MonoBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private Image _landScape;
    [SerializeField] private RegionSlot_UI _regionSlotPrefab;
    [SerializeField] private ExpeditionalRegionSlot_UI _ExpedRegionSlotPrefab;
    [SerializeField] private GameObject _listRegions;
    [SerializeField] private GameObject _listExpeditions;
    [SerializeField] private TextMeshProUGUI _descriptionRegionText;
    [SerializeField] private Button _researchButton;
    [SerializeField] private Button _keeper;
    [SerializeField] private GuildValutes _guild;
    [SerializeField] private NextDayController _nextDayController;
    [SerializeField] private DynamicQuestList _dynamicQuestList;
    [SerializeField] private QuestKeeper _questKeeper;
    [SerializeField] private int _dayResearchByRalent;
    [Header("Low Panel")]
    [SerializeField] private int _level;
    [SerializeField] private int _cost;
    [SerializeField] private float _coefCost;
    [SerializeField] private int _totalCost;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _costText;
    [SerializeField] private Button _addLevelButton;
    [SerializeField] private Button _reduceLevelButton;
    [Header("Sprites Landscape")]
    [SerializeField] private Sprite _verenaya;
    [SerializeField] private Sprite _moonwood;
    [SerializeField] private Sprite _recarianHighland;
    [SerializeField] private Sprite _castorianWasteland;
    [Header("List")]
    [SerializeField] private List<ExpeditionalRegionSlot_UI> _listExpeditionalRegionSlots = new List<ExpeditionalRegionSlot_UI>();
    [SerializeField] private List<int> _costResearchWeekList_1 = new List<int>(); // Веренайя
    [SerializeField] private List<int> _costResearchWeekList_2 = new List<int>(); // Лунный лес
    [SerializeField] private List<int> _costResearchWeekList_3 = new List<int>(); // Рекарское нагорье
    [SerializeField] private List<int> _costResearchWeekList_4 = new List<int>(); // Касторская пустошь

    private RegionSlot_UI _selectedRegionSlot;
    private ExpeditionalRegionSlot_UI _selectedExpedRegionSlot;

    public static event UnityAction<int> OnPayExpeditional;

    private void Awake()
    {
        _panel.gameObject.SetActive(false);

        ClearExpeditionalRegionSlots();

        _keeper?.onClick.AddListener(ShowDisplay);
        _researchButton?.onClick.AddListener(StartResearch);
        _addLevelButton?.onClick.AddListener(AddLevelExpedition);
        _reduceLevelButton?.onClick.AddListener(ReduceLevelExpedition);
    }

    private void Start()
    {
        _level = _guild.Level;

        SetCostResearchingExpeditionForWeek();
        SetRegions(_guild.Level);

        _levelText.text = $"Level: {_level}";
        _costText.text = $"Cost: {_totalCost}";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _selectedRegionSlot = null;

            _levelText.text = "Level:";
            _costText.text = "Cost:";
        }
    }

    private void OnEnable()
    {
        _level = _guild.Level;

        _nextDayController.OnNextWeek += SetCostResearchingExpeditionForWeek;
        _guild.OnChangeLevel += SetCostResearchingExpeditionForWeek;
    }

    private void OnDisable()
    {
        _nextDayController.OnNextWeek -= SetCostResearchingExpeditionForWeek;
        _guild.OnChangeLevel -= SetCostResearchingExpeditionForWeek;
    }

    public void ShowDisplay()
    {
        if (!_panel.gameObject.activeSelf)
            _panel.gameObject.SetActive(true);
    }

    public void SelectRegionSlot(RegionSlot_UI regionSlot)
    {
        _selectedRegionSlot = regionSlot;

        _descriptionRegionText.text = regionSlot.RegionData.RegionDescription;

        SetRegions(_level);
        SetLandscape(regionSlot.RegionData);
    }

    public void SelectExpeditionalRegionSlot(ExpeditionalRegionSlot_UI expedRegionSlot)
    {
        _selectedExpedRegionSlot = expedRegionSlot;

        _descriptionRegionText.text = expedRegionSlot.RegionData.RegionDescription;

        SetLandscape(expedRegionSlot.RegionData);
    }

    private void StartResearch()
    {
        if (_selectedRegionSlot != null)
        {
            if (_guild.Gold >= _totalCost)
            {
                OnPayExpeditional?.Invoke(-_totalCost);
                ExpeditionalRegionSlot_UI expedRegionSlot_UI = CreateExpeditionalRegionSlot(_selectedRegionSlot.RegionData);
                expedRegionSlot_UI.SetDays(_dayResearchByRalent);
                _listExpeditionalRegionSlots.Add(expedRegionSlot_UI);
            }

            else
            {
                Debug.Log("not enough gold");
                return;
            }
        }
    }

    private ExpeditionalRegionSlot_UI CreateExpeditionalRegionSlot(RegionData regionData)
    {
        ExpeditionalRegionSlot_UI expedRegionSlot_UI = Instantiate(_ExpedRegionSlotPrefab, _listExpeditions.transform);
        expedRegionSlot_UI.Init(regionData);

        return expedRegionSlot_UI;
    }

    private void ClearExpeditionalRegionSlots()
    {
        foreach (Transform slot in _listExpeditions.transform.Cast<Transform>())
        {
            Destroy(slot.gameObject);
        }
    }

    public void NextDay() // подписан на NExtDayController через событие в инспекторе
    {
        for (int i = _listExpeditionalRegionSlots.Count - 1; i >= 0; i--)
        {
            _listExpeditionalRegionSlots[i].NextDay();

            if (_listExpeditionalRegionSlots[i].DaysLeft == 0)
            {
                Expedition newExpedition = _dynamicQuestList.CreateNewExpedition(_listExpeditionalRegionSlots[i], _level);
                _questKeeper.BoardSystem.ExpeditionSlots.Add(newExpedition);
                Destroy(_listExpeditionalRegionSlots[i].gameObject);
                _listExpeditionalRegionSlots.RemoveAt(i);
                NotificationSystem.Instance.CheckNotifications();
            }
        }
    }

    private void AddLevelExpedition()
    {
        if (_level == _guild.Level)
            return;

        else
            _level++;

        SetRegions(_level);

        _levelText.text = $"Level: {_level}";
        _costText.text = $"Cost: {_totalCost}";
    }

    private void ReduceLevelExpedition()
    {
        if (_level == 1)
            return;

        else
            _level--;

        SetRegions(_level);

        _levelText.text = $"Level: {_level}";
        _costText.text = $"Cost: {_totalCost}";
    }

    private void SetCostResearchingExpeditionForWeek()
    {
        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_1.Add(0);

        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_2.Add(0);

        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_3.Add(0);

        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_4.Add(0);

        for (int i = 0; i < _costResearchWeekList_1.Count; i++)
            CalculateCosts(i + 1);

        for (int i = 0; i < _costResearchWeekList_2.Count; i++)
            CalculateCosts(i + 1);

        for (int i = 0; i < _costResearchWeekList_3.Count; i++)
            CalculateCosts(i + 1);

        for (int i = 0; i < _costResearchWeekList_4.Count; i++)
            CalculateCosts(i + 1);
    }

    private void SetCostResearchingExpeditionForWeek(int level)
    {
        for (int i = 0; i < level; i++)
            _costResearchWeekList_1.Add(0);

        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_2.Add(0);

        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_3.Add(0);

        for (int i = 0; i < _guild.Level; i++)
            _costResearchWeekList_4.Add(0);

        for (int i = 0; i < _costResearchWeekList_1.Count; i++)
            CalculateCosts(i + 1);

        for (int i = 0; i < _costResearchWeekList_2.Count; i++)
            CalculateCosts(i + 1);

        for (int i = 0; i < _costResearchWeekList_3.Count; i++)
            CalculateCosts(i + 1);

        for (int i = 0; i < _costResearchWeekList_4.Count; i++)
            CalculateCosts(i + 1);
    }

    private void CalculateCosts(int level)
    {
        switch (level)
        {
            case 1:
                _costResearchWeekList_1[0] = Random.Range(1017, 1398);
                _costResearchWeekList_2[0] = Random.Range(1017, 1398);
                _costResearchWeekList_3[0] = Random.Range(1017, 1398);
                _costResearchWeekList_4[0] = Random.Range(1017, 1398);
                break;

            case 2:
                _costResearchWeekList_1[1] = Random.Range(1400, 1846);
                _costResearchWeekList_2[1] = Random.Range(1400, 1846);
                _costResearchWeekList_3[1] = Random.Range(1400, 1846);
                _costResearchWeekList_4[1] = Random.Range(1400, 1846);
                break;

            case 3:
                _costResearchWeekList_1[2] = Random.Range(2224, 3058);
                _costResearchWeekList_2[2] = Random.Range(2224, 3058);
                _costResearchWeekList_3[2] = Random.Range(2224, 3058);
                _costResearchWeekList_4[2] = Random.Range(2224, 3058);
                break;

            case 4:
                _costResearchWeekList_1[3] = Random.Range(5368, 7380);
                _costResearchWeekList_2[3] = Random.Range(5368, 7380);
                _costResearchWeekList_3[3] = Random.Range(5368, 7380);
                _costResearchWeekList_4[3] = Random.Range(5368, 7380);
                break;
        }
    }

    private void SetRegions(int level)
    {
        if (_selectedRegionSlot != null && _selectedRegionSlot.RegionData != null)
        {
            switch (_selectedRegionSlot.RegionData.RegionType)
            {
                case RegionQuest.None:
                    break;

                case RegionQuest.Region_1:
                    _cost = _costResearchWeekList_1[level - 1];
                    break;

                case RegionQuest.Region_2:
                    _cost = _costResearchWeekList_2[level - 1];
                    break;

                case RegionQuest.Region_3:
                    _cost = _costResearchWeekList_3[level - 1];
                    break;

                case RegionQuest.Region_4:
                    _cost = _costResearchWeekList_4[level - 1];
                    break;

                case RegionQuest.Region_5:
                    break;
            }
        }

        _totalCost = (int)(_cost * _coefCost);
        _costText.text = $"Cost: {_totalCost}";
    }

    private void SetLandscape(RegionData regionData)
    {
        switch (regionData.RegionType)
        {
            case RegionQuest.None:
                break;

            case RegionQuest.Region_1:
                _landScape.sprite = _verenaya;
                break;

            case RegionQuest.Region_2:
                _landScape.sprite = _moonwood;
                break;

            case RegionQuest.Region_3:
                _landScape.sprite = _recarianHighland;
                break;

            case RegionQuest.Region_4:
                _landScape.sprite = _castorianWasteland;
                break;
        }
    }

    public void ChangeCostExpedionalTalent(float coefCost)
    {
        _coefCost += coefCost;
    }

    public void ChangeDayResearchByTalent(int day)
    {
        _dayResearchByRalent = day;
    }
}
