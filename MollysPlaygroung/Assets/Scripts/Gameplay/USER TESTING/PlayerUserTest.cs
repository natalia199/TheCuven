using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class PlayerUserTest : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed = 5f;
    [SerializeField] float speedModifier = 1f;
    [SerializeField] float jumpForce = 8f;

    // GREED
    bool cameraSwitch = false;
    bool chipAccess = false;
    bool throwAccess = false;
    public float throwForce;
    int collectionTracker = 0;
    int thrownTracker = 0;
    public GameObject interactedChip = null;
    List<GameObject> collectedChipies = new List<GameObject>();
    List<GameObject> CameraOptions = new List<GameObject>();

    // ENVY
    bool squirtAccess = false;

    // GLUTTONY
    bool eatFood = false;
    bool vomit = false;
    int collectedFoodies = 0;

    // SLOTH
    bool withinTheLight = false;
    bool pauseForDecrease = false;
    bool gotBearTrapped = false;
    bool lifeFullyDone = false;
    int lifeSource = 100;
    public GameObject interactedBearTrap = null;
    public int trapHeight;
    bool freezePlayer = false;
    public float lifeDropSpeed;

    // LUST
    public int hitKeys = 0;
    public bool resetPosition = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (SceneManager.GetActiveScene().name == "Greed")
        {
            CameraOptions.Add(GameObject.Find("Dice_MainCamera"));
            CameraOptions.Add(GameObject.Find("Collect_MainCamera"));
        }
    }

    void Update()
    {        
        if (SceneManager.GetActiveScene().name == "Greed")
        {
            // Dice Roll baby
            if (!cameraSwitch)
            {
                CameraOptions[0].SetActive(true);
                CameraOptions[1].SetActive(false);

                // Roll Dice
                if (Input.GetKeyDown(KeyCode.P))
                {
                    GameObject.Find("Dice").GetComponent<Dice>().RollDice();
                }

                // next step
                if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().goodToGo)
                {
                    collectionTracker = 0;
                    thrownTracker = 0;
                    cameraSwitch = true;
                    GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().goodToGo = false;
                }

            }
            // Chip extravaganza
            else if (cameraSwitch)
            {
                CameraOptions[0].SetActive(false);
                CameraOptions[1].SetActive(true);

                // next step
                if (thrownTracker >= GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue)
                {
                    cameraSwitch = false;
                }

                GameObject.Find("ZoneCanvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Chips: " + collectionTracker + "/" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue;

                // Collecting chip
                if (Input.GetKeyDown(KeyCode.P))
                {
                    if (chipAccess && interactedChip != null && interactedChip.GetComponent<ChipScript>().Available && collectionTracker < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue)
                    {
                        Destroy(interactedChip.GetComponent<Rigidbody>());
                        interactedChip.transform.position = new Vector3(transform.position.x, transform.position.y + 1f + (collectedChipies.Count * 0.5f), transform.position.z);
                        interactedChip.transform.rotation = Quaternion.identity;
                        interactedChip.transform.parent = this.transform;
                        interactedChip.GetComponent<MeshCollider>().isTrigger = true;

                        interactedChip.GetComponent<ChipScript>().Available = false;

                        collectedChipies.Add(interactedChip);
                        collectionTracker++;

                        interactedChip = null;
                    }
                }
                // Disposing chip
                else if (Input.GetKeyDown(KeyCode.O))
                {
                    if (collectedChipies.Count > 0 && throwAccess)
                    {
                        collectedChipies[0].transform.parent = null;
                        collectedChipies[0].AddComponent<Rigidbody>();
                        collectedChipies[0].GetComponent<MeshCollider>().isTrigger = false;
                        collectedChipies[0].GetComponent<ChipScript>().Available = true;
                        collectedChipies[0].GetComponent<ChipScript>().throwChip(GameObject.Find("Bucket").transform.GetChild(0).position, transform.position, throwForce);

                        collectedChipies.RemoveAt(0);
                        thrownTracker++;

                        for (int i = 0; i < collectedChipies.Count; i++)
                        {
                            collectedChipies[i].transform.position = new Vector3(transform.position.x, collectedChipies[i].transform.position.y - 0.5f, transform.position.z);
                        }
                    }
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Lust")
        {
            GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Keys: " + hitKeys;
        }
        else if (SceneManager.GetActiveScene().name == "Gluttony")
        {
            GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Munched: " + collectedFoodies;

            // Eating
            if (eatFood)
            {
                collectedFoodies++;
                eatFood = false;
            }

            // Puking
            if (Input.GetKeyDown(KeyCode.P))
            {
                if(collectedFoodies > 0 && !vomit)
                {
                    GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().VomittedFood(this.transform.position);
                    collectedFoodies--;
                    vomit = true;
                }
            }
            else
            {
                vomit = false;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Envy")
        {
            // Move Horse
            if (Input.GetKey(KeyCode.P))
            {
                if (squirtAccess)
                {
                    GameObject.Find("water").GetComponent<EnvySquirter>().squirterActivated = true;

                    if (GameObject.Find("water").transform.GetChild(0).GetComponent<EnvyBullseye>().Bullseye)
                    {
                        GameObject.Find("Horse").GetComponent<EnvyHorse>().MoveYourHorse();
                    }
                }
                else
                {
                    GameObject.Find("Horse").GetComponent<EnvyHorse>().StopYourHorse();
                    GameObject.Find("water").GetComponent<EnvySquirter>().squirterActivated = false;
                }
            }
            else
            {
                GameObject.Find("Horse").GetComponent<EnvyHorse>().StopYourHorse();
                GameObject.Find("water").GetComponent<EnvySquirter>().squirterActivated = false;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Wrath")
        {

        }
        else if (SceneManager.GetActiveScene().name == "Sloth")
        {
            GameObject.Find("Canvas").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Life: " + lifeSource;

            if (!lifeFullyDone)
            {
                if (!withinTheLight && !pauseForDecrease)
                {
                    StartCoroutine("LifeDrop", lifeDropSpeed);
                }

                if (gotBearTrapped && interactedBearTrap != null)
                {
                    Destroy(interactedBearTrap.GetComponent<BoxCollider>());
                    Destroy(interactedBearTrap.GetComponent<Rigidbody>());
                    interactedBearTrap.transform.position = new Vector3(transform.position.x, transform.position.y - trapHeight, transform.position.z);
                    freezePlayer = true;
                }

                if (Input.GetKeyDown(KeyCode.P))
                {
                    if (gotBearTrapped)
                    {
                        Destroy(interactedBearTrap.gameObject);
                        interactedBearTrap = null;
                        freezePlayer = false;
                        gotBearTrapped = false;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!freezePlayer)
        {
            MovePlayer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                GetComponent<Rigidbody>().velocity = vel;
            }
        }
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 playerPos = rb.position;
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        Quaternion targetRotation;

        /*
        if (movement == Vector3.zero)
        {
            return;
        }
        else
        {
            targetRotation = Quaternion.LookRotation(movement);

            targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360 * Time.fixedDeltaTime), 0.8f);
        }*/

        rb.MovePosition(playerPos + movement * speedModifier * speed * Time.fixedDeltaTime);
        //rb.MoveRotation(targetRotation);

    }

    IEnumerator LifeDrop(int value)
    {
        lifeSource --;
        pauseForDecrease = true;

        yield return new WaitForSeconds(value);

        if (lifeSource <= 0)
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
            lifeFullyDone = true;
        }

        pauseForDecrease = false;
    }

    public void OnToTheNextLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // GREED
        if (other.tag == "Chip")
        {
            chipAccess = true;
            interactedChip = other.gameObject;
        }
        
        if (other.tag == "ChipZone")
        {
            throwAccess = true;
        }

        // ENVY
        if (other.tag == "Squirter")
        {
            Debug.Log("gang");
            squirtAccess = true;
        }
        
        // GLUTTONY
        if (other.tag == "Food")
        {
            eatFood = true;
            Destroy(other.gameObject);
        }

        // SLOTH
        if (other.tag == "Light")
        {
            withinTheLight = true;
        }
        if(other.tag == "BearTrap")
        {
            gotBearTrapped = true;
            interactedBearTrap = other.gameObject;
        }

        // LUST
        if (other.tag == "PianoKey")
        {
            if (other.gameObject.GetComponent<LustPianoKey>().activatedPianoKey && resetPosition)
            {
                hitKeys++;
            }

            resetPosition = false;
        }
        if (other.tag == "PianoJumpZone")
        {
            resetPosition = true;
        }
        if (other.tag == "PianoBase")
        {
            resetPosition = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // GREED
        if (other.tag == "Chip")
        {
            chipAccess = false;
            interactedChip = null;
        }
        
        if (other.tag == "ChipZone")
        {
            throwAccess = false;
        }

        // ENVY
        if (other.tag == "Squirter")
        {
            squirtAccess = false;
        }

        // SLOTH
        if (other.tag == "Light")
        {
            withinTheLight = false;
        }
        if (other.tag == "BearTrap")
        {
            gotBearTrapped = false;
            interactedBearTrap = null;
        }
    }
}
