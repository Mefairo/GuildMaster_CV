using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChanceDropsItemTest : MonoBehaviour
{
    public Button Buttonself;
    public TestItemBlock[] testItems = new TestItemBlock[72];


    private void Awake()
    {
        Buttonself?.onClick.AddListener(DayNext);
    }
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < testItems.Length; i++)
        {
            testItems[i].Name = (i + 1).ToString();
        }
    }

    private void DayNext()
    {
        Debug.Log("next");
        foreach (var item in testItems)
        {
            int randomChance = Random.Range(0, 101);

            if(randomChance <= item.ChanceDrop)
            {
                int randomAmount = Random.Range(item.MinAmount, item.MaxAmount);
                Debug.Log($"{item.Name}: {randomAmount}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("1");
    }
}
