using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuildTalentSlot_UI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _frame;
    [SerializeField] private Button _buttonSelf;
    [Header("Properties")]
    [SerializeField] private string _nameTalent;
    [TextArea(10, 10)]
    [SerializeField] private string _descriptionTalent;
    [SerializeField] private int _requiredPoints;
    [SerializeField] private bool _isTakenTalent;

    public string NameTalent => _nameTalent;
    public string DescriptionTalent => _descriptionTalent;
    public int RequiredPoints => _requiredPoints;
    public bool IsTakenTalent => _isTakenTalent;
    public Button ButtonSelf => _buttonSelf;

    public GuildTalentSystem GuildTalentSystem { get; private set; }

    private void Awake()
    {
        _buttonSelf?.onClick.AddListener(SlotClicled);
        _frame.gameObject.SetActive(false);

        GuildTalentSystem = GetComponentInParent<GuildTalentSystem>();
    }

    private void OnEnable()
    {
        if (_isTakenTalent)
            _frame.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _frame.gameObject.SetActive(false);
    }

    public void SlotClicled()
    {
        Debug.Log("click guild");
        GuildTalentSystem.SelectedSlot(this);
    }

    public virtual void RealizationTalent()
    {
        _isTakenTalent = true;

        _frame.gameObject.SetActive(true);
    }
}
