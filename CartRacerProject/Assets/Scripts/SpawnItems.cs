using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public GameObject banana;
    public GameObject melon;

    public GameObject bananaCanvas;
    public GameObject melonCanvas;




    // Start is called before the first frame update

    Vector3 previousPos;
    Vector3 currPos;
    float updateTime = 0.15f;
    float timeSince;

    void Start()
    {

        bananaCanvas.SetActive(false);
        melonCanvas.SetActive(false);
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
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Release();
        }

    }



    void throwBanana()
    {
        if(bananaCanvas.activeSelf == true)
        {
            GameObject a;
            a = Instantiate(banana) as GameObject;
            a.transform.position = this.transform.position;
            a.GetComponent<Rigidbody>().velocity = this.transform.forward * 7.0f;
        }
    }

    //void throwMelon()
    //{
    //    if (melonCanvas.activeSelf == true)
    //    {
    //        GameObject a;
    //        a = Instantiate(melon) as GameObject;
    //        a.transform.position = this.transform.position;
    //        //this.transform.forward * 7.0f;
    //        a.GetComponent<Rigidbody>().velocity = this.transform.forward * 7.0f;
    //    }
    //}

    void Release()
    {
        throwBanana();
        //throwMelon();
        bananaCanvas.SetActive(false);
        melonCanvas.SetActive(false);
    }


}
