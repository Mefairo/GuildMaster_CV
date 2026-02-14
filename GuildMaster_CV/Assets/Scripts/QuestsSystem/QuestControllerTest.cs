using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestControllerTest : MonoBehaviour
{
    public static QuestControllerTest Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        else
            Destroy(gameObject);
    }
}
