using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockFlight : MonoBehaviour
{

    public GameObject player;

    public float baseSpeed,
        roll,
        pitch,
        smoothRot,
        smoothCam,
        yawrot;

    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //find the yaw and pitch
        roll = Input.GetAxis("Horizontal") * Time.deltaTime;
        pitch = Input.GetAxis("Vertical") * Time.deltaTime;
        var rollrot = -roll * 2500;
        var pitchrot = -pitch * 1500;

        baseSpeed = 10 + (roll * 2 + pitch * 2);

        //moves the objects at a delay and rotates them accordingly
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, player.transform.position + offset, Time.deltaTime);
        gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime);
    }
}
