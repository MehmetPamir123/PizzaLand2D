using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPaper : MonoBehaviour
{
    OrderRulergameObject oRGO;
    public int orderID;

    public Order order;
    [SerializeField] float TimeLeft;

    public OrderElement orderElement;

    public Sprite okey;
    public Sprite notOkey;
    public Image sr;
    private void OnEnable()
    {
        
        OrderRulergameObject.OnOrderComplete += DestroyMeQuestion;
        GlassSquare.SomethingChangedInGlasses += isOk;
    }

    private void OnDisable()
    {
        OrderRulergameObject.OnOrderComplete -= DestroyMeQuestion;
        GlassSquare.SomethingChangedInGlasses -= isOk;
    }

    public void SetPapier(OrderRulergameObject ORGO,int OrderID, Order Order,Vector2 pos)
    {
        oRGO = ORGO;
        orderID = OrderID;
        order = Order;
        transform.localPosition = pos;
        isOk();
    }

    public void OpenOrderMenu()
    {
        oRGO.OpenOrderMenu(orderID);
    }
    public void DestroyMeQuestion(int orderId)
    {
        if(orderID == orderId)
        {
            OrderRulergameObject.OnOrderComplete -= DestroyMeQuestion;
            Destroy(this.gameObject);
        }
    }

    void isOk()
    {
        if (oRGO.CheckOrder(orderID))
        {
            sr.sprite = okey;
        }
        else
        {
            sr.sprite = notOkey;
        }
    }
}
