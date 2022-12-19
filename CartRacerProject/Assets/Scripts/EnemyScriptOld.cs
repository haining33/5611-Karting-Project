using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyScriptOld : MonoBehaviour
{

    public int storedItem = 0;
    bool cartStopped;
    public Transform location1;
    public Transform location2;
    public Transform location3;
    private Transform location;
    private NavMeshAgent agent;
    private Stack locationList;

    public void Awake()
    {
        locationList = new Stack();
        locationList.Push(location1);
        locationList.Push(location2);
        locationList.Push(location3);
        agent = GetComponent<NavMeshAgent>();
        location = (Transform)locationList.Pop();
    }

    private void Update()
    {

        Vector3 targetDirection = location.position - this.transform.position;
        float mag = targetDirection.magnitude;
        agent.destination = location.position;
        if (locationList.Count > 0 && mag < 2.0f)
        {
            location = (Transform)locationList.Pop();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Box>())
        {
            storedItem = Random.Range(0, 2);

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.GetComponent<Banana>())
        {
            Destroy(collision.gameObject);
            cartStopped = true;
        }
        else if (collision.gameObject.GetComponent<Watermelon>())
        {
            Destroy(collision.gameObject);
            cartStopped = true;
        }
    }

}

//Vector3[] driveTargets;
//int currentTarget;
//float distThreshold = 2.0f;
//float maxY;

//public float kartSpeed = 5.0f;
//

//bool cartStopped;
//float stopTime;
//float stopTimeLimit;

//private void Start()
//{
//    maxY = transform.position.y;
//    cartStopped = false;
//    stopTime = 0.0f;
//    stopTimeLimit = 2.0f;


//    currentTarget = 0;
//    driveTargets = new Vector3[] {new Vector3(0.970000029f, 0.5f, 39.4625854f),
//                                    new Vector3(-0.5f,0.5f,47.2999992f),
//                                    new Vector3(-2.41000009f,0.5f,50.6199989f),
//                                    new Vector3(-6.96000004f,0.5f,55.3499985f),
//                                    new Vector3(-12.6199999f,0.5f,58.0600014f),
//                                    new Vector3(-32.9000015f,0.5f,58.5299988f),
//                                    new Vector3(-40.2000008f,0.5f,55.9500008f),
//                                    new Vector3(-45.7700005f,0.5f,50.6899986f),
//                                    new Vector3(-49.0800018f,0.5f,41.2599983f),
//                                    new Vector3(-49.1199989f,0.5f,9.23999977f),
//                                    new Vector3(-50.4199982f,0.5f,4.88999987f),
//                                    new Vector3(-53.5099983f,0.5f,1.42999995f),
//                                    new Vector3(-58.5900002f,0.5f,-0.25f),
//                                    new Vector3(-90.0500031f,0.5f,-0.150000006f),
//                                    new Vector3(-100.839996f,0.5f,-3.49000001f),
//                                    new Vector3(-106.720001f,0.5f,-10.3000002f),
//                                    new Vector3(-108.949997f,0.5f,-18.3400002f),
//                                    new Vector3(-108.940002f,0.5f,-34.7799988f),
//                                    new Vector3(-105.900002f,0.5f,-43.7099991f),
//                                    new Vector3(-101.010002f,0.5f,-48.6800003f),
//                                    new Vector3(-96.1100006f,0.5f,-51.1800003f),
//                                    new Vector3(-88.7799988f,0.5f,-52.2400017f),
//                                    new Vector3(-18.1399994f,0.5f,-52.1899986f),
//                                    new Vector3(-10.1199999f,0.5f,-50.4599991f),
//                                    new Vector3(-4.63999987f,0.5f,-46.8100014f),
//                                    new Vector3(-0.839999974f,0.5f,-41.5f),
//                                    new Vector3(0.99000001f,0.5f,-33.8899994f)};
//}


//private void Update()
//{

//    if (cartStopped)
//    {
//        stopTime += Time.deltaTime;
//        if (stopTime > stopTimeLimit) {
//            stopTime = 0;
//            cartStopped = false;
//        }
//    }
//    else
//    {

//        Vector3 targetDirection = driveTargets[currentTarget] - this.transform.position;

//        Vector3 newDirection = Vector3.RotateTowards(transform.forward, Vector3.Normalize(targetDirection), 0.35f * Time.deltaTime, 0.0f);
//        transform.rotation = Quaternion.LookRotation(newDirection);

//        transform.position += transform.forward * (kartSpeed * Time.deltaTime);
//        if (transform.position.y != maxY)
//        {
//            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
//        }

//        float distToTarget = (targetDirection).magnitude;

//        if (distToTarget < distThreshold || Vector3.Dot(targetDirection, transform.forward) < 0)
//        {
//            currentTarget++;
//            if (currentTarget >= driveTargets.Length)
//            {
//                currentTarget = 0;
//            }
//        }
//    }


//}