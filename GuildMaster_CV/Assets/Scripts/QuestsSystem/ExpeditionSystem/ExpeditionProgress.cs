using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionProgress : MonoBehaviour
{
    [SerializeField] private Slider _slider_1;
    [SerializeField] private Slider _slider_2;
    [SerializeField] private Slider _slider_3;
    [SerializeField] private Slider _slider_4;
    [Header("SliderBlocks")]
    [SerializeField] private Slider _sliderBlock_1;
    [SerializeField] private Slider _sliderBlock_2;
    [SerializeField] private Slider _sliderBlock_3;
    [SerializeField] private Slider _sliderBlock_4;

    private void Awake()
    {
        _slider_1.maxValue = 1;
        _slider_2.maxValue = 1;
        _slider_3.maxValue = 1;
        _slider_4.maxValue = 1;

        _slider_1.value = 0;
        _slider_2.value = 0;
        _slider_3.value = 0;
        _slider_4.value = 0;

        _sliderBlock_1.maxValue = 1;
        _sliderBlock_2.maxValue = 1;
        _sliderBlock_3.maxValue = 1;
        _sliderBlock_4.maxValue = 1;

        _sliderBlock_1.value = 0;
        _sliderBlock_2.value = 0;
        _sliderBlock_3.value = 0;
        _sliderBlock_4.value = 0;
    }

    public void ShowProgress(Expedition expedition)
    {
        ShowStages(expedition);

        Image fillImage1 = _slider_1.fillRect.GetComponent<Image>();
        Image fillImage2 = _slider_2.fillRect.GetComponent<Image>();
        Image fillImage3 = _slider_3.fillRect.GetComponent<Image>();
        Image fillImage4 = _slider_4.fillRect.GetComponent<Image>();

        Image fillImageBlock1 = _sliderBlock_1.fillRect.GetComponent<Image>();
        Image fillImageBlock2 = _sliderBlock_2.fillRect.GetComponent<Image>();
        Image fillImageBlock3 = _sliderBlock_3.fillRect.GetComponent<Image>();
        Image fillImageBlock4 = _sliderBlock_4.fillRect.GetComponent<Image>();

        switch (expedition.CurrentStage)
        {
            case 0:
                _slider_1.value = 0;
                _slider_2.value = 0;
                _slider_3.value = 0;
                _slider_4.value = 0;

                _sliderBlock_1.value = 0;
                _sliderBlock_2.value = 0;
                _sliderBlock_3.value = 0;
                _sliderBlock_4.value = 0;
                break;

            case 1:
                _slider_1.value = 1;
                _slider_2.value = 0;
                _slider_3.value = 0;
                _slider_4.value = 0;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 0;
                _sliderBlock_3.value = 0;
                _sliderBlock_4.value = 0;
                break;

            case 2:
                _slider_1.value = 1;
                _slider_2.value = 1;
                _slider_3.value = 0;
                _slider_4.value = 0;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 1;
                _sliderBlock_3.value = 0;
                _sliderBlock_4.value = 0;
                break;

            case 3:
                _slider_1.value = 1;
                _slider_2.value = 1;
                _slider_3.value = 1;
                _slider_4.value = 0;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 1;
                _sliderBlock_3.value = 1;
                _sliderBlock_4.value = 0;
                break;

            case 4:
                _slider_1.value = 1;
                _slider_2.value = 1;
                _slider_3.value = 1;
                _slider_4.value = 1;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 1;
                _sliderBlock_3.value = 1;
                _sliderBlock_4.value = 1;
                break;
        }

        fillImage1.color = new Color32(241, 162, 0, 255);
        fillImage2.color = new Color32(241, 162, 0, 255);
        fillImage3.color = new Color32(241, 162, 0, 255);
        fillImage4.color = new Color32(241, 162, 0, 255);

        fillImageBlock1.color = new Color32(241, 162, 0, 255);
        fillImageBlock2.color = new Color32(241, 162, 0, 255);
        fillImageBlock3.color = new Color32(241, 162, 0, 255);
        fillImageBlock4.color = new Color32(241, 162, 0, 255);
    }

    private void ShowStages(Expedition expedition)
    {
        switch (expedition.AmountStages)
        {
            case 2:
                _slider_3.gameObject.SetActive(false);
                _slider_4.gameObject.SetActive(false);

                _sliderBlock_3.gameObject.SetActive(false);
                _sliderBlock_4.gameObject.SetActive(false);
                break;

            case 3:
                _slider_3.gameObject.SetActive(true);
                _slider_4.gameObject.SetActive(false);

                _sliderBlock_3.gameObject.SetActive(true);
                _sliderBlock_4.gameObject.SetActive(false);
                break;

            case 4:
                _slider_3.gameObject.SetActive(true);
                _slider_4.gameObject.SetActive(true);

                _sliderBlock_3.gameObject.SetActive(true);
                _sliderBlock_4.gameObject.SetActive(true);
                break;
        }
    }

    public void ShowSuccessedResult(Expedition expedition)
    {
        ShowStages(expedition);

        Image fillImage1 = _slider_1.fillRect.GetComponent<Image>();
        Image fillImage2 = _slider_2.fillRect.GetComponent<Image>();
        Image fillImage3 = _slider_3.fillRect.GetComponent<Image>();
        Image fillImage4 = _slider_4.fillRect.GetComponent<Image>();

        Image fillImageBlock1 = _sliderBlock_1.fillRect.GetComponent<Image>();
        Image fillImageBlock2 = _sliderBlock_2.fillRect.GetComponent<Image>();
        Image fillImageBlock3 = _sliderBlock_3.fillRect.GetComponent<Image>();
        Image fillImageBlock4 = _sliderBlock_4.fillRect.GetComponent<Image>();

        switch (expedition.CurrentSaveStage)
        {
            case 1:
                _slider_1.value = 1;
                _slider_2.value = 0;
                _slider_3.value = 0;
                _slider_4.value = 0;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 0;
                _sliderBlock_3.value = 0;
                _sliderBlock_4.value = 0;

                fillImage1.color = Color.green;
                fillImageBlock1.color = Color.green;

                break;

            case 2:
                _slider_1.value = 1;
                _slider_2.value = 1;
                _slider_3.value = 0;
                _slider_4.value = 0;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 1;
                _sliderBlock_3.value = 0;
                _sliderBlock_4.value = 0;

                fillImage1.color = new Color32(241, 162, 0, 255);
                fillImage2.color = Color.green;

                fillImageBlock1.color = new Color32(241, 162, 0, 255);
                fillImageBlock2.color = Color.green;

                break;

            case 3:
                _slider_1.value = 1;
                _slider_2.value = 1;
                _slider_3.value = 1;
                _slider_4.value = 0;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 1;
                _sliderBlock_3.value = 1;
                _sliderBlock_4.value = 0;

                fillImage1.color = new Color32(241, 162, 0, 255);
                fillImage2.color = new Color32(241, 162, 0, 255);
                fillImage3.color = Color.green;

                fillImageBlock1.color = new Color32(241, 162, 0, 255);
                fillImageBlock2.color = new Color32(241, 162, 0, 255);
                fillImageBlock3.color = Color.green;

                break;

            case 4:
                _slider_1.value = 1;
                _slider_2.value = 1;
                _slider_3.value = 1;
                _slider_4.value = 1;

                _sliderBlock_1.value = 1;
                _sliderBlock_2.value = 1;
                _sliderBlock_3.value = 1;
                _sliderBlock_4.value = 1;

                fillImage1.color = new Color32(241, 162, 0, 255);
                fillImage2.color = new Color32(241, 162, 0, 255);
                fillImage3.color = new Color32(241, 162, 0, 255);
                fillImage4.color = Color.green;

                fillImageBlock1.color = new Color32(241, 162, 0, 255);
                fillImageBlock2.color = new Color32(241, 162, 0, 255);
                fillImageBlock3.color = new Color32(241, 162, 0, 255);
                fillImageBlock4.color = Color.green;

                break;
        }
    }

    public void ShowFailedResult(Expedition expedition)
    {

    }
}
