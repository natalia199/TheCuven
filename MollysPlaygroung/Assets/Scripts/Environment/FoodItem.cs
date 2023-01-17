using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    [SerializeField] Transform player;

    void Start()
    {
        Physics.IgnoreCollision(player.GetComponent<Collider>(), GetComponent<Collider>(), true);
    }
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
