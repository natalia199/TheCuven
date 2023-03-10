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
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] die = Physics.RaycastAll(ray);
                foreach (RaycastHit hit in die)
                {
                    Debug.Log("hit " + hit.transform.gameObject.name);
                    if (hit.collider.gameObject.tag == "CharacterChoice")
                    {
                        // OP 2
                        /*
                        if (previousTarg != null)
                        {
                            for (int i = 0; i < characters.Count; i++)
                            {
                                if (characters[i].name == previousTarg.name)
                                {
                                    previousTarg.transform.GetChild(0).gameObject.SetActive(false);

                                    if (i < 4)
                                    {
                                        previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, topRowSit, previousTarg.transform.position.z);
                                        break;
                                    }
                                    else if (i >= 4)
                                    {
                                        previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, bottomRowSit, previousTarg.transform.position.z);
                                        break;
                                    }
                                }
                            }
                        }

                        previousTarg = hit.collider.gameObject;

                        for (int i = 0; i < characters.Count; i++)
                        {
                            hit.collider.transform.GetChild(0).gameObject.SetActive(true);

                            if (characters[i].name == hit.collider.gameObject.name)
                            {
                                if (i < 4)
                                {
                                    hit.collider.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, topRowStand, hit.collider.gameObject.transform.position.z);
                                    break;
                                }
                                else if (i >= 4)
                                {
                                    hit.collider.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, bottomRowStand, hit.collider.gameObject.transform.position.z);
                                    break;
                                }
                            }
                        }
                        */

                        // OP 3

                        if (previousTarg != null)
                        {
                            previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, GameObject.Find("GameManager").GetComponent<SelectionGameManager>().regPos, previousTarg.transform.position.z);
                        }

                        previousTarg = hit.collider.gameObject;

                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.SetActive(true);
                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hit.collider.gameObject.name;

                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.GetComponent<MeshRenderer>().material = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                        hit.collider.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, GameObject.Find("GameManager").GetComponent<SelectionGameManager>().babyLift, hit.collider.gameObject.transform.position.z);
                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().spotLight.SetActive(true);

                        break;
                    }
                    else if (hit.collider.gameObject.tag == "Ignore")
                    {
                        // OP 2
                        /*
                        if (previousTarg != null)
                        {
                            previousTarg.transform.GetChild(0).gameObject.SetActive(false);

                            for (int i = 0; i < characters.Count; i++)
                            {
                                if (characters[i].name == previousTarg.name)
                                {
                                    if (i < 4)
                                    {
                                        previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, topRowSit, previousTarg.transform.position.z);
                                        break;
                                    }
                                    else if (i >= 4)
                                    {
                                        previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, bottomRowSit, previousTarg.transform.position.z);
                                        break;
                                    }
                                }
                            }
                        }                    

                        previousTarg = null;
                        */

                        // OP 3

                        if (previousTarg != null)
                        {
                            previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, GameObject.Find("GameManager").GetComponent<SelectionGameManager>().regPos, previousTarg.transform.position.z);
                        }
                        previousTarg = null;
                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().charInfo.SetActive(false);

                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().Target.GetComponent<MeshRenderer>().material = GameObject.Find("GameManager").GetComponent<SelectionGameManager>().clear;
                        GameObject.Find("GameManager").GetComponent<SelectionGameManager>().spotLight.SetActive(false);

                    }
                }
            }

            /*
            if (Target != null)
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    if (characters[i].name == Target.name)
                    {
                        if (i < 3) {
                            spotLight.transform.position = new Vector3(characters[i].transform.position.x, 2, characters[i].transform.position.z);
                            break;
                        }
                        else if (i >= 3 && i <= 5)
                        {
                            spotLight.transform.position = new Vector3(characters[i].transform.position.x, 6, characters[i].transform.position.z);
                            break;
                        }
                        else if(i > 5)
                        {
                            spotLight.transform.position = new Vector3(characters[i].transform.position.x, 10, characters[i].transform.position.z);
                            break;
                        }
                    }
                }
            }*/


            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (previousTarg != null)
                {
                    Debug.Log("pressy press");

                    view.RPC("settingSelectedCharacter", RpcTarget.AllBufferedViaServer, view.Owner.NickName, previousTarg.name);
                }
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
                if (GameObject.Find("Scene Manager").GetComponent<SceneManage>().playersInGame[i].username == pname)
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
}
