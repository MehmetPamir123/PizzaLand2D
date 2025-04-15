using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PizzaListMaker : MonoBehaviour
{
    public PizzaActivation[] pizzaController;

    public bool isThisPizzaActivated(PizzaTypes pizzaType)
    {
        foreach (var item in pizzaController)
        {
            if(item.type == pizzaType)
            {
                return item.isActive;

            }

        }
        Debug.Log($"Pizza type {pizzaType} not found!");
        return false;
    }
    public void TryActivateThisPizza(PizzaTypes pizzaType)
    {
        if(GameData.Money >= pizzaType.unlockFee)
        {
            foreach (var item in pizzaController)
            {
                if (item.type == pizzaType)
                {
                    item.isActive = true;
                    UpdatePizzaActivations();
                    Debug.Log($"{item.type.name} has unlocked!");
                }
            }
        }
        else
        {
            Debug.Log("Dont enaugh Money!");
        }
        
    }
    public void UpdatePizzaActivations()
    {
        foreach (var item in pizzaController)
        {
            if(item.isActive == false)
            {
                item.pizzaCookingButton.enabled = false;
                item.lockedPanel.SetActive(true);
            }
            else
            {
                item.pizzaCookingButton.enabled = true;
                item.lockedPanel.SetActive(false);
            }
        }
    }
    private void Start()
    {
        foreach (var item in pizzaController)
        {
            item.UnlockFeeText.text = item.type.unlockFee.ToString();
        }
    }
    private void OnEnable()
    {
        UpdatePizzaActivations();
    }

}

[System.Serializable]
public class PizzaActivation
{
    public PizzaTypes type;
    public bool isActive;
    public GameObject lockedPanel;
    public Button pizzaCookingButton;
    public TMP_Text UnlockFeeText;
}
