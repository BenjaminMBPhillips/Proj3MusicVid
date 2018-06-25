using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlight : MonoBehaviour
{

    public GameObject bird,
        cam;

    public Rigidbody rb,
        birdrb;

    public Vector3 offset,
        camOffset,
        lookdir;

    public float baseSpeed,
        roll,
        pitch,
        smoothRot,
        smoothCam,
        yawrot;

    public bool start,
        camFollow,
        endGame;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        start = false;
        camFollow = false;
    }

    void FixedUpdate()
    {
        //sets the players velocity so it moves forward
        if (start == true)
        {
            rb.velocity = transform.forward * baseSpeed;
        }
        PlayerInput();
        BirdMovement();
    }

    //player input
    public void PlayerInput()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 10.0f;
        var y = Input.GetAxis("Vertical") * Time.deltaTime * 10.0f;

        transform.Translate(x, 0, 0);
        transform.Translate(0, y, 0);

        if (x != 0 || y != 0)
        {
            start = true;
            camFollow = true;
        }
    }

    //Moves the player bird toward the point of flight
    public void BirdMovement()
    {
        //find the yaw and pitch
        roll = Input.GetAxis("Horizontal") * Time.deltaTime;
        pitch = Input.GetAxis("Vertical") * Time.deltaTime;
        var rollrot = -roll * 2500;
        var pitchrot = -pitch * 1500;
        //TODO figure this out bettererererer
        //yawrot = (rollrot + pitch) * 0.5f;
        var campitch = -pitch * 250;
        var camroll = -roll * 250;

        baseSpeed = 10 + (roll*2 + pitch*2);

        //finds the rotation needed to look at
        var blook = Quaternion.LookRotation(gameObject.transform.position - bird.transform.position);
        var clook = Quaternion.LookRotation(bird.transform.position - cam.transform.position);

        //moves the objects at a delay and rotates them accordingly
        bird.transform.position = Vector3.Lerp(bird.transform.position, gameObject.transform.position - offset, Time.deltaTime);
        bird.transform.rotation = Quaternion.RotateTowards(bird.transform.rotation, Quaternion.Euler(new Vector3(pitchrot, 0, rollrot)), smoothRot * Time.deltaTime);


        if (camFollow == true)
        {
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, Quaternion.Euler(new Vector3(campitch, cam.transform.rotation.y, camroll)), Time.deltaTime);
            cam.transform.position = Vector3.Lerp(cam.transform.position, bird.transform.position + camOffset, smoothCam * Time.deltaTime);
        }
        else
        {
            cam.transform.LookAt(bird.transform.position);
        }
    }
}
