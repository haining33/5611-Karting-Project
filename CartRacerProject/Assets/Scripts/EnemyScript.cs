using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{
    Vector3[] driveTargets;
    int currentTarget;
    float distThreshold = 2.0f;
    float maxY;

    float k_goal = 0.1f;
    float k_avoid = 0.1f;
    float agentRad = 2f;
    float goalSpeed = 0.1f;

    public int numAgents = 2;

    public float kartSpeed = 5.0f;
    public int storedItem = 0;

    bool cartStopped;
    float stopTime;
    float stopTimeLimit;

    public GameObject childPrefab;
    private Vector3[] agentPos = new Vector3[3];
    private Vector3[] agentVel = new Vector3[3];
    private Vector3[] agentAcc = new Vector3[3];

    private GameObject[] children = new GameObject[3];



    private void Start()
    {
        maxY = transform.position.y;
        cartStopped = false;
        stopTime = 0.0f;
        stopTimeLimit = 2.0f;
        currentTarget = 0;

        this.agentPos[0] = this.transform.position;
        for (int i = 1; i < numAgents; i++)
        {
            Vector3 spawnLocation = new Vector3(this.transform.position.x + i + 1, this.transform.position.y, this.transform.position.z);
            GameObject temp = Instantiate(childPrefab, spawnLocation, this.transform.rotation);
            this.children[i] = temp;
            this.agentPos[i] = temp.transform.position;
        }

        driveTargets = new Vector3[] {new Vector3(0.970000029f, 0.5f, 39.4625854f),
                                        new Vector3(-0.5f,0.5f,47.2999992f),
                                        new Vector3(-2.41000009f,0.5f,50.6199989f),
                                        new Vector3(-6.96000004f,0.5f,55.3499985f),
                                        new Vector3(-12.6199999f,0.5f,58.0600014f),
                                        new Vector3(-32.9000015f,0.5f,58.5299988f),
                                        new Vector3(-40.2000008f,0.5f,55.9500008f),
                                        new Vector3(-45.7700005f,0.5f,50.6899986f),
                                        new Vector3(-49.0800018f,0.5f,41.2599983f),
                                        new Vector3(-49.1199989f,0.5f,9.23999977f),
                                        new Vector3(-50.4199982f,0.5f,4.88999987f),
                                        new Vector3(-53.5099983f,0.5f,1.42999995f),
                                        new Vector3(-58.5900002f,0.5f,-0.25f),
                                        new Vector3(-90.0500031f,0.5f,-0.150000006f),
                                        new Vector3(-100.839996f,0.5f,-3.49000001f),
                                        new Vector3(-106.720001f,0.5f,-10.3000002f),
                                        new Vector3(-108.949997f,0.5f,-18.3400002f),
                                        new Vector3(-108.940002f,0.5f,-34.7799988f),
                                        new Vector3(-105.900002f,0.5f,-43.7099991f),
                                        new Vector3(-101.010002f,0.5f,-48.6800003f),
                                        new Vector3(-96.1100006f,0.5f,-51.1800003f),
                                        new Vector3(-88.7799988f,0.5f,-52.2400017f),
                                        new Vector3(-18.1399994f,0.5f,-52.1899986f),
                                        new Vector3(-10.1199999f,0.5f,-50.4599991f),
                                        new Vector3(-4.63999987f,0.5f,-46.8100014f),
                                        new Vector3(-0.839999974f,0.5f,-41.5f),
                                        new Vector3(0.99000001f,0.5f,-33.8899994f)};
        //Set initial velocities to cary agents towards their goals
        for (int i = 0; i < agentPos.Length; i++)
        {
            agentVel[i] = driveTargets[currentTarget] - agentPos[i];
            if (agentVel[i].magnitude > 0)
            {
                agentVel[i] = setToLength(agentVel[i], goalSpeed);
            }
        }

    }


    private float computeTTC(Vector3 pos1, Vector3 vel1, float radius1, Vector3 pos2, Vector3 vel2, float radius2)
    {
        float collisionDistance = radius1 + radius2;
        Vector3 dir = vel1 - vel2;
        float ttc = rayCircleIntersectTime(pos2, collisionDistance, pos1, dir);
        return ttc;
    }

    //No.3 avoidance force
    private Vector3 computeAgentForces(int id)
    {
        Vector3 acc = new Vector3(0, 0, 0);
        Vector3 goal_Vel = driveTargets[currentTarget] - this.transform.position;

        if (goal_Vel.magnitude > goalSpeed)
        {
            goal_Vel = setToLength(goal_Vel, goalSpeed);
        }
        Vector3 goal_force = (goal_Vel - agentVel[id]) * k_goal;
        acc = acc + goal_force;

        if (goal_Vel.magnitude < 1) return acc;

        for (int i = 0; i < numAgents; i++)
        {
            if(i != id)
            {
                float ttc = computeTTC(agentPos[id], agentVel[id], agentRad, agentPos[i], agentVel[i], agentRad);

                Vector3 A_NextPos = agentPos[id] + agentVel[id] * ttc;
                Vector3 B_NextPos = agentPos[i] + agentVel[i] * ttc;

                Vector3 nextDir = Vector3.Normalize(A_NextPos - B_NextPos);

                acc = acc + nextDir * (k_avoid * (1 / ttc));

            }
        }
        return acc;
    }

    private void moveAgent()
    {
        //Compute accelerations for every agents
        for (int i = 0; i < numAgents; i++)
        {
            agentAcc[i] = computeAgentForces(i);
        }
        //Update position and velocity using (Eulerian) numerical integration
        for (int i = 0; i < numAgents; i++)
        {
            agentVel[i] = agentVel[i] + agentAcc[i] * Time.deltaTime;
            agentPos[i] = new Vector3(agentPos[i].x, 0.5f, agentPos[i].z);
            agentPos[i] = agentPos[i] + new Vector3(agentVel[i].x, 0f, agentVel[i].z) * Time.deltaTime;
            if(i == 0)
            {
                this.transform.position = agentPos[i];
            }
            else
            {
                children[i].transform.position = agentPos[i];
            }
        }
    }

    private Vector3 setToLength(Vector3 v, float newL)
    {
        v = v.normalized;
        return new Vector3(newL / v.x, newL / v.y, newL / v.z);
    }
    private float rayCircleIntersectTime(Vector3 center, float r, Vector3 l_start, Vector3 l_dir)
    {
        Vector3 toCircle = center - l_start;
        float a = l_dir.magnitude * l_dir.magnitude;
        float b = -2 * Vector3.Dot(l_dir, toCircle);
        float c = Vector3.Dot(toCircle, toCircle) - (r*r);
        float d = b * b - 4 * a * c;

        if (d >= 0)
        {
            //If d is positive we know the line is colliding
            float t = (-b - Mathf.Sqrt(d)) / (2 * a); //Optimization: we typically only need the first collision!
            if (t >= 0)
            {
                return t;
            }
            return -1f;
        }
        return -1f;
    }

    private void Update()
    {

        if (cartStopped)
        {
            stopTime += Time.deltaTime;
            if (stopTime > stopTimeLimit) {
                stopTime = 0;
                cartStopped = false;
            }
        }
        else
        {

            Vector3 targetDirection = driveTargets[currentTarget] - this.transform.position;

            Vector3 newDirection = Vector3.RotateTowards(transform.forward, Vector3.Normalize(targetDirection), 0.35f * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);

            //transform.position += transform.forward * (kartSpeed * Time.deltaTime);
            //if (transform.position.y != maxY)
            //{
            //    transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
            //}
            moveAgent();
            print(agentPos[1]);
            float distToTarget = (targetDirection).magnitude;

            if (distToTarget < distThreshold || Vector3.Dot(targetDirection, transform.forward) < 0)
            {
                currentTarget++;
                if (currentTarget >= driveTargets.Length)
                {
                    currentTarget = 0;
                }
            }
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
