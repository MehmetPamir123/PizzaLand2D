using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoturMain : MonoBehaviour
{
    public GameObject paymentPanel;
    public PizzaIngredients gesamtIngredients;
    public float gesamtCost;
    public TMP_Text TotalText;

    private void OnEnable()
    {
        gesamtIngredients = new PizzaIngredients(0,0,0,0,0,0,0,0);
        gesamtCost = 0;
        ChangeTotalCostText();
    }
    public void addItem(string name, int amount, float costAmount)
    {
        PizzaIngredients addedIngredients = new PizzaIngredients(0, 0, 0, 0, 0, 0, 0, 0);
        switch (name)
        {

            case "dough":
                addedIngredients = new PizzaIngredients(amount, 0, 0, 0, 0, 0, 0, 0);
                break;
            case "souce":
                addedIngredients = new PizzaIngredients(0, amount, 0, 0, 0, 0, 0, 0);
                break;
            case "cheese":
                addedIngredients = new PizzaIngredients(0, 0, amount, 0, 0, 0, 0, 0);
                break;
            case "sausage":
                addedIngredients = new PizzaIngredients(0, 0, 0, amount, 0, 0, 0, 0);
                break;
            case "pepper":
                addedIngredients = new PizzaIngredients(0, 0, 0, 0, amount, 0, 0, 0);
                break;
            case "mushroom":
                addedIngredients = new PizzaIngredients(0, 0, 0, 0, 0, amount, 0, 0);
                break;
            case "chicken":
                addedIngredients = new PizzaIngredients(0, 0, 0, 0, 0, 0, amount, 0);
                break;
            case "pineapple":
                addedIngredients = new PizzaIngredients(0, 0, 0, 0, 0, 0, 0, amount);
                break;
            default:
                Debug.Log("ingredients bulunamadý");
                break;
        }
        gesamtIngredients += addedIngredients;
        gesamtCost += costAmount * amount;
        ChangeTotalCostText();
    }

    void ChangeTotalCostText()
    {
        TotalText.text = gesamtCost.ToString();
    }

    public void ExitMarket()
    {
        paymentPanel.SetActive(false);
    }

    public void GoToPayment()
    {
        paymentPanel.SetActive(true);
        paymentPanel.GetComponent<PaymentPanel>().SetPaymentPanel(gesamtCost,gesamtIngredients);

    }
}
