using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryShowcase : MonoBehaviour
{
    public Transform selectingObjectTransform;
    int selectedElement = 0;
    public GameObject[] inventoryElements;
    public GameData gameData;
    public Sprite UIMask;
    public PizzaTypes[] inv;
    PizzaTypes[] holderTypes;

    public Button putButton;
    public Button takeBackButton;
    public Button trashButton;

    public float deactivatedAlpha;
    public void UpdateInventory()
    {
        int itemSayisi = 0;
        for (int i = 0; i < inv.Length; i++)
        {
            if (gameData.Inventory.ContainsKey(inv[i]))
            {
                inventoryElements[itemSayisi].transform.GetChild(1).GetComponent<Image>().sprite = inv[i].spriteSlice;
                inventoryElements[itemSayisi].transform.GetChild(0).GetComponent<TMP_Text>().text = gameData.Inventory[inv[i]].ToString();
                holderTypes[itemSayisi] = inv[i];
                itemSayisi++;
            }
        }
        //Debug.Log(" : "+holderTypes.Length);
        for (int ii = holderTypes.Length; ii > itemSayisi; ii--)
        {
            //Debug.Log(ii-1);
            holderTypes[ii-1] = null;
            inventoryElements[ii-1].transform.GetChild(1).GetComponent<Image>().sprite = UIMask;
            inventoryElements[ii-1].transform.GetChild(0).GetComponent<TMP_Text>().text = "";

        }
    }
    private void Awake()
    {
        holderTypes = new PizzaTypes[inventoryElements.Length];

    }
    void Update()
    {
        //Sürekli update ediyor eventlerle yap.

    }
    private void OnEnable()
    {
        //Debug.Log(holderTypes.Length);
        UpdateInventory();
        RestartSelecting();
        SelectNewItem(-1);

    }


    void RestartSelecting()
    {
        selectedElement = -1;
        selectingObjectTransform.position = new Vector3(10000, 10000, selectingObjectTransform.position.z);
    }
    public void SelectNewItem(int elementNumber)
    {
        Debug.Log(elementNumber);
        if (selectedElement == elementNumber)
        {
            return;

        }
        else
        {
            selectedElement = elementNumber;
            selectingObjectTransform.position = inventoryElements[selectedElement].transform.position;

        }
        ButtonColorEditor();
    }
    void ButtonColorEditor()
    {
        Image img;
        if (selectedElement == -1)
        {
            img = putButton.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
            img = trashButton.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0.5f);
        }
        else
        {
            img = putButton.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            img = trashButton.GetComponent<Image>();
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
        }
    }

    public void TrashTheItem()
    {
        gameData.TrashTheGlass();
        UpdateInventory();
    }
    public void PutTheItem()
    {
        if(selectedElement == -1 || holderTypes[selectedElement] == null) { return; }
        gameData.PutItemToGlass(holderTypes[selectedElement]);
        UpdateInventory();
    }

}