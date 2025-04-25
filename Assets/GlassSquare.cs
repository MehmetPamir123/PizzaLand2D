using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class GlassSquare : MonoBehaviour
{
    public bool iHaveItem;

    public PizzaTypes pizzaType;
    public int amount;
    public int maxAmount;

    public TMP_Text text;

    public Image sliceImg;

    public UnityEvent pizzaTakeEvent;

    public static event Action SomethingChangedInGlasses;
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
        SomethingChangedInGlasses?.Invoke();
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
            int returnedZahl = iWantThisMuch - amount;
            Eat(amount);
            return returnedZahl;
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
        SomethingChangedInGlasses?.Invoke();
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
