using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using UnityEngine.Rendering.Universal;
using static System.TimeZoneInfo;
using UnityEngine.Experimental.GlobalIllumination;

public class NextDayController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _selfButton;
    [SerializeField] private TextMeshProUGUI _daysText;
    [SerializeField] private TextMeshProUGUI _weekText;
    [Header("Parametres")]
    [SerializeField] private int _weekDays;
    [SerializeField] private int _leftWeekDays;
    [SerializeField] private int _weekAmount;
    [Header("Others")]
    [SerializeField] private GuildValutes _guild;

    [Header("Light Sources")]
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private Light2D _sunLight;
    [SerializeField] private Light2D _moonLight;
    [SerializeField] private List<Light2D> _spotLocalLight;

    [Header("Light Settings")]
    [SerializeField] private float _dayStartIntensity;
    [SerializeField] private float _dayEndIntensity;

    [SerializeField] private float _nightStartIntensity;
    [SerializeField] private float _nightEndIntensity;

    [SerializeField] private float _spotsStartIntensity;
    [SerializeField] private float _spotsEndIntensity;

    [Header("Transition Settings")]
    [SerializeField] private float _dayDuration;
    [SerializeField] private float _nightDuration;
    [SerializeField] private float _nightPauseDuration;

    [Header("Move Moon Settings")]
    [SerializeField] private Vector3 _moonStartPos;
    [SerializeField] private Vector3 _moonEndPos;
    [SerializeField] private float _moonMoveDuration;

    [Header("Sun Settings")]
    [SerializeField] private float _sunInnerStart;
    [SerializeField] private float _sunInnerEnd;
    [SerializeField] private float _sunOuterStart;
    [SerializeField] private float _sunOuterEnd;

    private bool _inTransition = false;

    public int WeekDays => _weekDays;
    public int LeftWeekDays => _leftWeekDays;
    public int WeekAmount => _weekAmount;
    public bool InTransition => _inTransition;

    public event UnityAction OnNextDay;
    public static event UnityAction OnNextDayStatic;
    public event UnityAction OnNextWeek;

    public UnityEvent OnNextDayByHand;
    public UnityEvent OnNextWeekByHand;

    private void Awake()
    {
        _selfButton.onClick.AddListener(NextDay);

        _leftWeekDays = _weekDays;

        UpdateUI();
    }

    private void NextDay()
    {
        if (_guild.IsNotPaid)
        {
            Debug.Log("notpaid");
            return;
        }

        if (!_inTransition)
        {
            StartCoroutine(MoveSunMoon(_moonLight.transform, _moonStartPos, _moonEndPos, _moonMoveDuration));
            StartCoroutine(DayNightDaySequence());
        }
    }

    private void UpdateUI()
    {
        _daysText.text = $"Days: {_weekDays - (_leftWeekDays - 1)}";
        _weekText.text = $"Week: {_weekAmount}";
    }

    private IEnumerator DayNightDaySequence()
    {
        OnNextDayStatic?.Invoke();

        _inTransition = true;

        // День → Ночь
        yield return StartCoroutine(DayNightChange(_nightDuration, true, _dayStartIntensity, _dayEndIntensity, _nightEndIntensity, _nightStartIntensity));


        // Пауза (ночь)
        yield return new WaitForSeconds(_nightPauseDuration);

        NotificationSystem.Instance.CheckNotifications();

        _leftWeekDays--;

        if (_leftWeekDays == 0)
        {
            _leftWeekDays = _weekDays;
            _weekAmount++;
            OnNextWeek?.Invoke();
        }

        //UpdateUI();
        //OnNextDay?.Invoke();
        //OnNextDayByHand?.Invoke();

        // Ночь → Новый день
        yield return StartCoroutine(DayNightChange(_dayDuration, false, _dayEndIntensity, _dayStartIntensity, _nightStartIntensity, _nightEndIntensity));

        UpdateUI();
        OnNextDay?.Invoke();
        OnNextDayByHand?.Invoke();

        _inTransition = false;
    }

    private IEnumerator DayNightChange(float duration, bool isDayEnd, float startDayPoint, float endDayPoint, float startNightPoint, float endNightPoint)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;

            //_globalLight.color = Color.Lerp(startColor, targetColor, t);
            _globalLight.intensity = Mathf.Lerp(startDayPoint, endDayPoint, t);

            _moonLight.intensity = Mathf.Lerp(startNightPoint, endNightPoint, t);

            if (isDayEnd)
            {
                foreach (Light2D spot in _spotLocalLight)
                    spot.intensity = Mathf.Lerp(_spotsEndIntensity, _spotsStartIntensity, t);

                //_sunLight.pointLightInnerRadius = Mathf.Lerp(_sunInnerStart, _sunInnerEnd, t);
                //_sunLight.pointLightOuterRadius = Mathf.Lerp(_sunOuterStart, _sunOuterEnd, t);
                //_sunLight.falloffIntensity = Mathf.Lerp(_sunInnerEnd, _sunInnerStart, t);
            }

            else
            {
                foreach (Light2D spot in _spotLocalLight)
                    spot.intensity = Mathf.Lerp(_spotsStartIntensity, _spotsEndIntensity, t);

                _sunLight.pointLightInnerRadius = Mathf.Lerp(_sunInnerEnd, _sunInnerStart, t);
                _sunLight.pointLightOuterRadius = Mathf.Lerp(_sunOuterEnd, _sunOuterStart, t);
                //_sunLight.falloffIntensity = Mathf.Lerp(_sunInnerStart, _sunInnerEnd, _moonMoveDuration);
            }

            yield return null;
        }

        _globalLight.intensity = endDayPoint;
    }

    private IEnumerator MoveSunMoon(Transform transform, Vector3 fromPosition, Vector3 toPosition, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(fromPosition, toPosition, t);
            yield return null;
        }

        // Гарантируем точную финальную позицию
        transform.position = toPosition;
    }
}
