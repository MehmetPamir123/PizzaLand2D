using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public class GameData : MonoBehaviour
{
    public int maxPizzaSlices;
    public int maxInvSpace;
    public Dictionary<PizzaTypes, int> Inventory = new Dictionary<PizzaTypes, int>();

    public GameObject OvenTab;
    private static float _money; // Asýl deðiþken
    public static float Money // Property
    {
        get => _money;
        set
        {
            if (_money != value) // Eðer deðer deðiþmiþse
            {
                _money = value;
                UpdateMoney(); // Fonksiyonu çaðýr
            }
        }
    }
    public UnityEvent pizzaTakeEvent;
    public PizzaIngredients myIngredients = new PizzaIngredients(60,30,0,0,0,0,0,0);

    public GameObject[] ovens; // Fýrýnlarý tutan dizi
    public bool[] isOvenActive; // Fýrýnlarýn aktifliðini kontrol eden dizi
    public Sprite UIMask;
    public Sprite ovenFree;
    public Sprite ovenWorking;
    
    public GameObject smallTab;
    public GlassSquare[] Glasses;
    public Transform selectedGlass;
    public int selectedGlassNumber;
    public static int TotalCustomersHaveCame = 0;

    [SerializeField] private TextMeshProUGUI moneyText; // TextMeshPro referansý

    private static GameData instance; // Singleton mantýðý ile eriþim

    public GameObject GoturUstPanel;

    private void Awake()
    {
        instance = this; // Singleton referansýný al
    }
    static void UpdateMoney()
    {
        //Debug.Log("Money Changed! New value: " + _money);
        if (instance != null && instance.moneyText != null)
        {
            Money = MathF.Round(Money, 2, MidpointRounding.AwayFromZero);
            instance.moneyText.text = _money.ToString();
        }
    }

    void Start()
    {
        // Fýrýnlarýn durumlarýný takip eden dizi, baþlangýçta hepsi aktif (false) olacak
        isOvenActive = new bool[ovens.Length];
        selectedGlassNumber = -1;
        selectedGlass.position = new Vector2(10000, 10000);
        PaymentPanel.isOrderAvailable = true;
    }
    public bool CheckInventoryHasSpace(PizzaTypes pizzaType)
    {
        if (Inventory.ContainsKey(pizzaType))
        {
            return true;
        }
        else if(Inventory.Count >= maxInvSpace)
        {
            Debug.Log("Inventory is full!");
            return false;
        }
        return true;
    }
    public void InventoryAdd(PizzaTypes pizzaType, int amount)
    {
        if (!CheckInventoryHasSpace(pizzaType)) { return; }
        if (!Inventory.ContainsKey(pizzaType))
        {
            Inventory[pizzaType] = 0; // Yeni pizza türünü 0 ile baþlatýyoruz
        }
        int currentCount;

        // currentCount will be zero if the key id doesn't exist..
        Inventory.TryGetValue(pizzaType, out currentCount);


        if(currentCount + amount >= maxPizzaSlices)
        {
            Inventory[pizzaType] = maxPizzaSlices;
        }
        else
        {
            Inventory[pizzaType] = currentCount + amount;

        }

        
    }
    public bool HasInventoryThis(PizzaTypes pizzaType, int amount)
    {
        if (!Inventory.ContainsKey(pizzaType))
        {
            return false;
        }

        if (Inventory[pizzaType] >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void InventoryRemove(PizzaTypes pizzaType,int amount)
    {
        if (Inventory.ContainsKey(pizzaType))
        {
            int currentCount;
            Inventory.TryGetValue(pizzaType, out currentCount);
            currentCount -= amount;
            if (currentCount <= 0)
            {
                Inventory.Remove(pizzaType);
            }
            else
            {
                Inventory[pizzaType] = currentCount;
            }

        }
        else
        {
            Debug.Log($"Inventory has not such item {pizzaType}");
        }
    }

    public void OvenStartNow(PizzaTypes pizzaType, int whichOvenWasSelected)
    {
        StartCoroutine(OvenStart(pizzaType, whichOvenWasSelected));
    }
    // Fýrýn bekleme süresi için Coroutine
    IEnumerator OvenStart(PizzaTypes pizzaType, int whichOvenWasSelected)
    {
        Debug.Log($"pizzaType.name , {whichOvenWasSelected}");

        if (!AreIngredientsSufficient(pizzaType.ingrediens))
        {

            Debug.Log("Ingredients EKSIK!");
            yield break;
        }
        SubtractIngredients(pizzaType.ingrediens);
        ovens[whichOvenWasSelected - 1].GetComponent<Button>().enabled = false;
        ovens[whichOvenWasSelected - 1].GetComponent<Image>().sprite = ovenWorking;
        Image img = ovens[whichOvenWasSelected - 1].transform.GetChild(0).GetComponent<Image>();
        img.sprite = pizzaType.spritePizza;
        isOvenActive[whichOvenWasSelected-1] = true; // Fýrýn aktif hale getirildi
        Debug.Log($"Fýrýn {whichOvenWasSelected} aktive edildi. {pizzaType.timeSeconds} saniye bekleniyor...");

        // 5 saniye bekle
        for (int i = 0; i < pizzaType.timeSeconds; i++)
        {
            yield return new WaitForSeconds(1);
        }

        ovens[whichOvenWasSelected - 1].GetComponent<Button>().enabled = true;
        ovens[whichOvenWasSelected - 1].GetComponent<Image>().sprite = ovenFree;
        img.sprite = UIMask;
        InventoryAdd(pizzaType,pizzaType.SliceAmount);
        isOvenActive[whichOvenWasSelected-1] = false; // Fýrýn yeniden aktif hale getirildi
        ovens[whichOvenWasSelected - 1].GetComponent<Button>().enabled = true;
        Debug.Log($"Fýrýn {whichOvenWasSelected} yeniden aktif.");
        smallTab.GetComponent<InventoryShowcase>().UpdateInventory();
    }

    public bool AreIngredientsSufficient(PizzaIngredients requiredIngredients)
    {
        // Tüm malzemeleri kontrol et
        if (myIngredients.Dough < requiredIngredients.Dough) return false;
        if (myIngredients.Souce < requiredIngredients.Souce) return false;
        if (myIngredients.Cheese < requiredIngredients.Cheese) return false;
        if (myIngredients.Sausage < requiredIngredients.Sausage) return false;
        if (myIngredients.Pepper < requiredIngredients.Pepper) return false;
        if (myIngredients.Mushroom < requiredIngredients.Mushroom) return false;
        if (myIngredients.Chicken < requiredIngredients.Chicken) return false;
        if (myIngredients.Pineapple < requiredIngredients.Pineapple) return false;

        // Tüm malzemeler yeterli ise
        return true;
    }

    void SubtractIngredients(PizzaIngredients requiredIngredients)
    {
        myIngredients.Dough -= requiredIngredients.Dough;
        myIngredients.Souce -= requiredIngredients.Souce;
        myIngredients.Cheese -= requiredIngredients.Cheese;
        myIngredients.Sausage -= requiredIngredients.Sausage;
        myIngredients.Pepper -= requiredIngredients.Pepper;
        myIngredients.Mushroom -= requiredIngredients.Mushroom;
        myIngredients.Chicken -= requiredIngredients.Chicken;
        myIngredients.Pineapple -= requiredIngredients.Pineapple;
    }

    public void OpenOvenTab()
    {
        OvenTab.SetActive(true);
    }

    public void OpenSmallInv(int i)
    {
        Debug.Log("OpenInvSmall");
        if (selectedGlassNumber == -1)
        {
            smallTab.SetActive(true);
        }
        selectedGlassNumber = i-1;
        selectedGlass.position = Glasses[selectedGlassNumber].transform.position;
    }
    public void CloseSmallInv()
    {
        smallTab.SetActive(false);
        selectedGlassNumber = -1;
        selectedGlass.position = new Vector2(10000, 10000);
    }


    
    public void PutItemToGlass(PizzaTypes type)
    {
        if(type == null) { return; }
        Debug.Log(selectedGlassNumber);
        GlassSquare selectedGlass = Glasses[selectedGlassNumber];
        int itemAmount = Inventory[type];

        if (selectedGlass.iHaveItem)
        {
            if (selectedGlass.pizzaType != type)
            {
                Debug.Log(selectedGlass.pizzaType);
                Debug.Log(type);
                Debug.Log("Cant put this item");
                return;
            }
        }
        int addable = selectedGlass.HowManyCanIAdd();
        if (itemAmount > addable)
        {
            itemAmount = addable;
        }

        InventoryRemove(type, itemAmount);

        selectedGlass.AddToGlass(itemAmount,type);
        selectedGlass.iHaveItem = true;

    }

    public void TrashTheGlass()
    {
        GlassSquare selectedGlass = Glasses[selectedGlassNumber];
        if (!selectedGlass.iHaveItem) { return; }
        selectedGlass.Trash();
    }

    public int GetPizzaAmount(PizzaTypes pizzaType)
    {
        int returnedValue = 0;
        for (int i = 0; i < Glasses.Length; i++)
        {
            if (Glasses[i].pizzaType != pizzaType) { continue; }
            returnedValue += Glasses[i].amount;
        }

        return returnedValue;
    }

    public void ComplateOrder(PizzaTypes pizzaType, int amount)
    {

        for (int i = 0; i < Glasses.Length; i++)
        {
            if (Glasses[i].pizzaType != pizzaType) { continue; }
            amount = Glasses[i].TryEatThatMuch(amount);
            if(amount < 0) { Debug.Log("HATA! amount < 0????"); return; }
            if(amount == 0) { return; }
        }
    }

    public void AddIngredients(PizzaIngredients ings)
    {
        myIngredients += ings;
    }


    PizzaIngredients orderHolder;
    public void StartOrder(float time, PizzaIngredients ings)
    {
        orderHolder = ings;
        GoturUstPanel.GetComponent<GoturPanel>().timeLeft = time;
        GoturUstPanel.SetActive(true);
    }
    public void OrderArrived()
    {
        myIngredients += orderHolder;
        orderHolder = null;
        PaymentPanel.isOrderAvailable = true;
    }
}

