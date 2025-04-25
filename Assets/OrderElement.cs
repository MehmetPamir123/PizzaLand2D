using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderElement : MonoBehaviour
{
    public TMP_Text text;
    public Image image;
    public void MakeTheGivenOrder(PizzaTypes pizzaType, int amount, int currentAmount) //PizzaTypes pizzaType, int amount, int currentAmount olan yeri direkt Order yap.
    {
        image.sprite = pizzaType.spritePizza;
        text.text = currentAmount+"/"+amount.ToString();
        if(currentAmount >= amount)
        {
            text.color = Color.green;
        }
       
    }


}
