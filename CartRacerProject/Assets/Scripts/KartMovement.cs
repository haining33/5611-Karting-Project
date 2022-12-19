using UnityEngine;
using System.Collections;

public class KartMovement : MonoBehaviour
{
    private GameObject KartBody;

    private float direction;
    private float currentSteerAngle;

    public GameObject melonCanvas;
    public float motorForce;
    public float maxSteerAngle;

    public float maxValue;
    public float minValue;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;

    public Transform frontLeftWheelTransform;
    public Transform frontRightWheeTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;



    float temp;
    float temp1;
    float temp2;
    float temp3;
    float slowtime = Mathf.Infinity;
    float acctime = Mathf.Infinity;
    float duration = 2;
    float steeringRange;
    private void Start()
    {
        KartBody = GameObject.FindGameObjectWithTag("MainCamera");
        temp = motorForce;
        temp1 = motorForce / 2;
        temp2 = motorForce / 4;
        temp3 = motorForce * 3;
        direction = 0;
    }

    private void Update()
    {
        HandleMotor();
        Steer();
        UpdateWheels();

        if (melonCanvas.activeSelf == true && Input.GetKey(KeyCode.Space))
        {
            acctime = 0f;
        }
        if (slowtime < duration)
        {
            motorForce = temp2;
            slowtime += Time.deltaTime;
            
            if (slowtime >= duration)
            {
                
                //print(2);
                motorForce = temp;
                print(motorForce);
            }
        }
        if (acctime < duration)
        {
            motorForce = temp3;
            acctime += Time.deltaTime;
            //print(motorForce);
            if (acctime >= duration)
            {
                //print(2);
                print(motorForce);
                motorForce = temp;
            }
        }
        if (Input.GetKey("a"))
        {
            if (steeringRange > -1)
            {
                steeringRange -= 0.005f;
            }

        }
        if (Input.GetKey("d"))
        {
            if (steeringRange < 1)
            {
                steeringRange += 0.005f;
            }
        }
        if (Input.GetKey("w"))
        {
            //if (direction < 0.02)
            //{
            //    print(direction);
            //    direction += 0.0001f;
            //}
            direction = 1f;
        }
        if (Input.GetKey("s"))
        {
            //if (direction > -0.02)
            //{
            //    print(direction);
            //    direction -= 0.0001f;
            //}
            direction = -1f;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if(motorForce < 100)
            {
                motorForce *= 2;
                temp = motorForce;
            }
            
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (motorForce > 10)
            {
                motorForce /= 2;
                temp = motorForce;
            }
            
        }

    }

    private void HandleMotor()
    {

        frontLeftWheelCollider.motorTorque = direction * motorForce;
        frontRightWheelCollider.motorTorque = direction * motorForce;
    }

    private void Steer()
    {
        //float steeringNormal = Mathf.InverseLerp(minValue, maxValue, steeringWheel.transform.localRotation.x);
        //steeringRange = Mathf.Lerp(1, -1, steeringNormal);

        currentSteerAngle = maxSteerAngle * steeringRange;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }
    //private void Lever()
    //{
    //    float leverNormal = Mathf.InverseLerp(-35, 35, speedLever.transform.localRotation.x);
    //    float leverRange = Mathf.Lerp(-1, 1, leverNormal);
    //    direction = leverRange;
    //}

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheeTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Box>())
        {
            if(KartBody.GetComponent<SpawnItems>().bananaCanvas.activeSelf == false && KartBody.GetComponent<SpawnItems>().melonCanvas.activeSelf == false)
            {
                int itemNum = Random.Range(0, 2);
                if (itemNum == 0)
                {
                    KartBody.GetComponent<SpawnItems>().bananaCanvas.SetActive(true);
                }
                else if (itemNum == 1)
                {
                    KartBody.GetComponent<SpawnItems>().melonCanvas.SetActive(true);
                }
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.GetComponent<Banana>())
        {
            Destroy(collision.gameObject);
            slowtime = 0f;
        }
        else if (collision.gameObject.GetComponent<Watermelon>())
        {
            Destroy(collision.gameObject);
            acctime = 0f;
        }
    }

}
