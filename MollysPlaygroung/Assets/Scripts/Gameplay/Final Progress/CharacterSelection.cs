using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class CharacterSelection : MonoBehaviour
{
    Ray ray;
    public GameObject previousTarg;

    PhotonView view;

    bool readyToPost = false;

    void Start()
    {
        view = GetComponent<PhotonView>();

        if (view.IsMine)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            previousTarg = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().currentState == "characterSelection") 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit[] die = Physics.RaycastAll(ray);
                    foreach (RaycastHit hit in die)
                    {
                        Debug.Log("hit " + hit.transform.gameObject.name);
                        if (hit.collider.gameObject.tag == "CharacterChoice")
                        {
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().selectControl.SetActive(false);
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().confirmControl.SetActive(true);

                            if (previousTarg != null)
                            {
                                for (int x = 0; x < GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).childCount; x++)
                                {
                                    if (GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).GetChild(x).gameObject.activeInHierarchy)
                                    {
                                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).GetChild(x).gameObject.SetActive(false);
                                    }

                                }

                                previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, GameObject.Find("GameManager").GetComponent<SelectionGameManager>().regPos, previousTarg.transform.position.z);
                            }

                            previousTarg = hit.collider.gameObject;

                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.SetActive(true);
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hit.collider.gameObject.GetComponent<BabiesCharacteristics>().MyName;
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = hit.collider.gameObject.GetComponent<BabiesCharacteristics>().AboutMe;

                            for (int x = 0; x < GameObject.Find("GameManager").GetComponent<SelectionGameManager>().characters.Count; x++)
                            {
                                if (hit.collider.gameObject.name == GameObject.Find("GameManager").GetComponent<SelectionGameManager>().characters[x].name) {
                                    //GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.GetComponent<MeshRenderer>().material = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                                    GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).GetChild(x).gameObject.SetActive(true);
                                }

                            }

                            hit.collider.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, GameObject.Find("GameManager").GetComponent<SelectionGameManager>().babyLift, hit.collider.gameObject.transform.position.z);
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().spotLight.SetActive(true);

                            break;
                        }
                        else if (hit.collider.gameObject.tag == "Ignore")
                        {
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().selectControl.SetActive(true);
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().confirmControl.SetActive(false);
                            
                            if (previousTarg != null)
                            {
                                previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, GameObject.Find("GameManager").GetComponent<SelectionGameManager>().regPos, previousTarg.transform.position.z);
                            }
                            previousTarg = null;
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.SetActive(false);

                            for (int x = 0; x < GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).childCount; x++)
                            {
                                if (GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).GetChild(x).gameObject.activeInHierarchy)
                                {
                                    GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.transform.GetChild(1).GetChild(x).gameObject.SetActive(false);
                                }

                            }

                            GameObject.Find("GameManager").GetComponent<SelectionGameManager>().spotLight.SetActive(false);

                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (previousTarg != null)
                    {
                        Debug.Log("pressy press");

                        readyToPost = true;

                        GameObject.Find("Scene Manager").GetComponent<SceneManage>().switchCamera(true);
                    }
                }
            }

            if (readyToPost)
            {
                view.RPC("settingSelectedCharacter", RpcTarget.AllBufferedViaServer, view.Owner.NickName, previousTarg.name);
                readyToPost = false;
            }
        }
    }

    [PunRPC]
    public void settingSelectedCharacter(string pname, string chosenChar)
    {
        try
        {
            for (int i = 0; i < GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame.Count; i++)
            {
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == pname && !GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].variablesSet)
                {
                    GameObject.Find("Scene Manager").GetComponent<SceneManage>().updatePlayerCharacter(pname, chosenChar, i);
                    break;
                }
            }
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }

    [PunRPC]
    public void resettingCharactersPos(string pname, Vector3 pos)
    {
        try
        {
            GameObject.Find(pname).transform.position = pos;
        }
        catch (NullReferenceException e)
        {
            // error
        }
    }
}
