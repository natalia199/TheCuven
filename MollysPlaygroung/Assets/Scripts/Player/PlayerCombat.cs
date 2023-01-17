using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] Animator animator;
    public bool isPunching = false;  //Maybe create a public instance of the isPunching animation state. This will be used to access the animation state over networking since the animation controller is localized to current machine?
    public bool isPulling = false;
    public bool isDragged = false;
    public bool isPushing = false;
    [SerializeField] float dragDistance = 1.5f;
    private int isHitHash;
    PlayerInventory inventory;
    public GameObject[] foodItems;
    //I implemented this if it's needed. Not really sure if that's useful or if the animator can be accessed by PUN

    void Start()
    {
        inventory = GetComponent<PlayerInventory>();
        isHitHash = Animator.StringToHash("isHit");
    }

    // Update is called once per frame
    void Update()
    {
        isPunching = animator.GetBool("isPunching");
        isPulling = animator.GetBool("isPulling");
        isDragged = animator.GetBool("isDragged");
        isPushing = animator.GetBool("isPushing");
    }

    private void dropFood()
    {

        if (GetComponentInChildren<animationStateController>().isTestDummy)
        {
            spawnFood(3);
            return;
        }


        int numFood = inventory.NumFoodCollected;
        if (numFood != 0)
        {

            for (int i = 0; i <= 3; i++)
            {
                if (numFood < i)
                {
                    //drop all food
                    spawnFood(numFood);
                    break;
                }
                if (i == 3)
                {
                    //drop 3 food
                    spawnFood(i);
                }
            }
        }
        
    }

    private void spawnFood(int numToSpawn)
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            int foodItemNum = Random.Range(0, foodItems.Length);
            Vector3 spawnPosition = transform.position;
            spawnPosition.y += 4;
            GameObject spawnedFood = Instantiate(foodItems[foodItemNum], spawnPosition, transform.rotation);
            spawnedFood.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-5, 5), 2, Random.Range(-5, 5)), ForceMode.Impulse);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        //check if the colission is another player
        if (other.CompareTag("Player"))
        {
            //Check if the other player is punching
            PlayerCombat opponentCombatState = other.GetComponentInParent<PlayerCombat>();
            if (opponentCombatState.isPunching && !animator.GetCurrentAnimatorStateInfo(0).IsName("Vomit"))
            {
                animator.SetTrigger("isHitTrigger");
                /* --------------- vvvv GLUTTONY EXCLUSIVE ACTION vvvv ----------------- */
     
                dropFood();

                /* --------------- ^^^^ GLUTTONY EXCLUSIVE ACTION ^^^^ ----------------- */

            }
            else if(opponentCombatState.isPulling && !animator.GetCurrentAnimatorStateInfo(0).IsName("Being Dragged"))
            {
                //animate the player being dragged
                animator.SetTrigger("isDraggedTrigger");
                animator.SetBool("isDragged", true);

                //parenting to move the object with teh oponent
                Transform oppTransform = other.GetComponent<Transform>(); //transform of opponent
                Transform currentTransform = GetComponent<Transform>();
                currentTransform.SetPositionAndRotation(oppTransform.position, oppTransform.rotation);
                currentTransform.position += currentTransform.forward * dragDistance;
                currentTransform.parent = oppTransform;



            }
            else if (opponentCombatState.isPushing)
            {
                //animate the player being hit

                //push player back along direction it was hit
                Transform oppTransform = other.GetComponent<Transform>(); //transform of opponent
                Transform currentTransform = GetComponent<Transform>();
                // currentTransform.SetPositionAndRotation(currentTransform.position, oppTransform.rotation);
                // currentTransform.GetComponent<Rigidbody>().AddForce(oppTransform.forward * 5, ForceMode.Impulse);
            }
        }
    }
}
