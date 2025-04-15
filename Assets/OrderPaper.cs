using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPaper : MonoBehaviour
{
    OrderRulergameObject oRGO;
    public int orderID;

    public Order order;
    [SerializeField] float TimeLeft;

    private void OnEnable()
    {
        OrderRulergameObject.OnOrderComplete += DestroyMeQuestion;
    }

    private void OnDisable()
    {
        OrderRulergameObject.OnOrderComplete -= DestroyMeQuestion;
    }

    public void SetPapier(OrderRulergameObject ORGO,int OrderID, Order Order,Vector2 pos)
    {
        oRGO = ORGO;
        orderID = OrderID;
        order = Order;
        transform.localPosition = pos;
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
}
