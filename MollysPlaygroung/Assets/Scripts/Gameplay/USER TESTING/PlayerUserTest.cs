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

    public GameObject interactedOpponent = null;

    // GREED
    public bool cameraSwitch = false;
    public bool chipAccess = false;
    bool throwAccess = false;
    public float throwForce;
    public int collectionTracker = 0;
    int thrownTracker = 0;
    public int totalChipCollection = 0;
    public int diceRollValue = 0;
    public GameObject interactedChip = null;
    public List<GameObject> collectedChipies = new List<GameObject>();
    public List<GameObject> CameraOptions = new List<GameObject>();

    public Vector3 chipPosition;
    public Quaternion chipRotation;
    public GameObject theChip;
    public bool instantiateChipOnce = false;

    bool noMoreChipsNeeded = false;
    bool oneChipAtATimeThorw = false;
    public bool oneChipAtATimeCarry = false;

    public GameObject carriedChip = null;
    public bool throwChipAcces = false;

    public string bucketName;
    public string bucketNameInteracted;

    // ENVY
    public bool squirtAccess = false;
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
    public int foodInstanitationTracker;

    // SLOTH
    public bool withinTheLight = false;
    bool pauseForDecrease = false;
    public bool gotBearTrapped = false;
    public bool lifeFullyDone = false;
    public float lifeMax = 100;
    public float lifeSource = 100;
    public GameObject interactedBearTrap = null;
    public GameObject alreadySetBearTrap = null;
    public int trapHeight;
    public bool freezePlayer = false;
    public float lifeDropSpeed;

    public Vector2 lightPosition;
    public GameObject theLight;
    public bool instantiateLightOnce = false;

    public Vector2 trapPosition;
    public GameObject theTrap;
    public bool instantiateTrapOnce = false;

    public float timeRemaining;
    public bool timerIsRunning = false;

    public bool ranOutOfLife = false;

    // LUST
    public int hitKeys = 0;
    public int selectedKey;
    public bool resetPosition = true;
    //public bool readyForNewKey = true;
    public bool hitKeyScore = false;
    public bool landedOnFloor = false;

    // WRATH
    int directionIndex = 0;
    bool directionChosen = false;
    //public bool pickUpBox = false;
    GameObject interactedBox = null;
    public GameObject carriedBox = null;
    public bool plateMoving = false;
    public int boxThrowForce;
    public int boxScore;

    public bool fellOffPlatform = false;
    public bool deathRecorded = false;

    public int playerNumber = -1;

    public bool finishedSinglePlayer = false;
    public bool die = false;

    public GameObject competitor = null;

    public bool oopsyGotPushed = false;
    public bool oopsyGotHit = false;
    public bool oopsyGotDragged = false;
    public bool imDraggingMan = false;

    public bool actionPause = false;
    public bool onetimebruvidek = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            resetAllValues();
        }

        try
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                {
                    playerNumber = i;
                }
            }
        }
        catch (NullReferenceException e) { }

        if (SceneManager.GetActiveScene().name == "Greed")
        {
            CameraOptions.Add(GameObject.Find("Dice_MainCamera"));
            CameraOptions.Add(GameObject.Find("Collect_MainCamera"));

            theChip = null;
            interactedChip = null;


            if (view.IsMine)
            {
                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count; i++)
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame[i] == PhotonNetwork.LocalPlayer.NickName)
                    {
                        bucketName = "Bucket" + i;
                    }
                }
            }
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
        else if (SceneManager.GetActiveScene().name == "Sloth")
        {
            lifeSource = lifeMax;
            withinTheLight = false;
            pauseForDecrease = false;
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            // PLAYER ACTIONS - if not stunned
            if (!freezePlayer)
            {
                if (!actionPause)
                {
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        if (competitor != null && !competitor.GetComponent<PlayerUserTest>().oopsyGotPushed)
                        {
                            Vector3 direction = (competitor.transform.position - transform.position).normalized;
                            view.RPC("YouPushedThem", RpcTarget.AllBufferedViaServer, view.Owner.NickName, competitor.name, direction);
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.I))
                {
                    if (competitor != null && !competitor.GetComponent<PlayerUserTest>().oopsyGotHit)
                    {
                        view.RPC("YouHitThem", RpcTarget.AllBufferedViaServer, view.Owner.NickName, competitor.name);
                    }
                }

                if (Input.GetKey(KeyCode.P))
                {
                    if (competitor != null)
                    {
                        view.RPC("YouDraggedThem", RpcTarget.AllBufferedViaServer, view.Owner.NickName, competitor.name, true);
                    }
                }
                else
                {
                    if (competitor != null)
                    {
                        view.RPC("YouDraggedThem", RpcTarget.AllBufferedViaServer, view.Owner.NickName, competitor.name, false);
                    }
                }
            }
        }
    }

     void FixedUpdate()
    {
        // player name
        if (view.IsMine)
        {
            view.RPC("getPlayersNickName", RpcTarget.AllBufferedViaServer, PhotonNetwork.LocalPlayer.NickName);
        }
        this.name = playersUsername;

        if (view.IsMine)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "rumble")
            {
                view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

                // USERNAME SCENE - players picking their name and character skin
                if (SceneManager.GetActiveScene().name == "Username")
                {
                    for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                        {
                            try
                            {
                                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                            }

                            catch (NullReferenceException e)
                            { }
                        }
                    }

                    MovePlayer();

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                        view.RPC("jumpBoyJump", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel);
                    }
                }
                // LEVEL SCENES - players playing the level and their level actions
                else
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck)
                    {
                        for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                        {
                            try
                            {
                                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);

                            }

                            catch (NullReferenceException e)
                            {
                            }

                            try
                            {
                                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                                {
                                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].originalMesh;
                                }
                                else
                                {
                                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().deadSkin;
                                }
                            }
                            catch (NullReferenceException e)
                            {
                            }
                        }

                        if (SceneManager.GetActiveScene().name == "Greed")
                        {
                            // Single player: quickly collect as many chips as possible before the time runs out
                            // Multi player: collect the most chips before the time runs out

                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                // Dice Roll baby
                                if (!cameraSwitch)
                                {
                                    CameraOptions[0].SetActive(true);
                                    CameraOptions[1].SetActive(false);

                                    // Roll Dice
                                    if (Input.GetKeyDown(KeyCode.E))
                                    {
                                        GameObject.Find("Dice").GetComponent<Dice>().RollDice();
                                    }

                                    // next step
                                    if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().goodToGo)
                                    {
                                        collectionTracker = 0;
                                        view.RPC("resetChipTracker", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

                                        thrownTracker = 0;
                                        cameraSwitch = true;
                                        view.RPC("setDiceValue", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue);
                                        GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().goodToGo = false;
                                    }

                                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                    {
                                        if (!die && GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipTracker == GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips)
                                        {
                                            view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                        }
                                    }

                                }
                                // Chip extravaganza
                                else if (cameraSwitch)
                                {
                                    CameraOptions[0].SetActive(false);
                                    CameraOptions[1].SetActive(true);

                                    // ne
                                    if (thrownTracker >= GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue && !die)
                                    {
                                        cameraSwitch = false;
                                    }

                                    if (chipAccess && interactedChip != null)
                                    {
                                        view.RPC("chipInteractionActive", RpcTarget.AllBufferedViaServer, view.Owner.NickName, interactedChip.name);
                                    }
                                    else
                                    {
                                        view.RPC("chipInteractionDeactive", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }

                                    // Collecting chip
                                    if (Input.GetKeyDown(KeyCode.E))
                                    {
                                        if (!oneChipAtATimeCarry && interactedChip != null && interactedChip.GetComponent<ChipScript>().Available && collectionTracker < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue)
                                        {
                                            oneChipAtATimeCarry = true;
                                            Vector3 carriedChipPos = new Vector3(transform.position.x, transform.position.y + 2f + (collectedChipies.Count * 0.5f), transform.position.z);
                                            Quaternion carriedChipRot = Quaternion.identity;
                                            view.RPC("carryChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, carriedChipPos, carriedChipRot);
                                        }
                                    }
                                    // Disposing chip
                                    else if (Input.GetKeyDown(KeyCode.R))
                                    {
                                        if (collectedChipies.Count > 0 && throwAccess && !throwChipAcces && bucketNameInteracted != null)
                                        {
                                            //if (bucketName == bucketNameInteracted)
                                            //{
                                            throwChipAcces = true;
                                            view.RPC("throwChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, throwForce, playerNumber, bucketNameInteracted);
                                            //}
                                        }
                                    }
                                    else
                                    {
                                        oneChipAtATimeCarry = false;
                                        throwChipAcces = false;
                                    }


                                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                    {
                                        if (!die && GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipTracker == GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips)
                                        {
                                            view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                        }

                                    }
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Lust")
                        {
                            // Single player: get as many correct keys as possible
                            // Multi player: get the most correct keys

                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<LustGameplayManager>().newKey)
                                    {
                                        if (GameObject.Find("GameManager").GetComponent<LustGameplayManager>().readyForNewKey)
                                        {
                                            int i = UnityEngine.Random.Range(0, GameObject.Find("GameManager").GetComponent<LustGameplayManager>().pianoKeys.Count);

                                            // if index was already chosen or the key is being pressed on already don't do anything
                                            if (i == selectedKey || GameObject.Find("GameManager").GetComponent<LustGameplayManager>().pianoKeys[i].GetComponent<LustPianoKey>().keyPressed)
                                            {
                                                GameObject.Find("GameManager").GetComponent<LustGameplayManager>().readyForNewKey = true;
                                            }
                                            else
                                            {
                                                GameObject.Find("GameManager").GetComponent<LustGameplayManager>().readyForNewKey = false;
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

                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (!die && GameObject.Find("GameManager").GetComponent<LustGameplayManager>().keyAmountTracker == GameObject.Find("GameManager").GetComponent<LustGameplayManager>().maxKeys)
                                    {
                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Gluttony")
                        {
                            // Single player: Collect as many munchies as possible before the time runs out
                            // Multi player: Collect more mucnhies than the other before the time runs out, can sabotage other by hitting them with a mallet and emptying their munchies

                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {

                                /*
                                // Instantiation
                                if (PhotonNetwork.LocalPlayer.IsMasterClient && !GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().noMoreFoodNeeded)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().foodReady && theFood == null && GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood >= GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform.childCount)
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

                                    if (GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().foodInstantiationTracker == GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().AmountOfFood)
                                    {
                                        GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().noMoreFoodNeeded = true;
                                    }
                                }
                                */

                                if (interactedFood != null)
                                {
                                    view.RPC("foodInteractionActive", RpcTarget.AllBufferedViaServer, view.Owner.NickName, interactedFood.name);
                                }
                                else
                                {
                                    view.RPC("foodInteractionDeactive", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                }


                                // Eating
                                if (eatFood)
                                {
                                    view.RPC("eatUpFood", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    //view.RPC("muchiesScore", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                }


                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (!die && GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform.childCount == 0)
                                    {
                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }
                                }

                                // when player gets hit by a bomb or another player hits them w hammer
                                // Puking
                                /*if (Input.GetKeyDown(KeyCode.P))
                                {
                                    if (collectedFoodies > 0 && !vomit)
                                    {

                                        Vector3 vomitedPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                        Vector3 vomitedDirection = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(3, 5), UnityEngine.Random.Range(-5, 5));

                                        Vector3 direction = vomitedDirection;
                                        direction = direction.normalized;

                                        view.RPC("setVomitPosition", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vomitedPos, direction, 300f);
                                        collectedFoodies--;

                                // split here, above was commented out

                                        collectedFoodies = 0;
                                        view.RPC("muchiesScore", RpcTarget.AllBufferedViaServer, view.Owner.NickName, collectedFoodies);

                                        vomit = true;
                                    }
                                }
                                else
                                {
                                    vomit = false;
                                }
                                */

                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Envy")
                        {
                            // Single player:
                            // Mutli player: cross your horse over the finish line before the other

                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
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
                                if (Input.GetKey(KeyCode.E))
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


                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (!die && (GameObject.Find("Scene Manager").GetComponent<SceneManage>().allPlayersInGame.Count - 1) == GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().horseResults.Count)
                                    {
                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Wrath")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                // CAUSES ISSUES CUZ OF RANDOM RANGE I THINK
                                // platform rotation
                                if (PhotonNetwork.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().plateState)
                                    {
                                        if (!plateMoving)
                                        {
                                            try
                                            {
                                                directionIndex = UnityEngine.Random.Range(0, 3);
                                            }
                                            catch (NullReferenceException e)
                                            {
                                                directionIndex = 0;
                                            }

                                            plateMoving = true;
                                        }
                                        else
                                        {
                                            view.RPC("shakePlatform", RpcTarget.AllBufferedViaServer, directionIndex, view.Owner.NickName);
                                        }
                                    }
                                }
                                

                                // fell of the platform and lost
                                if (fellOffPlatform && !deathRecorded)
                                {
                                    view.RPC("fellOffWrathPlatform", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                }

                                // game ending - if the results has one less than the total of players means there 1 person left standing which is the winner
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().wrathResults.Count == (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count - 1))
                                    {
                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }                                    
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Sloth")
                        {
                            // Single Player: Stay within the light as much as possible before the time runs up
                            // Multi Player: Be the last one standing before your life source reaches 0

                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
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
                                if (gotBearTrapped && interactedBearTrap != null)
                                {
                                    Vector3 pos = new Vector3(transform.position.x, transform.position.y - trapHeight, transform.position.z);
                                    view.RPC("caughtByBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName, pos);
                                }

                                if (!gotBearTrapped && alreadySetBearTrap != null)
                                {
                                    if (Input.GetKeyDown(KeyCode.E))
                                    {
                                        view.RPC("unhookedBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }
                                }

                                if (!withinTheLight)
                                {
                                    if ((int)lifeSource < 0)
                                    {
                                        if (!ranOutOfLife)
                                        {
                                            lifeSource = 0f;
                                            view.RPC("displayLifePercentage", RpcTarget.AllBufferedViaServer, view.Owner.NickName, 0, true);
                                            //GameObject.Find("GameManager").GetComponent<TempLevelTimer>().CallGameEnd();
                                        }
                                    }
                                    else
                                    {
                                        if (!pauseForDecrease)
                                        {
                                            pauseForDecrease = true;
                                            StartCoroutine("LifeDropSpeed", lifeDropSpeed);
                                        }
                                    }
                                }

                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().slothResults.Count >= 1)
                                    {
                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }
                                }
                            }
                        }


                        if (!freezePlayer)
                        {
                            MovePlayer();

                            if (Input.GetKeyDown(KeyCode.Space))
                            {
                                Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                                view.RPC("jumpBoyJump", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel);
                            }
                        }
                    }
                }
            }
            else if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "gameover")
            {
                try
                {
                    for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                    {
                        try
                        {
                            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                        }

                        catch (NullReferenceException e)
                        {
                            Debug.Log("naw cuh");
                        }
                    }
                }
                catch (NullReferenceException e) { }

                MovePlayer();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                    view.RPC("jumpBoyJump", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel);
                }
            }
        }
    }

    public void stunnah()
    {
        StartCoroutine("stunTheBitch", 5);
    }
    
    public void pauseys()
    {
        StartCoroutine("pauseActions", 4);
    }
    
    public void pauseForReset()
    {
        StartCoroutine("likklePause", 0.5f);
    }
    
    IEnumerator stunTheBitch(float time)
    {
        freezePlayer = true;

        yield return new WaitForSeconds(time);

        freezePlayer = false;
    }
    
    IEnumerator pauseActions(float time)
    {
        actionPause = true;

        yield return new WaitForSeconds(time);

        actionPause = false;
    }

    IEnumerator likklePause(float time)
    {
        yield return new WaitForSeconds(time);

        GetComponent<PhotonTransformViewClassic>().m_PositionModel.TeleportEnabled = true;

    }

    IEnumerator LifeDropSpeed(float time)
    {
        if (lifeSource > 0)
        {
            lifeSource -= 1;
        }

        yield return new WaitForSeconds(time);

        pauseForDecrease = false;

        view.RPC("displayLifePercentage", RpcTarget.AllBufferedViaServer, view.Owner.NickName, (int)lifeSource, false);
        
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
            if (imDraggingMan || Input.GetKey(KeyCode.P))
            {
                targetRotation = Quaternion.LookRotation(-movement);

                targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360 * Time.fixedDeltaTime), 0.8f);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(movement);

                targetRotation = Quaternion.Lerp(transform.rotation, Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    360 * Time.fixedDeltaTime), 0.8f);
            }
        }

        rb.MovePosition(playerPos + movement * speedModifier * speed * Time.fixedDeltaTime);
        rb.MoveRotation(targetRotation);

    }

    void throwParabola(GameObject box, Vector3 targetPos, Vector3 startPos, float force)
    {
        Vector3 dir = targetPos - startPos;
        dir = dir.normalized;
        box.GetComponent<Rigidbody>().AddForce(dir * force);
        //box.GetComponent<Rigidbody>().AddTorque(dir * 50);
    }

    public void shakeHer(int dir)
    {
        GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().shakePlateVariables(dir);
    }

    /// RPCs
    [PunRPC]
    void PunchEffect(string name, Vector3 hit)
    {
        try
        {
            GameObject.Find(name).GetComponent<Rigidbody>().AddForce(hit);
            GameObject.Find(name).GetComponent<PlayerUserTest>().oopsyGotPushed = false;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void YouPushedThem(string name, string victim, Vector3 hit)
    {
        try
        {
            if (!GameObject.Find(victim).GetComponent<PlayerUserTest>().oopsyGotPushed) {

                GameObject.Find(victim).GetComponent<Rigidbody>().AddForce(hit * 400);

                GameObject.Find(victim).GetComponent<PlayerUserTest>().oopsyGotPushed = true;
                GameObject.Find(name).GetComponent<PlayerUserTest>().actionPause = true;
                GameObject.Find(name).GetComponent<PlayerUserTest>().pauseys();

                Debug.Log("PUSH ATTACK CALLED");
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void ZOOMBRO(string name, Vector3 hit)
    {
        try
        {
            GameObject.Find(name).GetComponent<Rigidbody>().AddForce(hit);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void YouHitThem(string name, string victim)
    {
        try
        {
            Debug.Log("y u not freezing");
            GameObject.Find(victim).GetComponent<PlayerUserTest>().freezePlayer = true;

            GameObject.Find(victim).GetComponent<PlayerUserTest>().oopsyGotHit = true;
            GameObject.Find(victim).GetComponent<PlayerUserTest>().stunnah();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void setMyParent(string name)
    {
        try
        {
            GameObject.Find(name).transform.parent = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void YouDraggedThem(string name, string victim, bool x)
    {
        try
        {         
            if (x)
            {
                GameObject.Find(victim).GetComponent<PlayerUserTest>().freezePlayer = x;
                GameObject.Find(victim).GetComponent<PlayerUserTest>().oopsyGotDragged = x;

                GameObject.Find(victim).GetComponent<Rigidbody>().isKinematic = true;
                GameObject.Find(victim).GetComponent<BoxCollider>().isTrigger = true;
                GameObject.Find(victim).transform.position = GameObject.Find(name).transform.GetChild(3).position;
                GameObject.Find(victim).transform.rotation = GameObject.Find(name).transform.rotation;
                GameObject.Find(victim).transform.parent = GameObject.Find(name).transform.GetChild(3);
                GameObject.Find(victim).GetComponent<PhotonTransformViewClassic>().m_PositionModel.TeleportEnabled = false;
                //GameObject.Find(victim).transform.position += GameObject.Find(victim).transform.forward * 1.5f;
                GameObject.Find(name).GetComponent<PlayerUserTest>().imDraggingMan= x;
                GameObject.Find(name).GetComponent<PlayerUserTest>().onetimebruvidek = true;
            }
            else if(onetimebruvidek)
            {
                GameObject.Find(victim).GetComponent<PlayerUserTest>().freezePlayer = x;
                GameObject.Find(victim).GetComponent<PlayerUserTest>().oopsyGotDragged = x;

                GameObject.Find(victim).transform.position = GameObject.Find(name).transform.GetChild(3).position;
                GameObject.Find(victim).transform.SetParent(null);
                GameObject.Find(victim).GetComponent<BoxCollider>().isTrigger = false;
                GameObject.Find(victim).GetComponent<Rigidbody>().isKinematic = false;
                GameObject.Find(name).GetComponent<PlayerUserTest>().imDraggingMan = x;                
                GameObject.Find(name).GetComponent<PlayerUserTest>().onetimebruvidek = false;
                GameObject.Find(victim).GetComponent<PlayerUserTest>().pauseForReset();
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // Username

    [PunRPC]
    void getPlayersNickName(string name)
    {
        try
        {
            playersUsername = name;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void setUsername(string Player)
    {
        try
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck)
            {
                GameObject.Find(Player).GetComponent<PlayerUserTest>().username.text = Player;
            }
            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void settingCharacterSkins()
    {
        try
        {
            for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
            {
                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
/*
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive)
                {
                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);

                    
                    for (int y = 0; y < GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).childCount; y++)
                    {
                        if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).name == GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].chosenCharacter)
                        {
                            try
                            {
                                GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject.SetActive(true);
                            }
                            catch (NullReferenceException e) { }
                        }
                        else
                        {
                            Destroy(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject);
                        }

                    }
                    
                }
                else
                {
                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);

                    
                    for (int y = 0; y < GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).childCount; y++)
                    {

                        if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).name == GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].chosenCharacter)
                        {
                            try
                            { 
                            GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject.SetActive(true);
                            }
                            catch (NullReferenceException e) { }
                            //GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().deadSkin;
                        }
                        else
                        {
                            Destroy(GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(y).gameObject);
                        }

                    }
                }*/
            }
        }
        catch (NullReferenceException e) { }
    }

    [PunRPC]
    void jumpBoyJump(string pName, Vector3 vel)
    {
        try
        {
            GameObject.Find(pName).GetComponent<Rigidbody>().velocity = vel;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }


    // Levels

    void actualEndGame()
    {
        GameObject.Find("GameManager").GetComponent<TempLevelTimer>().CallGameEnd();
    }

    [PunRPC]
    void endTheGame(string pName)
    {
        try
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;

            GameObject.Find(pName).GetComponent<PlayerUserTest>().actualEndGame();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

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

    [PunRPC]
    void fellOffWrathPlatform(string pName)
    {
        try
        {
            if (!GameObject.Find(pName).GetComponent<PlayerUserTest>().deathRecorded) 
            {
                GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().RecordWrathResults(GameObject.Find(pName).gameObject);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().deathRecorded = true;
            }
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
            carriedBox.transform.parent = GameObject.Find("BoxParent").transform;
            carriedBox.AddComponent<Rigidbody>();

            throwParabola(carriedBox, transform.GetChild(1).position, transform.position, boxThrowForce);

            carriedBox = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    void addPlayerToSlothResults(string player)
    {
        GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().RecordSlothResults(GameObject.Find(player).gameObject);
    }

    // Sloth
    [PunRPC]
    void displayLifePercentage(string pName, int v, bool state)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().lifeSource = v;

            if (state)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().ranOutOfLife = state;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().addPlayerToSlothResults(pName);
            }
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
            //GameObject.Find(GameObject.Find(GameObject.Find(pName).GetComponent<PlayerUserTest>().alreadySetBearTrap.name).GetComponent<SlothObstacle>().caughtPlayer.name).GetComponent<PlayerUserTest>().interactedBearTrap = null;
            //GameObject.Find(GameObject.Find(GameObject.Find(pName).GetComponent<PlayerUserTest>().alreadySetBearTrap.name).GetComponent<SlothObstacle>().caughtPlayer.name).GetComponent<PlayerUserTest>().freezePlayer = false;
            //GameObject.Find(GameObject.Find(GameObject.Find(pName).GetComponent<PlayerUserTest>().alreadySetBearTrap.name).GetComponent<SlothObstacle>().caughtPlayer.name).GetComponent<PlayerUserTest>().gotBearTrapped = false;
            Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().alreadySetBearTrap.gameObject);
            GameObject.Find(pName).GetComponent<PlayerUserTest>().alreadySetBearTrap = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void lifeSourceEmptied(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().lifeFullyDone = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    // GLUTTONY

    [PunRPC]
    void foodInteractionActive(string pName, string food)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood = GameObject.Find(food).gameObject;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void foodInteractionDeactive(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void muchiesScore(string pName)
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
            //GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood = GameObject.Find(food).gameObject;

            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood != null) 
            {
                Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood.gameObject);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood = null;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().eatFood = false;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedFoodies++;
            }
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
                GameObject chip = Instantiate(GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipPrefab, new Vector3(pos.x, pos.y, pos.z), rot, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform);
                chip.name = "Chip" + GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipTracker;
                GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().incChipCount();
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip = chip;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }


    [PunRPC]
    void resetChipTracker(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().collectionTracker = 0;
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
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip = GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedChip = null;
                Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.GetComponent<Rigidbody>());
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.GetComponent<MeshCollider>().isTrigger = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.transform.position = pos;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.transform.rotation = rot;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.transform.parent = GameObject.Find(pName).transform;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.GetComponent<ChipScript>().Available = false;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Add(GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip = null;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectionTracker++;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().totalChipCollection++;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void throwChip(string pName, float f, int pActor, string bucketName)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Count > 0 && !GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces) 
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].transform.parent = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].AddComponent<Rigidbody>();
                //GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().throwChip(GameObject.Find("Bucket" + pActor).transform.GetChild(0).position, GameObject.Find(pName).transform.position, f);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().throwChip(GameObject.Find(bucketName).transform.GetChild(0).position, GameObject.Find(pName).transform.position, f);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().Available = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.RemoveAt(0);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().thrownTracker++;

                for (int i = 0; i < GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Count; i++)
                {
                    GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[i].transform.position = new Vector3(GameObject.Find(pName).transform.position.x, GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[i].transform.position.y - 0.5f, GameObject.Find(pName).transform.position.z);
                }

                //GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces = true;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }


    [PunRPC]
    void setDiceValue(string pName, int x)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().diceRollValue = x;
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
        if (other.tag == "Player")
        {
            if (interactedOpponent == null)
            {
                interactedOpponent = other.gameObject;
            }

            competitor = other.gameObject;
        }

        // GREED
        if (other.tag == "Chip")
        {
            chipAccess = true;
            interactedChip = other.gameObject;
        }

        // ENVY
        if (other.tag == "Squirter")
        {
            squirtAccess = true;
            squirtGun = other.gameObject;
        }
        
        // GLUTTONY
        if (other.tag == "Food")
        {
            eatFood = true;
            interactedFood = other.gameObject;
            //Destroy(other.gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // GREED

        if (other.tag == "ChipZone")
        {
            throwAccess = true;
            bucketNameInteracted = other.transform.parent.name;
        }

        // SLOTH
        if (other.tag == "Light")
        {
            withinTheLight = true;
        }
        if (other.tag == "BearTrap" && interactedBearTrap == null && !other.GetComponent<SlothObstacle>().trapSet)
        {
            gotBearTrapped = true;
            interactedBearTrap = other.gameObject;
        }
        
        if (other.tag == "BearTrap" && other.GetComponent<SlothObstacle>().trapSet)
        {
            alreadySetBearTrap = other.gameObject;
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
        if (other.tag == "OffLimitsWrath")
        {
            fellOffPlatform = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interactedOpponent = null;
            competitor = null;
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
            bucketNameInteracted = null;
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

        if(other.tag == "BearTrap")
        {
            alreadySetBearTrap = null;
        }

    }

    void resetAllValues()
    {
        // GREED
        cameraSwitch = false;
        chipAccess = false;
        throwAccess = false;
        collectionTracker = 0;
        thrownTracker = 0;
        totalChipCollection = 0;
        diceRollValue = 0;
        interactedChip = null;
        collectedChipies = new List<GameObject>();
        CameraOptions = new List<GameObject>();

        theChip = null;
        instantiateChipOnce = false;

        noMoreChipsNeeded = false;
        oneChipAtATimeThorw = false;
        oneChipAtATimeCarry = false;

        carriedChip = null;
        throwChipAcces = false;

        bucketNameInteracted = null;

        // ENVY
        squirtAccess = false;
        squirtGunName = null;
        squirtGun = null;

        // GLUTTONY
        eatFood = false;
        vomit = false;
        collectedFoodies = 0;
        theFood = null;
        theVomittedFood = null;
        instantiateFoodOnce = false;
        interactedFood = null;
        gotPunched = false;
        foodInstanitationTracker = 0;

        // SLOTH
        withinTheLight = false;
        pauseForDecrease = false;
        gotBearTrapped = false;
        lifeFullyDone = false;
        interactedBearTrap = null;
        freezePlayer = false;

        theLight = null;
        instantiateLightOnce = false;

        theTrap = null;
        instantiateTrapOnce = false;

        ranOutOfLife = false;

        // LUST
        hitKeys = 0;
        selectedKey = 0;
        resetPosition = true;
        //readyForNewKey = true;
        hitKeyScore = false;
        landedOnFloor = false;

        // WRATH
        directionIndex = 0;
        directionChosen = false;
        interactedBox = null;
        carriedBox = null;
        plateMoving = false;
        boxScore = 0;
        fellOffPlatform = false;

        playerNumber = -1;
        finishedSinglePlayer = false;
        die = false;
    }
}
