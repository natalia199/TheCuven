using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    Ray ray;
    public GameObject Target;
    public GameObject previousTarg;
    public GameObject spotLight;
    public GameObject charInfo;
    public Material clear;
    public float babyLift;
    public float regPos;

    public List<GameObject> characters = new List<GameObject>();

    void Start()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        previousTarg = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] die = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in die) {
                Debug.Log("hit " + hit.transform.gameObject.name);
                if (hit.collider.gameObject.tag == "CharacterChoice")
                {
                    if (previousTarg != null)
                    {
                        previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, regPos, previousTarg.transform.position.z);
                    }

                    previousTarg = hit.collider.gameObject;

                    charInfo.SetActive(true);
                    charInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = hit.collider.gameObject.name;

                    Target.GetComponent<MeshRenderer>().material = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    hit.collider.gameObject.transform.position = new Vector3(hit.collider.gameObject.transform.position.x, babyLift, hit.collider.gameObject.transform.position.z);
                    spotLight.SetActive(true);
                    break;
                }
                else if (hit.collider.gameObject.tag == "Ignore")
                {
                    if (previousTarg != null)
                    {
                        previousTarg.transform.position = new Vector3(previousTarg.transform.position.x, regPos, previousTarg.transform.position.z);
                    }

                    previousTarg = null;
                    charInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                    charInfo.SetActive(false);

                    Target.GetComponent<MeshRenderer>().material = clear;
                    spotLight.SetActive(false);
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


    }
}
