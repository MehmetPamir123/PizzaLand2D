using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class GlassSquare : MonoBehaviour
{
    public bool iHaveItem;

    public PizzaTypes pizzaType;
    public int amount;
    public int maxAmount;

    public TMP_Text text;

    public Image sliceImg;

    public UnityEvent pizzaTakeEvent;
    public int HowManyCanIAdd()
    {
        return maxAmount - amount;
    }
    public void AddToGlass(int addAmount,PizzaTypes type)
    {
        if(maxAmount < amount + addAmount) { Debug.Log("I know what u doing but.. sure..."); }

        sliceImg.sprite = type.spriteSlice;
        pizzaType = type;
        sliceImg.enabled = true;
        amount += addAmount;
        ChangeText();
    }
    public int TryEatThatMuch(int iWantThisMuch)
    {
        if(amount >= iWantThisMuch)
        {
            Eat(iWantThisMuch);
            return 0;
        }
        else
        {
            Eat(amount);
            return iWantThisMuch - amount;
        }
    }
    public void Eat(int iWantThisMuch)
    {
        if(amount <= iWantThisMuch) { iWantThisMuch = amount; }
        GameData.Money += pizzaType.SliceIncome * iWantThisMuch;
        pizzaTakeEvent.Invoke();
        amount -= iWantThisMuch;

        if(amount <= 0)
        {
            amount = 0;
            iHaveItem = false;
            pizzaType = null;
            sliceImg.enabled = false;
        }
        ChangeText();
    }
    public void Trash()
    {
        amount = 0;
        sliceImg.enabled = false;
        pizzaType = null;
        iHaveItem = false;
        ChangeText();
    }
    void ChangeText()
    {
        text.text = $"{amount}/{maxAmount}";
    }
}
