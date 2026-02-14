using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSlot_UI : MonoBehaviour
{
    [Header("Preview Item")]
    [SerializeField] protected Image _heroIcon;
    //[SerializeField] private Image _backgroundSprite;
    //[SerializeField] private TextMeshProUGUI _className;
    [SerializeField] private TextMeshProUGUI _heroName;
    [SerializeField] protected TextMeshProUGUI _powerPoints;
    [SerializeField] protected TextMeshProUGUI _defencePoints;
    [SerializeField] protected TextMeshProUGUI _heroLevel;
    [SerializeField] protected Hero _hero;

    [SerializeField] protected Button _updatePreviewButton;

    public Hero Hero => _hero;

    public KeeperDisplay ParentDisplay { get; private set; }

    protected virtual void Awake()
    {
        _heroIcon.sprite = null;
        _heroIcon.preserveAspect = true;
        _heroIcon.color = Color.clear;

        //_backgroundSprite.sprite = null;
        //_backgroundSprite.preserveAspect = true;
        //_backgroundSprite.color = Color.clear;

        //_className.text = "";
        _heroName.text = "";
        //_powerPoints.text = "";
        //_defencePoints.text = "";
        _heroLevel.text = "";

        //_addItemToCartButton?.onClick.AddListener(AddItemToCart);
        //_removeItemFromCartButton?.onClick.AddListener(RemoveItemFromCart);
        _updatePreviewButton?.onClick.AddListener(UpdateItemPreview);

        ParentDisplay = GetComponentInParent<KeeperDisplay>();
    }

    public virtual void Init(Hero hero)
    {
        _hero = hero;

        UpdateUISlot();
    }

    public virtual void UpdateUISlot()
    {
        if (_hero != null)
        {
            _heroIcon.sprite = _hero.HeroData.Icon;
            _heroIcon.color = Color.white;

            //var modifiedPrice = RecruitKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, MarkUp);

            //_className.text = $"{_hero.HeroData.ClassName}";
            _heroName.text = $"{_hero.HeroName} ({_hero.HeroData.ClassName})";
            _powerPoints.text = $"Power: {_hero.HeroPowerPoints.VisibleAllPower}";
            _defencePoints.text = $"Defence: {_hero.HeroDefencePoints.VisibleAllDefence}";
            _heroLevel.text = $"Level: {_hero.HeroLevel}";
        }
        else
        {
            _heroIcon.sprite = null;
            _heroIcon.color = Color.clear;

            //_backgroundSprite.sprite = null;
            //_backgroundSprite.color = Color.clear;

            //_className.text = "";
            _heroName.text = "";
            _powerPoints.text = "";
            _defencePoints.text = "";
            _heroLevel.text = "";
        }
    }

    public void UpdateUISlot(Hero hero)
    {
        _heroIcon.sprite = hero.HeroData.Icon;
        _heroIcon.color = Color.white;

        //var modifiedPrice = RecruitKeeperDisplay.GetModifiedPrice(_assignedItemSlot.ItemData, 1, MarkUp);

        //_className.text = $"{hero.HeroData.ClassName}";
        _heroName.text = $"{_hero.HeroName} ({_hero.HeroData.ClassName})";
        _powerPoints.text = $"Power: {hero.HeroPowerPoints.VisibleAllPower}";
        _defencePoints.text = $"Defence: {hero.HeroDefencePoints.VisibleAllDefence}";
        _heroLevel.text = $"Level: {hero.HeroLevel}";
    }

    public virtual void UpdateItemPreview()
    {
        if (ParentDisplay != null)
        {
            ParentDisplay.SelectHero(this);
            ParentDisplay.UpdateHeroPreview(this);
        }
    }

    //private void ClearInfo()
}
