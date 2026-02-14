using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionalRegionSlot_UI : MonoBehaviour
{
    [SerializeField] private RegionData _regionData;
    [SerializeField] private int _daysLeft;
    [SerializeField] private int _minDays;
    [SerializeField] private int _maxDays;
    [SerializeField] private TextMeshProUGUI _regionName;
    [SerializeField] private TextMeshProUGUI _daysLeftText;
    [SerializeField] private Button _buttonSelf;

    public RegionData RegionData => _regionData;
    public int DaysLeft => _daysLeft;

    public ExpeditionalCompany ExpeditionalCompany { get; private set; }

    private void Awake()
    {
        ExpeditionalCompany = GetComponentInParent<ExpeditionalCompany>();

        _buttonSelf?.onClick.AddListener(ClickSlot);
    }

    public void Init(RegionData regionData)
    {
        _regionData = regionData;
        _regionName.text = regionData.RegionName;

        UpdateUI();
    }

    public void SetDays(int dayResearchTalent)
    {
        if (dayResearchTalent == 0)
            _daysLeft = Random.Range(_minDays, _maxDays);

        else
            _daysLeft = dayResearchTalent;

        UpdateUI();
    }

    private void UpdateUI()
    {
        _daysLeftText.text = $"Days: {_daysLeft}";
    }

    public void NextDay()
    {
        _daysLeft--;
        UpdateUI();
    }

    private void ClickSlot()
    {
        ExpeditionalCompany.SelectExpeditionalRegionSlot(this);
    }
}
