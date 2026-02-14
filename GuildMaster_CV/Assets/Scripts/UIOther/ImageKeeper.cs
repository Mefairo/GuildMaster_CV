using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ImageKeeper : MonoBehaviour
{
    [Header ("Resistance Image")]
    [SerializeField] private Sprite _physical;
    [SerializeField] private Sprite _fire;
    [SerializeField] private Sprite _cold;
    [SerializeField] private Sprite _lightning;
    [SerializeField] private Sprite _necrotic;
    [SerializeField] private Sprite _dark;
    [SerializeField] private Sprite _light;

    public Sprite Physical => _physical;
    public Sprite Fire => _fire;
    public Sprite Cold => _cold;
    public Sprite Lightning => _lightning;
    public Sprite Necrotic => _necrotic;
    public Sprite Dark => _dark;
    public Sprite Light => _light;

    public void SetIcon(Resistance res, Image imageRes)
    {
        switch (res.TypeDamage)
        {
            case TypeDamage.Physical:
                imageRes.sprite = _physical;
                break;
            case TypeDamage.Fire:
                imageRes.sprite = _fire;
                break;
            case TypeDamage.Cold:
                imageRes.sprite = _cold;
                break;
            case TypeDamage.Lightning:
                imageRes.sprite = _lightning;
                break;
            case TypeDamage.Necrotic:
                imageRes.sprite = _necrotic;
                break;
            case TypeDamage.Dark:
                imageRes.sprite = _dark;
                break;
            case TypeDamage.Light:
                imageRes.sprite = _light;
                break;
            default:
                imageRes.color = Color.white;
                imageRes.gameObject.SetActive(false);
                break;
        }
    }

    public void SetIcon(Resistance res, ResImage_UI imageRes)
    {
        switch (res.TypeDamage)
        {
            case TypeDamage.Physical:
                imageRes.ImageSelf.sprite = _physical;
                break;
            case TypeDamage.Fire:
                imageRes.ImageSelf.sprite = _fire;
                break;
            case TypeDamage.Cold:
                imageRes.ImageSelf.sprite = _cold;
                break;
            case TypeDamage.Lightning:
                imageRes.ImageSelf.sprite = _lightning;
                break;
            case TypeDamage.Necrotic:
                imageRes.ImageSelf.sprite = _necrotic;
                break;
            case TypeDamage.Dark:
                imageRes.ImageSelf.sprite = _dark;
                break;
            case TypeDamage.Light:
                imageRes.ImageSelf.sprite = _light;
                break;
            default:
                imageRes.ImageSelf.color = Color.white;
                imageRes.gameObject.SetActive(false);
                break;
        }

        imageRes.SetTypeDamage(res.TypeDamage);
    }
}
