using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PayKeeper : MonoBehaviour
{
    [SerializeField] private Button _selfButton;
    [SerializeField] private WeeklyPaySystem _weeklyPaySystem;

    public static event UnityAction<WeeklyPaySystem> OnWeeklyPaySystemOpen;

    private void Awake()
    {
        _selfButton?.onClick.AddListener(OpenPayWindow);
    }

    public void OpenPayWindow()
    {
        OnWeeklyPaySystemOpen?.Invoke(_weeklyPaySystem);
        _weeklyPaySystem.ShowDisplay();
    }
}
