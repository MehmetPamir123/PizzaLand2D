using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderRulergameObject : MonoBehaviour
{
    public int maxPaper;
    public Dictionary<int, Order> orders = new Dictionary<int, Order>();

    public GameData gameData;
    public GameObject OrderPaper;

    public Transform OrderElementHolder;
    public Transform OrderPaperHolder;
    public GameObject OrderElement;
    public GameObject OrderPaperPanel;

    public GameObject ServiceButton;
    public float ServiceMoney;

    public int currentPaperAmount = 0;
    [SerializeField] int currentOrderID;

    public CustomerProbability probability;
    [SerializeField] int amountOfCreatedPapers = 0;

    public Vector2[] paperPositions;

    public static event Action<int> OnOrderComplete;

    bool isLoopPaused = false;

    public void ParaVer()
    {
        GameData.Money += 10;
    }

    private void Start()
    {
        OrderLoop();
    }
    public void OrderLoop()
    {
        helperCustomProbabilityhelper currentProbability = null;
        for (int i = 0; i < probability.probabilities.Length; i++)
        {
            if (amountOfCreatedPapers < probability.probabilities[i].maxCustomerBarrier)
            {
                currentProbability = probability.probabilities[i];
                break;
            }
        }

        float time = UnityEngine.Random.Range(currentProbability.MinMaxTime.x, currentProbability.MinMaxTime.y);
        StartCoroutine(Loop(time, currentProbability));
    }
    public IEnumerator Loop(float time, helperCustomProbabilityhelper currentProbability)
    {
        while (!isLoopPaused)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                CreateOrderPaper(currentProbability);
                OrderLoop();
                break;
            }
            yield return null;
        }


    }
    public void CreateOrderPaper(helperCustomProbabilityhelper currentProbability)
    {
        int newID = 0;
        while (true)
        {
            if(newID >= maxPaper)
            {
                Debug.Log("Max Paper limit!");
                return;
            }

            if (orders.ContainsKey(newID))
            {
                newID++;
                continue;
            }
            break;
        }
        Debug.Log(newID);
        orders.Add(newID, CreateOrder(currentProbability));
        OrderPaper oP = Instantiate(OrderPaper,OrderPaperHolder).GetComponent<OrderPaper>();

        
        oP.SetPapier(this,newID, orders[newID], paperPositions[newID]);

        currentPaperAmount++;
        amountOfCreatedPapers++;
    }
    Order CreateOrder(helperCustomProbabilityhelper currentProbability)
    {
        // ScriptableObject için instance oluþturulmasý (new yerine CreateInstance kullanýlmalý)
        Order order = ScriptableObject.CreateInstance<Order>();
        // Dinamik olarak sipariþ öðelerini ekleyebilmek için geçici liste kullanýyoruz
        List<TypeAndAmount> orderList = new List<TypeAndAmount>();

        
        // Eðer uygun probability bulunamazsa, dizinin sonuncusunu seçebiliriz
        if (currentProbability == null)
        {
            currentProbability = probability.probabilities[probability.probabilities.Length - 1];
        }

        // Belirtilen aralýktan kaç defa döneceðimizi belirliyoruz (tam sayý olmasý için RoundToInt kullandýk)
        int repeat = Mathf.RoundToInt(UnityEngine.Random.Range(currentProbability.MinMaxAmount.x, currentProbability.MinMaxAmount.y));
        for (int j = 0; j < repeat; j++)
        {
            float randomNumber = UnityEngine.Random.Range(0, 101);
            // Aðýrlýklý seçim için pizzaProbabilities üzerinden geçiyoruz
            for (int i = 0; i < currentProbability.pizzaProbabilities.Length; i++)
            {
                if (randomNumber < currentProbability.pizzaProbabilities[i].dropChance)
                {
                    PizzaTypes chosenType = currentProbability.pizzaProbabilities[i].pizzaTypes;
                    bool found = false;
                    // Sipariþ listesinde bu pizza tipinin olup olmadýðýný kontrol ediyoruz
                    for (int k = 0; k < orderList.Count; k++)
                    {
                        if (orderList[k].pizzaType == chosenType)
                        {
                            orderList[k].amount++;
                            found = true;
                            break;
                        }
                    }
                    // Eðer sipariþte bu pizza tipine ait eleman yoksa, yeni eleman ekliyoruz
                    if (!found)
                    {
                        TypeAndAmount newElement = new TypeAndAmount();
                        newElement.pizzaType = chosenType;
                        newElement.amount = 1;
                        // Burada newElement.money'ya ilgili deðeri atayabilirsiniz.
                        // Örneðin, fiyat bilgisini sabit ya da currentProbability içinden alabilirsiniz.
                        newElement.money = 0; // veya uygun fiyat deðeri
                        orderList.Add(newElement);
                    }
                    // Bir pizza tipi seçildikten sonra döngüden çýkýyoruz
                    break;
                }
                else
                {
                    randomNumber -= currentProbability.pizzaProbabilities[i].dropChance;
                }
            }
        }

        // Listeyi diziye çevirerek order.orderElement'e atýyoruz
        order.orderElement = orderList.ToArray();


        return order;
    }




    public bool CheckOrder(int OrderID)
    {
        Order currentOrder = orders[OrderID];
        for (int i = 0; i < currentOrder.orderElement.Length; i++)
        {
            if(currentOrder.orderElement[i].amount > gameData.GetPizzaAmount(currentOrder.orderElement[i].pizzaType))
            {
                return false;
            }
        }
        return true;
    }
    public void CompleteOrder(int OrderID)
    {
        if(OrderID == -2) { OrderID = currentOrderID; }
        if (!CheckOrder(OrderID)) { return; }
        for (int i = 0; i < orders[OrderID].orderElement.Length; i++)
        {
            TypeAndAmount elm = orders[OrderID].orderElement[i];
            gameData.ComplateOrder(elm.pizzaType,elm.amount);
        }
        currentPaperAmount--;
        orders.Remove(OrderID);
        CloseOrderMenu();
        OnOrderComplete?.Invoke(OrderID);
    }

    public void CloseOrderMenu()
    {
        OrderPaperPanel.SetActive(false);
        if (OrderElementHolder.childCount <= 0) { return; }


        for (int i = 0; i < OrderElementHolder.childCount; i++)
        {
            Destroy(OrderElementHolder.GetChild(i).gameObject);
        }
    }

    public void OpenOrderMenu(int OrderID)
    {
        CloseOrderMenu();

        Order order = orders[OrderID];
        currentOrderID = OrderID;

        for (int i = 0; i < order.orderElement.Length; i++)
        {
            OrderElement elm = Instantiate(OrderElement, OrderElementHolder).GetComponent<OrderElement>();
            elm.MakeTheGivenOrder(order.orderElement[i].pizzaType, order.orderElement[i].amount, gameData.GetPizzaAmount(order.orderElement[i].pizzaType));
        }
        OrderPaperPanel.SetActive(true);
    }


}
