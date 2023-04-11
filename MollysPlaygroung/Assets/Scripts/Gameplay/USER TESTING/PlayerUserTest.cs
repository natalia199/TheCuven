using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class PlayerUserTest : MonoBehaviour
{
    Rigidbody rb;
    PhotonView view;

    public bool sceneChoiceDecided = false;
    public int sceneDecision;

    public TextMeshProUGUI username;
    string playersUsername;

    [SerializeField] float speed = 5f;
    [SerializeField] float speedModifier = 1f;
    [SerializeField] float jumpForce = 8f;

    public GameObject interactedOpponent = null;
    public bool youLoseTheLevel = false;

    // GREED
    public bool cameraSwitch = false;
    public bool chipAccess = false;
    public bool munchedYou = false;
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

    public bool waitHoldGreed = false;

    public GameObject carriedChip = null;
    public bool throwChipAcces = false;

    public string bucketName;
    public string bucketNameInteracted;

    // WHEEL
    public float rotationValueWheel;
    public bool goodToReset = true;
    public bool rotationValueSet = false;
    public bool stopTheWheel = false;
    public float _rotationSpeed;
    public float _rotationSpeedDecrease;

    // ENVY
    public bool oneTimeEnvyAssign = false;
    public bool envySetOneTime = false;
    public bool oneTimeSetUp = false;
    public bool squirtAccess = false;
    public string horseName;
    public string squirtGunName;
    public GameObject squirtGun;
    public int votedHead = -1;
    public int RacingPoints = 0;
    public List<GameObject> EnvyCameraOptions = new List<GameObject>();

    // GLUTTONY
    bool eatFood = false;
    public bool vomit = false;
    public bool foodCollected = false;
    public int collectedFoodies;
    public Vector2 foodPosition;
    public GameObject theFood;
    public GameObject theVomittedFood;
    public bool instantiateFoodOnce = false;
    public GameObject interactedFood = null;

    int typeOfFood;
    public int preveiousTypeOfFood = -1;

    public bool gotPunched = false;
    public bool bigBoyMunch = false;
    public int foodInstanitationTracker;

    // SLOTH
    public bool withinTheLight = false;
    bool pauseForDecrease = false;
    bool allowNewDecrease = false;
    public bool gotBearTrapped = false;
    public bool lifeFullyDone = false;
    public float lifeMax = 20;
    public float lifeSource = 100;
    public GameObject interactedBearTrap = null;
    //public string interactedBearTrapName = "";
    //public string alreadySetBearTrapName = "";
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
    public bool lustTimerRunning = true;
    public float lustTimer;
    public int hitKeys = 1;
    public int selectedKey;
    public bool resetPosition = true;
    //public bool readyForNewKey = true;
    public bool hitKeyScore = false;
    public bool landedOnFloor = false;
    public bool headsSet = false;

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

    // PRIDE
    public List<GameObject> PrideCameraOptions = new List<GameObject>();
    public GameObject selectedCup;
    Ray ray;
    public bool prideTimerRunning = true;
    public float prideTimer;
    bool reverseWalk = false;
    public bool gotPoisoned = false;


    public bool deathRecorded = false;

    public int playerNumber = -1;

    public bool finishedSinglePlayer = false;
    public bool die = false;

    public GameObject competitor = null;

    public bool oopsyGotPushed = false;
    public bool oopsyGotHit = false;
    public bool oopsyGotDragged = false;
    public bool imDraggingMan = false;
    public bool oofGotMunched = false;

    public bool actionPause = false;
    public bool onetimebruvidek = false;

    public bool diceRollFreeze = true;

    public bool playerIsGrounded = false;

    public bool skippedTheEndingCredits = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            resetAllValues();
            youLoseTheLevel = false;
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
            CameraOptions.Add(GameObject.Find("Main Camera"));

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
            EnvyCameraOptions.Add(GameObject.Find("Main Camera"));
            EnvyCameraOptions.Add(GameObject.Find("Main Camera 2"));

            if (view.IsMine)
            {
                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == PhotonNetwork.LocalPlayer.NickName)
                    {
                        horseName = "Horse" + i;
                        squirtGunName = "SquirtGun" + i;
                    }
                }

                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username != PhotonNetwork.LocalPlayer.NickName)
                    {
                        GameObject.Find("VotingGrid").transform.GetChild(i).gameObject.SetActive(true);
                        GameObject.Find("VotingGrid").transform.GetChild(i).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].characterID).gameObject.SetActive(true);
                        GameObject.Find("VotingGrid").transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username;
                    }

                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyPointsResult.Add(0);
                }
            }
        }
        else if (SceneManager.GetActiveScene().name == "Sloth")
        {
            lifeSource = lifeMax;
            withinTheLight = false;
            pauseForDecrease = false;
        }
        else if (SceneManager.GetActiveScene().name == "Pride")
        {
            if (view.IsMine)
            {
                PrideCameraOptions.Add(GameObject.Find("Main Camera"));
                PrideCameraOptions.Add(GameObject.Find("Main Camera 2"));

                
                for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                {
                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive && GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == PhotonNetwork.LocalPlayer.NickName)
                    {
                        PrideCameraOptions[1].SetActive(false);
                        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().CupSets[1].SetActive(false);

                        transform.position = GameObject.Find("PlayerSpawn").transform.GetChild(0).position;
                        transform.rotation = GameObject.Find("PlayerSpawn").transform.GetChild(0).rotation;
                        reverseWalk = false;

                        break;
                    }
                    else if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].stillAlive && GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username != PhotonNetwork.LocalPlayer.NickName)
                    {
                        PrideCameraOptions[0].SetActive(false);
                        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().CupSets[0].SetActive(false);

                        reverseWalk = true;

                        break;
                    }
                }
                

                prideTimer = GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().timeStamps[GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().timeStampTrack];
            }

            transform.localScale *= 5.2f;
        }
    }

    void Update()
    {
        if (view.IsMine)
        {
            // PLAYER ACTIONS - if not stunned
            if (!freezePlayer && playerIsGrounded && !youLoseTheLevel)
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

                    // begin game
                    if (PhotonNetwork.IsMasterClient)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().beginGame)
                        {
                            //PhotonNetwork.CurrentRoom.IsOpen = false;
                            //PhotonNetwork.CurrentRoom.IsVisible = false;

                            if (!sceneChoiceDecided)
                            {
                                sceneDecision = UnityEngine.Random.Range(0, GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count);
                                Debug.Log("scene decided " + sceneDecision);
                                sceneChoiceDecided = true;
                            }

                            view.RPC("beginTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                        }
                    }                            
                    
                    MovePlayer();

                    if (Input.GetKeyDown(KeyCode.Space) && playerIsGrounded)
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
                        // character skin
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


                        // GREED LEVEL
                        if (SceneManager.GetActiveScene().name == "Greed")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                if (!oneTimeSetUp)
                                {
                                    for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                    {
                                        GameObject.Find("Pockets").transform.GetChild(i).GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username;
                                    }
                                    oneTimeSetUp = true;
                                }

                                // Dice Roll baby
                                if (!cameraSwitch)
                                {
                                    diceRollFreeze = true;

                                    CameraOptions[0].SetActive(true);
                                    CameraOptions[1].SetActive(false);
                                    GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().diceControls.SetActive(true);

                                    // Roll Dice
                                    if (Input.GetKeyDown(KeyCode.E))
                                    {
                                        GameObject.Find("Dice").GetComponent<Dice>().RollDice();
                                    }

                                    //  Heading to poker table
                                    if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().goodToGo)
                                    {
                                        collectionTracker = 0;
                                        view.RPC("resetChipTracker", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

                                        thrownTracker = 0;
                                        cameraSwitch = true;
                                        diceRollValue = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue;
                                        view.RPC("setDiceValue", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue);
                                        GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().goodToGo = false;
                                    }
                                }
                                // Chip extravaganza
                                else if (cameraSwitch)
                                {
                                    diceRollFreeze = false;

                                    CameraOptions[0].SetActive(false);
                                    CameraOptions[1].SetActive(true);
                                    GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().diceControls.SetActive(false);


                                    // heading to dice roll scene
                                    //if (thrownTracker >= GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue && !die)
                                    if (thrownTracker >= diceRollValue && !die)
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
                                        //if (!oneChipAtATimeCarry && interactedChip != null && interactedChip.GetComponent<ChipScript>().Available && collectionTracker < GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().rolledValue)
                                        if (!oneChipAtATimeCarry && interactedChip != null && interactedChip.GetComponent<ChipScript>().Available && collectionTracker < diceRollValue)
                                        {
                                            oneChipAtATimeCarry = true;
                                            Vector3 carriedChipPos = new Vector3(transform.GetChild(4).position.x, transform.position.y + 2f + (collectedChipies.Count * 0.5f), transform.GetChild(4).position.z);
                                            Quaternion carriedChipRot = Quaternion.identity;
                                            view.RPC("carryChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, carriedChipPos, carriedChipRot);
                                        }
                                    }
                                    // Disposing chip
                                    else if (Input.GetKeyDown(KeyCode.R))
                                    {
                                        if (collectedChipies.Count > 0 && throwAccess && !throwChipAcces && bucketNameInteracted != null)
                                        {
                                            //throwChipAcces = true;

                                            Vector3 dir = GameObject.Find(bucketName).transform.GetChild(0).position - transform.position;
                                            dir = dir.normalized;

                                            view.RPC("throwChip", RpcTarget.AllBufferedViaServer, view.Owner.NickName, throwForce, playerNumber, bucketNameInteracted, dir);
                                            //throwChipAcces = true;
                                        }
                                    }
                                    else
                                    {
                                        view.RPC("updateChipState", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                                        oneChipAtATimeCarry = false;
                                    }
                                }

                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().chipTracker == GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().startingAmountOfChips)
                                    {                                        
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count == 2)
                                        {
                                            sceneDecision = 0;
                                        }
                                        else
                                        {
                                            if (!sceneChoiceDecided)
                                            {
                                                sceneDecision = UnityEngine.Random.Range(0, (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count-1));
                                                sceneChoiceDecided = true;
                                            }
                                        }

                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                    }
                                }
                            }
                        }
                        // LUST LEVEL
                        else if (SceneManager.GetActiveScene().name == "Lust")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                if (!headsSet)
                                {
                                    for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                    {
                                        GameObject.Find("fireGrid").transform.GetChild(i).gameObject.SetActive(true);
                                        GameObject.Find("fireGrid").transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username;
                                        GameObject.Find("fireGrid").transform.GetChild(i).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].characterID).gameObject.SetActive(true);
                                    }

                                    headsSet = true;
                                }


                                // Key instantiation
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

                                    if (GameObject.Find("GameManager").GetComponent<LustGameplayManager>().timerActivated)
                                    {
                                        // timer
                                        if (lustTimerRunning)
                                        {
                                            lustTimer -= Time.deltaTime;

                                            if (lustTimer < 0)
                                            {
                                                view.RPC("lustOutOfTime", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                                lustTimerRunning = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lustTimer = 10;
                                        lustTimerRunning = true;
                                    }
                                }

                                if (hitKeyScore)
                                {
                                    hitKeys += 1;
                                    hitKeyScore = false;
                                    view.RPC("keyScoreDisplay", RpcTarget.AllBufferedViaServer, view.Owner.NickName, hitKeys);
                                    view.RPC("lustOutOfTime", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                }

                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<LustGameplayManager>().keyAmountTracker == GameObject.Find("GameManager").GetComponent<LustGameplayManager>().maxKeys)
                                    {
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count == 2)
                                        {
                                            sceneDecision = 0;
                                        }
                                        else
                                        {
                                            if (!sceneChoiceDecided)
                                            {
                                                sceneDecision = UnityEngine.Random.Range(0, (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count - 1));
                                                sceneChoiceDecided = true;
                                            }
                                        }

                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                    }
                                }
                            }
                        }
                        // GLUTTONY LEVEL
                        else if (SceneManager.GetActiveScene().name == "Gluttony")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                if (vomit)
                                {
                                    Vector3 vomitedPos = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                                    Vector3 vomitedDirection = new Vector3(UnityEngine.Random.Range(-5, 5), UnityEngine.Random.Range(7, 10), UnityEngine.Random.Range(-5, 5));

                                    Vector3 direction = vomitedDirection;
                                    direction = direction.normalized;

                                    int typaFood = UnityEngine.Random.Range(0, 3);

                                    view.RPC("setVomitPosition", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vomitedPos, direction, 400f, typaFood);
                                }

                                // Instantiation
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().foodReady && theFood == null)
                                    {
                                        if (!instantiateFoodOnce)
                                        {
                                            float xPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[0].position.x, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[2].position.x);
                                            float zPos = UnityEngine.Random.Range(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[0].position.z, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodSpawnPoints[1].position.z);
                                            foodPosition = new Vector2(xPos, zPos);
                                            instantiateFoodOnce = true;

                                            while (true)
                                            {
                                                typeOfFood = UnityEngine.Random.Range(0, 3);

                                                if (preveiousTypeOfFood != typeOfFood)
                                                {
                                                    preveiousTypeOfFood = typeOfFood;
                                                    break;
                                                }
                                            }
                                        }

                                        view.RPC("setFoodPosition", RpcTarget.AllBufferedViaServer, foodPosition, view.Owner.NickName, typeOfFood);
                                    }
                                }

                                if (!youLoseTheLevel)
                                {
                                    if (interactedFood != null)
                                    {
                                        view.RPC("foodInteractionActive", RpcTarget.AllBufferedViaServer, view.Owner.NickName, interactedFood.name);
                                    }
                                    else
                                    {
                                        view.RPC("foodInteractionDeactive", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }

                                    // ACTIONS
                                    // Eating
                                    if (eatFood)
                                    {
                                        if (!foodCollected)
                                        {
                                            view.RPC("muchiesScore", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                            foodCollected = true;
                                        }

                                        view.RPC("eatUpFood", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }
                                    if (collectedFoodies >= 2)
                                    {
                                        view.RPC("munchersTime", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                                    }

                                    if (Input.GetKey(KeyCode.E) && bigBoyMunch)
                                    {
                                        if (competitor != null)
                                        {
                                            view.RPC("YouMunchedThem", RpcTarget.AllBufferedViaServer, view.Owner.NickName, competitor.name);
                                        }
                                    }
                                }

                                // game ending - if the results has one less than the total of players means there 1 person left standing which is the winner
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().gluttonyResults.Count >= (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count - 1))
                                    {
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count == 2)
                                        {
                                            sceneDecision = 0;
                                        }
                                        else
                                        {
                                            if (!sceneChoiceDecided)
                                            {
                                                sceneDecision = UnityEngine.Random.Range(0, (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count - 1));
                                                sceneChoiceDecided = true;
                                            }
                                        }

                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                    }
                                }

                                // Puking
                                if (Input.GetKeyDown(KeyCode.I))
                                {
                                    if (interactedOpponent != null)
                                    {
                                        if (interactedOpponent.GetComponent<PlayerUserTest>().collectedFoodies >= 2)
                                        {
                                            view.RPC("VomitBitch", RpcTarget.AllBufferedViaServer, interactedOpponent.name);
                                        }
                                    }
                                }
                            }
                        }
                        // ENVY LEVEL
                        else if (SceneManager.GetActiveScene().name == "Envy")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                if (!oneTimeEnvyAssign)
                                {
                                    // assigning horses
                                    for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                    {
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == PhotonNetwork.LocalPlayer.NickName)
                                        {
                                            horseName = "Horse" + i;
                                            squirtGunName = "SquirtGun" + i;
                                        }

                                        GameObject.Find("SquirtGun" + i).transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username;
                                        GameObject.Find("PointGrid").transform.GetChild(i).gameObject.SetActive(true);
                                        GameObject.Find("PointGrid").transform.GetChild(i).GetChild(0).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].characterID).gameObject.SetActive(true);
                                        GameObject.Find("PointGrid").transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username;
                                        GameObject.Find("PointGrid").transform.GetChild(i).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                                    }

                                    oneTimeEnvyAssign = true;
                                }

                                GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().roundTxt.text = "Round " + (GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().levelRounds + 1);

                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().levelRounds >= 2)
                                    {
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count == 2)
                                        {
                                            sceneDecision = 0;
                                        }
                                        else
                                        {
                                            if (!sceneChoiceDecided)
                                            {
                                                sceneDecision = UnityEngine.Random.Range(0, (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count - 1));
                                                sceneChoiceDecided = true;
                                            }
                                        }

                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                    }
                                }

                                if (GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().votingSystem)
                                {
                                    EnvyCameraOptions[0].SetActive(false);
                                    EnvyCameraOptions[1].SetActive(true);


                                    if (GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().oneTimeRound)
                                    {
                                        GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().setLoser();
                                        GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().setPlayerPoints();

                                        GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().oneTimeRound = false;

                                        GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().selectControl.SetActive(true);

                                    }
                                    else
                                    {
                                        envySetOneTime = false;

                                        if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                        {
                                            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                            {
                                                if (GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username).GetComponent<PlayerUserTest>().votedHead == -1)
                                                {
                                                    break;
                                                }

                                                if (i >= (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count - 1))
                                                {
                                                    view.RPC("switchingVotingAndGame", RpcTarget.AllBufferedViaServer, false);
                                                }
                                            }
                                        }

                                        if (votedHead != -1) 
                                        {
                                            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().betTxt.text = "Bet: " + GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[votedHead].username;
                                        }
                                        else
                                        {
                                            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().betTxt.text = "Bet: ";
                                        }

                                        // BETTING
                                        if (Input.GetMouseButtonDown(0))
                                        {
                                            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                            RaycastHit[] die = Physics.RaycastAll(ray);
                                            foreach (RaycastHit hit in die)
                                            {
                                                if (hit.collider.gameObject.tag == "VoteHead")
                                                {
                                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().selectControl.SetActive(false);
                                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().confirmControl.SetActive(true);

                                                    votedHead = hit.collider.gameObject.GetComponent<VotedHeadInfo>().headID;

                                                    Debug.Log("I hit head " + votedHead);
                                                    break;
                                                }
                                                /*else if (hit.collider.gameObject.tag == "Ignore")
                                                {
                                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().selectControl.SetActive(true);
                                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().confirmControl.SetActive(false);

                                                    votedHead = -1;

                                                    for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                                    {
                                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == PhotonNetwork.LocalPlayer.NickName)
                                                        {
                                                            GameObject.Find("VotingGrid").transform.GetChild(i).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].characterID).GetComponent<MeshRenderer>().material = GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().unchosenMesh[GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].characterID];
                                                        }
                                                    }

                                                    view.RPC("setVotedHead", RpcTarget.AllBufferedViaServer, view.Owner.NickName, votedHead);
                                                }*/
                                            }
                                        }
                                        else if (Input.GetKeyDown(KeyCode.E))
                                        {
                                            if(votedHead != -1)
                                            {
                                                GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().selectControl.SetActive(false);
                                                GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().confirmControl.SetActive(false);

                                                view.RPC("setVotedHead", RpcTarget.AllBufferedViaServer, view.Owner.NickName, votedHead);

                                                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                                {
                                                    if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == PhotonNetwork.LocalPlayer.NickName)
                                                    {
                                                        GameObject.Find("VotingGrid").transform.GetChild(i).GetChild(1).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].characterID).GetComponent<MeshRenderer>().material = GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().chosenMesh;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    EnvyCameraOptions[0].SetActive(true);
                                    EnvyCameraOptions[1].SetActive(false);

                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().betTxt.text = "";
                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().roundTxt.text = "";

                                    // Move Horse
                                    if (Input.GetKey(KeyCode.E))
                                    {
                                        // Checking if player is properly interacting with a gun
                                        if (squirtAccess && squirtGun != null)
                                        {
                                            // Checking if interacted gun is the ASSIGNED gun
                                            if (squirtGun.name == squirtGunName)
                                            {
                                                view.RPC("increaseSquirt", RpcTarget.AllBufferedViaServer, squirtGunName);

                                                // once water of gun INTERACTS with target
                                                //if (GameObject.Find(squirtGunName).transform.GetChild(0).GetChild(0).GetComponent<EnvyBullseye>().Bullseye)
                                                if (GameObject.Find(squirtGunName).transform.GetChild(1).GetComponent<EnvyBullseye>().Bullseye)
                                                {
                                                    // GameObject.Find(squirt).GetComponent<EnvySquirter>().correlatingHorse

                                                    view.RPC("moveHorsey", RpcTarget.AllBufferedViaServer, horseName, 0.02f);
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
                                        if (!envySetOneTime)
                                        {
                                            if ((GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count - 1) == GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyResults.Count)
                                            {
                                                view.RPC("nextEnvyRound", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

                                                envySetOneTime = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // WRATH LEVEL
                        else if (SceneManager.GetActiveScene().name == "Wrath")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
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
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count == 2)
                                        {
                                            sceneDecision = 0;
                                        }
                                        else
                                        {
                                            if (!sceneChoiceDecided)
                                            {
                                                sceneDecision = UnityEngine.Random.Range(0, (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count - 1));
                                                sceneChoiceDecided = true;
                                            }
                                        }

                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                    }
                                }
                            }
                        }
                        // SLOTH LEVEL
                        else if (SceneManager.GetActiveScene().name == "Sloth")
                        {
                            if (!GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone)
                            {
                                // Light / Trap Instantiation
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

                                // BEAR TRAP INTERACTION
                                if (!ranOutOfLife)
                                {
                                    // Resetting set trap after it got unset and destroyed!
                                    if (interactedBearTrap == null && gotBearTrapped)
                                    {
                                        freezePlayer = false;
                                        view.RPC("escapedBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                                    }

                                    // ACTIONS
                                    // Got trapped by bear trap
                                    if (gotBearTrapped && interactedBearTrap != null)
                                    {
                                        Vector3 pos = new Vector3(transform.position.x, transform.position.y - trapHeight, transform.position.z);
                                        view.RPC("caughtByBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName, pos, interactedBearTrap.name);
                                        freezePlayer = true;
                                    }
                                    // Opening bear trap
                                    if (!gotBearTrapped && alreadySetBearTrap != null)
                                    {
                                        if (Input.GetKeyDown(KeyCode.E))
                                        {
                                            view.RPC("unhookedBearTrap", RpcTarget.AllBufferedViaServer, view.Owner.NickName, alreadySetBearTrap.name);
                                        }
                                    }
                                }

                                // LIFE SOURCE
                                if (!withinTheLight && lifeSource > 0f)
                                {
                                    // decrease life
                                    if (GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().lifeDrop)
                                    {
                                        lifeSource--;
                                        view.RPC("displayLifePercentage", RpcTarget.AllBufferedViaServer, view.Owner.NickName, (int)lifeSource, false);
                                        GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().lifeDrop = false;
                                        GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().DecreaseLifeForce();
                                    }
                                }
                                else if (lifeSource <= 0f)
                                {
                                    if (!ranOutOfLife)
                                    {
                                        view.RPC("displayLifePercentage", RpcTarget.AllBufferedViaServer, view.Owner.NickName, 0, true);
                                    }
                                }

                                /*else if (withinTheLight && lifeSource > 0f)
                                {
                                    // stop life drop
                                    GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().lifeDrop = true;
                                }
                                else if ((int)lifeSource < 1f)
                                {
                                    // death
                                    lifeSource = 0f;
                                    view.RPC("displayLifePercentage", RpcTarget.AllBufferedViaServer, view.Owner.NickName, 0, true);
                                    ranOutOfLife = true;
                                }*/

                                // game ending - if the results has one less than the total of players means there 1 person left standing which is the winner
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().slothResults.Count >= (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count - 1))
                                    {
                                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count == 2)
                                        {
                                            sceneDecision = 0;
                                        }
                                        else
                                        {
                                            if (!sceneChoiceDecided)
                                            {
                                                sceneDecision = UnityEngine.Random.Range(0, (GameObject.Find("Scene Manager").GetComponent<SceneManage>().minigameLevels.Count - 1));
                                                sceneChoiceDecided = true;
                                            }
                                        }

                                        view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                    }
                                }
                            }
                        }
                        // PRIDE LEVEL
                        else if (SceneManager.GetActiveScene().name == "Pride")
                        {
                            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                            {
                                // timer
                                if (prideTimerRunning)
                                {
                                    if (prideTimer > 1)
                                    {
                                        view.RPC("setPrideTimer", RpcTarget.AllBufferedViaServer, view.Owner.NickName, "" + Mathf.FloorToInt(prideTimer));
                                    }
                                    else
                                    {
                                        view.RPC("setPrideTimer", RpcTarget.AllBufferedViaServer, view.Owner.NickName, "Time's Up");
                                    }

                                    prideTimer -= Time.deltaTime;

                                    if (prideTimer < 0)
                                    {
                                        if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerActionChoice)
                                        {
                                            view.RPC("outOfTime", RpcTarget.AllBufferedViaServer, view.Owner.NickName, true);
                                        }
                                        else
                                        {
                                            view.RPC("outOfTime", RpcTarget.AllBufferedViaServer, view.Owner.NickName, false);
                                        }

                                        prideTimerRunning = false;
                                    }
                                }

                                // game ending - if the results has one less than the total of players means there 1 person left standing which is the winner
                                if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().prideResults.Count >= 1)
                                {
                                    sceneDecision = 0;

                                    view.RPC("setGameWinner", RpcTarget.AllBufferedViaServer, GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().listOfPridePlayers[0]);
                                    view.RPC("endTheGame", RpcTarget.AllBufferedViaServer, view.Owner.NickName, sceneDecision);
                                }
                            }

                            // Picking a cup
                            if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerActionChoice && GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().listOfPridePlayers[GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerTurnTracker] == PhotonNetwork.LocalPlayer.NickName)
                            {
                                if (selectedCup == null)
                                {
                                    GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().selectControl.SetActive(true);
                                    GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().confirmControl.SetActive(false);
                                }
                                else
                                {
                                    GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().confirmControl.SetActive(true);
                                    GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().selectControl.SetActive(false);
                                }

                                // MY TURN
                                // activate mouse clicking
                                if (Input.GetMouseButtonDown(0))
                                {
                                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                                    RaycastHit[] die = Physics.RaycastAll(ray);
                                    foreach (RaycastHit hit in die)
                                    {
                                        if (hit.collider.gameObject.tag == "Chalice")
                                        {
                                            selectedCup = hit.collider.gameObject;
                                            Debug.Log("I hit cup " + selectedCup.name);
                                            break;
                                        }
                                        else if (hit.collider.gameObject.tag == "Ignore")
                                        {
                                            selectedCup = null;
                                            Debug.Log("unselected");
                                        }
                                    }
                                }
                                else if(GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().listOfPridePlayers[GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerTurnTracker] != PhotonNetwork.LocalPlayer.NickName)
                                {
                                    GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().selectControl.SetActive(false);
                                    GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().confirmControl.SetActive(false);
                                }

                                // send in results
                                if (Input.GetKeyDown(KeyCode.E) && selectedCup != null)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerTurnTracker == 0)
                                    {
                                        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().chosenCupText.text = "Cup : " + (GameObject.Find(selectedCup.name).GetComponent<chaliceInfo>().id + 1);

                                        view.RPC("poisoningCup", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GameObject.Find(selectedCup.name).GetComponent<chaliceInfo>().id);
                                    }
                                    else
                                    {
                                        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().chosenCupText.text = "Cup : " + (GameObject.Find(selectedCup.name).GetComponent<chaliceInfo>().id + 1);

                                        view.RPC("drinkingCup", RpcTarget.AllBufferedViaServer, view.Owner.NickName, GameObject.Find(selectedCup.name).GetComponent<chaliceInfo>().id);
                                    }
                                }
                            }

                            if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().resetCups)
                            {
                                view.RPC("resetChosenCup", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                            }
                        }


                        // Level movement
                        if (SceneManager.GetActiveScene().name == "Greed")
                        {
                            if (!freezePlayer && !diceRollFreeze)
                            {
                                MovePlayer();

                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                                    view.RPC("jumpBoyJump", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel);
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Envy")
                        {
                            if (!GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().votingSystem)
                            {
                                MovePlayer();

                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                                    view.RPC("jumpBoyJump", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
                                {
                                    GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().EnvyHorses[i].GetComponent<EnvyHorse>().resetAll();
                                }
                            }
                        }
                        else if (SceneManager.GetActiveScene().name == "Pride")
                        {
                            if (reverseWalk)
                            {
                                PrideMovePlayer();
                            }
                            else
                            {
                                MovePlayer();
                            }
                        }
                        else
                        {
                            if (!freezePlayer)
                            {
                                MovePlayer();

                                if (Input.GetKeyDown(KeyCode.Space) && playerIsGrounded)
                                {
                                    Vector3 vel = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                                    view.RPC("jumpBoyJump", RpcTarget.AllBufferedViaServer, view.Owner.NickName, vel);
                                }
                            }
                        }
                    }
                }
            }
            else if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "game over")
            {
                view.RPC("setUsername", RpcTarget.AllBufferedViaServer, view.Owner.NickName);

                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().countdownLevelCheck)
                {
                    try
                    {
                        for (int x = 0; x < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; x++)
                        {
                            try
                            {
                                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username == GameObject.Find("Scene Manager").GetComponent<SceneManage>().levelsWinner[0])
                                {
                                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                                    //GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(6).gameObject.SetActive(true);
                                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().winnerMesh;
                                }
                                else
                                {
                                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).gameObject.SetActive(true);
                                    GameObject.Find(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].username).transform.GetChild(2).GetChild(GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[x].characterID).GetChild(1).GetComponent<SkinnedMeshRenderer>().material = GameObject.Find("Scene Manager").GetComponent<SceneManage>().deadSkin;
                                }
                            }

                            catch (NullReferenceException e)
                            {
                                Debug.Log("naw cuh");
                            }
                        }
                    }
                    catch (NullReferenceException e) { }

                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().skipCreditsEnding)
                        {
                            view.RPC("finalSceneEnding", RpcTarget.AllBufferedViaServer, view.Owner.NickName);
                        }
                    }
                }

                MovePlayer();

                if (Input.GetKeyDown(KeyCode.Space) && playerIsGrounded)
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
        allowNewDecrease = true;
    }

    void PrideMovePlayer()
    {
        float moveHorizontal = -Input.GetAxisRaw("Horizontal");
        float moveVertical = -Input.GetAxisRaw("Vertical");

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
        rb.MoveRotation(targetRotation);
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
            GameObject.Find(victim).GetComponent<PlayerUserTest>().freezePlayer = true;

            GameObject.Find(victim).GetComponent<PlayerUserTest>().oopsyGotHit = true;
            GameObject.Find(victim).GetComponent<PlayerUserTest>().stunnah();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    public void slippyOffyWrathy(string victim)
    {
        GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().RecordWrathResults(GameObject.Find(victim));
        GameObject.Find(victim).GetComponent<PlayerUserTest>().youLoseTheLevel = true;
        GameObject.Find(victim).transform.position = GameObject.Find("holdingArea").transform.GetChild(0).position;
    }

    public void noMoreLifeSource(string victim)
    {
        GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().RecordSlothResults(GameObject.Find(victim));
        GameObject.Find(victim).GetComponent<PlayerUserTest>().youLoseTheLevel = true;
        GameObject.Find(victim).transform.position = GameObject.Find("holdingArea").transform.GetChild(0).position;
    }

    public void gotMunched(string victim)
    {
        GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().RecordGluttonyResults(GameObject.Find(victim));
        GameObject.Find(victim).GetComponent<PlayerUserTest>().youLoseTheLevel = true;
        GameObject.Find(victim).transform.position = GameObject.Find("holdingArea").transform.GetChild(0).position;
    }

    [PunRPC]
    void YouMunchedThem(string name, string victim)
    {
        try
        {
            GameObject.Find(victim).GetComponent<PlayerUserTest>().oofGotMunched = true;
            GameObject.Find(name).GetComponent<PlayerUserTest>().bigBoyMunch = false;
            GameObject.Find(name).GetComponent<PlayerUserTest>().collectedFoodies = 0;
            GameObject.Find(name).GetComponent<PlayerUserTest>().interactedOpponent = null;
            GameObject.Find(victim).GetComponent<PlayerUserTest>().gotMunched(victim);
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
                GameObject.Find(name).GetComponent<PlayerUserTest>().imDraggingMan = x;
                GameObject.Find(name).GetComponent<PlayerUserTest>().onetimebruvidek = true;
            }
            else if (onetimebruvidek)
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
    void beginTheGame(string name, int firstLvlIndex)
    {
        try
        {
            Debug.Log("scene " + sceneDecision);

            GameObject.Find("Scene Manager").GetComponent<SceneManage>().chosenLevelIndex = firstLvlIndex;

            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Intro_Scene");
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void finalSceneEnding(string name)
    {
        try
        {
            if (!GameObject.Find(name).GetComponent<PlayerUserTest>().skippedTheEndingCredits) 
            {
                GameObject.Find(name).GetComponent<PlayerUserTest>().skippedTheEndingCredits = true;
                GameObject.Find("GameManager").GetComponent<GameOverManager>().SkipCredits();
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

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

                if (SceneManager.GetActiveScene().name == "Sloth")
                {
                    GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find(Player).GetComponent<PlayerUserTest>().lifeSource + "";
                    GameObject.Find(Player).transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
                }
                else if (SceneManager.GetActiveScene().name == "Gluttony" && GameObject.Find(Player).GetComponent<PlayerUserTest>().collectedFoodies >= 1)
                {
                    GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find(Player).GetComponent<PlayerUserTest>().collectedFoodies + "";

                    if (GameObject.Find(Player).GetComponent<PlayerUserTest>().collectedFoodies < 2)
                    {
                        GameObject.Find(Player).transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        GameObject.Find(Player).transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
                        GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
                    }
                    else if (GameObject.Find(Player).GetComponent<PlayerUserTest>().collectedFoodies >= 2)
                    {
                        GameObject.Find(Player).transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        GameObject.Find(Player).transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
                        GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Greed" && (GameObject.Find(Player).GetComponent<PlayerUserTest>().diceRollValue - GameObject.Find(Player).GetComponent<PlayerUserTest>().thrownTracker) > 0)
                {
                    int x = GameObject.Find(Player).GetComponent<PlayerUserTest>().diceRollValue - GameObject.Find(Player).GetComponent<PlayerUserTest>().thrownTracker;
                    GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = x + "";
                    GameObject.Find(Player).transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                }
                else if (SceneManager.GetActiveScene().name == "Pride")
                {
                    GameObject.Find(Player).GetComponent<PlayerUserTest>().username.color = new Color(255, 255, 255, 0);
                }
                else if (SceneManager.GetActiveScene().name == "Lust")
                {
                    if (GameObject.Find(Player).GetComponent<PlayerUserTest>().resetPosition)
                    {
                        GameObject.Find(Player).transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject.Find(Player).transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Envy")
                {
                    //GameObject.Find(Player).GetComponent<PlayerUserTest>().transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find(Player).GetComponent<PlayerUserTest>().hitKeys + "";
                    //GameObject.Find(Player).transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
                }
                else
                {
                    GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                }

                if (SceneManager.GetActiveScene().name == "Gluttony")
                {
                    if (GameObject.Find(Player).GetComponent<PlayerUserTest>().collectedFoodies < 1)
                    {
                        GameObject.Find(Player).transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        GameObject.Find(Player).transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
                        GameObject.Find(Player).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.white;
                    }
                }
            }



            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.transform.LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).GetComponent<PlayerUserTest>().username.transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
            //GameObject.Find(Player).transform.GetChild(0).GetChild(1).LookAt(GameObject.Find("Main Camera").transform);
            //GameObject.Find(Player).transform.GetChild(0).GetChild(1).transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
            GameObject.Find(Player).transform.GetChild(0).LookAt(GameObject.Find("Main Camera").transform);
            GameObject.Find(Player).transform.GetChild(0).transform.rotation = Quaternion.LookRotation(GameObject.Find("Main Camera").transform.forward);
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    public void resettingWheel(float z)
    {
        GameObject.Find("WheelOfFortune").transform.rotation = new Quaternion(0, 0, z, GameObject.Find("WheelOfFortune").transform.rotation.w);
    }

    // WHEEL
    [PunRPC]
    void resetTheWheel(string pname, float val)
    {
        try
        {
            GameObject.Find(pname).GetComponent<PlayerUserTest>().resettingWheel(val);
            GameObject.Find(pname).GetComponent<PlayerUserTest>().goodToReset = false;
        }
        catch (NullReferenceException e) { }
    }

    [PunRPC]
    void startTheWheel(string pname, bool x)
    {
        try
        {
            GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().increasing = x;
            GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().dontStartYet = false;
        }
        catch (NullReferenceException e) { }
    }

    [PunRPC]
    void spinTheWheelMath(float speed, float time)
    {
        try
        {
            GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().currentSpeed = speed;
            GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().transform.Rotate(Vector3.forward, GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().currentSpeed * time);
            //GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>().transform.Rotate(0, 0, rotation);
            //GameObject.Find("WheelOfFortune").GetComponent<WheelSpin>()._rotationSpeed = rotation;
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
    void setGameWinner(string winner)
    {
        try
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameWinner = winner;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void endTheGame(string pName, int chosenLevel)
    {
        try
        {
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().GameplayDone = true;
            GameObject.Find("Scene Manager").GetComponent<SceneManage>().preChosenLevelIndex = chosenLevel;
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
                //GameObject.Find("GameManager").GetComponent<WrathGameplayManager>().RecordWrathResults(GameObject.Find(pName).gameObject);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().deathRecorded = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().slippyOffyWrathy(pName);
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
            GameObject.Find(pName).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = GameObject.Find(pName).GetComponent<PlayerUserTest>().lifeSource + "";

            if (state && !GameObject.Find(pName).GetComponent<PlayerUserTest>().ranOutOfLife)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().ranOutOfLife = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().lifeSource = 0;
                GameObject.Find(pName).transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = "0";
                GameObject.Find(pName).GetComponent<PlayerUserTest>().noMoreLifeSource(pName);
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
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theLight = Instantiate(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LightPrefab, new Vector3(pos.x, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LightParent.transform.position.y, pos.y), Quaternion.identity, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().LightParent.transform);
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
                GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().totalTrapAmount++;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theTrap = Instantiate(GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapPrefab, new Vector3(pos.x, 12f, pos.y), Quaternion.identity, GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().TrapParent.transform);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theTrap.gameObject.name = "BearTrap" + GameObject.Find("GameManager").GetComponent<SlothGameplayManager>().totalTrapAmount;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void caughtByBearTrap(string pName, Vector3 pos, string bearTrap)
    {
        try
        {
            if (!GameObject.Find(bearTrap).GetComponent<SlothObstacle>().oneTimeSet)
            {
                if (!GameObject.Find(bearTrap).GetComponent<BoxCollider>().isTrigger)
                {
                    Destroy(GameObject.Find(bearTrap).GetComponent<BoxCollider>());
                    Destroy(GameObject.Find(bearTrap).GetComponent<Rigidbody>());

                    GameObject.Find(bearTrap).GetComponent<SlothObstacle>().oneTimeSet = true;
                }
            }

            GameObject.Find(bearTrap).tag = "SetBearTrap";
            GameObject.Find(bearTrap).GetComponent<SlothObstacle>().caughtPlayer = GameObject.Find(pName).gameObject;
            GameObject.Find(pName).GetComponent<PlayerUserTest>().gotBearTrapped = true;
            GameObject.Find(bearTrap).GetComponent<SlothObstacle>().trapSet = true;
            GameObject.Find(bearTrap).transform.position = pos;
        }

        catch (NullReferenceException e)
        {
            // error
        }
    }


    [PunRPC]
    void unhookedBearTrap(string pName, string bearTrap)
    {
        try
        {
            GameObject.Find(bearTrap).GetComponent<SlothObstacle>().selfDestruct();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void escapedBearTrap(string pName, bool state)
    {
        try
        {
            if (state)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedBearTrap = null;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().gotBearTrapped = false;
            }
            else
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().alreadySetBearTrap = null;
            }
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
    void munchersTime(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().bigBoyMunch = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void setFoodPosition(Vector2 pos, string pName, int food)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().theFood == null)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theFood = Instantiate(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().TypesOfFood[food], new Vector3(pos.x, 15f, pos.y), Quaternion.identity, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform);
            }

        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void VomitBitch(string victim)
    {
        try
        {
            if (GameObject.Find(victim).GetComponent<PlayerUserTest>().collectedFoodies >= 2)
            {
                GameObject.Find(victim).GetComponent<PlayerUserTest>().vomit = true;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    [PunRPC]
    void setVomitPosition(string pName, Vector3 pos, Vector3 dir, float force, int type)
    //void setVomitPosition(string pName)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().vomit)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood = Instantiate(GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().TypesOfFood[type], new Vector3(pos.x, pos.y + 1f, pos.z), Quaternion.identity, GameObject.Find("GameManager").GetComponent<GluttonyGameplayManager>().FoodParent.transform);

                // dir = dir.normalized;
                //GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood.AddComponent<Rigidbody>();
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood.GetComponent<Rigidbody>().AddForce(dir * force);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood.GetComponent<Rigidbody>().AddTorque(dir * 50);

                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedFoodies--;

                GameObject.Find(pName).GetComponent<PlayerUserTest>().theVomittedFood = null;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().vomit = false;
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
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood != null)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().foodCollected = false;
                Destroy(GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood.gameObject);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().interactedFood = null;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().eatFood = false;
                //GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedFoodies++;
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
                GameObject.Find(pName).GetComponent<PlayerUserTest>().theChip.transform.parent = GameObject.Find(pName).transform.GetChild(4);
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

    /*
    IEnumerator lilWaitAfterChip()
    {
        waitHoldGreed = true;
        Debug.Log("wait");
        yield return new WaitForSeconds(0.3f);
        //throwChipAcces = false;
        waitHoldGreed = false;
        Debug.Log("go");
    }
    */
    [PunRPC]
    void updateChipState(string pName, bool x)
    {
        GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces = x;
    }

    [PunRPC]
    void throwChip(string pName, float f, int pActor, string bucketName, Vector3 dir)
    {
        try
        {
            if (GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Count > 0 && !GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].transform.parent = GameObject.Find("GameManager").GetComponent<GreedGameplayManager>().ChipParent.transform;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].AddComponent<Rigidbody>();

                Vector3 grr = GameObject.Find(bucketName).transform.GetChild(0).position - GameObject.Find(pName).transform.position;
                grr = grr.normalized;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<Rigidbody>().AddForce(grr * f);
                //GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<Rigidbody>().AddTorque(dir * 50);


                //GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().throwChip(GameObject.Find(bucketName).transform.GetChild(0).position, GameObject.Find(pName).transform.position, f);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<ChipScript>().Available = true;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[0].GetComponent<MeshCollider>().isTrigger = false;
                GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.RemoveAt(0);
                GameObject.Find(pName).GetComponent<PlayerUserTest>().thrownTracker++;

                for (int i = 0; i < GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies.Count; i++)
                {
                    GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[i].transform.position = new Vector3(GameObject.Find(pName).transform.GetChild(4).position.x, GameObject.Find(pName).GetComponent<PlayerUserTest>().collectedChipies[i].transform.position.y - 0.5f, GameObject.Find(pName).transform.GetChild(4).position.z);
                }

                GameObject.Find(pName).GetComponent<PlayerUserTest>().throwChipAcces = true;

                Debug.Log("yeee");
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
    void moveHorsey(string pName, float speed)
    {
        try
        {
            // GameObject.Find(squirt).GetComponent<EnvySquirter>().correlatingHorse

            // Side-to-side rotation for horse
            if (!GameObject.Find(pName).GetComponent<EnvyHorse>().finished)
            {
                GameObject.Find(pName).transform.position = Vector3.MoveTowards(GameObject.Find(pName).transform.position, GameObject.Find(pName).GetComponent<EnvyHorse>().finishLinePoint.position, speed);
            }

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

    public void nextEnvyRoundFunction()
    {
        GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().setLoser();
    }

    [PunRPC]
    void nextEnvyRound(string pName)
    {
        try
        {            
            //GameObject.Find(pName).GetComponent<PlayerUserTest>().envySetOneTime = true;
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().votingSystem = true;
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().oneTimeRound = true;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void switchingVotingAndGame(bool x)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<EnvyGameplayManager>().votingSystem = x;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void setVotedHead(string pname, int vote)
    {
        try
        {
            GameObject.Find(pname).GetComponent<PlayerUserTest>().votedHead = vote;
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
                GameObject.Find("GameManager").GetComponent<LustGameplayManager>().keyChosen = true;
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
            GameObject.Find("GameManager").GetComponent<LustGameplayManager>().keyAmountTracker++;

            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == pName)
                {
                    GameObject.Find("fireGrid").transform.GetChild(i).GetChild(2).gameObject.SetActive(true);
                    GameObject.Find("fireGrid").transform.GetChild(i).GetChild(3).gameObject.SetActive(true);
                    GameObject.Find("fireGrid").transform.GetChild(i).GetChild(2).localScale *= 1.5f;
                    break;
                }
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    public void resetTimerAndKey()
    {
        GameObject.Find("GameManager").GetComponent<LustGameplayManager>().holdKeyPick();
    }
    
    [PunRPC]
    void lustOutOfTime(string pName)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<LustGameplayManager>().timerActivated = false;
            GameObject.Find(pName).GetComponent<PlayerUserTest>().resetTimerAndKey();
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    public void callPoisonFunction(string name, int num)
    {
        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().setPoisonedCup(name, num);
    }

    public void callDrinkingFunction(string name, int num)
    {
        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().setDrinkingCup(name, num);
    }
    
    public void callOutOfTimeFunction(bool x)
    {
        GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().timesUpWrapItUp(x);
    }

    // PRIDE

    [PunRPC]
    void poisoningCup(string pName, int num)
    {
        try
        {
            if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerActionChoice) 
            {
                Debug.Log("what the hecks");
                GameObject.Find(pName).GetComponent<PlayerUserTest>().callPoisonFunction(pName, num);
                GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerActionChoice = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    void drinkingCup(string pName, int num)
    {
        try
        {
            if (GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerActionChoice)
            {
                GameObject.Find(pName).GetComponent<PlayerUserTest>().callDrinkingFunction(pName, num);
                GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().playerActionChoice = false;
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void resetChosenCup(string pName)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().selectedCup = null;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
    
    [PunRPC]
    void setPrideTimer(string pName, string time)
    {
        try
        {
            GameObject.Find("GameManager").GetComponent<PrideGameplayManager>().timeStampText.text = time;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
     
    [PunRPC]
    void outOfTime(string pName, bool x)
    {
        try
        {
            GameObject.Find(pName).GetComponent<PlayerUserTest>().callOutOfTimeFunction(x);
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
            Debug.Log("one time foo");
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

        // GREED

        if (other.tag == "ChipZone")
        {
            throwAccess = true;
            bucketNameInteracted = other.transform.parent.name;
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

        // SLOTH
        if (other.tag == "Light")
        {
            withinTheLight = true;
        }
        else
        {
            withinTheLight = false;
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

        
        if (other.tag == "BearTrap" && !gotBearTrapped)
        {
            gotBearTrapped = true;
            interactedBearTrap = other.gameObject;
            //interactedBearTrapName = other.name;
        }
        if (other.tag == "SetBearTrap" && !gotBearTrapped)
        {
            alreadySetBearTrap = other.gameObject;
            //alreadySetBearTrapName = other.name;
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
        if (other.tag == "WrathDeath")
        {
            youLoseTheLevel = true;
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

        if(other.tag == "SetBearTrap")
        {
            alreadySetBearTrap = null;
            //alreadySetBearTrapName = "";
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
        foodCollected = false;
        collectedFoodies = -1;
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
        alreadySetBearTrap = null;
        //alreadySetBearTrapName = "";
        //interactedBearTrapName = "";
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
