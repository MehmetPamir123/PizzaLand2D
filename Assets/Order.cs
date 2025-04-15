using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Order", menuName = "OrderElement")]
public class Order : ScriptableObject
{
    public float gesamteMoney;
    public TypeAndAmount[] orderElement;


    private void OnEnable()
    {
        CalculateTotalMoney();
    }

    private void OnValidate()
    {
        CalculateTotalMoney();
    }

    private void CalculateTotalMoney()
    {
        gesamteMoney = 0;
        if (orderElement != null)
        {
            foreach (var element in orderElement)
            {
                gesamteMoney += element.money;
            }
        }
    }
}

[System.Serializable]
public class TypeAndAmount
{
    public PizzaTypes pizzaType;
    public int amount;
    public float money;
}
