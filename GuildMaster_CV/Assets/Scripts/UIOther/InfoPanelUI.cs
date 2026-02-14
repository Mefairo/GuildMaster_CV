using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InfoPanelUI : MonoBehaviour
{
    public static InfoPanelUI Instance;

    [Header("Panels")]
    [SerializeField] private GameObject _itemInfoPanel;
    [SerializeField] private GameObject _infoPanelHorizontal;
    [SerializeField] private GameObject _infoPanelVertical;
    [SerializeField] private GameObject _infoBlankPanel;
    [SerializeField] private GameObject _infoPanelHorizontalWithNameAndDesc;
    [SerializeField] private GameObject _infoPanelVerticalWithNameAndDesc;
    [Header("UI Item Panel")]
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _itemPrimaryTypeText;
    [SerializeField] private TextMeshProUGUI _itemSecondaryTypeText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private TextMeshProUGUI _powerStatsNameText;
    [SerializeField] private TextMeshProUGUI _powerStatsValueText;
    [SerializeField] private TextMeshProUGUI _defenceStatsNameText;
    [SerializeField] private TextMeshProUGUI _defenceStatsValueText;
    [SerializeField] private float _xcorr1_InfoPanel;
    [SerializeField] private float _ycorr1_InfoPanel;
    [Header("UI Info Panel")]
    [SerializeField] private TextMeshProUGUI _infoTextHorizontal;
    [SerializeField] private TextMeshProUGUI _infoTextVertical;
    [SerializeField] private float _xcorr2_Horizontal;
    [SerializeField] private float _ycorr2_Horizontal;
    [Header("UI Item Panel")]
    [SerializeField] private TextMeshProUGUI _blankName;
    [SerializeField] private TextMeshProUGUI _blankPrimaryType;
    [SerializeField] private TextMeshProUGUI _blankSecondaryType;
    [SerializeField] private TextMeshProUGUI _blankTier;
    [SerializeField] private TextMeshProUGUI _blankCraftValue;
    [SerializeField] private TextMeshProUGUI _blankDescription;
    [SerializeField] private TextMeshProUGUI _blankPowerStatsName;
    [SerializeField] private TextMeshProUGUI _blankPowerStatsValue;
    [SerializeField] private TextMeshProUGUI _blankDefenceStatsName;
    [SerializeField] private TextMeshProUGUI _blankDefenceStatsValue;
    [SerializeField] private float _xcorr1_BlankPanel;
    [SerializeField] private float _ycorr1_BlankPanel;
    [Header("UI Info With Name and Description")]
    [SerializeField] private TextMeshProUGUI _textHorizontalWNADName;
    [SerializeField] private TextMeshProUGUI _textHorizontalWNADDescription;   
    [SerializeField] private TextMeshProUGUI _textVerticalWNADName;
    [SerializeField] private TextMeshProUGUI _textVericalWNADDescription;
    [SerializeField] private float _xcorr1_HorizontalPanelWNAD;
    [SerializeField] private float _ycorr1_HorizontalPanelWNAD;    
    [SerializeField] private float _xcorr1_VerticalPanelWNAD;
    [SerializeField] private float _ycorr1_VerticalPanelWNAD;
    [Header("Other")]
    [SerializeField] private ResImage_UI _prefabImage;
    [SerializeField] private Transform _resContainer;
    [SerializeField] private Transform _debuffContainer;

    private CraftItemData _selectItemData;
    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private QuestResistanceSystem _questResistanceSystem;
    private RectTransform _itemInfoPanelRectTransform;
    private RectTransform _infoPanelHorizontalRectTransform;
    private RectTransform _infoPanelVerticalRectTransform;
    private RectTransform _infoBlankPanelRectTr;
    private RectTransform _infoPanelHorizontalWithNameAndDescRectTr;
    private RectTransform _infoPanelVerticalWithNameAndDescRectTr;
    private Canvas _canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            _itemInfoPanelRectTransform = _itemInfoPanel.GetComponent<RectTransform>();
            _infoPanelHorizontalRectTransform = _infoPanelHorizontal.GetComponent<RectTransform>();
            _infoPanelVerticalRectTransform = _infoPanelHorizontal.GetComponent<RectTransform>();
            _infoBlankPanelRectTr = _infoBlankPanel.GetComponent<RectTransform>();
            _infoPanelHorizontalWithNameAndDescRectTr = _infoPanelHorizontalWithNameAndDesc.GetComponent<RectTransform>();
            _infoPanelVerticalWithNameAndDescRectTr = _infoPanelVerticalWithNameAndDesc.GetComponent<RectTransform>();

            _canvas = GetComponentInParent<Canvas>();
            return;
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _questResistanceSystem = GetComponent<QuestResistanceSystem>();

        if (_itemInfoPanel.activeSelf)
        {
            _itemInfoPanel.SetActive(false);
        }

        if (_infoPanelHorizontal.activeSelf)
        {
            _infoPanelHorizontal.SetActive(false);
        }

        if (_infoPanelVertical.activeSelf)
        {
            _infoPanelVertical.SetActive(false);
        }

        if (_infoBlankPanel.activeSelf)
            _infoBlankPanel.SetActive(false);

        if (_infoPanelHorizontalWithNameAndDesc.activeSelf)
            _infoPanelHorizontalWithNameAndDesc.SetActive(false);

        if (_infoPanelVerticalWithNameAndDesc.activeSelf)
            _infoPanelVerticalWithNameAndDesc.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _itemInfoPanel.SetActive(false);
            _infoPanelHorizontal.SetActive(false);
            _infoPanelVertical.SetActive(false);
            _infoBlankPanel.SetActive(false);
            _infoPanelHorizontalWithNameAndDesc.SetActive(false);
            _infoPanelVerticalWithNameAndDesc.SetActive(false);
        }

        if (_itemInfoPanel.activeSelf)
        {
            MovePanel(_xcorr1_InfoPanel, _ycorr1_InfoPanel, _itemInfoPanel, _itemInfoPanelRectTransform);
        }

        if (_infoPanelHorizontal.activeSelf)
        {
            MovePanel(_xcorr2_Horizontal, _ycorr2_Horizontal, _infoPanelHorizontal, _infoPanelHorizontalRectTransform);
        }

        if (_infoBlankPanel.activeSelf)
        {
            MovePanel(_xcorr1_BlankPanel, _ycorr1_BlankPanel, _infoBlankPanel, _infoBlankPanelRectTr);
        }

        if (_infoPanelHorizontalWithNameAndDesc.activeSelf)
        {
            MovePanel(_xcorr1_HorizontalPanelWNAD, _ycorr1_HorizontalPanelWNAD, _infoPanelHorizontalWithNameAndDesc, _infoPanelHorizontalWithNameAndDescRectTr);
        }

        if (_infoPanelVerticalWithNameAndDesc.activeSelf)
        {
            MovePanel(_xcorr1_VerticalPanelWNAD, _ycorr1_VerticalPanelWNAD, _infoPanelVerticalWithNameAndDesc, _infoPanelVerticalWithNameAndDescRectTr);
        }

        //if (_infoPanelVertical.activeSelf)
        //{
        //    MovePanel(_xcorr1, _ycorr1, _infoPanelVertical, _infoPanelVerticalRectTransform);
        //}

        //if (_infoPanel.activeSelf)
        //{
        //    //MovePanel(_xcorr2, _ycorr2, _infoPanel, _infoPanelRectTransform);
        //    Vector2 cursorPosition = Mouse.current.position.ReadValue();
        //    Vector2 offset = new Vector2(_xcorr2, -_ycorr2);
        //    _infoPanel.transform.position = cursorPosition + offset;
        //}

        //if (_itemInfoPanel.activeSelf)
        //{
        //    Vector2 cursorPosition = Mouse.current.position.ReadValue();
        //    Vector2 offset = new Vector2(_xcorr1, -_ycorr1);
        //    _itemInfoPanel.transform.position = cursorPosition + offset;
        //}
    }

    public void ShowCatalystInfo(CatalystSlot catalystSlot)
    {
        ClearInfo();

        if (catalystSlot != null && catalystSlot.CatalystSlotData != null)
        {
            _blankName.text = catalystSlot.CatalystSlotData.DisplayName;
            _blankDescription.text = catalystSlot.CatalystSlotData.Description;
            _blankPrimaryType.text = catalystSlot.CatalystSlotData.ItemPrimaryType.ToString();
            _blankTier.text = $"Reforge Spirit: {catalystSlot.CatalystSlotData.MinCraftValueSpend} - {catalystSlot.CatalystSlotData.MaxCraftValueSpend}";

            _infoBlankPanel.SetActive(true);
        }
    }

    public void ShowRuneInfo(RuneSlot runeSlot)
    {
        ClearInfo();

        if (runeSlot != null && runeSlot.RuneSlotData != null)
        {
            _blankName.text = runeSlot.RuneSlotData.DisplayName;
            _blankDescription.text = runeSlot.RuneSlotData.Description;
            _blankPrimaryType.text = runeSlot.RuneSlotData.ItemPrimaryType.ToString();
            _blankTier.text = $"Tier: {runeSlot.Tier}";

            foreach (RuneSlotStats stat in runeSlot.RuneSlotData.RuneSlotStats)
            {
                (string, string, string, string) statsText = _statDisplayManager.SetRuneStats(runeSlot.RuneSlotData);

                _blankPowerStatsName.text = statsText.Item1;
                _blankPowerStatsValue.text = statsText.Item2;

                _blankDefenceStatsName.text = statsText.Item3;
                _blankDefenceStatsValue.text = statsText.Item4;
            }

            UpdateResistance(runeSlot);
            UpdateDebuffs(runeSlot.RuneSlotData.DebuffLists);

            _infoBlankPanel.SetActive(true);
        }
    }

    public void ShowBlankInfo(BlankSlot blankSlot)
    {
        ClearInfo();

        if (blankSlot != null && blankSlot.BlankSlotData != null)
        {
            _blankName.text = blankSlot.BlankSlotData.DisplayName;
            _blankDescription.text = blankSlot.BlankSlotData.Description;
            _blankPrimaryType.text = blankSlot.BlankSlotData.ItemPrimaryType.ToString();
            _blankSecondaryType.text = blankSlot.BlankSlotData.EquipType.ToString();
            _blankTier.text = $"Properties: {blankSlot.PropertyAmount}";
            _blankCraftValue.text = $"Forge Spirit: {blankSlot.CraftValue}";

            foreach (HeroStats stat in blankSlot.ItemStats)
            {
                if (stat != null)
                {
                    (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(stat, false);

                    _blankPowerStatsName.text = statsText.Item1;
                    _blankPowerStatsValue.text = statsText.Item2;

                    _blankDefenceStatsName.text = statsText.Item3;
                    _blankDefenceStatsValue.text = statsText.Item4;
                }
            }

            UpdateResistance(blankSlot);
            UpdateDebuffs(blankSlot.ItemDefDebuffsData);

            _infoBlankPanel.SetActive(true);
        }
    }

    public void ShowItemInfo(SlotData itemData)
    {
        ClearInfo();

        if (itemData != null)
        {
            _nameText.text = itemData.DisplayName;
            _descriptionText.text = itemData.Description;
            _itemPrimaryTypeText.text = itemData.ItemPrimaryType.ToString();

            if (itemData is EquipItemData equiptemData)
            {
                foreach (HeroStats stat in equiptemData.Stats)
                {
                    if (stat != null)
                    {
                        (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(stat, false);

                        _powerStatsNameText.text = statsText.Item1;
                        _powerStatsValueText.text = statsText.Item2;

                        _defenceStatsNameText.text = statsText.Item3;
                        _defenceStatsValueText.text = statsText.Item4;

                        _itemSecondaryTypeText.text = equiptemData.EquipType.ToString();

                        if (equiptemData.EquipType == EquipType.Trinket)
                        {
                            _itemSecondaryTypeText.text = equiptemData.ItemSecondaryType.ToString();
                        }
                    }
                }
            }

            if (itemData is DrawingItemData drawingItemData)
                _itemSecondaryTypeText.text = "Recipe";

            UpdateResistance(itemData);

            _itemInfoPanel.SetActive(true);
        }
    }

    public void ShowResImageInfo(TypeDamage typeDamage)
    {
        if (typeDamage != TypeDamage.None)
        {
            _infoPanelHorizontal.SetActive(true);

            _infoTextHorizontal.text = typeDamage.ToString();
        }
    }

    public void ShowNotificationInfo(NotificationEnum notifType)
    {
        _infoPanelHorizontal.SetActive(true);

        switch (notifType)
        {
            case NotificationEnum.None:
                break;

            case NotificationEnum.HeroStatsPoints:
                _infoTextHorizontal.text = "There are unallocated hero attribute points";
                break;

            case NotificationEnum.HeroAbilityPoints:
                _infoTextHorizontal.text = "There are unspent ability points";
                break;

            case NotificationEnum.GuildStatPoints:
                _infoTextHorizontal.text = "There are unallocated guild attribute points";
                break;

            case NotificationEnum.ExpeditionResearch:
                _infoTextHorizontal.text = "An expedition is available";
                break;

            case NotificationEnum.NotGoldForWeeklyPay:
                _infoTextHorizontal.text = "Not enough gold to pay the heroes";
                break;

            case NotificationEnum.QuestShowResult:
                _infoTextHorizontal.text = "A quest has been completed";
                break;

            case NotificationEnum.NewRecipeUnlock:
                _infoTextHorizontal.text = "New recipes have been unlocked";
                break;

            case NotificationEnum.NewWorldEvent:
                _infoTextHorizontal.text = "A new event has occurred";
                break;
        }
    }

    public void ShowDebuffImageInfo(Debuff debuff)
    {
        ClearInfo();

        _textVerticalWNADName.text = debuff.DebuffData.Name.ToString();
        _textVericalWNADDescription.text = debuff.DebuffData.Description.ToString();

        _infoPanelVerticalWithNameAndDesc.SetActive(true);
    }

    public void ShowWoundImageInfo(WoundsType woundType)
    {
        _infoPanelHorizontal.SetActive(true);

        switch (woundType)
        {
            case WoundsType.Healthy:
                _infoTextHorizontal.text = "Healthy";
                break;

            case WoundsType.Light:
                _infoTextHorizontal.text = "Light Wound";
                break;

            case WoundsType.Medium:
                _infoTextHorizontal.text = "Medium Wound";
                break;

            case WoundsType.Heavy:
                _infoTextHorizontal.text = "Heavy Wound";
                break;

            case WoundsType.Dead:
                _infoTextHorizontal.text = "Dead";
                break;
        }
    }

    public void ShowWorldEventInfo(WorldEvent worldEvent)
    {
        _infoPanelVertical.SetActive(true);

        //_infoText.text = $"{worldEvent.EventData.Name}";
        _infoTextVertical.text = $"{worldEvent.EventData.Description}";
    }

    public void ShowText(string newText)
    {
        _infoPanelHorizontal.SetActive(true);

        _infoTextHorizontal.text = newText;
    }

    public void HideInfo()
    {
        _itemInfoPanel.SetActive(false);
        _infoPanelHorizontal.SetActive(false);
        _infoPanelVertical.SetActive(false);
        _infoBlankPanel.SetActive(false);
        _infoPanelHorizontalWithNameAndDesc.SetActive(false);
        _infoPanelVerticalWithNameAndDesc.SetActive(false);
    }

    private void ClearInfo()
    {
        _nameText.text = "";
        _itemPrimaryTypeText.text = "";
        _itemSecondaryTypeText.text = "";
        _descriptionText.text = "";
        _powerStatsNameText.text = "";
        _powerStatsValueText.text = "";
        _defenceStatsNameText.text = "";
        _defenceStatsValueText.text = "";

        _infoTextHorizontal.text = "";
        _infoTextVertical.text = "";

        _blankName.text = "";
        _blankPrimaryType.text = "";
        _blankSecondaryType.text = "";
        _blankDescription.text = "";
        _blankPowerStatsName.text = "";
        _blankPowerStatsValue.text = "";
        _blankDefenceStatsName.text = "";
        _blankDefenceStatsValue.text = "";
        _blankTier.text = "";
        _blankCraftValue.text = "";

        _textHorizontalWNADName.text = "";
        _textHorizontalWNADDescription.text = "";    
        _textVerticalWNADName.text = "";
        _textVericalWNADDescription.text = "";

        ClearContainer(_resContainer);
        ClearContainer(_debuffContainer);
    }

    private void UpdateResistance(SlotData itemData)
    {
        _questResistanceSystem.ClearAllIcons();

        if (itemData is EquipItemData equiptemData)
        {
            if (equiptemData.Resistance.Count != 0)
            {
                List<Resistance> resistances = equiptemData.Resistance[0].Resistances;

                _questResistanceSystem.ShowResistance(resistances);
            }
        }
    }

    private void UpdateResistance(RuneSlot runeSlot)
    {
        ClearContainer(_resContainer);

        foreach (RuneSlotResistance res in runeSlot.RuneSlotData.RuneSlotRes)
        {
            ResImage_UI imageRes = Instantiate(_prefabImage, _resContainer.transform);

            imageRes.SetTypeDamage(res.ResType, true);
            imageRes.Percent.text = $"{res.MinValueRes.ToString()}-{res.MaxValueRes.ToString()}(%)";

        }
    }

    private void UpdateResistance(BlankSlot blankSlot)
    {
        ClearContainer(_resContainer);

        foreach (ResistanceList resList in blankSlot.ItemDefRes)
        {
            foreach (Resistance res in resList.Resistances)
            {
                ResImage_UI imageRes = Instantiate(_prefabImage, _resContainer.transform);

                if (res.ValueResistance != 0)
                {
                    imageRes.SetTypeDamage(res.TypeDamage, true);
                    imageRes.Percent.text = $"{res.ValueResistance.ToString()}%";
                }

                else
                    imageRes.SetTypeDamage(res.TypeDamage);
            }
        }
    }

    private void UpdateDebuffs(List<Debuff> debuffList)
    {
        ClearContainer(_debuffContainer);

        foreach (Debuff debuff in debuffList)
        {
            ResImage_UI imageDebuff = Instantiate(_prefabImage, _debuffContainer.transform);

            imageDebuff.SetTypeDebuff(debuff);
            imageDebuff.ImageSelf.sprite = debuff.DebuffData.Icon;
        }
    }

    private void ClearContainer(Transform container)
    {
        if (container != null)
        {
            foreach (Transform child in container)
                Destroy(child.gameObject);
        }
    }

    private Vector2 ClampToScreenBounds(Vector2 targetPosition, RectTransform transform)
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 panelSize = transform.sizeDelta * _canvas.scaleFactor;

        // Проверяем правый край экрана
        if (targetPosition.x + panelSize.x > screenSize.x)
        {
            targetPosition.x = screenSize.x - panelSize.x;
        }
        // Проверяем левый край экрана
        else if (targetPosition.x < 0)
        {
            targetPosition.x = 0;
        }

        // Проверяем нижний край экрана
        if (targetPosition.y < panelSize.y)
        {
            targetPosition.y = panelSize.y;
        }
        // Проверяем верхний край экрана
        else if (targetPosition.y + panelSize.y > screenSize.y)
        {
            targetPosition.y = screenSize.y - panelSize.y;
        }

        return targetPosition;
    }

    private void MovePanel(float xcorr, float ycorr, GameObject panel, RectTransform transform)
    {
        Vector2 cursorPosition = Mouse.current.position.ReadValue();
        Vector2 targetPosition = cursorPosition + new Vector2(xcorr, -ycorr);
        Vector2 clampedPosition = ClampToScreenBounds(targetPosition, transform);
        panel.transform.position = clampedPosition;
    }
}
