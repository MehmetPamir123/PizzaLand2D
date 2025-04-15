using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryPanel : MonoBehaviour
{
    public Transform ParentObject;
    public GameData data;
    public void UpdateVeriables()
    {
        //EVENT KULLANARAK BUNU HALLET

        ParentObject.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Dough}/100";
        ParentObject.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Souce}/100";
        ParentObject.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Cheese}/100";
        ParentObject.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Sausage}/100";
        ParentObject.GetChild(4).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Pepper}/100";
        ParentObject.GetChild(5).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Mushroom}/100";
        ParentObject.GetChild(6).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Chicken}/100";
        ParentObject.GetChild(7).GetChild(1).GetComponent<TMP_Text>().text = $"{data.myIngredients.Pineapple}/100";

    }
    private void OnEnable()
    {
        UpdateVeriables();
    }
    private void Update()
    {
        UpdateVeriables();
    }
}
