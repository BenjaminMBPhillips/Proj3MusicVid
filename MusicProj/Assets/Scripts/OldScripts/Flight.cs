using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    public float speed,
        rotSpeed,
        smoothRot,
        timer,
        playerRotX;

    public Rigidbody rb;

    public GameObject playerBird;
    

    // Use this for initialization
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0,0,0);
        playerRotX = transform.eulerAngles.x;
        Movement();
        CamFollow();
        AngleReset();
        
    }

    void Movement()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;
        var z = transform.forward.z * speed * Time.deltaTime;

        transform.position += transform.forward * speed * Time.deltaTime;

        //gameObject.transform.position += transform.forward * speed * Time.deltaTime;

        speed -= transform.forward.y;

        if(speed < 10)
        {
            speed = 10;
        }

        if(speed > 50)
        {
            speed = 50;
        }

        transform.Rotate(-Input.GetAxis("Vertical"), 0.0f, -Input.GetAxis("Horizontal"));

        ClampRotation(-75, 75, 0);

        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3((Input.GetAxis("Vertical") * 360), 0, (Input.GetAxis("Horizontal")* 360))), smoothRot * Time.deltaTime);

        float CheckTerrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
        if(CheckTerrainHeight > transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, CheckTerrainHeight + 1, transform.position.z);
        }
    }

    void CamFollow()
    {
        Vector3 camMove = transform.position - transform.forward * 10 + Vector3.up * 5;
        float bias = 0.96f;
        Camera.main.transform.position = Camera.main.transform.position * bias + camMove * (1.0f - bias);
        Camera.main.transform.LookAt(transform.position + transform.forward * 30);
    }

    void AngleReset()
    {
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0), Time.deltaTime);
            }
        }
        else
        {
            timer = 0.5f;
        }
    }

    void ClampRotation(float minAngle, float maxAngle, float clampAroundAngle = 0)
    {
        //clampAroundAngle is the angle you want the clamp to originate from
        //For example a value of 90, with a min=-45 and max=45, will let the angle go 45 degrees away from 90

        //Adjust to make 0 be right side up
        clampAroundAngle += 180;

        //Get the angle of the z axis and rotate it up side down
        float z = transform.rotation.eulerAngles.z - clampAroundAngle;

        z = WrapAngle(z);

        //Move range to [-180, 180]
        z -= 180;

        //Clamp to desired range
        z = Mathf.Clamp(z, minAngle, maxAngle);

        //Move range back to [0, 360]
        z += 180;

        //Set the angle back to the transform and rotate it back to right side up
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z + clampAroundAngle);
    }

    //Make sure angle is within 0,360 range
    float WrapAngle(float angle)
    {
        //If its negative rotate until its positive
        while (angle < 0)
            angle += 360;

        //If its to positive rotate until within range
        return Mathf.Repeat(angle, 360);
    }
}
