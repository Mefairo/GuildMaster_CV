using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AbilityTreeSlot_UI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _selectIcon;
    [SerializeField] private Image _pickIcon;
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private bool _primarySlot = false;
    [SerializeField] private bool _pickAbility = false;
    [SerializeField] private RectTransform _drawLine;
    [SerializeField] private Image _drawLineImage;
    [SerializeField] private AbilityTreeSlot_UI _parentAbilitySlot;
    [SerializeField] private List<AbilityTreeSlot_UI> _childAbilitiesSlots = new List<AbilityTreeSlot_UI>();
    [SerializeField] private Ability _ability;

    private HorizontalLayoutGroup _HLGObject;

    public AbilityTree AbilityTree { get; private set; }

    public Ability Ability => _ability;
    public HorizontalLayoutGroup HLGObject => _HLGObject;
    public Image Icon => _icon;
    public Image SelectIcon => _selectIcon;
    public Image PickIcon => _pickIcon;
    public Button ButtonSelf => _buttonSelf;
    public RectTransform DrawLine => _drawLine;
    public Image DrawLineImage => _drawLineImage;
    public AbilityTreeSlot_UI ParentAbilitySlot => _parentAbilitySlot;
    public List<AbilityTreeSlot_UI> ChildAbilitiesSlots => _childAbilitiesSlots;
    public bool PickAbility => _pickAbility;

    private void Awake()
    {
        AbilityTree = GetComponentInParent<AbilityTree>();
        _HLGObject = GetComponentInChildren<HorizontalLayoutGroup>();

        _buttonSelf?.onClick.AddListener(ClickSlot);
    }

    private void OnDisable()
    {
        _childAbilitiesSlots.Clear();
    }

    public void Init(Ability ability)
    {
        _ability = new Ability(ability);
        _icon.sprite = ability.AbilityData.Icon;

        if (!_primarySlot)
            CreateNewAbilities(ability);
    }

    private void CreateNewAbilities(Ability ability)
    {
        if (ability.AbilityData.StudyAbilities.Count != 0)
        {
            foreach (Ability newTreeAbility in ability.AbilityData.StudyAbilities)
            {
                Ability newAbility = new Ability(newTreeAbility);
                AbilityTreeSlot_UI newAbilityTreeSlot = Instantiate(AbilityTree.AbilitySlotPrefab, _HLGObject.transform);
                newAbilityTreeSlot.Init(newAbility);

                _childAbilitiesSlots.Add(newAbilityTreeSlot);
            }
        }
    }
    private void ClickSlot()
    {
        AbilityTree.SelectAbilitySlot(this);

        _selectIcon.gameObject.SetActive(true);
    }

    public void SetDrawLine(RectTransform drawLine, AbilityTreeSlot_UI parentAbilitySlot)
    {
        _drawLine = drawLine;
        _drawLineImage = drawLine.GetComponentInChildren<Image>();

        _parentAbilitySlot = parentAbilitySlot;

        HeroHaveAbility();
    }

    private void HeroHaveAbility()
    {
        Ability matchingAbility = AbilityTree.SelectedHeroSlot.Hero.ListAbilities.FirstOrDefault(a => a.AbilityData == _ability.AbilityData);

        if (matchingAbility != null)
        {
            _pickAbility = true;

            if (_pickIcon != null) _pickIcon.color = Color.yellow;
            if (_drawLineImage != null) _drawLineImage.color = Color.yellow;

            if (_parentAbilitySlot != null && _parentAbilitySlot.DrawLineImage != null)
                _parentAbilitySlot.DrawLineImage.color = Color.yellow;
            if (_parentAbilitySlot != null && _parentAbilitySlot.PickIcon != null)
                _parentAbilitySlot.PickIcon.color = Color.yellow;
        }
    }

    public void PickNewAbility()
    {
        _pickAbility = true;
    }
}
