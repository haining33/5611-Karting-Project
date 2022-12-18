using UnityEngine;
using System.Collections;

public class KartMovement : MonoBehaviour
{
    private GameObject KartBody;

    private float verticalInput;
    private float currentSteerAngle;

    public GameObject melonCanvas;
    public float motorForce;
    public float maxSteerAngle;
    public HingeJoint steeringWheel;
    public HingeJoint speedLever;

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

    public Transform steeringTransform;

    bool shouldSlow;
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
        verticalInput = 0;
    }

    private void Update()
    {
        HandleMotor();
        Steer();
        UpdateWheels();
        if (melonCanvas.activeSelf == true)
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
                steeringRange -= 0.003f;
            }

        }
        if (Input.GetKey("d"))
        {
            if (steeringRange < 1)
            {
                steeringRange += 0.003f;
            }
        }
        if (Input.GetKey("w"))
        {
            //if (verticalInput < 0.02)
            //{
            //    print(verticalInput);
            //    verticalInput += 0.0001f;
            //}
            verticalInput = 0.01f;
        }
        if (Input.GetKey("s"))
        {
            //if (verticalInput > -0.02)
            //{
            //    print(verticalInput);
            //    verticalInput -= 0.0001f;
            //}
            verticalInput = -0.01f;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if(motorForce < 10000)
            {
                motorForce *= 2;
            }
            
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (motorForce > 1000)
            {
                motorForce /= 2;
            }
            
        }

    }

    private void HandleMotor()
    {

        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
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
    //    verticalInput = leverRange;
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
