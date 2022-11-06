using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if( inventory != null)
        {
            inventory.FoodCollected();
            gameObject.SetActive(false);
        }
    }
}
