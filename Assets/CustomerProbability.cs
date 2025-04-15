using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Probability", menuName = "PizzaProbability")]
public class CustomerProbability : ScriptableObject
{
    public helperCustomProbabilityhelper[] probabilities;
}



[System.Serializable]
public class helperCustomProbabilityhelper
{
    public int maxCustomerBarrier;
    public Vector2 MinMaxAmount;
    public helperPizzaProbability[] pizzaProbabilities;
}



[System.Serializable]
public class helperPizzaProbability
{
    public PizzaTypes pizzaTypes;
    public float dropChance;

}
