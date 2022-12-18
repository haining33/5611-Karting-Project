using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObject banana;
    public GameObject melon;

    public GameObject bananaCanvas;
    public GameObject melonCanvas;
    public GameObject ghostCanvas;


    
    bool ghostActive;



    // Start is called before the first frame update

    Vector3 previousPos;
    Vector3 currPos;
    float updateTime = 0.15f;
    float timeSince;

    void Start()
    {

        bananaCanvas.SetActive(false);
        melonCanvas.SetActive(false);
        ghostCanvas.SetActive(false);
        previousPos = new Vector3(0,0,0);
        currPos = new Vector3(0, 0, 0);
        timeSince = 0;

    }
    private void OnDestroy()
    {
        //release.action.performed -= Release;
    }

    // Update is called once per frame
    void Update()
    {
        currPos = this.transform.position;
        timeSince += Time.deltaTime;
        if (timeSince > updateTime) {
            timeSince = 0;
            previousPos = currPos;
        }
    }



    void throwBanana()
    {
        if(bananaCanvas.activeSelf == true)
        {
            GameObject a;
            a = Instantiate(banana) as GameObject;
            a.transform.position = this.transform.position;
            a.GetComponent<Rigidbody>().velocity = (currPos - previousPos) / Time.deltaTime;
        }
    }

    void throwMelon()
    {
        if (melonCanvas.activeSelf == true)
        {
            GameObject a;
            a = Instantiate(melon) as GameObject;
            a.transform.position = this.transform.position;
            a.GetComponent<Rigidbody>().velocity = this.transform.forward * 7.0f;
        }
    }

    //void Release(InputAction.CallbackContext context)
    //{
    //    throwBanana();
    //    throwMelon();
    //    GhostGrab();
    //    bananaCanvas.SetActive(false);
    //    melonCanvas.SetActive(false);
    //    ghostCanvas.SetActive(false);
    //}


}
