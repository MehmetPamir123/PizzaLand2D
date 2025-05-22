using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PaymentPanel : MonoBehaviour
{
    public static bool isOrderAvailable;

    public GameData gameData;
    public GameObject paymentPanel;
    public Transform deliverySelectTransform;
    public Transform[] buttonTransforms;
    public GameObject button_Coupon;
    public GameObject errorMessage;
    public float currentBalance = 0;
    float currentDeliveryCost = 0;
    float deliveryTime;

    float shoppingAmount = 0;
    float couponAmount = 0;

    float newBalance = 0;

    public TMP_Text text_shopping;
    public TMP_Text text_delivery;
    public TMP_Text text_coupon;
    public TMP_Text text_total;
    public TMP_Text text_currentBalance;
    public TMP_Text text_newBalance;
    PizzaIngredients gesamtIngredients;
    private void OnEnable()
    {
        ChangeDeliveryTime(2);
    }
    public void SetPaymentPanel(float ShoppingAmount,PizzaIngredients ings)
    {
        
        gesamtIngredients = ings;
        currentBalance = GameData.Money;
        newBalance = currentBalance;
        text_currentBalance.text = currentBalance.ToString();
        couponAmount = 0;
        shoppingAmount = ShoppingAmount;
        ChangeDeliveryTime(2);
    }

    public void ChangeDeliveryTime(int a)
    {
        switch (a)
        {
            case 0:
                currentDeliveryCost = 10;
                deliveryTime = UnityEngine.Random.Range(10,15);

                break;
            case 1:
                currentDeliveryCost = 4.25f;
                deliveryTime = UnityEngine.Random.Range(60, 90);
                break;
            case 2:
                currentDeliveryCost = 0;
                deliveryTime = UnityEngine.Random.Range(120, 180);
                break;
            default:
                Debug.LogError(a);
                break;

        }
        deliverySelectTransform.position = buttonTransforms[a].position;
        UpdateCost();
    }
    void UpdateCost()
    {
        float gesamtCost = (shoppingAmount + currentDeliveryCost) * ((100 - couponAmount) / 100);
        text_delivery.text = currentDeliveryCost.ToString();
        text_shopping.text = shoppingAmount.ToString();
        text_coupon.text = ((shoppingAmount + currentDeliveryCost) * (-couponAmount / 100)).ToString();
        text_total.text = gesamtCost.ToString();
        newBalance = MathF.Round(currentBalance - gesamtCost,2, MidpointRounding.AwayFromZero);
        text_newBalance.text = newBalance.ToString();

        if (newBalance < 0 || gesamtCost > 50)
        {
            MadeAnOffer();
        }
        if (newBalance < 0)
        {
            text_newBalance.color = Color.red;
        }
        else
        {
            text_newBalance.color = new Color32(8, 175, 0, 255);
        }

    }
    void MadeAnOffer()
    {

    }

    public void GoBack()
    {
        paymentPanel.SetActive(false);
    }
    public void Order()
    {
        if (!isOrderAvailable)
        {
            Instantiate(errorMessage,transform);

            return;
        }
        if (newBalance < 0){ return; }
        GameData.Money = newBalance;
        isOrderAvailable = false;
        gameData.StartOrder(deliveryTime, gesamtIngredients);
        paymentPanel.SetActive(false);
    }
}
