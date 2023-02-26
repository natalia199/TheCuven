using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System;

public class PlayerUserTest : MonoBehaviour
{
    Rigidbody rb;
    PhotonView view;

    public TextMeshProUGUI username;
    string playersUsername;

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

    public Vector3 chipPosition;
    public Quaternion chipRotation;
    public GameObject theChip;
    public bool instantiateChipOnce = false;

    bool noMoreChipsNeeded = false;
    bool oneChipAtATimeThorw = false;
    bool oneChipAtATimeCarry = false;

    public GameObject carriedChip = null;
    public bool throwChipAcces = false;

    // ENVY
    bool squirtAccess = false;
    public string horseName;
    public string squirtGunName;
    public GameObject squirtGun;

    // GLUTTONY
    bool eatFood = false;
    bool vomit = false;
    public int collectedFoodies = 0;
    public Vector3 foodPosition;
    public GameObject theFood;
    public GameObject theVomittedFood;
    public bool instantiateFoodOnce = false;
    public GameObject interactedFood = null;

    public bool gotPunched = false;

    // SLOTH
    bool withinTheLight = false;
    bool pauseForDecrease = false;
    bool gotBearTrapped = false;
    bool lifeFullyDone = false;
    public float lifeSource = 100;
    public GameObject interactedBearTrap = null;
    public int trapHeight;
    bool freezePlayer = false;
    public float lifeDropSpeed;

    public Vector2 lightPosition;
    public GameObject theLight;
    public bool instantiateLightOnce = false;

    public Vector2 trapPosition;
    public GameObject theTrap;
    public bool instantiateTrapOnce = false;

    public float timeRemaining;
    public bool timerIsRunning = false;

    // LUST
    public int hitKeys = 0;
    public int selectedKey;
    public bool resetPosition = true;
    public bool readyForNewKey = true;
    public bool hitKeyScore = false;
    public bool landedOnFloor = false;

