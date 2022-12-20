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
    private float initSpeed;
    private float stopTime;
    private float stopTimeLimit;
    public void Awake()
    {
        locationList = new Stack();
        locationList.Push(location1);
        locationList.Push(location2);
        locationList.Push(location3);
        agent = GetComponent<NavMeshAgent>();
        location = (Transform)locationList.Pop();
        initSpeed = agent.speed;
        stopTime = 0.0f;
        stopTimeLimit = 2.0f;
    }

    private void Update()
    {

        Vector3 targetDirection = location.position - this.transform.position;
        float mag = targetDirection.magnitude;
        agent.destination = location.position;
        //if the enemy kart hit the banana, the kart will stop;
        if (cartStopped)
        {
            stopTime += Time.deltaTime;
            agent.speed = 0;
            if (stopTime > stopTimeLimit)
            {
                stopTime = 0;
                cartStopped = false;
                agent.speed = initSpeed;
            }
        }
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
    }

}
