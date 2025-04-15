using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OvenPanel : MonoBehaviour
{
    public PizzaTypes tutorialType;

    public GameData gameData;
    public GameObject[] ShowerArrow;

    int whichOvenWasSelected = 0;
    private void DisableToDo()
    {
        if(whichOvenWasSelected == 0) { return; }
        ShowerArrow[whichOvenWasSelected - 1].gameObject.SetActive(false);
        whichOvenWasSelected = 0;
    }
    public void OpenPizzaMenu(int selectedOven)
    {
        if(selectedOven == whichOvenWasSelected) { return; }
        if(whichOvenWasSelected == 0)
        {
            ShowerArrow[selectedOven-1].SetActive(true);
            whichOvenWasSelected = selectedOven;
        }
        else
        {
            ShowerArrow[whichOvenWasSelected - 1].SetActive(false);
            ShowerArrow[selectedOven - 1].SetActive(true);
            whichOvenWasSelected = selectedOven;
        }
    }
    public void CookThis(PizzaTypes pizzaType)
    {
        if(whichOvenWasSelected == 0) {Debug.Log(whichOvenWasSelected); return; }
        gameData.OvenStartNow(pizzaType,whichOvenWasSelected);
        DisableToDo();
    }

    public void GoBack()
    {
        DisableToDo();
        this.gameObject.SetActive(false);
        //geriDön
    }
    private void OnEnable()
    {
        //Fýrýnlarý kontrol et
    }


}
