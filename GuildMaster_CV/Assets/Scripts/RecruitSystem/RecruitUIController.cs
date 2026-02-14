using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitUIController : MonoBehaviour
{
    [SerializeField] private RecruitKeeperDisplay _recruitKeeperDisplay;

    private void Awake()
    {
        _recruitKeeperDisplay.gameObject.SetActive(false);
    }
}
