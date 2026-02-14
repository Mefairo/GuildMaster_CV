using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private Slider _mainSlider;
    [SerializeField] private Slider _extraSlider;
    [SerializeField] private TextMeshProUGUI _textPlace;

    public Slider MainSlider => _mainSlider;
    public Slider ExtraSlider => _extraSlider;
    public TextMeshProUGUI TextPlace => _textPlace;

    private void Awake()
    {
        ClearInfo();
    }

    private void ClearInfo()
    {
        _mainSlider.maxValue = 0;
        _mainSlider.value = 0;

        _extraSlider.maxValue = 0;
        _extraSlider.value = 0;

        _textPlace.text = "";
    }
}