    // WRATH
    int directionIndex = 0;
    bool directionChosen = false;
    public bool pickUpBox = false;
    GameObject interactedBox = null;
    public GameObject carriedBox = null;
    public bool plateMoving = false;
    public int boxThrowForce;
    public int boxScore;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        if (SceneManager.GetActiveScene().name == "Greed")
        {
            CameraOptions.Add(GameObject.Find("Dice_MainCamera"));
            CameraOptions.Add(GameObject.Find("Collect_MainCamera"));
        }
        else if (SceneManager.GetActiveScene().name == "Envy")
        {
            if (view.IsMine)
            {
                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        horseName = "Horse" + i;
                        squirtGunName = "SquirtGun" + i;
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;

        if (view.IsMine)
        {
            view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState)
                {
                    GameObject.Find("Scene Manager").GetComponent<SceneManage>().NextGameaz();
                }
            }

            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
            {
                if (SceneManager.GetActiveScene().name == "Greed")
                {
                    // Single player: quickly collect as many chips as possible before the time runs out
                    // Multi player: collect the most chips before the time runs out

                    if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().AmountOfChips == GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform.childCount)
                    {
                        noMoreChipsNeeded = true;
                    }

                    // Instantiation
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (!noMoreChipsNeeded && GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipReady && theChip == null && GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().AmountOfChips != GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform.childCount)
                        {
                            if (!instantiateChipOnce)
                            {
                                float xPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipSpawnPoints[0].position.x, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipSpawnPoints[2].position.x);
                                float zPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipSpawnPoints[0].position.z, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipSpawnPoints[1].position.z);
                                chipPosition = new Vector3(xPos, 12, zPos);
                                chipRotation = new Quaternion(UnityEngine.Random.Range(15, -15), 0, UnityEngine.Random.Range(15, -15), GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipPrefab.transform.rotation.w);
                                instantiateChipOnce = true;
                            }

                            view.RPC("setChipPosition", RpcTarget.AllBufferedViaServer, view.Owner.NickName, chipPosition, chipRotation);
                        }
                    }


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

                        if (chipAccess)
                        {
                            view.RPC("chipInteractionActive", RpcTarget.AllBufferedViaServer, view.Owner.NickName, interactedChip.name);
                        }
                        else
                        {
                            view.RPC("chipInteractionDeactive", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                        }

                        // Collecting chip
                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            /*
                            if (interactedChip != null && interactedChip.GetComponent<ChipScript>().Available && collectionTracker < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue)
                            {
                                
                                //view.RPC("carryChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, carriedChipPos, carriedChipRot);

                                view.RPC("pickUpTheChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                            }
                            }*/

                            if (!oneChipAtATimeCarry && chipAccess && interactedChip != null && interactedChip.GetComponent<ChipScript>().Available && collectionTracker < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue)
                            {
                                Vector3 carriedChipPos = new Vector3(transform.position.x, transform.position.y + 1f + (collectedChipies.Count * 0.5f), transform.position.z);
                                Quaternion carriedChipRot = Quaternion.identity;
                                oneChipAtATimeCarry = true;
                                view.RPC("carryChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, carriedChipPos, carriedChipRot);
                            }
                            oneChipAtATimeThorw = false;
                        }
                        // Disposing chip
                        else if (Input.GetKeyDown(KeyCode.O))
                        {
                            /*
                            if (collectedChipies.Count > 0 && throwChipAcces)
                            {
                                view.RPC("throwChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, throwForce);
                            }
                            */

                            if (collectedChipies.Count > 0 && throwAccess && !oneChipAtATimeThorw)
                            {
                                view.RPC("throwChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, throwForce);
                                oneChipAtATimeThorw = true;
                            }
                            oneChipAtATimeCarry = false;
                        }
                        else
                        {
                            view.RPC("chipAcess", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                            oneChipAtATimeCarry = false;
                            oneChipAtATimeThorw = false;
                        }
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Lust")
                {
                    // Single player: get as many correct keys as possible
                    // Multi player: get the most correct keys

                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (GameObject.Find("GameManager").GetComponent<LustGameplayManager>().newKey)
                        {
                            if (readyForNewKey)
                            {
                                int i = UnityEngine.Random.Range(0, 4);

                                // if index was already chosen or the key is being pressed on already don't do anything
                                if (i == selectedKey || GameObject.Find("GameManager").GetComponent<LustGameplayManager>().pianoKeys[i].GetComponent<LustPianoKey>().keyPressed)
                                {
                                    readyForNewKey = true;
                                }
                                else
                                {
                                    readyForNewKey = false;
                                    selectedKey = i;
                                }
                            }
                            else
                            {
                                view.RPC("setPianoKey", RpcTarget.AllBufferedViaServer, selectedKey);
                            }
                        }
                    }

                    if (hitKeyScore)
                    {
                        hitKeys++;
                        hitKeyScore = false;
                        view.RPC("keyScoreDisplay", RpcTarget.AllBufferedViaServer, view.Owner.NickName, hitKeys);
                    }

                }
                else if (SceneManager.GetActiveScene().name == "Gluttony")
                {
                    // Single player: Collect as many munchies as possible before the time runs out
                    // Multi player: Collect more mucnhies than the other before the time runs out, can sabotage other by hitting them with a mallet and emptying their munchies

                    // Instantiation
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().foodReady && theFood == null && GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood != GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform.childCount)
                        {
                            if (!instantiateFoodOnce)
                            {
                                float xPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[0].position.x, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[2].position.x);
                                float zPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[0].position.z, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[1].position.z);
                                foodPosition = new Vector3(xPos, 12f, zPos);
                                instantiateFoodOnce = true;
                            }

                            view.RPC("setFoodPosition", RpcTarget.AllBufferedViaServer, foodPosition, view.Owner.NickName);
                        }
                    }

                    // Eating
                    if (eatFood)
                    {
                        view.RPC("eatUpFood", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                        eatFood = false;
                        view.RPC("muchiesScore", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                    }

                    // MULTI-PLAYER
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
                    {
                        // when player gets hit by a bomb or another player hits them w hammer
                        // Puking
                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            if (collectedFoodies > 0 && !vomit)
                            {
                                /*
                                Vector3 vomitedPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                Vector3 vomitedDirection = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(3, 5), UnityEngine.Random.Range(-5, 5));

                                Vector3 direction = vomitedDirection;
                                direction = direction.normalized;

                                view.RPC("setVomitPosition", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vomitedPos, direction, 300f);
                                collectedFoodies--;
                                */
                            collectedFoodies = 0;
                                view.RPC("muchiesScore", RpcTarget.AllBufferedViaServer, view.Owner.NickName, collectedFoodies);

                                vomit = true;
                            }
                        }
                        else
                        {
                            vomit = false;
                        }
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Envy")
                {
                    // Single player:
                    // Mutli player: cross your horse over the finish line before the other

                    // assigning horses
                    for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                        {
                            horseName = "Horse" + i;
                            squirtGunName = "SquirtGun" + i;
                        }
                    }

                    // Move Horse
                    if (Input.GetKey(KeyCode.P))
                    {
                        if (squirtAccess && squirtGun != null)
                        {
                            if (squirtGun.name == squirtGunName)
                            {
                                view.RPC("increaseSquirt", RpcTarget.AllBufferedViaServer, squirtGunName);

                                if (GameObject.Find(squirtGunName).transform.GetChild(0).GetChild(0).GetComponent<EnvyBullseye>().Bullseye)
                                {
                                    view.RPC("moveHorsey", RpcTarget.AllBufferedViaServer, horseName);
                                }
                            }
                        }
                        else
                        {
                            view.RPC("stopHorsey", RpcTarget.AllBufferedViaServer, horseName);
                            view.RPC("decreaseSquirt", RpcTarget.AllBufferedViaServer, squirtGunName);
                        }
                    }
                    else
                    {
                        view.RPC("stopHorsey", RpcTarget.AllBufferedViaServer, horseName);
                        view.RPC("decreaseSquirt", RpcTarget.AllBufferedViaServer, squirtGunName);
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Wrath")
                {
                    // Single player: quickly drop all the boxes off of the platform 
                    // Multi player: be the last standing on the platform

                    if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().SingleOrMultiPlayer)
                    {           
                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            if (interactedBox != null && carriedBox == null)
                            {
                                pickUpTheBox();
                            }
                        }
                        else if (Input.GetKeyDown(KeyCode.O))
                        {
                            if(carriedBox != null)
                            {
                                dropTheBox();
                            }
                        }
                    }

                    // MULTI-PLAYER
                    else
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            if (GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().plateState)
                            {
                                if (!plateMoving)
                                {
                                    directionIndex = UnityEngine.Random.Range(0, 3);
                                    plateMoving = true;
                                }
                                else
                                {
                                    view.RPC("shakePlatform", RpcTarget.AllBufferedViaServer, directionIndex, view.Owner.NickName);
                                }
                            }
                        }
                    }                
                }
                else if (SceneManager.GetActiveScene().name == "Sloth")
                {
                    // Single Player: Stay within the light as much as possible before the time runs up
                    // Multi Player: Be the last one standing before your life source reaches 0

                    // Instantiation
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().lightReady && theLight == null)
                        {
                            if (!instantiateLightOnce)
                            {
                                float xPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[0].position.x, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[2].position.x);
                                float zPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[0].position.z, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[1].position.z);
                                lightPosition = new Vector2(xPos, zPos);
                                instantiateLightOnce = true;
                            }

                            view.RPC("setLightPosition", RpcTarget.AllBufferedViaServer, lightPosition, view.Owner.NickName);
                        }

                        if (GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().trapReady && theTrap == null && GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().AmountOfTraps != GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapParent.transform.childCount)
                        {
                            if (!instantiateTrapOnce)
                            {
                                float xPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[0].position.x, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[2].position.x);
                                float zPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[0].position.z, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapSpawnPoints[1].position.z);
                                trapPosition = new Vector2(xPos, zPos);
                                instantiateTrapOnce = true;
                            }

                            view.RPC("setTrapPosition", RpcTarget.AllBufferedViaServer, trapPosition, view.Owner.NickName);
                        }
                    }

                    // Actions
                    if (!lifeFullyDone)
                    {
                        if (gotBearTrapped && interactedBearTrap != null)
                        {
                            Vector3 pos = new Vector3(transform.position.x, transform.position.y - trapHeight, transform.position.z);
                            view.RPC("caughtByBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName, pos);
                        }

                        if (Input.GetKeyDown(KeyCode.P))
                        {
                            if (gotBearTrapped)
                            {
                                view.RPC("unhookedBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                            }
                        }
                    }

                    if (!withinTheLight)
                    {
                        if ((int)lifeSource <= 0)
                        {
                            lifeSource = 0;
                            lifeFullyDone = true;
                        }
                        else
                        {
                            lifeSource -= Time.deltaTime;
                        }
                    }

                    view.RPC("displayLifePercentage", RpcTarget.AllBufferedViaServer, view.Owner.NickName, (int)lifeSource);
                }

            }

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
    }

    void MovePlayer()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 playerPos = rb.position;
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical).normalized;

        Quaternion targetRotation;

        
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
        }

        rb.MovePosition(playerPos + movement * speedModifier * speed * Time.fixedDeltaTime);
        //rb.MoveRotation(targetRotation);

    }

    void throwParabola(GameObject box, Vector3 targetPos, Vector3 startPos, float force)
    {
        Vector3 dir = targetPos - startPos;
        dir = dir.normalized;
        box.GetComponent<Rigidbody>().AddForce(dir * force);
        box.GetComponent<Rigidbody>().AddTorque(dir * 50);
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

    public void shakeHer(int dir)
    {
        GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().shakePlateVariables(dir);
    }

    /// RPCs

    // Username
    [PunRPC]
    void getPlayersNickName(string name)
    {
        playersUsername = name;
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.text = Player;
            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // Levels

    // Wrath
    [PunRPC]
    void shakePlatform(int dir, string pName)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().shakePlateVariables(dir);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void pickUpTheBox()
    {
        try
        {
            if (interactedBox != null) 
            {
                carriedBox = interactedBox;
                Destroy(carriedBox.GetComponent<Rigidbody>());
                carriedBox.transform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                carriedBox.transform.parent = this.transform;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void dropTheBox()
    {
        try
        {
            carriedBox.transform.parent = null;
            carriedBox.AddComponent<Rigidbody>();

            throwParabola(carriedBox, transform.GetChild(1).position, transform.position, boxThrowForce);

            carriedBox = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // Sloth
    [PunRPC]
    void displayLifePercentage(string pName, float v)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().lifeSource = v;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void setLightPosition(Vector2 pos, string pName)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().theLight == null) 
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theLight = Instantiate(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LightPrefab, new Vector3(pos.x, -1.4f, pos.y), Quaternion.identity, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LightParent.transform);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void setTrapPosition(Vector2 pos, string pName)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().theTrap == null)
            {
                Debug.Log("imm akms");
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theTrap = Instantiate(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapPrefab, new Vector3(pos.x, 12f, pos.y), Quaternion.identity, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapParent.transform);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void caughtByBearTrap(string pName, Vector3 pos)
    {
        try
        {
            Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap.GetComponent<BoxCollider>());
            Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap.GetComponent<Rigidbody>());
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap.GetComponent<SlothObstacle>().trapSet = true;
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap.tag = "Untagged";
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap.transform.position = pos;
            GameObject.Find(pName).GetComponent<PlayerUserTest>().freezePlayer = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void unhookedBearTrap(string pName)
    {
        try
        {
            Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap.gameObject);
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap = null;
            GameObject.Find(pName).GetComponent<PlayerUserTest>().freezePlayer = false;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // GLUTTONY
    [PunRPC]
    void muchiesScore(string pName, int v)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedFoodies++;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void setFoodPosition(Vector3 pos, string pName)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().theFood == null)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theFood = Instantiate(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodPrefab, pos, Quaternion.identity, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void setVomitPosition(string pName, Vector3 pos, Vector3 dir, float force)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood == null)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood = Instantiate(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodPrefab, new Vector3(pos.x, pos.y + 1f, pos.z), Quaternion.identity, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform);

                // dir = dir.normalized;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood.AddComponent<Rigidbody>();
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood.GetComponent<Rigidbody>().AddForce(dir * force);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood.GetComponent<Rigidbody>().AddTorque(dir * 50);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void eatUpFood(string pName)
    {
        try
        {
            Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood.gameObject);
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // GREED
    [PunRPC]
    void setChipPosition(string pName, Vector3 pos, Quaternion rot)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip == null)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip = Instantiate(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipPrefab, new Vector3(pos.x, pos.y, pos.z), rot, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void carryChip(string pName, Vector3 pos, Quaternion rot)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip != null)
            { 
                Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.GetComponent<Rigidbody>());
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.GetComponent<MeshCollider>().isTrigger = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.transform.position = pos;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.transform.rotation = rot;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.transform.parent = GameObject.Find(pName).transform;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.GetComponent<ChipScript>().Available = false;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Add(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip = null;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectionTracker++;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void chipAcess(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces = false;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void throwChip(string pName, float f)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Count > 0 && !GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces) 
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].transform.parent = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].AddComponent<Rigidbody>();
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().throwChip(GameObject.Find("Bucket").transform.GetChild(0).position, GameObject.Find(pName).transform.position, f);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().Available = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.RemoveAt(0);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().thrownTracker++;

                for (int i = 0; i < GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Count; i++)
                {
                    GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[i].transform.position = new Vector3(GameObject.Find(pName).transform.position.x, GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[i].transform.position.y - 0.5f, GameObject.Find(pName).transform.position.z);
                }

                GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces = true;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // greed testing
    [PunRPC]
    void chipInteractionActive(string pName, string chip)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip = GameObject.Find(chip).gameObject;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void chipInteractionDeactive(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void pickUpTheChip(string pName)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip != null)
            {
                Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.GetComponent<Rigidbody>());
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.GetComponent<MeshCollider>().isTrigger = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.transform.position = new Vector3(GameObject.Find(pName).transform.position.x, GameObject.Find(pName).transform.position.y + 1f, GameObject.Find(pName).transform.position.z);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.transform.rotation = Quaternion.identity;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.transform.parent = GameObject.Find(pName).transform;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip.GetComponent<ChipScript>().Available = false;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Add(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectionTracker++;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // ENVY
    [PunRPC]
    void moveHorsey(string pName)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().MoveHorseForward(pName);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void stopHorsey(string pName)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().StopHorseForward(pName);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void increaseSquirt(string pName)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().squirtWater(pName);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void decreaseSquirt(string pName)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().desquirtWater(pName);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    // LUST
    [PunRPC]
    void setPianoKey(int x)
    {
        try
        {
            if (GameObject.Find("GameManager").GetComponent<LustGameplayManager>().newKey) 
            {
                GameObject.Find("GameManager").GetComponent<LustGameplayManager>().NewKey(x);
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void resettingPianoPos(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().resetPosition = x;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void keyScoreDisplay(string pName, int k)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().hitKeys = k;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }


    public void OnToTheNextLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().CurrentLevelState = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // WRATH
        if (other.tag == "WrathBox")
        {
            pickUpBox = true;
            interactedBox = other.gameObject;
        }

        // GREED
        if (other.tag == "Chip")
        {
            chipAccess = true;
            interactedChip = other.gameObject;
        }

    }
    
    void OnTriggerEnter(Collider other)
    {
        // GREED
        
        if (other.tag == "ChipZone")
        {
            throwAccess = true;
        }

        // ENVY
        if (other.tag == "Squirter")
        {
            squirtGun = other.gameObject;
            squirtAccess = true;
        }
        
        // GLUTTONY
        if (other.tag == "Food")
        {
            eatFood = true;
            interactedFood = other.gameObject;
            //Destroy(other.gameObject);
        }

        // SLOTH
        if (other.tag == "Light")
        {
            withinTheLight = true;
        }
        if(other.tag == "BearTrap" && interactedBearTrap == null && !other.GetComponent<SlothObstacle>().trapSet)
        {
            gotBearTrapped = true;
            interactedBearTrap = other.gameObject;
        }

        // LUST
        if (other.tag == "PianoKey")
        {
            if (other.gameObject.GetComponent<LustPianoKey>().activatedPianoKey && resetPosition && !other.gameObject.GetComponent<LustPianoKey>().pressedWhenSelected)
            {
                hitKeyScore = true;
            }

            resetPosition = false;
        }
        if (other.tag == "PianoJumpZone")
        {
            resetPosition = true;
            landedOnFloor = false;
        }
        if (other.tag == "PianoBase")
        {
            resetPosition = false;
            landedOnFloor = true;
        }

        // WRATH

    }

    void OnTriggerExit(Collider other)
    {
        // WRATH
        if (other.tag == "WrathBox")
        {
            pickUpBox = false;
            interactedBox = null;
        }

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
            squirtGun = null;
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
