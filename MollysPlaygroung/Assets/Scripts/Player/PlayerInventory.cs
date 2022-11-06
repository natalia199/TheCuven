using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerInventory : MonoBehaviour
{

    public int NumFoodCollected { get; private set; }

    public UnityEvent<PlayerInventory> onFoodCollection;

    public void FoodCollected()
    {
        NumFoodCollected++;
        onFoodCollection.Invoke(this);
    }
}
