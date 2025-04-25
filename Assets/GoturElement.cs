using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoturElement : MonoBehaviour
{
    public string myIngredient;
    public int amount = 0;
    public float cost = 0;
    public GameObject outline;
    public GameObject subtractButton;
    public TMP_Text textAmount;
    public TMP_Text textCost;
    public GoturMain goturMain;

    private void OnEnable()
    {
        amount = 0;
        outline.SetActive(false);
        subtractButton.SetActive(false);
        textAmount.text = amount.ToString();
        textCost.text = cost.ToString();
    }
    public void Add()
    {
        amount+=5;
        if(amount > 0)
        {
            outline.SetActive(true);
            subtractButton.SetActive(true);
        }
        goturMain.addItem(myIngredient, 5, cost);
        changeText();
    }
    public void Substract()
    {
        amount--;
        if (amount <= 0)
        {
            outline.SetActive(false);
            subtractButton.SetActive(false);
        }
        goturMain.addItem(myIngredient, -1, cost);
        changeText();
    }
    void changeText()
    {
        textAmount.text = amount.ToString();
    }
}
