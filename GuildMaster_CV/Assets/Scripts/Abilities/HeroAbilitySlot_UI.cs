using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroAbilitySlot_UI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _abilityIcon;
    [SerializeField] private Image _frame;
    [SerializeField] private Image _forbiddenImage;
    [SerializeField] private bool _isSlotClicked = false;
    [SerializeField] private bool _isCanUseSlot;
    [SerializeField] private int _indexSlot = -1;
    [SerializeField] private Ability _ability;
    [SerializeField] private Sprite _defaulIcon;
    [SerializeField] private Sprite _forbiddenIcon;
    [Header("Buttons")]
    [SerializeField] private Button _delete;

    public bool IsSlotClicked => _isSlotClicked;
    public bool IsCanUseSlot => _isCanUseSlot;
    public Ability Ability => _ability;

    public DynamicHeroAbilityDisplay DynamicAbilityDisplay { get; private set; }
    public PrepareQuestUIController PrepareQuestUIController { get; private set; }
    public KeeperDisplay KeeperDisplay { get; private set; }

    public static UnityAction OnDeleteAbility;

    private void Awake()
    {
        DynamicAbilityDisplay = GetComponentInParent<DynamicHeroAbilityDisplay>();
        PrepareQuestUIController = GetComponentInParent<PrepareQuestUIController>();
        KeeperDisplay = GetComponentInParent<KeeperDisplay>();

        _delete?.onClick.AddListener(DeleteSlot);
    }

    private void OnEnable()
    {
        SetDefaultIcon();
    }

    private void OnDisable()
    {
        ClearChooseSlot();
    }

    public void SetDefaultIcon()
    {
        if(_ability == null || _ability.AbilityData == null)
            _abilityIcon.sprite = _defaulIcon;
    }

    public void ClearChooseSlot()
    {
        _isSlotClicked = false;
        ChangeColorFrame(Color.black);

        SetDefaultIcon();
    }

    public void Init(Ability ability)
    {
        _ability = ability;
        UpdateUISlot(ability);
    }

    public void UpdateUISlot(HeroAbilitySlot slot)
    {
        if (slot.Ability != null)
        {
            _abilityIcon.sprite = slot.Ability.AbilityData.Icon;
        }
    }

    public void UpdateUISlot(Ability ability)
    {
        if (ability.AbilityData != null)
            _abilityIcon.sprite = ability.AbilityData.Icon;

        else
            SetDefaultIcon();
    }

    private void ClickedSlot()
    {
        //if (_isCanUseSlot)
        //{
        //    if (DynamicAbilityDisplay != null)
        //    {
        //        DynamicAbilityDisplay.SelectSlot(this);
        //    }

        //    _isSlotClicked = true;
        //}
    }

    private void ClearSlot()
    {
        ClearChooseSlot();
        _ability = null;
        SetDefaultIcon();
    }

    public void DeleteSlot()
    {
        if (_ability != null && _ability.AbilityData != null)
        {
            Hero selectHero = new Hero();

            if (PrepareQuestUIController != null)
                selectHero = PrepareQuestUIController.GuildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(i => i == PrepareQuestUIController.SelectedHero.Hero);

            if (KeeperDisplay != null)
                selectHero = KeeperDisplay.GuildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(i => i == KeeperDisplay.SelectedHero.Hero);

            if (!selectHero.IsSentOnQuest)
            {
                int index = GetIndex();

                ChangeColorFrame(Color.black);
                selectHero.AbilityHolder.HeroAbilitySystem.RemoveAbility(index);

                ClearSlot();
                _ability = new Ability();

                OnDeleteAbility?.Invoke();
            }
        }

        else
            return;
    }

    private int GetIndex()
    {
        int index = DynamicAbilityDisplay.AbilitySlots.IndexOf(this);
        return index;
    }

    public void ChangeColorFrame(Color color)
    {
        Color cbLast = _frame.color;
        cbLast = color;
        _frame.color = cbLast;
    }

    public void SetSlotClicked(bool set)
    {
        _isSlotClicked = set;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isCanUseSlot)
        {
            if (!_isSlotClicked)
            {
                DynamicAbilityDisplay.SelectSlot(this);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isCanUseSlot)
        {
            ChangeColorFrame(Color.yellow);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isCanUseSlot)
        {
            if (!_isSlotClicked)
                ChangeColorFrame(Color.black);
        }
    }

    public void LockSlot()
    {
        _isCanUseSlot = false;
        _forbiddenImage.gameObject.SetActive(true);

        SetDefaultIcon();
    }

    public void UnlockSlot()
    {
        _isCanUseSlot = true;
        _forbiddenImage.gameObject.SetActive(false);
    }
}
