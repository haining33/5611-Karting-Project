using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GhostGrabber : MonoBehaviour
{
    bool active;

    float d = 0.4f;
    float previousDistance;

    float ghostTimeLimit = 7.0f;
    float currentGhostTime;

    public InputActionProperty steal;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
        previousDistance = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) {

            string grabberName = "controller_l";
            string controllerName = "LeftHand Controller";
            Vector3 grabberPos = GameObject.Find(controllerName).transform.position;

            Vector3 cameraPos = GameObject.Find("Main Camera").transform.position;

            Vector3 distanceVec = (new Vector3(grabberPos.x, 0, grabberPos.z) - new Vector3(cameraPos.x, 0, cameraPos.z));



            if (distanceVec.magnitude > d)
            {
                float extendedDistance = Mathf.Abs(distanceVec.magnitude - d);
                float extensionFactor = Mathf.Exp(extendedDistance * 8) - 1;

                Vector3 baseLocalPos = GameObject.Find(grabberName).transform.localPosition;
                Vector3 newLocalPos = new Vector3(baseLocalPos.x, baseLocalPos.y, extensionFactor);

                previousDistance = distanceVec.magnitude;

                GameObject.Find(grabberName).transform.localPosition = newLocalPos;
            }

            currentGhostTime += Time.deltaTime;
            if (currentGhostTime > ghostTimeLimit) {
                currentGhostTime = 0;
                active = false;
                GameObject.Find("controller_l").transform.localPosition = new Vector3(0, 0, 0);
            }

            float distToE1 = (GameObject.Find("controller_l").transform.position - enemy1.transform.position).magnitude;
            float distToE2 = (GameObject.Find("controller_l").transform.position - enemy2.transform.position).magnitude;
            float distToE3 = (GameObject.Find("controller_l").transform.position - enemy3.transform.position).magnitude;

            if (distToE1 < 3.0f) {
                GetComponent<BoxCollider>().enabled = false;
                currentGhostTime = 0;
                active = false;
                GameObject.Find("controller_l").transform.localPosition = new Vector3(0, 0, 0);
                if (enemy1.GetComponent<EnemyScript>().storedItem == 0)
                {
                    GameObject.FindGameObjectWithTag("Left").GetComponent<SpawnItems>().bananaCanvas.SetActive(true);
                }
                else {
                    GameObject.FindGameObjectWithTag("Left").GetComponent<SpawnItems>().melonCanvas.SetActive(true);
                }
                
            }

            if (distToE2 < 3.0f)
            {
                GetComponent<BoxCollider>().enabled = false;
                currentGhostTime = 0;
                active = false;
                GameObject.Find("controller_l").transform.localPosition = new Vector3(0, 0, 0);
                if (enemy2.GetComponent<EnemyScript>().storedItem == 0)
                {
                    GameObject.FindGameObjectWithTag("Left").GetComponent<SpawnItems>().bananaCanvas.SetActive(true);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Left").GetComponent<SpawnItems>().melonCanvas.SetActive(true);
                }

            }

            if (distToE3 < 3.0f)
            {
                GetComponent<BoxCollider>().enabled = false;
                currentGhostTime = 0;
                active = false;
                GameObject.Find("controller_l").transform.localPosition = new Vector3(0, 0, 0);
                if (enemy3.GetComponent<EnemyScript>().storedItem == 0)
                {
                    GameObject.FindGameObjectWithTag("Left").GetComponent<SpawnItems>().bananaCanvas.SetActive(true);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Left").GetComponent<SpawnItems>().melonCanvas.SetActive(true);
                }

            }


        }

    }

    public void Activate() {
        active = true;
    }
}
