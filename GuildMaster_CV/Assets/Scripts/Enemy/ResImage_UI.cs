using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResImage_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _buttonImageSelf;
    [SerializeField] private Image _imageSelf;
    [SerializeField] private TextMeshProUGUI _percent;
    [SerializeField] private TypeDamage _typeDamage;
    [SerializeField] private Debuff _debuff;
    [Header("Res Icons")]
    [SerializeField] private Sprite _physical;
    [SerializeField] private Sprite _fire;
    [SerializeField] private Sprite _cold;
    [SerializeField] private Sprite _lightning;
    [SerializeField] private Sprite _necrotic;
    [SerializeField] private Sprite _dark;
    [SerializeField] private Sprite _light;
    [Header("Debuff Icons")]
    [SerializeField] private Sprite _bleed;
    [SerializeField] private Sprite _poison;
    [SerializeField] private Sprite _burn;
    [SerializeField] private Sprite _freeze;

    public Image ImageSelf => _imageSelf;
    public TextMeshProUGUI Percent => _percent;
    public TypeDamage TypeDamage => _typeDamage;
    public Debuff Debuff => _debuff;

    private void Awake()
    {
        SetResIcons();
    }

    public void SetTypeDamage(TypeDamage typeDamage)
    {
        _typeDamage = typeDamage;

        SetResIcons();
    }

    public void SetTypeDamage(TypeDamage typeDamage, bool percentEnable)
    {
        _typeDamage = typeDamage;
        _percent.gameObject.SetActive(percentEnable);

        SetResIcons();
    }

    public void SetTypeDebuff(Debuff debuff)
    {
        _debuff = debuff;
    }

    private void SetResIcons()
    {
        switch (_typeDamage)
        {
            case TypeDamage.None:
                //_imageSelf = null;
                break;

            case TypeDamage.Physical:
                _imageSelf.sprite = _physical;
                break;

            case TypeDamage.Fire:
                _imageSelf.sprite = _fire;
                break;

            case TypeDamage.Cold:
                _imageSelf.sprite = _cold;
                break;

            case TypeDamage.Lightning:
                _imageSelf.sprite = _lightning;
                break;

            case TypeDamage.Necrotic:
                _imageSelf.sprite = _necrotic;
                break;

            case TypeDamage.Dark:
                _imageSelf.sprite = _dark;
                break;

            case TypeDamage.Light:
                _imageSelf.sprite = _light;
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_typeDamage != TypeDamage.None)
            InfoPanelUI.Instance.ShowResImageInfo(_typeDamage);

        if (_debuff != null && _debuff.DebuffData != null)
            InfoPanelUI.Instance.ShowDebuffImageInfo(_debuff);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
