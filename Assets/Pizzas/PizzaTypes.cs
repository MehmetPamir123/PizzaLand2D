using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName ="New Pizza", menuName = "Pizza")]
public class PizzaTypes : ScriptableObject
{
    public string pizzaName;
    public bool isLocked;
    public int unlockFee;
    public float timeSeconds;
    
    public PizzaIngredients ingrediens;


    public Sprite spritePizza;
    public Sprite spriteSlice;

    public float SliceIncome;
    public int SliceAmount;

}


[System.Serializable]
public class PizzaIngredients
{
    public int Dough;
    public int Souce;
    public int Cheese;

    public int Sausage;
    public int Pepper;
    public int Mushroom;
    public int Chicken;
    public int Pineapple;

    // Bu sýnýf için baþlangýç deðerlerini tanýmla
    public PizzaIngredients(int dough, int souce, int cheese, int sausage, int pepper, int mushroom, int chicken, int pineapple)
    {
        Dough = dough;
        Souce = souce;
        Cheese = cheese;
        Sausage = sausage;
        Pepper = pepper;
        Mushroom = mushroom;
        Chicken = chicken;
        Pineapple = pineapple;
    }
    public static PizzaIngredients operator +(PizzaIngredients a, PizzaIngredients b)
    {
        return new PizzaIngredients(
            a.Dough + b.Dough,
            a.Souce + b.Souce,
            a.Cheese + b.Cheese,
            a.Sausage + b.Sausage,
            a.Pepper + b.Pepper,
            a.Mushroom + b.Mushroom,
            a.Chicken + b.Chicken,
            a.Pineapple + b.Pineapple
        );
    }
}