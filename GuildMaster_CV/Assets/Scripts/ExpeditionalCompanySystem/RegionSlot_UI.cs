using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegionSlot_UI : MonoBehaviour
{
    [SerializeField] private RegionData _regionData;
    [SerializeField] private TextMeshProUGUI _regionName;
    [SerializeField] private Button _buttonSelf;

    public RegionData RegionData => _regionData;

    public ExpeditionalCompany ExpeditionalCompany { get; private set; }

    private void Awake()
    {
        ExpeditionalCompany = GetComponentInParent<ExpeditionalCompany>();

        _buttonSelf?.onClick.AddListener(ClickSlot);
    }

    private void OnEnable()
    {
        _regionName.text = _regionData.RegionName;
    }

    private void ClickSlot()
    {
        ExpeditionalCompany.SelectRegionSlot(this);
    }
}
