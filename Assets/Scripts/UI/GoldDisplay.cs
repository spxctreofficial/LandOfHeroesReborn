using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GoldDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text goldAmountText;

    private Coroutine currentTextRoutine;

    private void Start()
    {
        goldAmountText.text = DataManager.instance.GetGoldAmount().ToString();
    }
    private void Update()
    {
        if (DataManager.instance.GetGoldAmount().ToString() == goldAmountText.text || currentTextRoutine != null) return;
        currentTextRoutine = StartCoroutine(UpdateText());
    }

    private IEnumerator UpdateText()
    {
        var goldAmountShown = int.Parse(goldAmountText.text);
        while (goldAmountShown < DataManager.instance.GetGoldAmount())
        {
            goldAmountShown++;
            goldAmountText.text = goldAmountShown.ToString();
            yield return null;
        }
        while (goldAmountShown > DataManager.instance.GetGoldAmount())
        {
            goldAmountShown--;
            goldAmountText.text = goldAmountShown.ToString();
            yield return null;
        }
        currentTextRoutine = null;
    }
}
