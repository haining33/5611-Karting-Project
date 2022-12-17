using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour
{
    public GameObject box;
    List<Vector3> part = new List<Vector3>();
    //Vector3 spawnLocation1 = new Vector3(1f, 0.5f, 10f);
    //Vector3 spawnLocation2 = new Vector3(1f, 0.5f, 25f);
    // Start is called before the first frame update
    void Start()
    {

        part.Add(new Vector3(-48f, 0.5f, 43f));
        part.Add(new Vector3(1f, 0.5f, 50f));
        part.Add(new Vector3(-30f, 0.5f, 60f));
        part.Add(new Vector3(-0.389999986f, 0.5f, 1.73000002f));
    }

    void spawnBox(Vector3 position)
    {
        GameObject a;
        a = Instantiate(box) as GameObject;
        a.transform.position = position;
        a.transform.Rotate(new Vector3(0, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (Vector3 i in part)
        {
            Collider[] hitColliders = Physics.OverlapSphere(i, 3.0f);
            if (hitColliders.Length == 2)
            {
                spawnBox(i);
            }
        }
    }
}
